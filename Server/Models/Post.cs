﻿using Microsoft.AspNetCore.Identity;
using System;

namespace ForumSnackis.Server.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public ApplicationUser PostedBy { get; set; }
        public Post Quote { get; set; }
        public int SubjectId { get; set; }
        public Subject Subject { get; set; }
    }
}