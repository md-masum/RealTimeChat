using System.ComponentModel.DataAnnotations;
using Core.Entity;
using Core.Interfaces.Common;

namespace Core.Dto
{
    public class ChatMessageRequestDto : IMapFrom<ChatMessage>
    {
        public string Id { get; set; }
        [Required]
        public string ToUserId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
