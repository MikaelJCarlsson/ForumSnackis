using ForumSnackis.Server.Data;
using ForumSnackis.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Services
{
    public class ChatService
    {

        private readonly ApplicationDbContext dbContext;
        public ChatService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal Task<ChatDTO> GetChatRoomByIdAsync()
        {
            return null;
        }
    }
}
