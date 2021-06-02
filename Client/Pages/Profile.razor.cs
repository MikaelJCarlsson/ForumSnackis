using ForumSnackis.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Pages
{
    public partial class Profile : ComponentBase
    {
		List<ImageFile> filesBase64 = new List<ImageFile>();
		string message = "InputFile";
		bool isDisabled = false;

		[Inject]
		public IHttpClientFactory HttpFactory { get; set; }
		async Task OnChange(InputFileChangeEventArgs e)
		{
			var files = e.GetMultipleFiles();
			foreach (var file in files)
			{
				var resizedFile = await file.RequestImageFileAsync(file.ContentType, 64, 64); // resize the image file
				var buf = new byte[resizedFile.Size];
				using (var stream = resizedFile.OpenReadStream())
				{
					await stream.ReadAsync(buf);
				}
				filesBase64.Add(new ImageFile { base64data = Convert.ToBase64String(buf), contentType = file.ContentType, fileName = file.Name });
			}
			message = "Click UPLOAD to continue";
		}

		async Task Upload()
		{
			isDisabled = true;
			var http = HttpFactory.CreateClient("private");
			using (var msg = await http.PostAsJsonAsync<List<ImageFile>>("/api/upload", filesBase64, System.Threading.CancellationToken.None))
			{
				isDisabled = false;
				if (msg.IsSuccessStatusCode)
				{
					message = $"{filesBase64.Count} files uploaded";
					filesBase64.Clear();
				}
			}
		}
	}
}
