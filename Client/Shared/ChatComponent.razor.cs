using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace ForumSnackis.Client.Shared
{
    public partial class ChatComponent : ComponentBase
    {
        [Inject]
        public IHttpClientFactory HttpFactory { get; set; }
        public List<UserDTO> Users { get; set; } = new();
        [Parameter]
        public EventCallback UpdateContacts { get; set; }
        [CascadingParameter]
        private Task<AuthenticationState> authenticationStateTask { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Users = await GetContacts();
            StateHasChanged();
        }

        public async Task<List<UserDTO>> GetContacts()
        {
            var authState = await authenticationStateTask;
            var user = authState.User;
            var httpclient = HttpFactory.CreateClient("private");

            var request = await httpclient.GetAsync($"api/User");
            if (request.IsSuccessStatusCode)
            {
                return await request.Content.ReadFromJsonAsync<List<UserDTO>>();

            }
            return null;
        }
        public class ChatMessage {

            public string Message { get; set; } = "";
        }

        public ChatMessage MessageModel { get; set; } = new();

    }
}
