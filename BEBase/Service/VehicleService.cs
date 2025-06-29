using BEBase.Dto;
using BEBase.Dto.BEBase.Dto;
using BEBase.Dto.vehicle;
using BEBase.Entity;
using BEBase.Repository;
using BEBase.Service.IService;
using Microsoft.EntityFrameworkCore;

namespace BEBase.Service
{
    public class VehicleService : IVehicleService
    {
        private readonly IRepo<Vehicle> _vehicleRepo;
        private readonly IRepo<VehicleImage> _imageRepo;
        private readonly IRepo<VehicleDocument> _docRepo;
        private readonly IRepo<Booking> _bookingRepo;

        public VehicleService(
            IRepo<Vehicle> vehicleRepo,
            IRepo<VehicleImage> imageRepo,
            IRepo<VehicleDocument> docRepo,
            IRepo<Booking> bookingRepo)
        {
            _vehicleRepo = vehicleRepo;
            _imageRepo = imageRepo;
            _docRepo = docRepo;
            _bookingRepo = bookingRepo;
        }

        public async Task<int> CreateVehicleAsync(int ownerId, VehicleCreateDto dto)
        {
            var vehicle = new Vehicle
            {
                Name = dto.Name,
                Brand = dto.Brand,
                Model = dto.Model,
                Year = dto.Year,
                Type = dto.Type,
                LicensePlate = dto.LicensePlate,
                Description = dto.Description,
                PricePerDay = dto.PricePerDay,
                OwnerId = ownerId,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Status = "draft"
            };

            await _vehicleRepo.AddAsync(vehicle);
            await _vehicleRepo.SaveChangesAsync();
            return vehicle.Id;
        }

        public async Task<ApiResponse<object>> UploadVehicleImagesAsync(int vehicleId, List<IFormFile> files)
        {
            foreach (var file in files)
            {
                var url = await UploadToStorageAsync(file);
                await _imageRepo.AddAsync(new VehicleImage
                {
                    VehicleId = vehicleId,
                    Url = url,
                    IsPrimary = false
                });
            }

            await _imageRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse("Đã lưu ảnh");
        }

        public async Task<ApiResponse<object>> UploadVehicleDocumentAsync(int vehicleId, string type, IFormFile file)
        {
            var url = await UploadToStorageAsync(file);
            await _docRepo.AddAsync(new VehicleDocument
            {
                VehicleId = vehicleId,
                Type = type,
                Url = url
            });

            await _docRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse("Đã lưu giấy tờ");
        }

        public async Task<ApiResponse<object>> SubmitVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepo.Get().Where(x => x.Id == vehicleId).FirstOrDefaultAsync();
            if (vehicle == null) return ApiResponse<object>.Failure("Không tìm thấy xe");

            vehicle.Status = "pending";
            vehicle.UpdatedAt = DateTime.UtcNow;
            await _vehicleRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse("Đã gửi yêu cầu duyệt");
        }

        private async Task<string> UploadToStorageAsync(IFormFile file)
        {
            // Tùy bạn: lưu vào folder local, S3, Firebase, v.v.
            return "/uploads/" + Guid.NewGuid() + Path.GetExtension(file.FileName);
        }

        public async Task<ApiResponse<VehicleDto>> GetVehicleDetailAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepo
                .Get()
                .Include(v => v.Owner)
                .Include(v => v.Images)
                .Include(v => v.Documents)
                .FirstOrDefaultAsync(v => v.Id == vehicleId);

            if (vehicle == null)
                return ApiResponse<VehicleDto>.Failure("Xe không tồn tại");

            var dto = new VehicleDto
            {
                Id = vehicle.Id,
                Name = vehicle.Name,
                Brand = vehicle.Brand,
                Model = vehicle.Model,
                Year = vehicle.Year,
                Type = vehicle.Type,
                LicensePlate = vehicle.LicensePlate,
                Description = vehicle.Description,
                PricePerDay = vehicle.PricePerDay,
                OwnerId = vehicle.OwnerId,
                Status = vehicle.Status,
                CreatedAt = vehicle.CreatedAt,
                OwnerName = vehicle.Owner?.Name,
                OwnerEmail = vehicle.Owner?.Email,
                ImageUrls = vehicle.Images?.Select(i => i.Url).ToList(),
                DocumentUrls = vehicle.Documents?.Select(d => d.Url).ToList()
            };

