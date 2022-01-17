using System.ComponentModel.DataAnnotations;

namespace Ui.Models.Auth.Request
{
    public class ForgotPasswordRequest
    {
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
    }
}
