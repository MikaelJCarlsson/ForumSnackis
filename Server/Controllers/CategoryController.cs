using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Server.Services;
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
        private readonly CategoryService service;

        public CategoryController(CategoryService service)
        {
            this.service = service;
        }

        // GET: api/<CategoryController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var categories = await service.GetAsync();
            if (categories is not null)
                return Ok(categories);
            else return NotFound();
        }

        // GET api/<CategoryController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var category = await service.GetAsync(id);
            if (category is not null)
                return Ok(category);
            else
                return NotFound();

        }

        // POST api/<CategoryController>
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] string value)
        {
          var result = await service.CreateAsync(value);
            if(result == 0)
                //Internal Server Error
                return StatusCode(500);          
            else
                //Created
                return StatusCode(202);
        }

        // PUT api/<CategoryController>/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] string value)
        {
            var result = await service.UpdateAsync(id, value);
            if (result == 1)
                return Ok();
            else
                return StatusCode(409);
        }

        // DELETE api/<CategoryController>/5
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await service.DeleteAsync(id);
            if (category == 1)
                return Ok();
            else
                return NotFound();
        }
    }
}
