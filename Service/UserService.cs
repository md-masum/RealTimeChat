using AutoMapper;
using Core.Dto;
using Core.Entity.Auth;
using Core.Interfaces.Common;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }
        public async Task<UserToReturnDto> GetCurrentUser()
        {
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            return _mapper.Map<UserToReturnDto>(currentUser);
        }

        public async Task<UserToReturnDto> UpdateUser(UserUpdateRequest request)
        {
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            currentUser.DateOfBirth = request.DateOfBirth!;
            currentUser.FirstName = request.FirstName!;
            currentUser.IsActive = request.IsActive!;
            currentUser.LastName = request.LastName!;
            currentUser.PermanentAddress = request.PermanentAddress!;
            currentUser.PresentAddress = request.PresentAddress!;
            currentUser.ProfilePicture = request.ProfilePicture!;
            await _userManager.UpdateAsync(currentUser);
            return _mapper.Map<UserToReturnDto>(currentUser);
        }

        public async Task<UserToReturnDto> GetAllUser()
        {
            throw new NotImplementedException();
        }
    }
}
