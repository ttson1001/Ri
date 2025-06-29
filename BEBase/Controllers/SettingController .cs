using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SettingController : ControllerBase
    {
        private readonly ISettingService _settingService;

        public SettingController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpGet("{key}")]
        public async Task<IActionResult> Get(string key)
        {
            var value = await _settingService.GetValueAsync(key);
            if (value == null)
                return NotFound();

            return Ok(value);
        }

        [HttpPost]
        public async Task<IActionResult> Set([FromBody] SettingDto dto)
        {
            await _settingService.SetValueAsync(dto.Key, dto.Value);
            return Ok(new { success = true });
        }
    }

    public class SettingDto
    {
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
