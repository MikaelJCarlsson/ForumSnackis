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
    public partial class ReportedPostComponent : ComponentBase
    {
        private List<PostDTO> ReportedPosts;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public string PostContent { get; set; } = "";
        public bool ShowEditForm { get; set; }
        private void ToggleEditForm(bool show)
        {
            if (show)
            {
                ShowEditForm = true;
            }
            else
            {
                ShowEditForm = false;
            }

        }
        protected override async Task OnInitializedAsync()
        {
            if (ReportedPosts is null)
            {
                var privateHttp = HttpFactory.CreateClient("private");
                var request = await privateHttp.GetAsync("api/Post/reportedposts");

                if (request.IsSuccessStatusCode)
                {
                    ReportedPosts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
        }
        private async Task DeleteReportedPost(int postId)
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var request = await privateHttp.DeleteAsync($"api/Post/Report/{postId}");

            if (request.IsSuccessStatusCode)
            {
                request = await privateHttp.GetAsync("api/Post/reportedposts");

                if (request.IsSuccessStatusCode)
                {
                    ReportedPosts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
        }
        private async Task EditPost(int postId)
        {
            ShowEditForm = false;
            var privateHttp = HttpFactory.CreateClient("private");
            var request = await privateHttp.PutAsJsonAsync($"api/Post/Report/{postId}",PostContent);

            if (request.IsSuccessStatusCode)
            {
                request = await privateHttp.GetAsync("api/Post/reportedposts");

                if (request.IsSuccessStatusCode)
                {
                    ReportedPosts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }      
        }
        private async Task DeletePost(int postId)
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var request = await privateHttp.DeleteAsync($"api/Post/{postId}");

            if (request.IsSuccessStatusCode)
            {
                request = await privateHttp.GetAsync("api/Post/reportedposts");

                if (request.IsSuccessStatusCode)
                {
                    ReportedPosts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
        }
    }
}
