using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Services
{
    public class PostService
    {

        private readonly ApplicationDbContext dbContext;

        public PostService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal async Task<int> Report(int id, ClaimsPrincipal user)
        {
            try
            {
                var reportedPost = await dbContext.Posts.Where(x => x.Id == id).FirstOrDefaultAsync();

                if (reportedPost != default)
                {
                    var userId = user.Claims.FirstOrDefault().Value;
                    var reportedByUser = await dbContext.Users.FindAsync(userId);

                    var report = new Report()
                    {
                        ReportedBy = reportedByUser,
                        Post = reportedPost
                    };


                    dbContext.Add(report);
                    return await dbContext.SaveChangesAsync();
                }
                return 0;
            } catch(Exception)
            {
                return 0;
            }
        }
    }
}
