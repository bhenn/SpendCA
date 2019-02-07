using System;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Core.Entities;
using SpendCA.Core.Interfaces;
using SpendCA.Core.Exceptions;

namespace SpendCA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {
    
        private readonly ICategoryRepository _categoryRepository; 

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet("ping")]
        public IActionResult Ping()
        {
            return Ok("Pong!");
        }

        // GET api/categories
        [HttpGet]
        public ActionResult<IList<Category>> Get()
        {
            var userId = GetUserId();

            return _categoryRepository.GetAll(userId);

        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public ActionResult<Category> GetItem(int id)
        {
            try
            {
                return _categoryRepository.GetItem(id);
            }
            catch (ItemNotFoundException ex)
            {
                return NotFound(ex.Message);
            }

        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {

            category.UserId = GetUserId();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _categoryRepository.Add(category);

            return CreatedAtAction("GetItem", new { id = category.Id }, category);
        }

        private int GetUserId()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            return Convert.ToInt32(userId);
        }


    }
}
