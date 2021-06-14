using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.Web.CodeGeneration.Contracts.Messaging;

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
                var users = await dbContext.Users.Where(u => u.Id == contactId || u.Id == userId).Include((r => r.ChatRooms)).ThenInclude(x => x.Users).ToListAsync();
                var commonRooms = users.First().ChatRooms.Intersect(users.Last().ChatRooms);
                var roomsWithTwo = commonRooms.FirstOrDefault(room => room.Users.Count == 2);
                
                if (roomsWithTwo != default)
                {
                    return await CreateChatDto(roomsWithTwo);
                }

                ChatRoom newRoom = new() {Users = users, OwnerId = userId};
                await dbContext.AddAsync(newRoom);
                await dbContext.SaveChangesAsync();
                
                var chatDTO = await CreateChatDto(newRoom);

                return chatDTO;
            }
            catch (Exception)
            {
                return null;
            }
        }

        internal async Task<List<ChatDTO>> GetAllRoomsAsync(string userId)
        {
            var rooms = await dbContext.ChatRooms.Include(x => x.Users).ToListAsync();
            var user = await dbContext.Users.Include(x => x.ChatRooms).Where(x => x.Id == userId).FirstOrDefaultAsync();
            
            List<ChatDTO> chatrooms = new();
            foreach (var room in rooms)
            {
                if (room.Users.Contains(user))
                {
                    chatrooms.Add(new ChatDTO
                    {
                        id = room.Id,
                        Title = room.Id.ToString(),
                        Messages = CreateChatMessageDTO(room),
                        Users = CreateUserDTO(room.Users)

                    });
                }
            }
            return chatrooms; 
        }

        internal async Task<bool> AddUserToChatRoom(int roomId, string userIdToAdd, string userId)
        {
            var room = await dbContext.ChatRooms.Where(r => r.Id == roomId).Include(x => x.Users).FirstOrDefaultAsync();
            var userIdsInRoom = room.Users.Select(u => u.Id);

            if (userIdsInRoom.Contains(userId) && !userIdsInRoom.Contains(userIdToAdd))
            {
                var user = await dbContext.Users.FindAsync(userIdToAdd);
                if(user is null)
                    return false;

                room.Users.Add(user);
                dbContext.Update(room);
                return await dbContext.SaveChangesAsync() > 0;
            }

            return false;
        }

        internal async Task<bool> RemoveUserFromChatRoom(int roomId, string userIdToRemove, string userId)
        {
            var room = await dbContext.ChatRooms.Include(x => x.Users).Where(x => x.Id == roomId).FirstOrDefaultAsync();
            var userIdsInRoom = room.Users.Select(x => x.Id);

            if ( room.Users.Count <= 2)
                return false;

            if (userIdsInRoom.Contains(userIdToRemove) && userId == room.OwnerId)
            {
                var userToRemove = room.Users.Where(x => x.Id == userIdToRemove).FirstOrDefault();
                room.Users.Remove(userToRemove);
                dbContext.Update(room);
                return await dbContext.SaveChangesAsync() > 0;
            }
            return false;
        }

        private static Task<ChatDTO> CreateChatDto(ChatRoom room)
        {
            ChatDTO chatDTO = new()
            {
                id = room.Id,
                OwnerId = room.OwnerId,
                Users = CreateUserDTO(room.Users),
                Messages = CreateChatMessageDTO(room),
            };
            return Task.FromResult(chatDTO);
        }

        private static List<MessageDTO> CreateChatMessageDTO(ChatRoom room)
        {
            if(room.ChatMessages is null)
                return new List<MessageDTO>();
            List<MessageDTO> messageDtos = new List<MessageDTO>();
            foreach (var message in room.ChatMessages)
            {
                messageDtos.Add(new MessageDTO()
                {
                    Message = message.Message,
                    TimeStamp = message.TimeStamp,
                    PostedBy = message.PostedBy,
                    UserId = message.ApplicationUserId
                });
            }
            return messageDtos;
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

        public async Task<bool> CreateMessage(int roomId, string userId, MessageDTO messageDTO)
        {
            var room = await dbContext.ChatRooms.Include(m => m.ChatMessages).Include(u => u.Users).Where(r => r.Id == roomId).FirstOrDefaultAsync();

            if (room.Users is null)
                return false;
            var user = room.Users?.Where(u => u.Id == userId).FirstOrDefault();

            if (user == default)
                return false;
            
            var message = new ChatMessage()
                {
                    Message = messageDTO.Message,
                    PostedBy = user.UserName,
                    ApplicationUser = user,
                    TimeStamp = DateTime.Now,
                    ChatRoom = room
                };

            await dbContext.AddAsync(message);
            return await dbContext.SaveChangesAsync() > 0;
        }

        public async Task<ChatDTO> GetChatRoom(int roomId, string userId)
        {
            // var room = await dbContext.ChatRooms.Include(u => u.Users.Where(u => u.Id == userId))
            //     .Where(r => r.Id == roomId).Include(m => m.ChatMessages).FirstOrDefaultAsync();

            var room = await dbContext.ChatRooms.Where(r => r.Id == roomId)
                                                .Include(m => m.ChatMessages)
                                                .Include(u => u.Users)
                                                .Where(x => x.Users.Select(u => u.Id).Contains(userId))
                                                .FirstOrDefaultAsync();
            
            if (room != default)
                return await CreateChatDto(room);
            else
                return null;
        }
    }
}
