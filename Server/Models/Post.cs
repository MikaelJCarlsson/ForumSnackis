using Microsoft.AspNetCore.Identity;
using System;

namespace ForumSnackis.Server.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string PostedById { get; set; }
        public ApplicationUser PostedBy { get; set; }
        public Post Quote { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
        public Report Report { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
        public string ImagePath { get; set; }

    }
}