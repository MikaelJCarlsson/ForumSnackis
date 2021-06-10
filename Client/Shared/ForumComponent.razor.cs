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
    public partial class ForumComponent : ComponentBase
    {
        public class CreateCategoryModel
        {
            public string Title { get; set; }
            public int ForumId { get; set; }
        }
        public CreateCategoryModel NewCategory { get; set; } = new();
        [Parameter]
        public ForumDTO Forum { get; set; } = new();
        public bool ShowCreateCategoryForm { get; set; }
        [Parameter]
        public EventCallback<ForumDTO> UpdateForum { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }

        public async Task UpdateCategory(CategoryDTO category)
        {
            var publicHttp = HttpFactory.CreateClient("public");
            var request = await publicHttp.GetAsync($"api/Category/{category.Id}");

            if (request.StatusCode == System.Net.HttpStatusCode.NotFound)
                await UpdateForum.InvokeAsync(Forum);

            if (request.IsSuccessStatusCode)
            {
                var tmp = Forum.Categories.Find(c => c.Id == category.Id);
                Forum.Categories.Remove(tmp);
                Forum.Categories.Add(await request.Content.ReadFromJsonAsync<CategoryDTO>());
            }

        }

        private void ToggleCreateCategoryForm() =>
            ShowCreateCategoryForm = ShowCreateCategoryForm ? false : true;
        public async Task CreateCategory()
        {

            CategoryDTO category = new(){
                Title = NewCategory.Title,
                ForumCategoryId = Forum.Id,
            };

            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PostAsJsonAsync($"api/Category", category);

            if (result.IsSuccessStatusCode)
            {
                await UpdateForum.InvokeAsync(Forum);
            }
        }

        private async Task UpdateSelf()
        {
            var publicHttp = HttpFactory.CreateClient("public");

            var request = await publicHttp.GetAsync($"api/Forum/{Forum.Id}");

            if (request.IsSuccessStatusCode)
                Forum = await request.Content.ReadFromJsonAsync<ForumDTO>();
        }
    }
}
