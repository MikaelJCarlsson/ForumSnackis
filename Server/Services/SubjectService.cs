using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ForumSnackis.Shared.DTO;
using ForumSnackis.Shared;

namespace ForumSnackis.Server.Services
{
    public class SubjectService
    {
        private readonly ApplicationDbContext dbContext;

        public SubjectService(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }

        public async Task<SubjectsDTO> GetAsync(int id)
        {
            try
            {
                var result = await dbContext.Subjects.Include(x => x.Posts)
                    .Include(x => x.CreatedBy)
                    .Where(x => x.Id == id)
                    .Select(x => new SubjectsDTO 
                    {
                        Title = x.SubjectTitle, 
                        TimeStamp = x.SubjectDate, 
                        CreatedBy = x.CreatedBy.NormalizedUserName, 
                        PostAmount = x.Posts.Count() 
                    })                    
                    .FirstOrDefaultAsync();

                if (result is default(SubjectsDTO))
                    return null;
                else
                    return result;
            }
            catch (ArgumentNullException)
            {
                return null;
            }
        }

        public async Task<int> CreateSubject(CreateSubjectCommand csc)
        {
            var category = await dbContext.ForumCategories
                .Include(x => x.Subjects)
                .Where(x => x.Id == csc.CategoryId)
                .FirstOrDefaultAsync();

            if(category != null)
            {
               category.Subjects = category.Subjects.ToList();
                var subject = new Subject()
                {
                    SubjectTitle = csc.Title,
                    ForumCategoryId = csc.CategoryId,
                    Posts = new List<Post>(),                   
                    
                };
                category.Subjects.Add(subject);
                dbContext.SaveChanges();
                var post = new Post()
                {
                    SubjectId = subject.Id,
                    Content = csc.FirstPost
                };
                subject.Posts.Add(post);
                
                return await dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
