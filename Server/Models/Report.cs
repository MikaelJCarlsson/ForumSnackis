using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class Report
    {
        public int Id { get; set; }
        public ApplicationUser ReportedBy { get; set; }
        public Post Post { get; set; }
    }
}
