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
        private CategoryDTO Category;
        [Parameter]
        public int CategoryId { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }

        protected override async Task OnInitializedAsync()
        {
            if (Category is null)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category/Subjects/{CategoryId}");

                if (request.IsSuccessStatusCode)
                {
                    Category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                }
            }
        }
    }
}
