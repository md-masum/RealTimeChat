using AutoMapper;
using Core.Dto;
using Core.Entity.Auth;
using Core.Exceptions;
using Core.Interfaces.Common;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Service
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IFileUploadService _fileUploadService;
        private readonly IBaseRepository<UserImage> _userImageRepository;
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;

        public UserService(UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IFileUploadService fileUploadService,
            IBaseRepository<UserImage> userImageRepository,
            ApplicationDbContext context,
            IMapper mapper)
        {
            _userManager = userManager;
            _currentUserService = currentUserService;
            _fileUploadService = fileUploadService;
            _userImageRepository = userImageRepository;
            _context = context;
            _mapper = mapper;
        }
        public async Task<UserToReturnDto> GetCurrentUser()
        {
            var currentUser = await _context.Users
                .Include(c => c.UserImages)
                .FirstOrDefaultAsync(c => c.Id == _currentUserService.UserId && c.UserImages.All(d => d.IsDeleted == false));
            return _mapper.Map<UserToReturnDto>(currentUser);
        }

        public async Task<List<UserToReturnDto>> GetAllUser()
        {
            var allUsers = await _context.Users
                .Where(user => user.Id != _currentUserService.UserId && user.IsActive).ToListAsync();
            return _mapper.Map<List<UserToReturnDto>>(allUsers);
        }

        public async Task<UserToReturnDto> UpdateUser(UserUpdateRequest request)
        {
            var currentUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            currentUser.DateOfBirth = request.DateOfBirth;
            currentUser.FirstName = request.FirstName;
            currentUser.IsActive = request.IsActive;
            currentUser.LastName = request.LastName;
            currentUser.PermanentAddress = request.PermanentAddress;
            currentUser.PresentAddress = request.PresentAddress;
            var result = await _userManager.UpdateAsync(currentUser);

            if(result.Succeeded) return _mapper.Map<UserToReturnDto>(currentUser);

            throw new CustomException("Can't update user");
        }

        public async Task<UserImageDto> UploadUserImage(UserImageUploadRequest request)
        {
            if (request.UserId != _currentUserService.UserId) throw new CustomException("Invalid Request");

            var user = await _context.Users.FirstOrDefaultAsync(c => c.Id == request.UserId);
            if (user is null) throw new CustomException("No user found");

            if (request.IsProfile)
            {
                var existingProfileImage = await _userImageRepository.GetAsync(c =>
                    c.ApplicationUserId == _currentUserService.UserId && c.IsProfile == true && c.IsDeleted == false);
                if (existingProfileImage != null)
                {
                    existingProfileImage.IsProfile = false;
                    await _userImageRepository.UpdateAsync(existingProfileImage);
                }
            }

            var imageKey = Guid.NewGuid().ToString();

            var userImage = _mapper.Map<UserImage>(request);
            userImage.ApplicationUser = user;
            userImage.IsDeleted = false;
            userImage.ImagePath = await _fileUploadService.UploadFile(imageKey, request.ImageFile);
            if (await _userImageRepository.AddAsync(userImage))
            {
                return _mapper.Map<UserImageDto>(userImage);
            }

            throw new CustomException("Can't add image");
        }

        public async Task<bool> SetProfilePic(string imageId)
        {
            var currentProfileImage = await _userImageRepository.GetAsync(c =>
                c.ApplicationUserId == _currentUserService.UserId && c.IsProfile == true && c.IsDeleted == false);
            if (currentProfileImage is not null)
            {
                currentProfileImage.IsProfile = false;
                await _userImageRepository.UpdateAsync(currentProfileImage);
            }

            var image = await _userImageRepository.GetAsync(c => c.Id.ToString() == imageId && c.IsDeleted == false);
            if (image is null) throw new NotFoundException("Can't find image");

            image.IsProfile = true;
            if (await _userImageRepository.UpdateAsync(image))
            {
                return true;
            }

            throw new CustomException("Can't set profile picture");
        }

        public async Task<bool> DeleteImage(string imageId)
        {
            var image = await _userImageRepository.GetAsync(c => c.Id.ToString() == imageId && c.IsDeleted == false);
            if (image is not null)
            {
                image.IsDeleted = true;
                await _userImageRepository.UpdateAsync(image);
            }

            throw new NotFoundException("Image not found");
        }

        public async Task<bool> ActivateUser(string id)
        {
            if (id != _currentUserService.UserId)
                throw new CustomException("User Id didn't match");

            var user = await _userManager.FindByIdAsync(id);
            user.IsActive = true;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return true;

            throw new CustomException("Can't activated user");
        }

        public async Task<bool> DeactivateUser(string id)
        {
            if (id != _currentUserService.UserId)
                throw new CustomException("User Id didn't match");

            var user = await _userManager.FindByIdAsync(id);
            user.IsActive = false;
            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded) return true;

            throw new CustomException("Can't deactivated user");
        }

        public async Task<bool> DeleteUser(string id)
        {
            if (id != _currentUserService.UserId)
                throw new CustomException("Invalid User Id");

            var user = await _userManager.FindByIdAsync(_currentUserService.UserId);
            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded) return true;
            throw new CustomException("Can't delete user");
        }
    }
}
