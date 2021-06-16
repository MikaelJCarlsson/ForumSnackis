using ForumSnackis.Shared;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
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
        [Parameter]
        public string UserName { get; set; }
        public UserDTO UserData { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public bool ShowEditForm { get; set; }
        public bool ToggleControll { get; set; }
        public UserDTO EditedUser { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            var httpclient = HttpFactory.CreateClient("private");
            var response = await httpclient.GetAsync($"api/User/{UserName}");
            if (response.IsSuccessStatusCode)
            {
                UserData = await response.Content.ReadFromJsonAsync<UserDTO>();
            }
        }
        private async Task UpdateUserBio()
        {
            ShowEditForm = false;
            ToggleControll = true;

            var httpclient = HttpFactory.CreateClient("private");
            var response = await httpclient.PutAsJsonAsync($"api/User/EditBio/{UserData.UserId}",EditedUser.UserBio);
            if (response.IsSuccessStatusCode)
            {
                response = await httpclient.GetAsync($"api/User/{UserName}");
                if (response.IsSuccessStatusCode)
                {
                    UserData = await response.Content.ReadFromJsonAsync<UserDTO>();
                }
            }
        }
        private void ToggleEdit(bool show)
        {
            if (show && !ToggleControll)
            {
                if(EditedUser is null)
                {
                    EditedUser = new();
                    EditedUser.UserBio = "";
                    EditedUser.UserId = UserData.UserId;
                    
                }
                ShowEditForm = true;
                ToggleControll = true;
            }
            else
            {
                ShowEditForm = false;
                ToggleControll = false;
            }
        }
        async Task OnChange(InputFileChangeEventArgs e)
        {
            var files = e.GetMultipleFiles();
            foreach (var file in files)
            {
                var resizedFile = await file.RequestImageFileAsync(file.ContentType, 128, 128); // resize the image file
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
            await OnParametersSetAsync();
        }
    }
}
