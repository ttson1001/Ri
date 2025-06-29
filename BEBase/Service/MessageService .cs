using BEBase.Database;
using BEBase.Dto;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Service
{
    public class MessageService : IMessageService
    {
        private readonly IRepo<Message> _repo;

        public MessageService(IRepo<Message> repo)
        {
            _repo = repo;
        }

        public async Task<List<MessageDto>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id)
        {
            var messages = await _repo.Get()
                .Where(m =>
                    (m.SenderId == user1Id && m.ReceiverId == user2Id) ||
                    (m.SenderId == user2Id && m.ReceiverId == user1Id))
                .OrderBy(m => m.Timestamp)
                .ToListAsync();

            return messages.Select(m => new MessageDto
            {
                SenderId = m.SenderId,
                ReceiverId = m.ReceiverId,
                Text = m.Text,
                Timestamp = m.Timestamp
            }).ToList();
        }

        public async Task<List<ChatUserSummaryDto>> GetUsersChattedWithAdminAsync()
        {
            int adminId = 3;

            var messages = await _repo.Get()
                .Include(m => m.Sender)
                .Include(m => m.Receiver)
                .Where(m => m.SenderId == adminId || m.ReceiverId == adminId)
                .ToListAsync();

            var users = messages
                .Select(m => m.SenderId == adminId ? m.Receiver : m.Sender)
                .Where(u => u != null)
                .GroupBy(u => u.Id)
                .Select(g => g.First())
                .Select(u => new ChatUserSummaryDto
                {
                    UserId = u.Id,
                    UserName = u.Name,
                    UserAvatar = u.AvatarUrl ?? "/placeholder.svg",
                    Role = u.Role
                })
                .ToList();

            return users;
        }





        public async Task SaveMessageAsync(MessageDto dto)
        {
            var message = new Message
            {
                SenderId = dto.SenderId,
                ReceiverId = dto.ReceiverId,
                Text = dto.Text,
                Timestamp = DateTime.UtcNow
            };

            await _repo.AddAsync(message);
            await _repo.SaveChangesAsync();
        }
    }
}
