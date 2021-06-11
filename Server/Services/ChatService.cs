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
                var users = await dbContext.Users.Where(u => u.Id == contactId || u.Id == userId).ToListAsync();
                var result = await dbContext.ChatRooms.Include(u => u.Users.Where(c => c.Id == contactId).Where(u => u.Id == userId)).Where(r => r.Users.Count == 2).FirstOrDefaultAsync();

                if (result is not null && result.Users.Count >= 2)
                    return null;

                ChatRoom newRoom = new() {Users = users};
                await dbContext.AddAsync(newRoom);
                await dbContext.SaveChangesAsync();
                var userDTOs = CreateUserDTO(users);
                var chatDTO = CreateChatDto(newRoom, userDTOs);

                return chatDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private static ChatDTO CreateChatDto(ChatRoom newRoom, List<UserDTO> userDTOs)
        {
            ChatDTO chatDTO = new()
            {
                id = newRoom.Id,
                Users = userDTOs,
                Messages = new(),
            };
            return chatDTO;
        }

        private static List<UserDTO> CreateUserDTO(IEnumerable<ApplicationUser> users)
        {
            List<UserDTO> userDTOs = new();
            foreach (var user in users)
            {
                userDTOs.Add(new()
                {
                    UserId = user.Id,
                    UserName = user.UserName,
                });
            }

            return userDTOs;
        }

        internal async Task<int> CreateNewChatRoomAsync(ChatDTO chatRoomDTO)
        {
            var userNames = chatRoomDTO.Users.Select(x => x.UserName).ToList();

            var users = await dbContext.Users.ToListAsync();

            var chatRoom = new ChatRoom();
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
