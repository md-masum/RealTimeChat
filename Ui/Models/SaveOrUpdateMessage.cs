using System.ComponentModel.DataAnnotations;

namespace Ui.Models
{
    public class SaveOrUpdateMessage
    {
        [Required]
        public string? ToUserId { get; set; }
        [Required]
        public string? Message { get; set; }
    }
}
