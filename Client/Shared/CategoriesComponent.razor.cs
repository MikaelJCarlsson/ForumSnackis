using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Shared
{
    public partial class CategoriesComponent : ComponentBase
    {

        private List<string> categories;
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        protected override async Task OnInitializedAsync()
        {

            if (categories is null)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category");

                if (request.IsSuccessStatusCode)
                {
                    categories = await request.Content.ReadFromJsonAsync<List<string>>();
                }
            }
        }
    }
}
