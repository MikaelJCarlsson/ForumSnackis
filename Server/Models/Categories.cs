using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class Categories
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<ForumCategory> ForumCategories { get; set; }
    }
}
