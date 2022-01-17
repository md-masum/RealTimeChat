using System.ComponentModel.DataAnnotations;

namespace Ui.Models.Auth.Request
{
    public class ConfirmSignUpRequest
    {
        [Required]
        public string? ConfirmationCode { get; set; }
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }
    }
}
