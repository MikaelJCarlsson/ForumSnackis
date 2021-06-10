using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ChatController : Controller
    {
        private readonly ChatService service;
        private readonly UserManager<ApplicationUser> userManager;
        public ChatController(ChatService service, UserManager<ApplicationUser> userManager)
        {
            this.service = service;
            this.userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> GetChatRoomAsync()
        {
            var chatroom = await service.GetChatRoomByIdAsync()

        }
/*        [HttpGet("users")]

        public async Task<IActionResult> GetUsersAsync()
        {
            var userId = User.Claims.Where(a => a.Type == ClaimTypes.NameIdentifier).Select(a => a.Value).FirstOrDefault();
            var allUsers = await dbContext.Users.Where(user => user.Id != userId).ToListAsync();
            return Ok(allUsers);
        }
        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetUserDetailsAsync(string userId)
        {
            var user = await dbContext.Users.Where(user => user.Id == userId).FirstOrDefaultAsync();
            return Ok(user);
        }*/

    }
}
