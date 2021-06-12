using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class ChatDTO
    {
        public int id { get; set; }
        public string OwnerId { get; set; }
        public string Title { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
}
