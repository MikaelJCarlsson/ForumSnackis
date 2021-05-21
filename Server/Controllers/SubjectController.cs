using ForumSnackis.Server.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
