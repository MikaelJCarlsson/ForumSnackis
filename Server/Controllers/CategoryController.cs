using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ForumSnackis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryController(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var category = dbContext.ForumCategories.Find(id);
            if (category is not null)
                return Ok(JsonSerializer.Serialize(category));
            else
                return NotFound();

        }

        // POST api/<CategoryController>
        [HttpPost]
        [Authorize]
        public IActionResult Post([FromBody] string value)
        {
            dbContext.ForumCategories.Add(new ForumCategory() { Title = value });
            dbContext.SaveChanges();
            //Created
            return StatusCode(202);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        [Authorize]
        public IActionResult Put(int id, [FromBody] string value)
        {
            return StatusCode(501);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public IActionResult Delete(int id)
        {
            var category = dbContext.ForumCategories.Find(id);
            if (category is not null)
            {
                dbContext.ForumCategories.Remove(category);
                dbContext.SaveChanges();
                return Ok();
            } else
            {
                return NotFound();
            }
        }
    }
}
