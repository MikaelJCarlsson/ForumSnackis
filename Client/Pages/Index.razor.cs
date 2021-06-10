using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public ForumCategoryModel NewForumCategory { get; set; } = new();
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public bool ShowCreateForumForm { get; set; }
        public List<ForumDTO> ListOfForums { get; private set; }
        private async Task CreateForumCategory()
        {
            var privateHttp = HttpFactory.CreateClient("private");

            var request = await privateHttp.PostAsJsonAsync("api/Forum", new ForumDTO() { Title = NewForumCategory.Title });

            if (request.IsSuccessStatusCode)
            {
                NewForumCategory = new();
                await GetForumCategories();
            }

        }

        public async Task UpdateForum(ForumDTO forum)
        {
            await GetForumCategories();

            // var publicHttp = HttpFactory.CreateClient("public");

            // var request = await publicHttp.GetAsync($"api/Forum/{forum.Id}");

            // if (request.IsSuccessStatusCode)
            // {
            //     var tmp = ListOfForums.Find(f => f.Id == forum.Id);
            //     ListOfForums.Remove(tmp);
            //     tmp = await request.Content.ReadFromJsonAsync<ForumDTO>();
            //     ListOfForums.Add(tmp);
            // }

        }

        protected override async Task OnInitializedAsync()
        {
            if (ListOfForums is null)
                await GetForumCategories();
        }
        private async Task GetForumCategories()
        {
            var publicHttp = HttpFactory.CreateClient("public");

            var request = await publicHttp.GetAsync("api/Forum");

            if (request.IsSuccessStatusCode)
                ListOfForums = await request.Content.ReadFromJsonAsync<List<ForumDTO>>();
        }

        private void ToggleCreateForumForm() =>
            ShowCreateForumForm = ShowCreateForumForm ? false : true;
    }

}
