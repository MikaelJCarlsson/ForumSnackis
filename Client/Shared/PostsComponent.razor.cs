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
                    CurrentSubject = await request.Content.ReadFromJsonAsync<SubjectsDTO>();
                }
            }
        }
    }
}
