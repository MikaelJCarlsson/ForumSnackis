using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Shared
{
    public partial class PostCardComponent : ComponentBase
    {
        [Parameter]
        public PostDTO Post { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public PostDTO Reply { get; set; }
        public bool ShowReplyForm { get; set; }
        [Parameter]
        public EventCallback UpdatePosts { get; set; }

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
