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
    public partial class PostsComponent : ComponentBase
    {
        [Parameter]
        public SubjectsDTO CurrentSubject { get; set; }
        public List<PostDTO> Posts { get; set; }

        public PostDTO NewPost { get; set; } = new();
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            if (CurrentSubject is not null)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync($"api/Subject/Posts/{CurrentSubject.Id}");

                if (request.IsSuccessStatusCode)
                {
                    Posts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
        }
        private async Task CreatePost()
        {
            if (NewPost is not null && CurrentSubject is not null)
            {
                NewPost.SubjectId = CurrentSubject.Id;
                var privateHttp = HttpFactory.CreateClient("private");
                var request = await privateHttp.PostAsJsonAsync($"api/Subject/Posts/",NewPost);

                if (request.IsSuccessStatusCode)
                {
                    NewPost.Content = "";
                    var publicHttp = HttpFactory.CreateClient("public");
                    var getRequest = await publicHttp.GetAsync($"api/Subject/Posts/{CurrentSubject.Id}");
                    Posts = await getRequest.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
           
        }

        private async Task UpdatePosts()
        {
            await OnParametersSetAsync();
        }
    }
}
