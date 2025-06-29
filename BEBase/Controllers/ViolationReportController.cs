using BEBase.Dto;
using BEBase.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace BEBase.Controllers
{
    [ApiController]
    [Route("api/violations")]
    public class ViolationReportController : ControllerBase
    {
        private readonly IViolationReportService _violationReportService;

        public ViolationReportController(IViolationReportService violationReportService)
        {
            _violationReportService = violationReportService;
        }

        [HttpGet]
        public async Task<IActionResult> GetViolationReports()
        {
            var response = await _violationReportService.GetViolationReportsAsync();
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetViolationReport(int id)
        {
            var response = await _violationReportService.GetViolationReportByIdAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }

            return NotFound(response);
        }

        [HttpPost("{id}/resolve")]
        public async Task<IActionResult> MarkResolved(int id)
        {
            var response = await _violationReportService.MarkResolvedAsync(id);
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }
    }
}
