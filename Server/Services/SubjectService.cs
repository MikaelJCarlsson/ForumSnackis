using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Services
{
    public class SubjectService
    {
        private readonly ApplicationDbContext dbContext;

        public SubjectService(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }

        public async Task<Subject> GetAsync(int id)
        {
            try
            {
                var result = await dbContext.Subjects.Include(x => x.Posts)
                    .Include(x => x.CreatedBy)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                if (result is default(Subject))
                    return null;
                else
                    return result;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }
    }
}
