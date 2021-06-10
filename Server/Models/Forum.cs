using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class Forum
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<Category> Categories { get; set; }
    }
}
