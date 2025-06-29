using BEBase.Database;
using BEBase.Dto;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Service
{
    public class ViolationReportService : IViolationReportService
    {
        private readonly IRepo<ViolationReport> _violationRepo;

        public ViolationReportService(IRepo<ViolationReport> violationRepo)
        {
            _violationRepo = violationRepo;
        }

        public async Task<ApiResponse<List<ViolationReportDTO>>> GetViolationReportsAsync()
        {
            try
            {
                var reports = await _violationRepo.Get()
                    .Include(v => v.Reporter) 
                    .Include(v => v.Reported)
                    .ToListAsync();

                var reportDTOs = reports.Select(r => new ViolationReportDTO
                {
                    Id = r.Id,
                    ReporterName = r.Reporter.Name,
                    ReportedName = r.Reported.Name,
                    Content = r.Content,
                    Time = r.Time,
                    Status = r.Status,
                    Type = r.Type
                }).ToList();

                return ApiResponse<List<ViolationReportDTO>>.SuccessResponse(reportDTOs, "Lấy báo cáo vi phạm thành công");
            }
            catch (Exception ex)
            {
                return ApiResponse<List<ViolationReportDTO>>.Failure($"Có lỗi xảy ra: {ex.Message}");
            }
        }
        public async Task<ApiResponse<ViolationReportDTO>> GetViolationReportByIdAsync(int id)
        {
            try
            {
                var report = await _violationRepo.Get()
                    .Include(v => v.Reporter)
                    .Include(v => v.Reported)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (report == null)
                {
                    return ApiResponse<ViolationReportDTO>.Failure("Báo cáo vi phạm không tồn tại.");
                }

                var reportDTO = new ViolationReportDTO
                {
                    Id = report.Id,
                    ReporterName = report.Reporter.Name,
                    ReportedName = report.Reported.Name,
                    Content = report.Content,
                    Time = report.Time,
                    Status = report.Status,
                    Type = report.Type
                };

                return ApiResponse<ViolationReportDTO>.SuccessResponse(reportDTO, "Lấy báo cáo vi phạm thành công.");
            }
            catch (Exception ex)
            {
                return ApiResponse<ViolationReportDTO>.Failure($"Có lỗi xảy ra: {ex.Message}");
            }
        }

        public async Task<ApiResponse<string>> MarkResolvedAsync(int id)
        {
            try
            {
                var report = await _violationRepo.Get().Where(x => x.Id == id).FirstOrDefaultAsync();
                if (report != null)
                {
                    report.Status = "resolved";
                    await _violationRepo.SaveChangesAsync();
                    return ApiResponse<string>.SuccessResponse("Báo cáo vi phạm đã được đánh dấu là giải quyết.");
                }

                return ApiResponse<string>.Failure("Báo cáo vi phạm không tồn tại.");
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Failure($"Có lỗi xảy ra: {ex.Message}");
            }
        }
    }
}
