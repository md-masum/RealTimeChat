using System.ComponentModel.DataAnnotations;

namespace Ui.Models.Auth.Request
{
    public class ChangePwdRequest
    {
        [Required]
        [EmailAddress]
        public string? EmailAddress { get; set; }

        [Required]
        public string? CurrentPassword { get; set; }

        [Required]
        [MinLength(6)]
        public string? NewPassword { get; set; }

        [Required]
        [Compare("NewPassword")]
        public string? ConfirmPassword { get; set; }
    }
}
