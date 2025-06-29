using BEBase.Dto;

namespace BEBase.Service.IService
{
    public interface IViolationReportService
    {
        Task<ApiResponse<List<ViolationReportDTO>>> GetViolationReportsAsync();
        Task<ApiResponse<ViolationReportDTO>> GetViolationReportByIdAsync(int id);
        Task<ApiResponse<string>> MarkResolvedAsync(int id);
    }
}
