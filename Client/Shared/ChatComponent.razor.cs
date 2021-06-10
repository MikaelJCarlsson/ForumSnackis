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
    public partial class ChatComponent : ComponentBase
    {
        public class ChatMessage {

            public string Message { get; set; } = "";
        }

        public ChatMessage MessageModel { get; set; } = new();
        private void subas() {

        }
    }
}
