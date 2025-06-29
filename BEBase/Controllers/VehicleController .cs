using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Dto.vehicle;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/vehicles")]
    public class VehicleController : ControllerBase
    {
        private readonly IVehicleService _vehicleService;

        public VehicleController(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        [HttpPut("{vehicleId}/submit")]
        public async Task<IActionResult> Submit(int vehicleId)
        {
            var result = await _vehicleService.SubmitVehicleAsync(vehicleId);
            return Ok(result);
        }

        [HttpGet("owner/{ownerId}")]
        public async Task<ActionResult<ApiResponse<List<VehicleDto>>>> GetByOwner(int ownerId)
        {
            var result = await _vehicleService.GetVehiclesByOwnerAsync(ownerId);
            return result;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<VehicleDto>>> GetDetail(int id)
        {
            var result = await _vehicleService.GetVehicleDetailAsync(id);
            return result;
        }

        [HttpGet("dashboard/owner/{ownerId}")]
        public async Task<ActionResult<ApiResponse<OwnerDashboardDto>>> GetOwnerDashboard(int ownerId)
        {
            var result = await _vehicleService.GetOwnerDashboardAsync(ownerId);
            return result;
        }

        [HttpGet]
        public async Task<ActionResult<ApiResponse<List<VehicleDto>>>> GetAll()
        {
            var result = await _vehicleService.GetAllVehiclesAsync();
            return Ok(result);
        }

        [HttpGet("getallforadmin")]
        public async Task<ActionResult<ApiResponse<List<VehicleDetailDto>>>> GetAllAdmin()
        {
            var result = await _vehicleService.GetAllVehiclesAdminAsync();
            return Ok(result);
        }

        [HttpPut("{id}/approve")]
        public async Task<IActionResult> ApproveVehicle(int id)
        {
            var result = await _vehicleService.ApproveVehicleAsync(id);
            return Ok(result);
        }

        [HttpPut("{id}/reject")]
        public async Task<IActionResult> RejectVehicle(int id)
        {
            var result = await _vehicleService.RejectVehicleAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
        {
            try
            {
                var id = await _vehicleService.CreateVehicleAsync(request);
                return Ok(new { vehicleId = id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "Có lỗi xảy ra", details = ex.Message });
            }
        }

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded.");

            var uploadsPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            if (!Directory.Exists(uploadsPath))
            {
                Directory.CreateDirectory(uploadsPath);
            }

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
            return Ok(new { url });
        }


    }

}
