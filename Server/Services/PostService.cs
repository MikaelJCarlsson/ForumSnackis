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
        internal async Task<PostDTO> GetAsync(int id)
        {
            try
            {
                var post = await dbContext.Posts.Include(x => x.PostedBy).Where(x => x.Id == id).FirstOrDefaultAsync();
                if(post != null)
                {
                    var postDto = new PostDTO
                    {
                        PostDate = post.PostDate,
                        Content = post.Content,
                        Id = post.Id,
                        PostedBy = post.PostedBy.UserName,
                        SubjectId = post.SubjectId
                        
                     
                    };
                    return postDto;
                }
                return null;
            }
            catch(Exception)
            {
                return null;
            }
           
        }

        internal async Task<int> DeleteAsync(int id)
        {
            try
            {
                var post = await dbContext.Posts.FindAsync(id);
                if(post is not null)
                {
                    await dbContext.Posts.Where(p => p.Quote == post).ForEachAsync(p => p.Quote = null);
                    dbContext.Remove(post);
                    return await dbContext.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        internal async Task<int> UpdateAsync(int id, PostDTO post, ClaimsPrincipal claim)
        {
            try
            {
                var query = await dbContext.Posts.Include(x => x.PostedBy).Where(x => x.Id == id).FirstOrDefaultAsync();
                var cls = claim.Claims;
                var user = claim.Claims.First().Value;
                if (user.Equals(query.PostedBy.Id) || claim.IsInRole("Administrators"))
                {
                    query.Content = post.Content;
                    query.LikeCount = post.LikeCount;
                    query.DislikeCount = post.DislikeCount;
                    dbContext.Update(query);

                    return await dbContext.SaveChangesAsync();
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        internal async Task<List<PostDTO>> GetReportsAsync()
        {
            try
            {
                var post = await dbContext.Reports.Include(x => x.Post).ThenInclude(x => x.PostedBy).ToListAsync();

                List<PostDTO> posts = new();
                if (post is not null)
                {
                    foreach (var p in post)
                    {
                        posts.Add(
                            new PostDTO()
                            {
                                
                                Id = p.Post.Id,
                                Content = p.Post.Content,
                                PostDate = p.Post.PostDate,
                                PostedBy = p.Post.PostedBy.UserName,
                                SubjectId = p.Post.SubjectId,
                            });
                    }
                    return posts;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
