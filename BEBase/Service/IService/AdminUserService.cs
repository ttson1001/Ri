using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Entity;
using BEBase.Repository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace BEBase.Service.IService
{
    public class AdminUserService : IAdminUserService
    {
        private readonly IRepo<User> _userRepo;

        public AdminUserService(IRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<List<UserAdminDto>> GetUsersAsync()
        {
            return _userRepo.Get().Include(x => x.Bookings).Include(x => x.Vehicles)
                .Select(u => new UserAdminDto
                {
                    Id = u.Id,
                    Email = u.Email,
                    Name = u.Name,
                    Role = u.Role,
                    Status = u.IsBlocked ? "blocked" : "active",
                    VehicleCount = u.Vehicles.Count,
                    RentalCount = u.Bookings.Count,
                    JoinDate = u.JoinDate,
                    LastActive = u.LastActiveDate
                })
                .ToList();
        }

        public async Task<ApiResponse<object>> ToggleUserStatusAsync(int id)
        {
            var user = await _userRepo.Get().Where(x => x.Id ==id).FirstOrDefaultAsync();
            if (user == null)
                return ApiResponse<object>.Failure("Người dùng không tồn tại");

            user.IsBlocked = !user.IsBlocked;
            await _userRepo.SaveChangesAsync();

            var status = user.IsBlocked ? "khóa" : "mở khóa";
            return ApiResponse<object>.SuccessResponse($"Đã {status} người dùng");
        }

    }

}
