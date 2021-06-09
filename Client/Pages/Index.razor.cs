using ForumSnackis.Client.Shared;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Shared.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Pages
{
    public partial class Index : ComponentBase
    {
        public class ForumCategoryModel
        {
            [Required]
            [MinLength(3)]
            public string Title { get; set; }
        }

        private async Task CreateForumCategory()
        {
            var privateHttp = HttpFactory.CreateClient("private");

            var request = await privateHttp.PostAsJsonAsync("api/ForumCategory", new ForumDTO() { Title = NewForumCategory.Title });

            if (request.IsSuccessStatusCode)
            {
                NewForumCategory = new();
                await GetForumCategories();
            }

        }

        private async Task GetForumCategories()
        {
            var publicHttp = HttpFactory.CreateClient("public");

            var request = await publicHttp.GetAsync("api/ForumCategory");

            if (request.IsSuccessStatusCode)
                ForumCategories = await request.Content.ReadFromJsonAsync<List<ForumDTO>>();
        }

        public ForumCategoryModel NewForumCategory { get; set; } = new();
        private CategoriesComponent CategoriesRef;
        private CategoryDTO NewCategory = new();
        public int PageState { get; set; } = 1;
        [Parameter]
        public CategoryDTO Category { get; set; }
        [Parameter]
        public SubjectsDTO CurrentSubject { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }

        [Parameter]
        public int CurrentPageStateChanged { get; set; }
        public bool CategoryForm { get; set; }
        public List<ForumDTO> ForumCategories { get; private set; }

        private void CreateCategory()
        {
            if (NewCategory is not null)
            {
                CategoryDTO category = new();
                category.Title = NewCategory.Title;
                CategoriesRef.CreateCategory(category.Title);
            }
        }
    }

}
