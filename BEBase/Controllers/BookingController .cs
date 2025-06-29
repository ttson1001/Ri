using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/bookings")]
    public class BookingController : ControllerBase
    {
        private readonly IBookingService _bookingService;

        public BookingController(IBookingService bookingService)
        {
            _bookingService = bookingService;
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<ApiResponse<List<BookingDto>>>> GetAllOrByStatus(int ownerId, [FromQuery] string? status)
        {
            var result = await _bookingService.GetBookingsByOwnerAndStatusAsync(ownerId, status);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookingCreateDto dto)
        {
            var result = await _bookingService.CreateBookingAsync(dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookingService.GetAllBookingsAsync();
            return Ok(result);
        }

        [HttpGet("renter/{renterId}")]
        public async Task<IActionResult> GetByRenter(int renterId, [FromQuery] string? status)
        {
            var result = await _bookingService.GetBookingsByRenterAsync(renterId, status);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _bookingService.GetByIdAsync(id);
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("detail/{id}")]
        public async Task<ActionResult<ApiResponse<BookingDetailDto>>> GetBookingDetail(int id)
        {
            var result = await _bookingService.GetBookingDetailAsync(id);
            if (result == null)
                return NotFound(ApiResponse<BookingDetailDto>.Failure("Booking not found"));

            return Ok(ApiResponse<BookingDetailDto>.SuccessResponse(result));
        }

        [HttpPut("status")]
        public async Task<IActionResult> UpdateStatus([FromBody] BookingStatusUpdateDto dto)
        {
            var result = await _bookingService.UpdateBookingStatusAsync(dto.BookingId, dto.NewStatus);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("detail/transactions")]
        public async Task<IActionResult> GetBookingTransactions()
        {
            var data = await _bookingService.GetAllBookingDetailsAsync();
            return Ok(ApiResponse<List<BookingDetailDto>>.SuccessResponse(data));
        }
    }

}
