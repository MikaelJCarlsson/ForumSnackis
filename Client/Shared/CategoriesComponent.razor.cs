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
        public string NewCategoryTitle { get; set; } = "";
        public bool ShowInputField { get; set; }
        public int ShowInputFieldForId { get; set; }
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
        public async void GetContent(CategoryDTO category)
        {

            await CategoryChanged.InvokeAsync(category);
           
        }

        public async void CreateCategory(string title)
        {

            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PostAsJsonAsync($"api/Category/", title);
            if (result.IsSuccessStatusCode)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category");
                categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                StateHasChanged();
            }

        }
        public void EditCategoryTitle(int id)
        {
            if (ShowInputField == true)
            {
                ShowInputFieldForId = 0;
                ShowInputField = false;
            }
            else
            {
                ShowInputFieldForId = id;
                ShowInputField = true;
            }            
        }
        public void CloseEditTitle()
        {
            ShowInputFieldForId = 0;
            ShowInputField = false;
        }
        public async Task EditCategory(CategoryDTO category)
        {
            CloseEditTitle();
            var title = NewCategoryTitle;
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PutAsJsonAsync($"api/Category/{category.Id}", title);
            if (result.IsSuccessStatusCode)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category");
                categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                NewCategoryTitle = "";
                       
            }            
            StateHasChanged();
        }
        public async Task DeleteCategory(CategoryDTO category)
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.DeleteAsync($"api/Category/{category.Id}");
            if (result.IsSuccessStatusCode)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync("api/Category");
                categories = await request.Content.ReadFromJsonAsync<List<CategoryDTO>>();
                NewCategoryTitle = "";

            }
            StateHasChanged();
        }
    }
}
