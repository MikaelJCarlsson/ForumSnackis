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
    public partial class PostCardComponent : ComponentBase
    {
        [Parameter]
        public PostDTO Post { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            this.StateHasChanged();
        }

        private async Task ReportPost(int id)
        {
            if(Post is not null) { 
                var privateHttp = HttpFactory.CreateClient("private");
                var result = await privateHttp.PostAsJsonAsync($"api/Post/Report/", id);

                if (result.IsSuccessStatusCode)
                { 
                    return;
                }
            return;
            }
        }
    }
}
