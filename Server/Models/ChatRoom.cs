using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class ChatRoom
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string OwnerId { get; set; }
        public ICollection<ChatMessage> ChatMessages { get; set; }
        public ICollection <ApplicationUser> Users { get; set; }
    }
}
