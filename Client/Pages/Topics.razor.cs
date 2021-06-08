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
    public partial class Topics : ComponentBase
    {
        [Parameter]
        public int CategoryId { get; set; }
        [Parameter]
        public CategoryDTO Category { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }
        public bool SubjectForm { get; set; }
        public CreateSubjectCommand NewSubject { get; set; } = new();

        public string NewSubjectTitle = "";
        private HttpClient publicHttp;
        private HttpClient privateHttp;

        protected override async Task OnParametersSetAsync() =>
            Category = await GetSubjects();
        private async Task<CategoryDTO> GetSubjects()
        {
            publicHttp = HttpFactory.CreateClient("public");
            var request = await publicHttp.GetAsync($"api/Category/Subjects/{CategoryId}");

            if (request.IsSuccessStatusCode)
                return await request.Content.ReadFromJsonAsync<CategoryDTO>();
            else
                return default;
        }

        public async Task SubjectComponentChanged() =>
            Category = await GetSubjects();

        public async Task CreateSubject()
        {
            if (Category != null && NewSubject != null)
            {
                NewSubject.CategoryId = CategoryId;

                if (CategoryId > 0)
                {
                    privateHttp = HttpFactory.CreateClient("private");
                    var result = await privateHttp.PostAsJsonAsync($"api/Subject/", NewSubject);

                    if (result.IsSuccessStatusCode)
                    {
                        var publicHttp = HttpFactory.CreateClient("public");

                        var request = await publicHttp.GetAsync("api/Category/Subjects/{Category.Id}");
                        Category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                    }
                }
            }
        }
    }
}
