using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForumSnackis.Shared.DTO
{
    public class ChatDTO
    {
        public int id { get; set; }
        public string Title { get; set; }
        public List<UserDTO> Users { get; set; }
        public List<MessageDTO> Messages { get; set; }
    }
    public class MessageDTO
    {
        public int Id { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string PostedBy { get; set; }
    } 
}
