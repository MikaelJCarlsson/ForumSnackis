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
                Console.WriteLine(user.UserName);
                return user;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
