using Microsoft.AspNetCore.Identity;

namespace Core.Entity.Auth
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool IsActive { get; set; }

        public IList<UserImage> UserImages { get; set; }
    }
}
