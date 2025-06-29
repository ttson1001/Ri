using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IAdminUserService
    {
        Task<List<UserAdminDto>> GetUsersAsync();
        Task<ApiResponse<object>> ToggleUserStatusAsync(int id);

    }

}
