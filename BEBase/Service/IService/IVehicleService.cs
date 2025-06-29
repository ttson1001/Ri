using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Dto.vehicle;

namespace BEBase.Service.IService
{
    public interface IVehicleService
    {
        Task<int> CreateVehicleAsync(int ownerId, VehicleCreateDto dto);
        Task<ApiResponse<object>> UploadVehicleImagesAsync(int vehicleId, List<IFormFile> files);
        Task<ApiResponse<object>> UploadVehicleDocumentAsync(int vehicleId, string type, IFormFile file);
        Task<ApiResponse<object>> SubmitVehicleAsync(int vehicleId);

        Task<ApiResponse<VehicleDto>> GetVehicleDetailAsync(int id);
        Task<ApiResponse<List<VehicleDto>>> GetVehiclesByOwnerAsync(int ownerId);
        Task<ApiResponse<OwnerDashboardDto>> GetOwnerDashboardAsync(int ownerId);
        Task<ApiResponse<List<VehicleDto>>> GetAllVehiclesAsync();
        Task<ApiResponse<List<VehicleDetailDto>>> GetAllVehiclesAdminAsync();
        Task<ApiResponse<object>> ApproveVehicleAsync(int vehicleId);
        Task<ApiResponse<object>> RejectVehicleAsync(int vehicleId);
        Task<int> CreateVehicleAsync(CreateVehicleRequest request);
    }

}
