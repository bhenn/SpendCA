using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Exceptions;
using Microsoft.AspNetCore.Authorization;
using SpendCA.Core.Interfaces;

namespace SpendCA.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class SpendsController : ControllerBase
    {

        private readonly ISpendService _spendService;

        public SpendsController(ISpendService spendService)
        {
            _spendService = spendService;
        }

        // GET api/spends
        [HttpGet]
        public ActionResult<IList<Spend>> Get()
        {
            return _spendService.GetAll(GetUserId());
        }

        // POST api/values
        [HttpPost]
        public ActionResult<Spend> Post([FromBody] Spend spend)
        {
            spend.UserId = GetUserId();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _spendService.Add(spend);

            return CreatedAtAction("GetItem", new { id = spend.Id }, spend);
        }

        [HttpPut("{id}")]
        public ActionResult<Spend> Update(int id, [FromBody] Spend spend){

            if (id != spend.Id)
                return BadRequest();

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            _spendService.Update(spend, GetUserId());

            return NoContent();

        }

        public Spend GetItem(int id)
        {
            return _spendService.GetItem(id);
        }

        [HttpDelete("{id}")]
        public ActionResult<Spend> DeleteSpend(int id){
            try
            {
                return _spendService.Delete(id, GetUserId());
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
        private int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(userId);
        }

    }
}
