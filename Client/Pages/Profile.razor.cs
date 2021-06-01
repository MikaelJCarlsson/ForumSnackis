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

        private List<File> files = new();
        private List<UploadResult> uploadResults = new();
        private int maxAllowedFiles = 1;
        private bool shouldRender;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        protected override bool ShouldRender() => shouldRender;

    private async Task OnInputFileChange(InputFileChangeEventArgs e)
    {
        shouldRender = false;
        long maxFileSize = 1024 * 1024 * 15;
        var upload = false;

        using var content = new MultipartFormDataContent();

        foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
        {
            if (uploadResults.SingleOrDefault(
                f => f.FileName == file.Name) is null)
            {
                using var fileContent = new StreamContent(file.OpenReadStream());

                files.Add(
                    new()
                    {
                        Name = file.Name
                    });

                if (file.Size < maxFileSize)
                {
                    content.Add(
                        content: fileContent,
                        name: "\"files\"",
                        fileName: file.Name);

                    upload = true;
                }
                else
                {

                    uploadResults.Add(
                        new()
                        {
                            FileName = file.Name,
                            ErrorCode = 6,
                            Uploaded = false
                        });
                }
            }
        }

        if (upload)
        {
                var privateHttp = HttpFactory.CreateClient("private");
                var publicHttp = HttpFactory.CreateClient("public");

                var response = await publicHttp.PostAsync("Filesave/", content);
                
                var newUploadResults = await response.Content
                    .ReadFromJsonAsync<IList<UploadResult>>();

            uploadResults = uploadResults.Concat(newUploadResults).ToList();
        }

        shouldRender = true;
    }

    private static bool FileUpload(IList<UploadResult> uploadResults, string fileName, out UploadResult result)
    {
        result = uploadResults.SingleOrDefault(f => f.FileName == fileName);

        if (result is null)
        {
            result = new();
            result.ErrorCode = 5;
        }

        return result.Uploaded;
    }

    private class File
    {
        public string Name { get; set; }
    }
}


}
