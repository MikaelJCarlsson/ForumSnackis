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
    public partial class SubjectComponent : ComponentBase
    {
        [Parameter]
        public SubjectsDTO Subject { get; set; }
        [Parameter]
        public EventCallback SubjectUpdateEvent { get; set; }
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public int SubjectsDTO { get; private set; }
        public bool SubjectForm { get; set; }
        public CreateSubjectCommand NewSubject { get; set; } = new();

        private bool ShowInputField;
        private int ShowInputFieldForId;
        public string NewSubjectTitle = "";

        private void ToggleSubjectEditForm(int id)
        {
            if (ShowInputField)
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
                await SubjectUpdateEvent.InvokeAsync();
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
                await SubjectUpdateEvent.InvokeAsync();
            }
        }
    }
}
