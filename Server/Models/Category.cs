using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Subject> Subjects { get; set; }
        public int CategoriesId { get; set; }
        public Forum Categories { get; set; }
    }
}
