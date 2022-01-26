namespace Ui.Models
{
    public class UserDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PresentAddress { get; set; }
        public string PermanentAddress { get; set; }
        public string DateOfBirth { get; set; }
        public string ProfilePicture { get; set; }
        public bool IsActive { get; set; }
        public bool IsOnline { get; set; }
    }
}
