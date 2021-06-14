using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Shared.DTO;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Services
{
    public class UserService
    {
        private readonly ApplicationDbContext dbContext;
        public UserService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<UserDTO> GetUserAsync(string username)
        {
            
            try
            {
                var query = await dbContext.Users.Include(x => x.Posts).Where(x => x.UserName == username).FirstOrDefaultAsync();
                var user = new UserDTO
                {
                    UserName = query.UserName,
                    ImagePath = query.ImagePath,
                    JoinDate = query.RegistrationDate,
                    Posts = query.Posts.Count()
                };
                return user;
            }
            catch(Exception)
            {
                return null;
            }
        }

        internal async Task<List<UserDTO>> GetUsersAsync(ClaimsPrincipal claim)
        {
            try
            {
                var query = await dbContext.Users.Where(x => x.UserName != claim.Identity.Name).ToListAsync();
                var users = new List<UserDTO>();
                foreach (var q in query)
                {
                    users.Add(new UserDTO
                    {
                        UserName = q.UserName,
                        UserId = q.Id,
                        JoinDate = DateTime.Now,
                        ImagePath = ""
                    });
                }
                return users;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
