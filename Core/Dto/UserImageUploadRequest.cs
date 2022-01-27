using System.ComponentModel.DataAnnotations;
using Core.Common.CustomValidationAttributes;
using Core.Entity.Auth;
using Core.Interfaces.Common;
using Microsoft.AspNetCore.Http;

namespace Core.Dto
{
    public class UserImageUploadRequest : IMapFrom<UserImage>
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        [MaxFileSize(5)]
        [AllowedExtensions(new[] {".jpg", ".jpeg", ".png", ".gif"})]
        public IFormFile ImageFile { get; set; }
        public string ImageDescription { get; set; }
        public bool IsProfile { get; set; }
    }
}
