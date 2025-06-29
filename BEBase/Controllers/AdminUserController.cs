using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/admin/users")]
    public class AdminUserController : ControllerBase
    {
        private readonly IAdminUserService _adminUserService;

        public AdminUserController(IAdminUserService adminUserService)
        {
            _adminUserService = adminUserService;
        }

        [HttpGet]
        public async Task<List<UserAdminDto>> GetUsers()
        {
            return await _adminUserService.GetUsersAsync();
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<ApiResponse<object>> ToggleStatus(int id)
        {
            var result = await _adminUserService.ToggleUserStatusAsync(id);
            return result;
        }

    }

}
