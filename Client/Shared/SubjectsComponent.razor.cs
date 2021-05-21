using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForumSnackis.Shared.DTO;
namespace ForumSnackis.Client.Shared
{
    public partial class SubjectsComponent : ComponentBase
    {
        private List<SubjectsDTO> subjects;
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            if (subjects is null)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category/{CategoryId}");

                if (request.IsSuccessStatusCode)
                {
                    subjects = await request.Content.ReadFromJsonAsync<List<SubjectsDTO>>();
                }
            }
        }
    }
}
