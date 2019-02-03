using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SpendCA.Interfaces;
using SpendCA.Models;
using SpendCA.Services;

namespace SpendCA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoriesController : ControllerBase
    {

        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
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

            return _categoryService.GetAll(userId);

        }

        // GET api/categories/1
        [HttpGet("{id}")]
        public ActionResult<Category> GetItem(int id)
        {
            var category = _categoryService.GetItem(id);

            if (category == null)
                return NotFound();

            return category;
        }

        // POST api/categories
        [HttpPost]
        public IActionResult Post([FromBody] Category category)
        {

            category.UserId = GetUserId();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _categoryService.Add(category);

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
