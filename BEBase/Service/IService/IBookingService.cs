using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IBookingService
    {
        Task<ApiResponse<List<BookingDto>>> GetBookingsByOwnerAndStatusAsync(int ownerId, string status);
        Task<ApiResponse<object>> CreateBookingAsync(BookingCreateDto dto);
        Task<ApiResponse<List<BookingDto>>> GetAllBookingsAsync();
        Task<ApiResponse<List<BookingDto>>> GetBookingsByRenterAsync(int renterId, string? status);
        Task<BookingDto?> GetByIdAsync(int id);
        Task<BookingDetailDto?> GetBookingDetailAsync(int id);
        Task<ApiResponse<string>> UpdateBookingStatusAsync(int bookingId, string newStatus);

        Task<List<BookingDetailDto>> GetAllBookingDetailsAsync();
    }
}
