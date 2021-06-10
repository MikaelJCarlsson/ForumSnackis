using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class CategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int ForumCategoryId { get; set; }
        public List<SubjectsDTO> subjects { get; set; }
    }

}
