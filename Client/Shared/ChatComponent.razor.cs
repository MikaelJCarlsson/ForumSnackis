﻿using ForumSnackis.Shared.DTO;
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
        public ChatDTO CurrentChat { get; set; }

        protected override async Task OnInitializedAsync()
        {
            Users = await GetContacts();
            StateHasChanged();
        }
        private async Task OpenChat(string contactId)
        {
            var httpclient = HttpFactory.CreateClient("private");
            var request = await httpclient.GetAsync($"api/Chat/{contactId}");
            if (request.IsSuccessStatusCode)
            {
               CurrentChat = await request.Content.ReadFromJsonAsync<ChatDTO>();

            }
            
        }
        private async Task CreateChat()
        {   
            
            var httpclient = HttpFactory.CreateClient("private");
            
        }
        public async Task<List<UserDTO>> GetContacts()
        {
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