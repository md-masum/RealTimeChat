using Core.Entity.Auth;
using Core.Interfaces.Common;

namespace Core.Dto
{
    public class UserUpdateRequest : IMapFrom<ApplicationUser>
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsActive { get; set; }
    }
}
