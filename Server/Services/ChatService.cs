using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Shared.DTO;
using Microsoft.EntityFrameworkCore;
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

        internal async Task<ChatDTO> GetChatRoomByIdAsync(string contactId, string userId)
        {
            try
            {
                var user = await dbContext.Users.Include(x => x.ChatRooms).Where(x => x.Id == userId).FirstOrDefaultAsync();
                foreach (var room in user.ChatRooms)
                {
                    if(room.Users.Count == 2)
                    {
                        var userRoom = room.Users.Where(x => x.Id == contactId);
                        if(userRoom != null)
                        {
                            ChatDTO dto = new ChatDTO()
                            {
                                id = room.Id,
                                Title = "bleh",
                                Messages = new(),
                                Users = new()
                            };
                            
                            return room;
                        }
                    }
                    
                }
            }
            catch (Exception)
            {
                return null;
            }            
        }

        internal async Task<int> CreateNewChatRoomAsync(ChatDTO chatRoomDTO)
        {
            var userNames = chatRoomDTO.Users.Select(x => x.UserName).ToList();

            var users = await dbContext.Users.ToListAsync();

            ChatRoom chatRoom = new ChatRoom();
            chatRoom.Users = chatRoom.Users.ToList();
            chatRoom.ChatMessages = chatRoom.ChatMessages.ToList();
            chatRoom.Name = "Lé Chat";


            foreach (var item in users)
            {
                if (userNames.Contains(item.UserName))
                {
                    chatRoom.Users.Add(item);
                }
                return await dbContext.SaveChangesAsync();
            }

            return 0;
        }
    }
}
