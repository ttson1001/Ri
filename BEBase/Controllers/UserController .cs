using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<UserDto>>> GetById(int id)
        {
            var user = await _userService.GetByIdAsync(id);
            if (user == null)
                return NotFound(ApiResponse<UserDto>.Failure("User not found"));

            return Ok(ApiResponse<UserDto>.SuccessResponse(user));
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<object>>> Update(int id, [FromBody] UserUpdateDto dto)
        {
            var result = await _userService.UpdateAsync(id, dto);
            if (!result.Success)
                return BadRequest(result);

            return Ok(result);
        }
    }
}
