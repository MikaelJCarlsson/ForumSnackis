using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Shared
{
    public partial class PostCardComponent : ComponentBase
    {
        [Parameter]
        public PostDTO Post { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            this.StateHasChanged();
        }
    }
}
