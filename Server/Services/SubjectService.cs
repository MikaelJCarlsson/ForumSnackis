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
using System.Security.Claims;

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

        internal async Task<List<PostDTO>> GetPosts(int id)
        {
            try
            {
                var subject = await dbContext.Subjects.Where(x => x.Id == id).Include(x => x.Posts).FirstOrDefaultAsync();

                List<PostDTO> posts = new();
                if (subject is not null)
                {
                    foreach (Post post in subject.Posts)
                    {
                        posts.Add(
                            new PostDTO
                            {
                                Id = post.Id,
                                Content = post.Content,
                                PostDate = post.PostDate,
                                PostedBy = post.PostedBy.UserName,
                                SubjectId = post.SubjectId,
                                Quote = post.Quote.Content,
                            });
                    }
                    return posts;
                }
                return null;
            } catch(Exception)
            {
                return null;
            }
        }

        public async Task<ApplicationUser> FetchUserId(string userId)
        {
            return await dbContext.Users.Where(x => x.Id.Contains(userId)).FirstOrDefaultAsync();
        }
        public async Task<int> CreateSubject(CreateSubjectCommand csc, ClaimsPrincipal claims)
        {
            var category = await dbContext.ForumCategories
                .Include(x => x.Subjects)
                .Where(x => x.Id == csc.CategoryId)
                .FirstOrDefaultAsync();

            var user = await FetchUserId(claims.Claims.First().Value);

            if(category != null)
            {
               category.Subjects = category.Subjects.ToList();
                var subject = new Subject()
                {
                    SubjectTitle = csc.Title,
                    ForumCategoryId = csc.CategoryId,
                    Posts = new List<Post>(),
                    CreatedBy = user,
                    SubjectDate = DateTime.Now
                    
                };
                category.Subjects.Add(subject);
                dbContext.SaveChanges();

                var post = new Post()
                {
                    SubjectId = subject.Id,
                    Content = csc.FirstPost,
                    PostedBy = user,
                    PostDate = DateTime.Now
                };
                subject.Posts.Add(post);
                
                return await dbContext.SaveChangesAsync();
            }
            return 0;
        }
    }
}
