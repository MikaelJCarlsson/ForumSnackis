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
    [Authorize]
    public class UserController : Controller
    {
        private readonly UserService service;
        public UserController(UserService service)
        {
            this.service = service;
        }
        [HttpGet("{name}")]
        public async Task<IActionResult> GetAsync(string name)
        {
            var user = await service.GetUserAsync(name);
            if(user != null)
            {
                return Ok(user);
            }
            return StatusCode(404);
        }
    }
}
