using BEBase.Dto;
using BEBase.Entity;
using BEBase.Extension;
using BEBase.Repository;
using BEBase.Service.IService;

namespace BEBase.Service
{
    public class AuthService : IAuthService
    {
        private readonly IRepo<User> _userRepo;

        public AuthService(IRepo<User> userRepo)
        {
            _userRepo = userRepo;
        }

        public async Task<ApiResponse<LoginResultDto>> LoginAsync(UserLoginDto dto)
        {
            // Lấy user theo điều kiện
            var user =  _userRepo.Get().Where(x => x.Email == dto.Email).FirstOrDefault();

            if (user == null)
                return ApiResponse<LoginResultDto>.Failure("Tài khoản không tồn tại");

            if (user.HashedPassword != dto.Password)
                return ApiResponse<LoginResultDto>.Failure("Sai mật khẩu");

            var token = JwtTokenGenerator.GenerateToken(user);

            var loginResult = new LoginResultDto
            {
                Token = token,
                UserId = user.Id,
                Role = user.Role
            };

            return ApiResponse<LoginResultDto>.SuccessResponse(loginResult, "Đăng nhập thành công");
        }

        public async Task<ApiResponse<object>> RegisterAsync(UserRegisterDto dto)
        {
            var existingUser = _userRepo.Get().FirstOrDefault(u => u.Email == dto.Email);
            if (existingUser != null)
                return ApiResponse<object>.Failure("Email đã được sử dụng");

            var validRoles = new[] { "renter", "owner", "admin" };
            if (!validRoles.Contains(dto.Role))
                return ApiResponse<object>.Failure("Vai trò không hợp lệ");

            var newUser = new User
            {
                Email = dto.Email,
                Name = dto.Name,
                HashedPassword = dto.Password,
                Role = dto.Role,
                JoinDate = DateTime.UtcNow,
                Address = dto.Address,
                AvatarUrl = "",
                Phone = "",
                Rating = null
            };

            await _userRepo.AddAsync(newUser);
            await _userRepo.SaveChangesAsync();

            return ApiResponse<Object>.SuccessResponse("Tạo tài khoản thành công");
        }
    }
}
