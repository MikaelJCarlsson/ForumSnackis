using ForumSnackis.Client.Shared;
using ForumSnackis.Shared;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Pages
{
    public partial class Posts : ComponentBase
    {
        [Parameter]
        public int SubjectId { get; set; }
        [Parameter]
        public SubjectsDTO Subject { get; set; } = new();
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }
        public bool SubjectForm { get; set; }
        public CreateSubjectCommand NewSubject { get; set; } = new();

        public string NewSubjectTitle = "";
        private HttpClient publicHttp;

        protected override async Task OnInitializedAsync()
        {
            if (publicHttp is null)
                publicHttp = HttpFactory.CreateClient("public");
            if (Subject is null)
                Subject = await GetSubject() ?? new();
            if (Subject.Posts is null)
                Subject.Posts = await GetPosts() ?? new();
        }
        protected override async Task OnParametersSetAsync() =>
            Subject.Posts ??= await GetPosts();
        private async Task<List<PostDTO>> GetPosts()
        {
            if (publicHttp == null)
                return null;

            var request = await publicHttp.GetAsync($"api/Subject/Posts/{SubjectId}");

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadFromJsonAsync<List<PostDTO>>();
            else
                return null;
        }

        private async Task<SubjectsDTO> GetSubject()
        {
            if (publicHttp == null)
                return null;

            var request = await publicHttp.GetAsync($"api/Subject/{SubjectId}");

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadFromJsonAsync<SubjectsDTO>();
            else
                return null;
        }
        public async Task UpdatePosts() =>
            Subject.Posts ??= await GetPosts();
    }
}

