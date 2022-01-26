using AutoMapper;
using Core.Dto;
using Core.Entity;
using Core.Exceptions;
using Core.Interfaces.Common;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Repository.Context;
using Service.Hubs;

namespace Service
{
    public class ChatService : IChatService
    {
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub, IChatClient> _chatHub;


        public ChatService(ApplicationDbContext context, 
            ICurrentUserService currentUserService, 
            IMapper mapper,
            IHubContext<ChatHub, IChatClient> chatHub)
        {
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
            _chatHub = chatHub;
        }

        public async Task<List<UserToReturnDto>> GetAllUser()
        {
            var allUsers = await _context.Users.Where(user => user.Id != _currentUserService.UserId).ToListAsync();
            return _mapper.Map<List<UserToReturnDto>>(allUsers);
        }

        public async Task<List<ConversationToReturnDto>> SaveMessage(ChatMessageRequestDto messageDto)
        {
            var message = _mapper.Map<ChatMessage>(messageDto);
            message.FromUserId = _currentUserService.UserId!;
            message.IsDeleteFromUser = false;
            message.IsDeleteToUserUser = false;
            await _context.ChatMessages!.AddAsync(message);
            await _context.SaveChangesAsync();
            await _chatHub.Clients.User(message.ToUserId).ReceiveMessage(_mapper.Map<ConversationToReturnDto>(message));
            await _chatHub.Clients.User(message.ToUserId).ReceiveChatNotification(message.FromUserId);
            return await GetConversation(message.ToUserId!);
        }

        public async Task<List<ConversationToReturnDto>> UpdateMessage(ChatMessageRequestDto messageDto)
        {
            var existingMessage = await _context.ChatMessages!.FirstOrDefaultAsync(c => c.Id == messageDto.Id);
            if (existingMessage is null) throw new NotFoundException("no message found by provided id");

            existingMessage.Message = messageDto.Message;

            _context.ChatMessages?.Update(existingMessage);
            await _context.SaveChangesAsync();
            return await GetConversation(existingMessage.ToUserId!);
        }

        public async Task<List<ConversationToReturnDto>> DeleteMessage(string id)
        {
            throw new NotImplementedException("feature coming soon");
        }

        public async Task<List<ConversationToReturnDto>> GetConversation(string contactId)
        {
            var fromUserId = _currentUserService.UserId;
            var toUserId = contactId;
            var messages = await _context.ChatMessages!
                .Where(c => c.FromUserId == fromUserId && c.ToUserId == toUserId || c.FromUserId == toUserId && c.ToUserId == fromUserId)
                // .OrderByDescending(a => a.CreatedDate)
                // .TakeLast(50)
                .ToListAsync();

            return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
           
        }

        private async Task<List<ConversationToReturnDto>> MapUserToConversation(
            List<ConversationToReturnDto> conversations, string fromUserId, string toUserId)
        {
            var user = await _context.Users.Where(c => c.Id == fromUserId || c.Id == toUserId).ToListAsync();
            var fromUser = user.FirstOrDefault(c => c.Id == fromUserId);
            var toUser = user.FirstOrDefault(c => c.Id == toUserId);

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
            var messages = await _context.ChatMessages!
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
                var messages = await _context.ChatMessages!
                    .Where(h => h.FromUserId == fromUserId && h.ToUserId == toUserId || h.FromUserId == toUserId && h.ToUserId == fromUserId)
                    .OrderByDescending(a => a.CreatedDate)
                    .TakeLast(lastTake).SkipLast(lastSkip)
                    .ToListAsync();

                return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
            }
            else
            {
                var messages = await _context.ChatMessages!
                    .Where(h => h.FromUserId == fromUserId && h.ToUserId == toUserId || h.FromUserId == toUserId && h.ToUserId == fromUserId)
                    .OrderByDescending(a => a.CreatedDate)
                    .TakeLast(lastTake)
                    .ToListAsync();

                return await MapUserToConversation(_mapper.Map<List<ConversationToReturnDto>>(messages), fromUserId!, toUserId);
            }
            
        }
    }
}
