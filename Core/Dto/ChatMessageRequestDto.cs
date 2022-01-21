using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Entity;
using Core.Interfaces.Common;

namespace Core.Dto
{
    public class ChatMessageRequestDto : IMapFrom<ChatMessage>
    {
        public string? FromUserId { get; set; }
        [Required]
        public string ToUserId { get; set; }
        [Required]
        public string Message { get; set; }
    }
}
