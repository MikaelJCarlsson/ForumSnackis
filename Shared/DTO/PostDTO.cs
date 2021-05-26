using System;

namespace ForumSnackis.Shared.DTO
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public DateTime PostDate { get; set; }
        public string PostedBy { get; set; }
        public string Quote { get; set; }
        public int SubjectId { get; set; }
    }
}