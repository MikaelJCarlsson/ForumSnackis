using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Shared.DTO;
using Microsoft.AspNetCore.Identity;
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
        private readonly UserManager<ApplicationUser> userManager;
        public UserService(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }

        public async Task<UserDTO> GetUserAsync(string username)
        {
            
            try
            {
                var query = await dbContext.Users.Include(x => x.Posts).Where(x => x.UserName == username).FirstOrDefaultAsync();
                var user = new UserDTO
                {
                    UserName = query.UserName,
                    UserId = query.Id,
                    ImagePath = query.ImagePath,
                    JoinDate = query.RegistrationDate,
                    Posts = query.Posts.Count(),
                    UserBio = query?.UserBio                   
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

        internal async Task<int> UpdatePasswordAsync(string id, string password)
        {
            var user = await dbContext.Users.FindAsync(id);

            if(user != null)
            {
                user.PasswordHash = password;
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var passworduser = await userManager.ResetPasswordAsync(user, token, user.PasswordHash);
                if (passworduser.Succeeded)
                {
                    dbContext.Update(user);
                    return await dbContext.SaveChangesAsync();
                }
                return 0;
            }
            return 0;
        }

        internal async Task<int> EditUserBioAsync(string id, string bio)
        {
            var user = await dbContext.Users.FindAsync(id);
            if(user != null)
            {
                user.UserBio = bio;
                dbContext.Update(user);
                return await dbContext.SaveChangesAsync();
            }
            else
            return 0;
        }
    }
}
