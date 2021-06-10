using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;

namespace ForumSnackis.Server.Models
{
    public class Subject
    {
        public int Id { get; set; }
        public string SubjectTitle { get; set; }
        public DateTime SubjectDate { get; set; }
        public ApplicationUser CreatedBy { get; set; }
        public Category Category { get; set; }
        public int ForumCategoryId { get; set; }
        public List<Post> Posts { get; set; }
    }
}