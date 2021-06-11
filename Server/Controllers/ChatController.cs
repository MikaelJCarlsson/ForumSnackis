using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Server.Services;
using ForumSnackis.Shared.DTO;
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

        public ChatController(ChatService service)
        {
            this.service = service;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetChatRoom(string id)
        {
            var result = await service.GetChatRoomByIdAsync(id,User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            return Ok(result);
        }
        
        [HttpGet("Room/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await service.GetChatRoom(id,User.Claims.FirstOrDefault(x => x.Type == "sub")?.Value);
            return Ok(result);
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> Post([FromBody] MessageDTO message,int id)
        {
            var result = await service.CreateMessage(id, User.Claims.First(t => t.Type == "sub").Value, message);
            return result ? Ok() : NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> PostChatRoom([FromBody] ChatDTO chatDTO)
        {
            var result = await service.CreateNewChatRoomAsync(chatDTO);
            return result == 1 ? Ok() : StatusCode(500);
        }

    }
}
