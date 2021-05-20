using ForumSnackis.Server.Data;
using ForumSnackis.Server.Models;
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

        public async Task<List<string>> GetAsync()
        {
            //await dbContext.ForumCategories.ToListAsync();
            return await dbContext.ForumCategories.Select(x => x.Title).ToListAsync();
                      
        }
        public async Task<ForumCategory> GetAsync(int id)
        {
            return await dbContext.ForumCategories.FindAsync(id);
        }
        public async Task<int> CreateAsync(string title)
        {
            ForumCategory fc = new ForumCategory();
            fc.Title = title;
            dbContext.Add(fc);
            return await dbContext.SaveChangesAsync();
        }
        public async Task<int> UpdateAsync(int id, string title)
        {
            var category = await dbContext.ForumCategories.FindAsync(id);
            if(category is null)
                return 0;

            category.Title = title;

            dbContext.Update(category);
            return await dbContext.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(int id)
        {
            var category = await dbContext.ForumCategories.FindAsync(id);
            if(category is null)
            {
                return 0;
            }
            dbContext.Remove(category);
            return await dbContext.SaveChangesAsync();
        }
    }
}
