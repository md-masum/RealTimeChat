namespace Ui.Models
{
    public class ChatMessage
    {
        public string Id { get; set; }
        public string FromUserId { get; set; }
        public string ToUserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedDate { get; set; }
        public virtual UserDto FromUser { get; set; }
        public virtual UserDto ToUser { get; set; }
    }
}
