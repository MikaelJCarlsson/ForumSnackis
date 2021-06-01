using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using ForumSnackis.Shared;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
	private readonly IWebHostEnvironment env;

	public UploadController(IWebHostEnvironment env)
	{
		this.env = env;
	}

	[HttpPost]
	public async Task Post([FromBody] ImageFile[] files)
	{
		foreach (var file in files)
		{
			var buf = Convert.FromBase64String(file.base64data);
			await System.IO.File.WriteAllBytesAsync(env.ContentRootPath + "\\" + Guid.NewGuid().ToString("N") + "-" + file.fileName, buf);
		}
	}
}
