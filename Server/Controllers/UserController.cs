using ForumSnackis.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumSnackis.Shared.DTO;
using System.Security.Claims;

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
        [HttpGet]
        public async Task<IActionResult> GetUsersAsync()
        {
            var users = await service.GetUsersAsync(User);
            if (users != null)
            {
                return Ok(users);
            }
            return StatusCode(500);
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
        [HttpPut("EditBio/{id}")]
        public async Task<IActionResult> EditBioAsync(string id,[FromBody] string bio)
        {
            var result = await service.EditUserBioAsync(id, bio);
            if(result == 1)
            {
                return Ok();
            }
            return StatusCode(500);
        }
    }
}
