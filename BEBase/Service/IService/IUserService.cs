using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IUserService
    {
        Task<UserDto?> GetByIdAsync(int id);
        Task<ApiResponse<object>> UpdateAsync(int id, UserUpdateDto dto);
    }
    
}
