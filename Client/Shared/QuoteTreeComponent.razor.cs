using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;

namespace ForumSnackis.Client.Shared
{
    public partial class QuoteTreeComponent : ComponentBase
    {
        [Parameter]
        public int PostId { get; set; }
        public List<PostDTO> ChildQuotes { get; set; } = new();
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public bool ShowChildren { get; set; }
        [Parameter]
        public bool Visible { get; set; }

        public PostDTO Quote { get; set; }
        protected override async Task OnParametersSetAsync() {
             await GetPost();
             await GetQuotes(PostId);
         }

        private async Task GetPost() {
            var publicHttp = HttpFactory.CreateClient("public");
            var request = await publicHttp.GetAsync($"api/post/{PostId}");

            if(request.IsSuccessStatusCode)
                Quote = await request.Content.ReadFromJsonAsync<PostDTO>();
        } 
        private async Task GetQuotes(int postId){
            var publicHttp = HttpFactory.CreateClient("public");
            var request = await publicHttp.GetAsync($"api/post/quotes/{postId}");

            if (request.IsSuccessStatusCode)
            {
                ChildQuotes = await request.Content.ReadFromJsonAsync<List<PostDTO>>();
            }
        }
    }
}
