using Microsoft.AspNetCore.Mvc;
using BEBase.Dto;
using BEBase.Service.IService;
using BEBase.Extension;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/email")]
    public class EmailController : ControllerBase
    {

        [HttpPost("send")]
        public async Task<IActionResult> SendEmail([FromBody] EmailRequest request)
        {
            var result = await Helper.SendNotificationEmail(request.To, request.Subject, request.HtmlBody);
            if (result)
                return Ok(new { success = true, message = "Email sent successfully" });
            return StatusCode(500, new { success = false, message = "Failed to send email" });
        }
    }
}
