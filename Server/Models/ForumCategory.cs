using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class ForumCategory
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Subject> Subjects { get; set; }
        public List<ForumCategory> ForumSubCategories { get; set; }
    }
}
