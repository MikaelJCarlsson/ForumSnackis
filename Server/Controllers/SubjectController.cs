using ForumSnackis.Server.Services;
using ForumSnackis.Shared;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly SubjectService service;

        public SubjectController(SubjectService service)
        {
            this.service = service;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {

            var subject = await service.GetAsync(id);
            if (subject is not null)
                return Ok(subject);
            else
                return NotFound();

        }

        [HttpGet("Posts/{id}")]
        public async Task<IActionResult> GetPosts(int id)
        {

            var subject = await service.GetPosts(id);
            if (subject is not null)
                return Ok(subject);
            else
                return NotFound();

        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateSubjectCommand csc)
        {       
            var result = await service.CreateSubject(csc, User);
            if (result == 0)
                //Internal Server Error
                return StatusCode(500);
            else
                //Created
                return StatusCode(202);
        }

        [Authorize]
        [HttpPost("Posts/")]
        public async Task<IActionResult> Post([FromBody] PostDTO post)
        {
            var result = await service.CreatePost(post,User);
            if (result == 0)
                //Internal Server Error
                return StatusCode(500);
            else
                //Created
                return StatusCode(202);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Delete(int id)
        {
            var category = await service.DeleteAsync(id);
            if (category != 0)
                return Ok();
            else
                return NotFound();
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] SubjectsDTO subj)
        {
            var result = await service.UpdateAsync(id,subj);
            if (result != 0)
                return Ok();
            else
                return StatusCode(409);
        }
    }
}
