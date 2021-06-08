using System;

namespace ForumSnackis.Shared.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string PostedBy { get; set; }
        public DateTime AccountCreated { get; set; }
        public int PostCount { get; set; }
        public int SubjectId { get; set; }
        public string ImagePath { get; set; }
        public string UserId { get; set; }
        public string? QuoteContent { get; set; }
        public int? QuoteId { get; set; }
        public string? QuotePostedBy { get; set; }
        public int LikeCount { get; set; }
        public int DislikeCount { get; set; }
    }
}