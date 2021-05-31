using ForumSnackis.Server.Services;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private readonly PostService service;

        public PostController(PostService service)
        {
            this.service = service;
        }
        [Authorize(Roles = "Administrators")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await service.GetReportsAsync();
            if (posts is not null)
                return Ok(posts);
            else return NotFound();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var post = await service.GetAsync(id);
            if (post is not null)
                return Ok(post);
            else
                return NotFound();

        }
        [Authorize]
        [HttpPost("Report/")]
        public async Task<IActionResult> Post([FromBody] int id)
        {
            
            int result = await service.Report(id, User);

            if(result != 0)
            {
                return Ok();
            }
            else
            {
                return StatusCode(500);
            }
        }
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] PostDTO post)
        {
            var result = await service.UpdateAsync(id, post,User);
            if (result != 0)
                return Ok();
            else
                return StatusCode(409);
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await service.DeleteAsync(id);
            if (post != 0)
                return Ok();
            else
                return NotFound();
        }
    }
}
