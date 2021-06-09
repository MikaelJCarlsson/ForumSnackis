// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using ForumSnackis.Server.Services;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.DTO;
using System;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ForumController : ControllerBase
    {

        private readonly ForumService service;

        public ForumController(ForumService service)
        {
            this.service = service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {

            var post = await service.GetAsync();
            return post is not null ? Ok(post) : NotFound();

        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ForumDTO forumCategory)
        {
            try
            {
                await service.CreateAsync(forumCategory);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }

        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] ForumDTO forumCategory)
        {
            try
            {
                await service.UpdateAsync(forumCategory);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrators")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await service.DeleteAsync(id);
                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }
    }

}
