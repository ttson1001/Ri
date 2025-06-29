using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IReviewService
    {
        Task<ApiResponse<List<ReviewDto>>> GetAllAsync();
        Task<ApiResponse<ReviewDto>> GetByIdAsync(int id);
        Task<ApiResponse<List<ReviewDto>>> GetByUserIdAsync(int userId);
        Task<ApiResponse<ReviewDto>> CreateAsync(ReviewCreateDto dto);
        Task<ApiResponse<ReviewDto?>> GetByBookingIdAsync(int bookingId);
    }

}
