using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/admin/statistics")]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("overview")]
        public async Task<ActionResult<ApiResponse<DashboardOverviewDto>>> GetDashboardOverview()
        {
            try
            {
                var overview = await _statisticsService.GetDashboardOverviewAsync();
                return Ok(new ApiResponse<DashboardOverviewDto>
                {
                    Success = true,
                    Data = overview,
                    Message = "Lấy thống kê tổng quan thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<DashboardOverviewDto>
                {
                    Success = false,
                    Message = "Lỗi khi lấy thống kê: " + ex.Message
                });
            }
        }

        [HttpGet("vehicles-count")]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalVehiclesCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalVehiclesCountAsync();
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Data = count,
                    Message = "Lấy tổng số xe thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Message = "Lỗi khi lấy tổng số xe: " + ex.Message
                });
            }
        }

        [HttpGet("users-count")]
        public async Task<ActionResult<ApiResponse<int>>> GetTotalUsersCount()
        {
            try
            {
                var count = await _statisticsService.GetTotalUsersCountAsync();
                return Ok(new ApiResponse<int>
                {
                    Success = true,
                    Data = count,
                    Message = "Lấy tổng số người dùng thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<int>
                {
                    Success = false,
                    Message = "Lỗi khi lấy tổng số người dùng: " + ex.Message
                });
            }
        }

        [HttpGet("total-commission")]
        public async Task<ActionResult<ApiResponse<decimal>>> GetTotalCommission()
        {
            try
            {
                var totalCommission = await _statisticsService.GetTotalCommissionAsync();
                return Ok(new ApiResponse<decimal>
                {
                    Success = true,
                    Data = totalCommission,
                    Message = "Lấy tổng hoa hồng thành công"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<decimal>
                {
                    Success = false,
                    Message = "Lỗi khi lấy tổng hoa hồng: " + ex.Message
                });
            }
        }
    }
}
