using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ForumSnackis.Server.Models
{
    public class ApplicationUser : IdentityUser
    {
        public Report Report { get; set; }
        public DateTime RegistrationDate { get; set; }
        public List<Post> Posts { get; set; }
        public string ImagePath { get; set; }
        public string UserBio { get; set; }
        public ICollection <ChatRoom> ChatRooms { get; set; }
        public ICollection <ChatMessage> ChatMessages { get; set; }
    }
}
