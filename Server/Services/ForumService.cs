using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
using ForumSnackis.Shared.DTO;
using Microsoft.EntityFrameworkCore;

namespace ForumSnackis.Server.Services
{
    public class ForumService
    {
        private readonly ApplicationDbContext dbContext;

        public ForumService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal async Task<int> CreateAsync(ForumDTO forumCategory)
        {
            // var count = dbContext.Forums.Where(forum => forum.Title == forumCategory.Title).Count();
            // if (count == 0)
            // {
                dbContext.Forums.Add(new()
                {
                    Title = forumCategory.Title,
                });
                return await dbContext.SaveChangesAsync();
            // }
        }

        internal async Task<ForumDTO> GetAsync(int id)
        {
            try
            {
                var tmp = await dbContext.Forums.Where(x => x.Id == id)
                    .Include(x => x.Categories)
                    .FirstOrDefaultAsync();

                if (tmp == default)
                    return null;
                return MapForumToDTO(tmp);
            }
            catch (Exception)
            {
                throw;
            }
        }

        internal Task UpdateAsync(ForumDTO forumCategory)
        {
            throw new NotImplementedException();
        }

        internal Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        internal async Task<List<ForumDTO>> GetAsync()
        {
            try
            {
                var listOfForum = await dbContext.Forums.Include(c => c.Categories).ToListAsync();

                return listOfForum.Select(MapForumToDTO).ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        private static ForumDTO MapForumToDTO(Forum forum)
        {
            ForumDTO forumDTO = new()
            {
                Id = forum.Id,
                Title = forum.Title,
                Categories = new()
            };

            foreach (var category in forum.Categories)
            {
                CategoryDTO categoryDTO = new()
                {
                    Id = category.Id,
                    Title = category.Title,
                    ForumCategoryId = category.CategoriesId,
                };
                forumDTO.Categories.Add(categoryDTO);
            }

            return forumDTO;
        }
    }
}
