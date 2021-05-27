using ForumSnackis.Server.Services;
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
            var posts = await service.GetAsync();
            if (posts is not null)
                return Ok(posts);
            else return NotFound();
        }
        [Authorize]
        [HttpPost("Report/")]
        public async Task<IActionResult> Post([FromBody] int id)
        {
            
            int result = await service.Report(id, User);

            if(result == 0)
            {
                return StatusCode(500);
            } else
            {
                return Ok();
            }
        }


    }
}
