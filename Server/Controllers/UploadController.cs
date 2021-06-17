using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ForumSnackis.Server.Services;
using ForumSnackis.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
	private readonly IWebHostEnvironment env;
	private readonly ImageService service;

	public UploadController(IWebHostEnvironment env, ImageService imageService)
	{
		this.env = env;
		service = imageService;
	}

	[HttpPost]
	[Authorize]
	public async Task Post([FromBody] ImageFile[] files)
	{
		await service.UploadFileAsync(files, User);
	}

	[HttpPost("{id}")]
	[Authorize]
	public async Task PostImage([FromBody] ImageFile[] files, int id)
	{
		await service.UploadImageAsync(files, id);
	}

	[HttpGet("{id}")]
    public async Task Get(string id)
    {
        var imageResult = await service.GetImage(id);
		if(imageResult is not null)
        {
			Ok(imageResult);
        } else
        {
			NotFound();
        }
    }
}
