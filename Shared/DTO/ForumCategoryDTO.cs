using System.Collections.Generic;
using ForumSnackis.Shared.DTO;

namespace Shared.DTO
{
    public class ForumCategoryDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public List<CategoryDTO> Categories { get; set; }
    }
}