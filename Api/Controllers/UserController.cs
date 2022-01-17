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

        [HttpPut]
        public async Task<ActionResult<ApiResponse<UserToReturnDto>>> Update(UserUpdateRequest request)
        {
            var response = await _userService.UpdateUser(request);
            return Ok(new ApiResponse<UserToReturnDto>(response));
        }
    }
}
