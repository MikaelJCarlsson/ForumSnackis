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
        public int PageState { get; set; } = 1;
        [Parameter]
        public CategoryDTO Category { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }

        [Parameter]
        public int CurrentPageStateChanged { get; set; }

        private void UpdateCategoryState(CategoryDTO category)
        {
            Category = category;
            PageState = 2;
        }

        private void NavigateHome()
        {
            PageState = 1;
        }
    }

}
