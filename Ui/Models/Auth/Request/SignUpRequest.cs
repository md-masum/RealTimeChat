using System.ComponentModel.DataAnnotations;

namespace Ui.Models.Auth.Request
{
    public class SignUpRequest
    {
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        public string? UserName { get; set; }

        public string? PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [Required]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
    }
}