            return ApiResponse<VehicleDto>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<OwnerDashboardDto>> GetOwnerDashboardAsync(int ownerId)
        {
            var vehicles = await _vehicleRepo
                .Get()
                .Where(v => v.OwnerId == ownerId)
                .ToListAsync();

            var totalVehicles = vehicles.Count;
            var activeVehicles = vehicles.Count(v => v.Status == "approved");

            var now = DateTime.UtcNow;

            var bookings = await _bookingRepo.Get()
                .Include(b => b.Vehicle)
                .Where(b => b.Vehicle.OwnerId == ownerId && b.Status == "completed")
                .ToListAsync();

            var totalRevenue = bookings.Sum(b => b.TotalAmount);
            var monthlyRevenue = bookings
                .Where(b => b.EndDate.Month == now.Month && b.EndDate.Year == now.Year)
                .Sum(b => b.TotalAmount);

            var newRequests = await _bookingRepo.Get()
                .Where(b => b.Vehicle.OwnerId == ownerId && b.Status == "requested")
                .CountAsync();

            var dto = new OwnerDashboardDto
            {
                TotalVehicles = totalVehicles,
                ActiveVehicles = activeVehicles,
                NewRequests = newRequests,
                MonthlyRevenue = monthlyRevenue,
                TotalRevenue = totalRevenue
            };

            return ApiResponse<OwnerDashboardDto>.SuccessResponse(dto);
        }

