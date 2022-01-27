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
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<List<ConversationToReturnDto>>>> SaveMessageAsync(ChatMessageRequestDto message)
        {
            var response = await _chatService.SaveMessage(message);
            return Ok(new ApiResponse<List<ConversationToReturnDto>>(response));
        }

        [HttpGet("{contactId}")]
        public async Task<ActionResult<ApiResponse<List<ConversationToReturnDto>>>> GetConversationAsync(string contactId)
        {
            var response = await _chatService.GetConversation(contactId);
            return Ok(new ApiResponse<List<ConversationToReturnDto>>(response));
        }

        [HttpPut]
        public async Task<ActionResult<ApiResponse<List<ConversationToReturnDto>>>> UpdateMessageAsync(ChatMessageRequestDto message)
        {
            var response = await _chatService.UpdateMessage(message);
            return Ok(new ApiResponse<List<ConversationToReturnDto>>(response));
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<bool>>> DeleteMessageAsync(string id)
        {
            var response = await _chatService.DeleteMessage(id);
            return Ok(new ApiResponse<bool>(response));
        }
    }
}
