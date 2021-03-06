using Core.Entity.Auth;
using Core.Interfaces.Common;

namespace Core.Dto
{
    public class UserToReturnDto : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; }

        public IList<UserImageDto> UserImages { get; set; }
    }
}
