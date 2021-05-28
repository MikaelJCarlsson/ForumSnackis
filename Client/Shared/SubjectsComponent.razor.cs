using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using ForumSnackis.Shared.DTO;
using ForumSnackis.Shared;
namespace ForumSnackis.Client.Shared
{
    public partial class SubjectsComponent : ComponentBase
    {
        [Parameter]
        public CategoryDTO Category { get; set; }
        [Parameter]
        public EventCallback<SubjectsDTO> SubjectPosts { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }
        public bool SubjectForm { get; set; }
        public CreateSubjectCommand NewSubject { get; set; } = new();

        private bool ShowInputField;
        private int ShowInputFieldForId;
        public string NewSubjectTitle = "";

        protected override async Task OnParametersSetAsync()
        {
            if (Category is not null)
            {
                var publicHttp = HttpFactory.CreateClient("public");
                var request = await publicHttp.GetAsync($"api/Category/Subjects/{Category.Id}");

                if (request.IsSuccessStatusCode)
                {
                    Category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                }
            }
        }

        public async void OpenSubject(SubjectsDTO subject)
        {
            await SubjectPosts.InvokeAsync(subject);

        }
        public async void CreateSubject()
        {
            if(Category != null && NewSubject != null)
            {         
                NewSubject.CategoryId = Category.Id;

                if (Category.Id > 0)
                {
                    var privateHttp = HttpFactory.CreateClient("private");
                    var result = await privateHttp.PostAsJsonAsync($"api/Subject/",NewSubject);

                    if (result.IsSuccessStatusCode)
                    {
                        var publicHttp = HttpFactory.CreateClient("public");
               
                        var request = await publicHttp.GetAsync($"api/Category/Subjects/{Category.Id}");
                        Category = await request.Content.ReadFromJsonAsync<CategoryDTO>();
                        StateHasChanged();
                    }
                }
            }
        }

        private void ToggleSubjectEditForm(int id)
        {
            if (ShowInputField == true)
            {
                ShowInputFieldForId = 0;
                ShowInputField = false;
            }
            else
            {
                ShowInputFieldForId = id;
                ShowInputField = true;
            }
        }

        public async Task DeleteSubject(SubjectsDTO subj)
        {
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.DeleteAsync($"api/Subject/{subj.Id}");
            if (result.IsSuccessStatusCode)
            {
                ToggleSubjectEditForm(0);
                await OnParametersSetAsync();
            }
        }

        public async Task SubmitEditSubjectTitle(SubjectsDTO subj)
        {
            subj.Title = NewSubjectTitle;
            var privateHttp = HttpFactory.CreateClient("private");
            var result = await privateHttp.PutAsJsonAsync($"api/Subject/{subj.Id}", subj);
            if (result.IsSuccessStatusCode)
            {
                NewSubjectTitle = "";
                ToggleSubjectEditForm(0);
                await OnParametersSetAsync();
            }
        }
    }
}
