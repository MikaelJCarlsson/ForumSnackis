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
