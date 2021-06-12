using System;

namespace ForumSnackis.Shared.DTO
{
    public class MessageDTO
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public DateTime TimeStamp { get; set; }
        public string Message { get; set; }
        public string PostedBy { get; set; }
    }
}
