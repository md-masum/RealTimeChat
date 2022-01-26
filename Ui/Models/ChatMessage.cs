namespace Ui.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }
        public string FromUserId { get; set; }
        public string FromUserName { get; set; }
        public string FromUserEmail { get; set; }

        public string ToUserId { get; set; }
        public string ToUserName { get; set; }
        public string ToUserEmail { get; set; }
        public string Message { get; set; }

        public bool IsDeleteFromUser { get; set; }
        public bool IsDeleteToUserUser { get; set; }


        public DateTime CreatedDate { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
