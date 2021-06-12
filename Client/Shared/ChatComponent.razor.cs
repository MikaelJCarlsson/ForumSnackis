using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        public int Count { get; set; } = 1;
        [Parameter]
        public EventCallback UpdateContacts { get; set; }
        public List <ChatDTO> ChatRooms { get; set; }
        public ChatDTO CurrentChat { get; set; }
        [CascadingParameter] public Task<AuthenticationState> AuthState { get; set; }
        protected override async Task OnInitializedAsync()
        {
            Users = await GetContacts();
            ChatRooms = await GetChatRooms();
            StateHasChanged();
        }

        private async Task<List<ChatDTO>> GetChatRooms()
        {
            var httpclient = HttpFactory.CreateClient("private");
            var request = await httpclient.GetAsync("api/chat");
            if (request.IsSuccessStatusCode)
            {
                return ChatRooms = await request.Content.ReadFromJsonAsync<List<ChatDTO>>();
            }
            else
                return null;
        }

        private async Task OpenChat(string contactId)
        {
            var httpclient = HttpFactory.CreateClient("private");
            var request = await httpclient.GetAsync($"api/chat/{contactId}");
            if (request.IsSuccessStatusCode)
            {
               CurrentChat = await request.Content.ReadFromJsonAsync<ChatDTO>();
            }
            
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
        public class ChatMessageModel {
            [Required]
            [MinLength(1)]
            [MaxLength(255)]
            public string Message { get; set; } = "";
        }

        public ChatMessageModel MessageModelModel { get; set; } = new();

        private async Task SendMessage()
        {
            var privateHttp = HttpFactory.CreateClient("private");

            var message = new MessageDTO()
            {
                Message = MessageModelModel.Message,
                
            };

            var request = await privateHttp.PostAsJsonAsync($"api/chat/{CurrentChat.id}", message);

            if (request.IsSuccessStatusCode)
            {
                await GetChatRoom(CurrentChat.id);
                MessageModelModel = new ChatMessageModel();
            }
        }

        private async Task GetChatRoom(int currentChatId)
        {
            var privateHttp = HttpFactory.CreateClient("private");

            var request = await privateHttp.GetAsync($"api/chat/room/{currentChatId}");

            if (request.IsSuccessStatusCode)
                CurrentChat = await request.Content.ReadFromJsonAsync<ChatDTO>();
        }
    }
}
