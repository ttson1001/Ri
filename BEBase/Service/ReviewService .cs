using BEBase.Dto;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Service
{
    public class ReviewService : IReviewService
    {
        private readonly IRepo<Review> _reviewRepo;

        public ReviewService(IRepo<Review> reviewRepo)
        {
            _reviewRepo = reviewRepo;
        }

        public async Task<ApiResponse<List<ReviewDto>>> GetAllAsync()
        {
            var reviews = await _reviewRepo.Get().ToListAsync();
            var result = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                BookingId = r.BookingId,
                ReviewerId = r.ReviewerId,
                RevieweeId = r.RevieweeId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            return ApiResponse<List<ReviewDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<ReviewDto>> GetByIdAsync(int id)
        {
            var r = await _reviewRepo.Get().FirstOrDefaultAsync(x => x.Id == id);
            if (r == null) return ApiResponse<ReviewDto>.Failure("Review not found");

            return ApiResponse<ReviewDto>.SuccessResponse(new ReviewDto
            {
                Id = r.Id,
                BookingId = r.BookingId,
                ReviewerId = r.ReviewerId,
                RevieweeId = r.RevieweeId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            });
        }

        public async Task<ApiResponse<List<ReviewDto>>> GetByUserIdAsync(int userId)
        {
            var reviews = await _reviewRepo.Get()
                .Where(x => x.RevieweeId == userId)
                .ToListAsync();

            var result = reviews.Select(r => new ReviewDto
            {
                Id = r.Id,
                BookingId = r.BookingId,
                ReviewerId = r.ReviewerId,
                RevieweeId = r.RevieweeId,
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            }).ToList();

            return ApiResponse<List<ReviewDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<ReviewDto?>> GetByBookingIdAsync(int bookingId)
        {
            var review = await _reviewRepo.Get()
                .FirstOrDefaultAsync(r => r.BookingId == bookingId);

            if (review == null)
                return ApiResponse<ReviewDto?>.SuccessResponse(null); // trả null nếu chưa có

            return ApiResponse<ReviewDto?>.SuccessResponse(new ReviewDto
            {
                Id = review.Id,
                BookingId = review.BookingId,
                ReviewerId = review.ReviewerId,
                RevieweeId = review.RevieweeId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            });
        }


        public async Task<ApiResponse<ReviewDto>> CreateAsync(ReviewCreateDto dto)
        {
            var existingReview = await _reviewRepo
                .Get()
                .FirstOrDefaultAsync(r => r.BookingId == dto.BookingId && r.ReviewerId == dto.ReviewerId);

            if (existingReview != null)
            {
                // Nếu đã có, thì cập nhật
                existingReview.Rating = dto.Rating;
                existingReview.Comment = dto.Comment;
                existingReview.CreatedAt = DateTime.UtcNow;

                _reviewRepo.Update(existingReview);
                await _reviewRepo.SaveChangesAsync();

                return ApiResponse<ReviewDto>.SuccessResponse(new ReviewDto
                {
                    Id = existingReview.Id,
                    BookingId = existingReview.BookingId,
                    ReviewerId = existingReview.ReviewerId,
                    RevieweeId = existingReview.RevieweeId,
                    Rating = existingReview.Rating,
                    Comment = existingReview.Comment,
                    CreatedAt = existingReview.CreatedAt
                });
            }

            // Nếu chưa có, thì tạo mới
            var review = new Review
            {
                BookingId = dto.BookingId,
                ReviewerId = dto.ReviewerId,
                RevieweeId = dto.RevieweeId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };

            await _reviewRepo.AddAsync(review);
            await _reviewRepo.SaveChangesAsync();

            return ApiResponse<ReviewDto>.SuccessResponse(new ReviewDto
            {
                Id = review.Id,
                BookingId = review.BookingId,
                ReviewerId = review.ReviewerId,
                RevieweeId = review.RevieweeId,
                Rating = review.Rating,
                Comment = review.Comment,
                CreatedAt = review.CreatedAt
            });
        }

    }

}
