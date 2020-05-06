using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SpendCA.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult Get()
        {

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var value = new {
                name = userId
            };

            return Ok(value);
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
