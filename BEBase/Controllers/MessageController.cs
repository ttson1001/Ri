using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/messages")]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;

        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("between/{user1Id}/{user2Id}")]
        public async Task<IActionResult> GetMessagesBetweenUsers(int user1Id, int user2Id)
        {
            var messages = await _messageService.GetMessagesBetweenUsersAsync(user1Id, user2Id);
            return Ok(messages);
        }


        // POST: api/messages
        [HttpPost]
        public async Task<IActionResult> SaveMessage([FromBody] MessageDto dto)
        {
            await _messageService.SaveMessageAsync(dto);
            return Ok();
        }

        [HttpGet("admin/users")]
        public async Task<IActionResult> GetUsersChattedWithAdmin()
        {
            var result = await _messageService.GetUsersChattedWithAdminAsync();
            return Ok(ApiResponse<List<ChatUserSummaryDto>>.SuccessResponse(result));
        }
    }
}
