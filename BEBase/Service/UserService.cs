using BEBase.Dto;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;
using System;

namespace BEBase.Service
{
    public class UserService : IUserService
    {
        private readonly IRepo<User> _userRepo;

        public UserService(IRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            var user = await _userRepo.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null) return null;

            return new UserDto
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                AvatarUrl = user.AvatarUrl,
                Phone = user.Phone,
                JoinDate = user.JoinDate,
                Rating = user.Rating,
                Role = user.Role,
                IsBlocked = user.IsBlocked,
                LastActiveDate = user.LastActiveDate
            };
        }

        public async Task<ApiResponse<object>> UpdateAsync(int id, UserUpdateDto dto)
        {
            var user = await _userRepo.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
            if (user == null)
                return ApiResponse<object>.Failure("Không tìm thấy được user");

            user.Name = dto.Name;
            user.AvatarUrl = dto.AvatarUrl;
            user.Phone = dto.Phone;
            user.LastActiveDate = DateTime.Now;

            await _userRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse("Cập nhật thành công");
        }
    }

}
