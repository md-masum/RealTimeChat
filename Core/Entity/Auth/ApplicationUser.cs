using Microsoft.AspNetCore.Identity;

namespace Core.Entity.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public virtual ICollection<ChatMessage> ChatMessagesFromUsers { get; set; } = new HashSet<ChatMessage>();
        public virtual ICollection<ChatMessage> ChatMessagesToUsers { get; set; } = new HashSet<ChatMessage>();

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
    }
}
