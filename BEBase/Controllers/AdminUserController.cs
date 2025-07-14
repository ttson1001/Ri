using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;

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
        [EnableQuery(AllowedQueryOptions = AllowedQueryOptions.All, MaxExpansionDepth = 3, MaxTop = 1000)]
        public IQueryable<UserAdminDto> GetUsers()
        {
            return _adminUserService.GetUsersQueryable();
        }

        [HttpGet("simple")]
        public async Task<List<UserAdminDto>> GetUsersSimple()
        {
            return await _adminUserService.GetUsersAsync();
        }

        [HttpGet("count")]
        public async Task<int> GetUsersCount()
        {
            var users = await _adminUserService.GetUsersAsync();
            return users.Count;
        }

        [HttpGet("debug")]
        public async Task<object> GetUsersDebug()
        {
            var users = await _adminUserService.GetUsersAsync();
            return new
            {
                totalCount = users.Count,
                roles = users.GroupBy(u => u.Role).Select(g => new { role = g.Key, count = g.Count() }),
                firstFewUsers = users.Take(5).Select(u => new { u.Id, u.Name, u.Role, u.Status })
            };
        }

        [HttpPut("{id}/toggle-status")]
        public async Task<ApiResponse<object>> ToggleStatus(int id)
        {
            var result = await _adminUserService.ToggleUserStatusAsync(id);
            return result;
        }

    }

}
