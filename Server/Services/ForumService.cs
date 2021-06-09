using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForumSnackis.Server.Data;
using Shared.DTO;

namespace Server.Services
{
    public class ForumService
    {
        private readonly ApplicationDbContext dbContext;

        public ForumService(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        internal Task CreateAsync(ForumDTO forumCategory)
        {
            throw new NotImplementedException();
        }

        internal Task UpdateAsync(ForumDTO forumCategory)
        {
            throw new NotImplementedException();
        }

        internal Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }

        internal Task<List<ForumDTO>> GetAsync()
        {
            throw new NotImplementedException();
        }
    }
}