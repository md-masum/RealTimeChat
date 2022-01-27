using Core.Dto;
using Core.Interfaces.Services;
using Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<UserToReturnDto>>> Get()
        {
            var response = await _userService.GetCurrentUser();
            return Ok(new ApiResponse<UserToReturnDto>(response));
        }

        [HttpGet("Users")]
        public async Task<ActionResult<ApiResponse<IList<UserToReturnDto>>>> GetAllUser()
        {
            var response = await _userService.GetAllUser();
            return Ok(new ApiResponse<IList<UserToReturnDto>>(response));
        }

        [HttpGet("ActivateUser")]
        public async Task<ActionResult<ApiResponse<bool>>> ActivateUser(string userId)
        {
            var response = await _userService.ActivateUser(userId);
            return Ok(new ApiResponse<bool>(response));
        }

        [HttpGet("DeactivateUser")]
        public async Task<ActionResult<ApiResponse<bool>>> DeactivateUser(string userId)
        {
            var response = await _userService.DeactivateUser(userId);
            return Ok(new ApiResponse<bool>(response));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<UserToReturnDto>>> Update(UserUpdateRequest request)
        {
            var response = await _userService.UpdateUser(request);
            return Ok(new ApiResponse<UserToReturnDto>(response));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteUser(string id)
        {
            var response = await _userService.DeleteUser(id);
            return Ok(new ApiResponse<bool>(response));
        }

        [HttpPost("UploadImage")]
        public async Task<ActionResult<ApiResponse<UserImageDto>>> UploadImage([FromForm] UserImageUploadRequest request)
        {
            var response = await _userService.UploadUserImage(request);
            return Ok(new ApiResponse<UserImageDto>(response));
        }

        [HttpGet("SetProfilePic")]
        public async Task<ActionResult<ApiResponse<bool>>> SetProfilePic(string imageId)
        {
            var response = await _userService.SetProfilePic(imageId);
            return Ok(new ApiResponse<bool>(response));
        }

        [HttpDelete("DeleteImage")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteImage(string imageId)
        {
            var response = await _userService.DeleteImage(imageId);
            return Ok(new ApiResponse<bool>(response));
        }
    }
}
