using System.Security.Claims;
using Core.Dto;
using Core.Entity;
using Core.Interfaces.Services;
using Core.Response;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("users")]
        public async Task<ActionResult<ApiResponse<List<UserToReturnDto>>>> GetUsersAsync()
        {
            var response = await _chatService.GetAllUser();
            return Ok(new ApiResponse<List<UserToReturnDto>>(response));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<bool>>> SaveMessageAsync(ChatMessageRequestDto message)
        {
            var response = await _chatService.SaveMessage(message);
            return Ok(new ApiResponse<bool>(response));
        }

        [HttpGet("{contactId}")]
        public async Task<ActionResult<ApiResponse<List<ChatMessage>>>> GetConversationAsync(string contactId)
        {
            var response = await _chatService.GetConversation(contactId);
            return Ok(new ApiResponse<List<ChatMessage>>(response));
        }
    }
}
