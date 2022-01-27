using Core.Entity.Auth;

namespace Core.Entity
{
    public class ChatMessage : BaseEntity
    {
        public virtual ApplicationUser FromUser { get; set; }
        public string FromUserId { get; set; }

        public virtual ApplicationUser ToUser { get; set; }
        public string ToUserId { get; set; }

        public string Message { get; set; }
        public bool IsRead { get; set; }

        public bool IsDeleteFromUser { get; set; }
        public bool IsDeleteToUser { get; set; }
    }
}
