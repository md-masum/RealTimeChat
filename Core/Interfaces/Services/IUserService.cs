using Core.Dto;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserToReturnDto> GetCurrentUser();
        Task<UserToReturnDto> UpdateUser(UserUpdateRequest request);
        Task<List<UserToReturnDto>> GetAllUser();
        Task<bool> DeactivateUser(string id);
        Task<bool> ActivateUser(string id);
        Task<UserImageDto> UploadUserImage(UserImageUploadRequest request);
        Task<bool> SetProfilePic(string imageId);
        Task<bool> DeleteImage(string imageId);
        Task<bool> DeleteUser(string id);
    }
}
