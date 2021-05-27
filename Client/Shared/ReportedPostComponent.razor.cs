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
    public partial class ReportedPostComponent : ComponentBase
    {
        private List<PostDTO> ReportedPosts;

        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }

        protected override async Task OnInitializedAsync()
        {
            if (ReportedPosts is null)
            {
                var privateHttp = HttpFactory.CreateClient("private");
                var request = await privateHttp.GetAsync("api/Post");

                if (request.IsSuccessStatusCode)
                {
                    ReportedPosts = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
                }
            }
        }
    }
}
