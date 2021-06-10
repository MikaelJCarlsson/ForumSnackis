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
    public class CategoryService
    {
        private readonly ApplicationDbContext dbContext;

        public CategoryService(ApplicationDbContext DbContext)
        {
            dbContext = DbContext;
        }

        public async Task<List<CategoryDTO>> GetAsync()
        {
            var categories = await dbContext.Categories.Select(x => new { x.Id, x.Title }).ToListAsync();
            var listOfCategories = new List<CategoryDTO>();
            foreach (var c in categories)
            {
                listOfCategories.Add(new CategoryDTO() 
                {
                    Id = c.Id, 
                    Title = c.Title
                });
            }

            return listOfCategories;                             
        }
        public async Task<CategoryDTO> GetAsync(int id)
        {
            try
            {
                var result = await dbContext.Categories.Include(x => x.Subjects).Where(x => x.Id == id)
                    .Select(x => new CategoryDTO
                    {
                        Id = x.Id,
                        Title = x.Title
                    })
                    .FirstOrDefaultAsync();

                if (result is default(CategoryDTO))
                    return null;
                else
                    return result;
            }
            catch(ArgumentNullException)
            {
                return null;
            }

        }
        public async Task<int> CreateAsync(CategoryDTO category)
        {
            Category fc = new Category();
            fc.Title = category.Title;
            fc.CategoriesId = category.ForumCategoryId;
            dbContext.Add(fc);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<CategoryDTO> GetSubjectsInCategory(int id)
        {
            try
            {
                Category category = await dbContext.Categories.Include(x => x.Subjects)
                    .ThenInclude(x => x.CreatedBy)
                    .Include(x => x.Subjects)
                    .ThenInclude(x => x.Posts)
                    .Where(x => x.Id == id)
                    .FirstOrDefaultAsync();

                CategoryDTO result = new();
                result.Id = category.Id;
                result.Title = category.Title;
                result.subjects = new();

                category.Subjects
                    .ForEach(x =>
                    result.subjects.Add(new SubjectsDTO()
                    {
                        Id = x.Id,
                        Title = x.SubjectTitle,
                        CreatedBy = x.CreatedBy.UserName,
                        PostAmount = x.Posts.Count(),
                        TimeStamp = x.SubjectDate
                    }));

                return result;
            } catch(Exception)
            {
                return null;
            }
        }

        public async Task<int> UpdateAsync(int id, string title, List<Subject> subjects = null)
        {
            try
            {
                var category = await dbContext.Categories.Include(x => x.Subjects).Where(x => x.Id == id).FirstOrDefaultAsync();
                if (category is null)
                    return 0;

                category.Title = title;

                if (subjects is not null)
                {
                    category.Subjects = subjects;
                }
                dbContext.Update(category);
                return await dbContext.SaveChangesAsync();
            } catch(Exception)
            {
                return 0;
            }
        }

        public async Task<int> DeleteAsync(int id)
        {
            var category = await dbContext.Categories.FindAsync(id);
            if(category is null)
            {
                return 0;
            }
            dbContext.Remove(category);
            return await dbContext.SaveChangesAsync();
        }
    }
}
