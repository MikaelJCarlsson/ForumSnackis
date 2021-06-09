using System.Collections.Generic;
using ForumSnackis.Shared.DTO;

namespace ForumSnackis.Shared.DTO
{
    public class ForumDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}