using BEBase.Dto;
using BEBase.Entity;

namespace BEBase.Service.IService
{
    public interface IMessageService
    {
        Task<List<MessageDto>> GetMessagesBetweenUsersAsync(int user1Id, int user2Id);
        Task SaveMessageAsync(MessageDto dto);
        Task<List<ChatUserSummaryDto>> GetUsersChattedWithAdminAsync();
    }

}
