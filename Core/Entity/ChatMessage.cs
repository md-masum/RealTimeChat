namespace Core.Entity
{
    public class ChatMessage : BaseEntity
    {
        public string? FromUserId { get; set; }
        public string? ToUserId { get; set; }
        public string? Message { get; set; }

        public bool IsDeleteFromUser { get; set; }
        public bool IsDeleteToUserUser { get; set; }
    }
}
