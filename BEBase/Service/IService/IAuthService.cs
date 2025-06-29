using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IAuthService
    {
        Task<ApiResponse<LoginResultDto>> LoginAsync(UserLoginDto dto);
        Task<ApiResponse<object>> RegisterAsync(UserRegisterDto dto);
    }
}
