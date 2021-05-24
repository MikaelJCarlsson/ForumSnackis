using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Pages
{
    public partial class Index : ComponentBase
    {
        public int PageState { get; set; } = 1;
        [Parameter]
        public CategoryDTO Category { get; set; }
        
        
        [Parameter]
        public int SubjectId { get; set; }
        [Parameter]
        public int CurrentPageStateChanged { get; set; }

        private void UpdateCategoryState(CategoryDTO category)
        {
            Category = category;
            PageState = 2;
        }
    }

}
