using AutoMapper;
using Core.Dto;
using Core.Entity;
using Core.Entity.Auth;
using Core.Interfaces.Common;
using Core.Interfaces.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Repository.Context;

namespace Service
{
    public class ChatService : IChatService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;

        public ChatService(UserManager<ApplicationUser> userManager, 
            ApplicationDbContext context, 
            ICurrentUserService currentUserService, 
            IMapper mapper)
        {
            _userManager = userManager;
            _context = context;
            _currentUserService = currentUserService;
            _mapper = mapper;
        }

        public async Task<List<UserToReturnDto>> GetAllUser()
        {
            var allUsers = await _context.Users.Where(user => user.Id != _currentUserService.UserId).ToListAsync();
            return _mapper.Map<List<UserToReturnDto>>(allUsers);
        }

        public async Task<bool> SaveMessage(ChatMessageRequestDto messageDto)
        {
            var message = _mapper.Map<ChatMessage>(messageDto);
            message.FromUserId = _currentUserService.UserId!;
            message.CreatedDate = DateTime.Now;
            message.ToUser = (await _context.Users.Where(user => user.Id == message.ToUserId).FirstOrDefaultAsync())!;
            await _context.ChatMessages!.AddAsync(message);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<List<ChatMessage>> GetConversation(string contactId)
        {
            var messages = await _context.ChatMessages!
                .Where(h => (h.FromUserId == contactId && h.ToUserId == _currentUserService.UserId) || (h.FromUserId == _currentUserService.UserId && h.ToUserId == contactId))
                .OrderBy(a => a.CreatedDate)
                .Include(a => a.FromUser)
                .Include(a => a.ToUser)
                .Select(x => new ChatMessage
                {
                    FromUserId = x.FromUserId,
                    Message = x.Message,
                    CreatedDate = x.CreatedDate,
                    Id = x.Id,
                    ToUserId = x.ToUserId,
                    ToUser = x.ToUser,
                    FromUser = x.FromUser
                }).ToListAsync();

            return messages;
        }
    }
}
