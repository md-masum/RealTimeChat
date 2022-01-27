using AutoMapper;
using Core.Dto;
using Core.Entity;
using Core.Entity.Auth;
using Core.Exceptions;
using Core.Interfaces.Common;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Service.Hubs;

namespace Service
{
    public class ChatService : IChatService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IBaseRepository<ChatMessage> _chatMessageRepository;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;


        public ChatService(ICurrentUserService currentUserService, 
            IBaseRepository<ChatMessage> chatMessageRepository,
            UserManager<ApplicationUser> userManager,
            IMapper mapper,
            IHubContext<ChatHub, IChatClient> chatHub)
        {
            _currentUserService = currentUserService;
            _chatMessageRepository = chatMessageRepository;
            _userManager = userManager;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        public async Task<List<ConversationToReturnDto>> SaveMessage(ChatMessageRequestDto messageDto)
        {
            var fromUser = await _userManager.FindByIdAsync(_currentUserService.UserId);
            var toUser = await _userManager.FindByIdAsync(messageDto.ToUserId);

            if (fromUser is null || toUser is null)
                throw new NotFoundException("User not found");

            var message = _mapper.Map<ChatMessage>(messageDto);
            message.FromUserId = _currentUserService.UserId;
            message.IsDeleteFromUser = false;
            message.IsDeleteToUser = false;
            message.IsRead = false;
            message.FromUser = fromUser;
            message.ToUser = toUser;

            if (await _chatMessageRepository.AddAsync(message))
            {
                await _chatHub.Clients.User(message.ToUserId).ReceiveMessage(_mapper.Map<ConversationToReturnDto>(message));
                await _chatHub.Clients.User(message.ToUserId).ReceiveChatNotification(message.FromUser.UserName);
                return await GetConversation(message.ToUserId!);
            }

            throw new CustomException("Can't process message");
        }

        public async Task<List<ConversationToReturnDto>> UpdateMessage(ChatMessageRequestDto messageDto)
        {
            var existingMessage = await _chatMessageRepository.GetByIdAsync(messageDto.Id);
            if (existingMessage is null) throw new NotFoundException("no message found by provided id");

            existingMessage.Message = messageDto.Message;

            if (await _chatMessageRepository.UpdateAsync(existingMessage))
            {
                return await GetConversation(existingMessage.ToUserId!);
            }

            throw new CustomException("Can't update message");
        }

        public async Task<bool> DeleteMessage(string id)
        {
            var message = await _chatMessageRepository.GetByIdAsync(id);
            if (message is null) throw new NotFoundException("Can't found message");

            message.IsDeleteFromUser = true;
            await _chatMessageRepository.UpdateAsync(message);

            if (message.IsDeleteFromUser && message.IsDeleteToUser)
            {
                await _chatMessageRepository.RemoveAsync(message);
            }

            return true;
        }

        public async Task<List<ConversationToReturnDto>> GetConversation(string contactId)
        {
            var fromUserId = _currentUserService.UserId;
            var toUserId = contactId;
            var messages = await _chatMessageRepository.GetAsQueryable()
                .Where(c => c.FromUserId == fromUserId && c.ToUserId == toUserId || c.FromUserId == toUserId && c.ToUserId == fromUserId)
                // .OrderByDescending(a => a.CreatedDate)
                // .TakeLast(50)
                .ToListAsync();

            return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
           
        }

        private async Task<List<ConversationToReturnDto>> MapUserToConversation(
            List<ConversationToReturnDto> conversations, string fromUserId, string toUserId)
        {
            var fromUser = await _userManager.FindByIdAsync(fromUserId);
            var toUser = await _userManager.FindByIdAsync(toUserId);

            foreach (var conversation in conversations)
            {
                conversation.FromUserEmail = fromUser?.Email;
                conversation.FromUserName = fromUser?.UserName;

                conversation.ToUserEmail = toUser?.Email;
                conversation.ToUserName = toUser?.UserName;
            }

            return conversations;
        }

        public async Task<List<ConversationToReturnDto>> GetAllConversation(string contactId)
        {
            var fromUserId = _currentUserService.UserId;
            var toUserId = contactId;
            var messages = await _chatMessageRepository.GetAsQueryable()
                .Where(h => h.FromUserId == fromUserId && h.ToUserId == toUserId || h.FromUserId == toUserId && h.ToUserId == fromUserId)
                .OrderByDescending(a => a.CreatedDate)
                .ToListAsync();

            return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
        }

        public async Task<List<ConversationToReturnDto>> GetAllConversation(string contactId, int batchNumber)
        {
            
            var fromUserId = _currentUserService.UserId;
            var toUserId = contactId;
            var lastTake = 50 * batchNumber;
            var lastSkip = 50 * (batchNumber - 1);

            if (batchNumber >= 2)
            {
                var messages = await _chatMessageRepository.GetAsQueryable()
                    .Where(h => h.FromUserId == fromUserId && h.ToUserId == toUserId || h.FromUserId == toUserId && h.ToUserId == fromUserId)
                    .OrderByDescending(a => a.CreatedDate)
                    .TakeLast(lastTake).SkipLast(lastSkip)
                    .ToListAsync();

                return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
            }
            else
            {
                var messages = await _chatMessageRepository.GetAsQueryable()
                    .Where(h => h.FromUserId == fromUserId && h.ToUserId == toUserId || h.FromUserId == toUserId && h.ToUserId == fromUserId)
                    .OrderByDescending(a => a.CreatedDate)
                    .TakeLast(lastTake)
                    .ToListAsync();

                return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
            }
            
        }
    }
}
