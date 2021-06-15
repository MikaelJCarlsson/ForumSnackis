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

namespace ForumSnackis.Client.Shared
{
    public partial class PostCardComponent : ComponentBase
    {
        List<ImageFile> filesBase64 = new List<ImageFile>();
        bool isDisabled = false;
        [Parameter]
        public PostDTO Post { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public PostDTO Reply { get; set; }
        private bool ShowReplyForm { get; set; }
        private bool ShowEditForm { get; set; }
        public PostDTO EditedPost { get; set; }
        public List<PostDTO> Quotes { get; set; }
        public bool Liked { get; set; }
        public bool Disliked { get; set; }
        [Parameter]
        public EventCallback UpdatePosts { get; set; }

        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            this.StateHasChanged();
        }

        private async Task ReportPost()
        {
            if(Post is not null) { 
                var privateHttp = HttpFactory.CreateClient("private");
                var result = await privateHttp.PostAsJsonAsync($"api/Post/Report/", Post.Id);

                if (result.IsSuccessStatusCode)
                { 
                    return;
                }
            }
        }


        private void ToggleReplyForm(bool show)
        {
            if (show)
            {
                if (Reply is null)
                {
                    Reply = new();
                    Reply.QuoteContent = $"\"{Post.Content}\"";
                }
                ShowReplyForm = true;
            }
            else
            {
                ShowReplyForm = false;
            }
        }
        private async Task DeletePost()
        {
            if(Post is not null)
            {
                var privateHttp = HttpFactory.CreateClient("private");
                var result = await privateHttp.DeleteAsync($"api/Post/{Post.Id}");

                if(result.IsSuccessStatusCode)
                {
                    await UpdatePosts.InvokeAsync();
                }
            }
        }
        private async Task CreateReply()
        {
            if (Reply is not null)
            {
                Reply.SubjectId = Post.SubjectId;
                Reply.QuoteId = Post.Id;
                var privateHttp = HttpFactory.CreateClient("private");
                /*               
                                var filterdString = await privateHttp.PostAsJsonAsync("https://localhost:44353/Filter", Reply.Content);
                                if (filterdString.IsSuccessStatusCode)
                                {
                                    Reply.Content = await filterdString.Content.ReadAsStringAsync();
                                    var result = await privateHttp.PostAsJsonAsync($"api/Subject/Posts/", Reply);

                                    if (result.IsSuccessStatusCode)
                                    {
                                        await UpdatePosts.InvokeAsync();
                                        ToggleReplyForm(false);
                                    }
                                }*/
                var result = await privateHttp.PostAsJsonAsync($"api/Subject/Posts/", Reply);
                await Upload(await result.Content.ReadFromJsonAsync<int>());
                if (result.IsSuccessStatusCode)
                {
                    await UpdatePosts.InvokeAsync();
                    ToggleReplyForm(false);
                }
            }
        }
        private void ToggleEditForm(bool show)
        {
            if (show)
            { 
                EditedPost = new();
                EditedPost.Id = Post.Id;
                EditedPost.Content = Post.Content;
                ShowEditForm = true;
            }
            else
            {
                ShowEditForm = false;
            }

        }
        private async Task LikePostAsync(bool liked)
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {

                if (liked && !Liked)
                {
                    Liked = true;
                    Post.LikeCount++;
                }
                else
                {
                    Liked = false;
                    Post.LikeCount--;
                }
                await UpdatePostLikes();
            }
        }
        private async Task DislikePostAsync(bool liked)
        {
            var authState = await authenticationStateTask;
            var user = authState.User;

            if (user.Identity.IsAuthenticated)
            {

                if (liked && !Disliked)
                {
                    Disliked = true;
                    Post.DislikeCount++;
                }
                else
                {
                    Disliked = false;
                    Post.DislikeCount--;
                }
                await UpdatePostLikes();
            }
        }
        private async Task UpdatePostLikes()
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PutAsJsonAsync($"api/Post/{Post.Id}", Post);
            if (result.IsSuccessStatusCode)
            {
                await UpdatePosts.InvokeAsync();
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
        private async Task EditPost()
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PutAsJsonAsync($"api/Post/{EditedPost.Id}", EditedPost);
            if (result.IsSuccessStatusCode)
            {
                await UpdatePosts.InvokeAsync();
                ToggleEditForm(false);
            }
        }

    }
}
