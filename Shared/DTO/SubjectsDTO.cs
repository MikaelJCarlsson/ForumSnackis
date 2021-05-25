using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class SubjectsDTO
    {
        public int CategoryId { get; set; }
        public string Title { get; set; }
        public DateTime TimeStamp { get; set; }
        public string CreatedBy { get; set; }
        public int PostAmount { get; set; }
    }
}
