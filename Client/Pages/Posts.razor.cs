using ForumSnackis.Client.Shared;
using ForumSnackis.Shared;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Pages
{
    public partial class Posts : ComponentBase
    {
        List<ImageFile> filesBase64 = new List<ImageFile>();
        bool isDisabled = false;
        [Parameter]
        public int SubjectId { get; set; }
        [Parameter]
        public SubjectsDTO Subject { get; set; } = new();
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }
        private HttpClient publicHttp;
        private ClaimsPrincipal user;
        private HttpClient privateHttp;

        public PostDTO NewPost { get; set; } = new();

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Subject ??= await GetSubject();
            await UpdatePosts();
        }

        private bool CreatePublicHttpClient()
        {
            publicHttp ??= HttpFactory.CreateClient("public");
            return !(publicHttp is null);
        }

        private async Task<bool> CreatePrivateHttpClientAsync()
        {
            if (privateHttp is null)
            {
                if (user is null)
                {
                    var authState = await authenticationStateTask;
                    user = authState.User;
                }

                if (user.Identity.IsAuthenticated)
                    privateHttp = HttpFactory.CreateClient("private");
            }

            return !(privateHttp is null);
        }
        private async Task<List<PostDTO>> GetPosts()
        {
            if (publicHttp is null)
                CreatePublicHttpClient();

            var request = await publicHttp.GetAsync($"api/Subject/Posts/{SubjectId}");

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadFromJsonAsync<List<PostDTO>>();
            else
                return default;
        }

        private async Task<SubjectsDTO> GetSubject()
        {
            if (publicHttp is null)
                CreatePublicHttpClient();

            var request = await publicHttp.GetAsync($"api/Subject/{SubjectId}");

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadFromJsonAsync<SubjectsDTO>();
            else
                return default;
        }
        public async Task UpdatePosts() =>
            Subject.Posts = await GetPosts() ?? new();

        public async Task CreatePost()
        {
            if (privateHttp is null && await CreatePrivateHttpClientAsync() && NewPost?.Content?.Length > 2 || filesBase64 != null)
            {
                NewPost.SubjectId = SubjectId;
                if(filesBase64 != null && string.IsNullOrEmpty(NewPost.Content))
                {
                    NewPost.Content = "";
                }
                var request = await privateHttp.PostAsJsonAsync("api/Subject/Posts/", NewPost);

                if (request.IsSuccessStatusCode)
                {
                    NewPost.Content = "";
                    if(filesBase64 != null)
                    {
                        var id = await request.Content.ReadFromJsonAsync<int>();
                        await Upload(id);
                    }
                    await UpdatePosts();
                    
                }
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

        }

        async Task Upload(int id)
        {
            isDisabled = true;
            var http = HttpFactory.CreateClient("private");
            using (var msg = await http.PostAsJsonAsync($"/api/upload/{id}", filesBase64, System.Threading.CancellationToken.None))
            {
                isDisabled = false;
                if (msg.IsSuccessStatusCode)
                {
                    filesBase64.Clear();
                }
            }
        }
    }
}


