// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ForumSnackis.Server.Data;
using Microsoft.EntityFrameworkCore;
using Shared.DTO;

namespace ForumSnackis.Server.Services
{
    public class ForumCategoryService
    {
        private readonly ApplicationDbContext dbcontext;

        public ForumCategoryService(ApplicationDbContext dbcontext)
        {
            this.dbcontext = dbcontext;
        }

        internal async Task<List<ForumCategoryDTO>> GetAsync()
        {
            throw new NotImplementedException();
        }

        internal Task UpdateAsync(ForumCategoryDTO forumCategory)
        {
            throw new NotImplementedException();
        }

        internal Task CreateAsync(ForumCategoryDTO forumCategory)
        {
            throw new NotImplementedException();
        }

        internal Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}