        public async Task<ApiResponse<List<VehicleDto>>> GetVehiclesByOwnerAsync(int ownerId)
        {
            var vehicles = await _vehicleRepo
                .Get()
                .Where(v => v.OwnerId == ownerId)
                .Include(v => v.Images)
                .Include(v => v.Documents)
                .ToListAsync();

            var result = vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                Name = v.Name,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                Type = v.Type,
                LicensePlate = v.LicensePlate,
                Description = v.Description,
                PricePerDay = v.PricePerDay,
                Status = v.Status,
                CreatedAt = v.CreatedAt,
                ImageUrls = v.Images?.Select(i => i.Url).ToList(),
                DocumentUrls = v.Documents?.Select(d => d.Url).ToList()
            }).ToList();

            return ApiResponse<List<VehicleDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<object>> ApproveVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepo.Get().FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
                return ApiResponse<object>.Failure("Vehicle not found");

            vehicle.Status = "approved";
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _vehicleRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Vehicle approved");
        }

        public async Task<ApiResponse<object>> RejectVehicleAsync(int vehicleId)
        {
            var vehicle = await _vehicleRepo.Get().FirstOrDefaultAsync(v => v.Id == vehicleId);
            if (vehicle == null)
                return ApiResponse<object>.Failure("Vehicle not found");

            vehicle.Status = "rejected";
            vehicle.UpdatedAt = DateTime.UtcNow;

            await _vehicleRepo.SaveChangesAsync();
            return ApiResponse<object>.SuccessResponse(null, "Vehicle rejected");
        }

        public async Task<ApiResponse<List<VehicleDetailDto>>> GetAllVehiclesAdminAsync()
        {
            var vehicles = await _vehicleRepo.Get()
           .Include(v => v.Owner)
           .Include(v => v.Images)
           .Include(v => v.Documents)
           .ToListAsync();

            var result = new List<VehicleDetailDto>();

            foreach (var v in vehicles)
            {
                // Handle null collections safely
                var images = v.Images ?? new List<VehicleImage>();
                var documents = v.Documents ?? new List<VehicleDocument>();
                var owner = v.Owner;

                var dto = new VehicleDetailDto
                {
                    Id = v.Id,
                    Name = v.Name,
                    Owner = owner?.Name ?? "Không rõ",
                    SubmittedDate = v.CreatedAt,
                    Status = v.Status,
                    Image = images.FirstOrDefault()?.Url ?? "",
                    Type = v.Type,
                    Price = $"{v.PricePerDay:N0}đ/ngày",
                    DocumentsCount = documents.Count,
                    ImagesCount = images.Count,
                    Completeness = 95,
                    Description = v.Description,
                    Location = "Chưa cập nhật", // FE tự xử nếu chưa có
                    Year = v.Year.ToString(),
                    Brand = v.Brand,
                    Model = v.Model,
                    LicensePlate = v.LicensePlate,
                    EngineSize = "150cc",       // giả định default nếu chưa có trường
                    FuelType = "Xăng",
                    Transmission = "Tự động",
                    Features = new List<string>(), // nếu chưa có field Features trong Vehicle
                    Images = images.Select(i => i.Url ?? "").ToList(),
                    Documents = documents.ToDictionary(
                        d => d.Type?.ToLower() ?? "unknown",
                        d => d.Url ?? ""
                    ),
                    OwnerInfo = new OwnerInfoDto
                    {
                        Name = owner?.Name ?? "Không rõ",
                        Phone = owner?.Phone ?? "",
                        Email = owner?.Email ?? "",
                        IdCard = "",
                        Address = "",
                        JoinDate = owner?.JoinDate ?? DateTime.MinValue,
                        Rating = owner?.Rating ?? 0,
                        TotalVehicles = owner != null
                            ? await _vehicleRepo.Get().CountAsync(x => x.OwnerId == owner.Id)
                            : 0
                    }
                };

                result.Add(dto);
            }

            return ApiResponse<List<VehicleDetailDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<List<VehicleDto>>> GetAllVehiclesAsync()
        {
            var vehicles = await _vehicleRepo.Get()
                .Include(v => v.Owner)
                .Include(v => v.Images)
                .Include(v => v.Documents)
                .ToListAsync();

            var result = vehicles.Select(v => new VehicleDto
            {
                Id = v.Id,
                Name = v.Name,
                Brand = v.Brand,
                Model = v.Model,
                Year = v.Year,
                Type = v.Type,
                LicensePlate = v.LicensePlate,
                Description = v.Description,
                PricePerDay = v.PricePerDay,
                Status = v.Status,
                CreatedAt = v.CreatedAt,
                OwnerName = v.Owner?.Name,
                OwnerEmail = v.Owner?.Email,
                ImageUrls = v.Images?.Select(i => i.Url).ToList(),
                DocumentUrls = v.Documents?.Select(d => d.Url).ToList()
            }).ToList();

            return ApiResponse<List<VehicleDto>>.SuccessResponse(result);
        }

        public async Task<int> CreateVehicleAsync(CreateVehicleRequest request)
        {
            var vehicle = new Vehicle
            {
                OwnerId = request.OwnerId,
                Name = request.Name,
                Brand = request.Brand,
                Type = request.Type,
                Year = request.Year,
                Model = "",
                LicensePlate = request.LicensePlate,
                Description = request.Description,
                PricePerDay = request.PricePerDay,
                LicenseRequired = request.LicenseRequired,
                MinAge = request.MinAge,
                Quantity = request.Quantity,
                Status = "pending",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                Images = new List<VehicleImage>(),
                Documents = new List<VehicleDocument>()
            };

            // Add Images
            foreach (var url in request.Images)
            {
                vehicle.Images.Add(new VehicleImage
                {
                    Url = url,
                    IsPrimary = false
                });
            }

            // Add Documents
            vehicle.Documents.Add(new VehicleDocument { Type = "idCardFront", Url = request.IdCardFrontURL });
            vehicle.Documents.Add(new VehicleDocument { Type = "idCardBack",  Url = request.IdCardBackURL });
            vehicle.Documents.Add(new VehicleDocument { Type = "registration",  Url = request.VehicleRegistrationURL });

            if (!string.IsNullOrEmpty(request.AuthorizationURL))
            {
                vehicle.Documents.Add(new VehicleDocument { Type = "authorization", Url = request.AuthorizationURL });
            }

            await _vehicleRepo.AddAsync(vehicle);
            await _vehicleRepo.SaveChangesAsync();

            return vehicle.Id;
        }
    }

}
