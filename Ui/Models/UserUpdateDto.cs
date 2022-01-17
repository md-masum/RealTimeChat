namespace Ui.Models
{
    public class UserUpdateDto
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? PresentAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? DateOfBirth { get; set; }
        public string? ProfilePicture { get; set; } = "N/A";
        public bool IsActive { get; set; } = true;
    }
}
