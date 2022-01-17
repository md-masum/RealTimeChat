using System.ComponentModel.DataAnnotations;

namespace Ui.Models.Auth.Request
{
    public class LoginRequest
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        [MinLength(6)]
        public string? Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
