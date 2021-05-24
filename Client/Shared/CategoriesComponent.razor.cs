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
    public partial class CategoriesComponent : ComponentBase
    {

        [Parameter]
        public int CurrentPageState { get; set; } 


        private List<CategoryDTO> categories;
        [Parameter]
        public CategoryDTO Category { get; set; }
        [Parameter]
        public EventCallback<CategoryDTO> CategoryChanged { get; set; }
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
                    categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                }
            }
        }
        public async void GetContent(int id)
        {
            var publicHttp = HttpFactory.CreateClient("public");
            var request = await publicHttp.GetAsync($"api/Category/{id}");

            if (request.IsSuccessStatusCode)
            {
                Category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                await CategoryChanged.InvokeAsync(Category);
            }

        }
    }
}
