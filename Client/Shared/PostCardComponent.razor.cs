using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        [Parameter]
        public PostDTO Post { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public PostDTO Reply { get; set; }
        public bool ShowReplyForm { get; set; }
        [Parameter]
        public EventCallback UpdatePosts { get; set; }
        public ClaimsPrincipal CurrentUser { get; set; }

        protected override async Task OnInitializedAsync()
        {
            CurrentUser = (await authenticationStateTask).User;         
        }
        protected override async Task OnParametersSetAsync()
        {
            
            this.StateHasChanged();
        }

        private async Task ReportPost(int id)
        {
            if(Post is not null) { 
                var privateHttp = HttpFactory.CreateClient("private");
                var result = await privateHttp.PostAsJsonAsync($"api/Post/Report/", id);

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
                var result = await privateHttp.PostAsJsonAsync($"api/Subject/Posts/", Reply);

                if (result.IsSuccessStatusCode)
                {
                    await UpdatePosts.InvokeAsync();
                    ToggleReplyForm(false);
                }
            }
        }

    }
}
