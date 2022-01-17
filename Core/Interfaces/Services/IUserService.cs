using Core.Dto;

namespace Core.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserToReturnDto> GetCurrentUser();
        Task<UserToReturnDto> UpdateUser(UserUpdateRequest request);
        Task<UserToReturnDto> GetAllUser();
    }
}
