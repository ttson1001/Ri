using BEBase.Dto;
using Microsoft.AspNetCore.Mvc;
using Net.payOS;
using Net.payOS.Types;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IConfiguration _config;

        public PaymentController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePaymentLink([FromBody] CreatePaymentRequest request)
        {
            var clientId = _config["PayOS:ClientId"];
            var apiKey = _config["PayOS:ApiKey"];
            var checksumKey = _config["PayOS:ChecksumKey"];

            var domain = "http://localhost:5173";

            var payOS = new PayOS(clientId, apiKey, checksumKey);

            var paymentRequest = new PaymentData(
                orderCode: int.Parse(DateTimeOffset.UtcNow.ToString("ffffff")),
                amount: request.Amount,
                description: "Thanh toán đơn đặt xe",
                items: [new("Đặt xe Rideon", 1, request.Amount)],
                returnUrl: domain + "/checkout-success",
                cancelUrl: domain + "/checkout-cancel"
            );

            var response = await payOS.createPaymentLink(paymentRequest);

            return Ok(new { paymentUrl = response.checkoutUrl });
        }
    }
}
