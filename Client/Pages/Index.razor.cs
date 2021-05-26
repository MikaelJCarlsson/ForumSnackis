using ForumSnackis.Client.Shared;
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
    public partial class Index : ComponentBase
    {
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

        private void UpdateCategoryState(CategoryDTO category)
        {
            Category = category;
            PageState = 2;
        }

        private void NavigateHome()
        {
            PageState = 1;
        }
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
