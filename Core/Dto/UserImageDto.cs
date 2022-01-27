using Core.Entity.Auth;
using Core.Interfaces.Common;

namespace Core.Dto
{
    public class UserImageDto : IMapFrom<UserImage>
    {
        public string ApplicationUserId { get; set; }
        public string ImagePath { get; set; }
        public string ImageDescription { get; set; }
        public bool IsProfile { get; set; }
    }
}
