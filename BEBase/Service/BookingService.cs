namespace BEBase.Service
{
    using BEBase.Dto;
    using BEBase.Entity;
    using BEBase.Repository;
    using BEBase.Service.IService;
    using Microsoft.EntityFrameworkCore;

    public class BookingService : IBookingService
    {
        private readonly IRepo<Booking> _bookingRepo;
        private readonly IRepo<Vehicle> _vehicleRepo;
        private readonly IRepo<Setting> _settingRepo;

        public BookingService(IRepo<Booking> bookingRepo, IRepo<Vehicle> vehicleRepo, IRepo<Setting> settingRepo)
        {
            _bookingRepo = bookingRepo; _vehicleRepo = vehicleRepo;
            _settingRepo = settingRepo;
        }

        public async Task<ApiResponse<List<BookingDto>>> GetBookingsByOwnerAndStatusAsync(int ownerId, string status)
        {
            var query = _bookingRepo.Get()
                .Include(b => b.Vehicle)
                .ThenInclude(x => x.Owner)
                .Include(b => b.Renter)
                .Where(b => b.Vehicle.OwnerId == ownerId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            var bookings = await query.ToListAsync();

            var result = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                VehicleName = b.Vehicle?.Name,
                RenterName = b.Renter?.Name,
                RenterEmail = b.Renter?.Email,
                OwnerName = b.Vehicle?.Owner?.Name,
                OwnerEmail = b.Vehicle?.Owner?.Email,
                OwnerId = b.Vehicle?.Owner?.Id,
                RenterId = b.RenterId,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                PickupTime = b.PickupTime,
                ReturnTime = b.ReturnTime,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            }).ToList();

            return ApiResponse<List<BookingDto>>.SuccessResponse(result);
        }

        public async Task<ApiResponse<object>> CreateBookingAsync(BookingCreateDto dto)
        {
            var vehicle = await _vehicleRepo.Get().FirstOrDefaultAsync(v => v.Id == dto.VehicleId);
            if (vehicle == null)
                return ApiResponse<object>.Failure("Xe không tồn tại");

            var hasConflict = await _bookingRepo.Get()
                .AnyAsync(b => b.VehicleId == dto.VehicleId &&
                               b.Status != "cancelled" &&
                               b.EndDate >= dto.StartDate &&
                               b.StartDate <= dto.EndDate);
            if (hasConflict)
                return ApiResponse<object>.Failure("Xe đã được đặt trong thời gian này");

            var days = (dto.EndDate - dto.StartDate).Days + 1;
            var rentTotal = days * vehicle.PricePerDay;
            var total = rentTotal ;
            var com = decimal.Parse(_settingRepo.Get().First(x => x.Key == "commission").Value);
            var deposit = total * com;
            var booking = new Booking
            {
                VehicleId = dto.VehicleId,
                RenterId = dto.RenterId,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                PickupTime = dto.PickupTime,
                ReturnTime = dto.ReturnTime,
                ServiceFee = dto.ServiceFee,
                InsuranceFee = dto.InsuranceFee,
                DepositAmount = deposit,
                TotalAmount = total,
                Status = "requested"
            };

            await _bookingRepo.AddAsync(booking);
            await _bookingRepo.SaveChangesAsync();

            return ApiResponse<object>.SuccessResponse(booking.Id, "Đặt xe thành công");
        }

        public async Task<ApiResponse<List<BookingDto>>> GetBookingsByRenterAsync(int renterId, string? status)
        {
            var query = _bookingRepo.Get()
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                    .ThenInclude(x => x.Images)
                .Where(b => b.RenterId == renterId);

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            var bookings = await query.ToListAsync();

            var result = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                VehicleName = b.Vehicle?.Name,
                RenterName = b.Renter?.Name,
                RenterEmail = b.Renter?.Email,
                RenterId = b.RenterId,
                OwnerName = b.Vehicle?.Owner?.Name,
                OwnerEmail = b.Vehicle?.Owner?.Email,
                Image = b.Vehicle?.Images[0].Url,
                OwnerId = b.Vehicle?.Owner?.Id,
                StartDate = b.StartDate,
                EndDate = b.EndDate,
                PickupTime = b.PickupTime,
                ReturnTime = b.ReturnTime,
                TotalAmount = b.TotalAmount,
                Status = b.Status
            }).ToList();

            return ApiResponse<List<BookingDto>>.SuccessResponse(result);
        }


        public async Task<ApiResponse<List<BookingDto>>> GetAllBookingsAsync()
        {
            var bookings = await _bookingRepo.Get()
                .Include(b => b.Renter)
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Owner)
                .ToListAsync();

            var result = bookings.Select(b => new BookingDto
            {
                Id = b.Id,
                VehicleName = b.Vehicle?.Name,

                RenterName = b.Renter?.Name,
                RenterEmail = b.Renter?.Email,
                RenterId = b.RenterId,

                OwnerName = b.Vehicle?.Owner?.Name,
                OwnerEmail = b.Vehicle?.Owner?.Email,
                OwnerId = b.Vehicle?.Owner?.Id,

                StartDate = b.StartDate,
                EndDate = b.EndDate,
                PickupTime = b.PickupTime,
                ReturnTime = b.ReturnTime,

                TotalAmount = b.TotalAmount,
                Status = b.Status
            }).ToList();

            return ApiResponse<List<BookingDto>>.SuccessResponse(result);
        }

        public async Task<List<BookingDetailDto>> GetAllBookingDetailsAsync()
        {
            return await _bookingRepo.Get()
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Images)
                .Include(b => b.Renter)
                .Select(booking => new BookingDetailDto
                {
                    Id = booking.Id,
                    Status = booking.Status,
                    Notes = "Xe đã được kiểm tra kỹ thuật. Vui lòng giữ xe sạch sẽ và đổ đầy xăng khi trả.",

                    Vehicle = new BookingDetailDto.VehicleInfo
                    {
                        Id = booking.VehicleId,
                        Name = booking.Vehicle.Name,
                        LicensePlate = booking.Vehicle.LicensePlate,
                        Type = booking.Vehicle.Type,
                        Year = booking.Vehicle.Year,
                        Image = booking.Vehicle.Images.FirstOrDefault().Url ?? "/placeholder.svg"
                    },

                    Owner = new BookingDetailDto.UserInfo
                    {
                        Id = booking.Vehicle.OwnerId,
                        Name = booking.Vehicle.Owner.Name,
                        Phone = booking.Vehicle.Owner.Phone,
                        Avatar = booking.Vehicle.Owner.AvatarUrl ?? "/placeholder.svg",
                        Rating = booking.Vehicle.Owner.Rating ?? 4.5f,
                        Address = "123 Nguyễn Huệ, Quận 1, TP.HCM"
                    },

                    Renter = new BookingDetailDto.UserInfo
                    {
                        Id = booking.RenterId,
                        Name = booking.Renter.Name,
                        Phone = booking.Renter.Phone,
                        Avatar = booking.Renter.AvatarUrl ?? "/placeholder.svg",
                        Rating = booking.Renter.Rating ?? 4.5f,
                        Address = "Quận 5, TP.HCM"
                    },

                    Rental = new BookingDetailDto.RentalInfo
                    {
                        Id = booking.Id,
                        StartDate = booking.StartDate,
                        EndDate = booking.EndDate,
                        PickupTime = booking.PickupTime,
                        ReturnTime = booking.ReturnTime,
                        PickupLocation = "TP.HCM", // placeholder nếu chưa có
                        ReturnLocation = "TP.HCM"  // placeholder nếu chưa có
                    },

                    Pricing = new BookingDetailDto.PricingInfo
                    {
                        DailyRate = booking.Vehicle.PricePerDay,
                        TotalRental = ((booking.EndDate - booking.StartDate).Days) * booking.Vehicle.PricePerDay,
                        ServiceFee = booking.ServiceFee,
                        Insurance = booking.InsuranceFee,
                        Deposit = booking.DepositAmount
                    }
                })
                .ToListAsync();
        }


        public async Task<BookingDetailDto?> GetBookingDetailAsync(int id)
        {
            var booking = await _bookingRepo.Get()
                .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Owner)
                .Include(b => b.Renter)
                 .Include(b => b.Vehicle)
                    .ThenInclude(v => v.Images)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return null;

            return new BookingDetailDto
            {
                Id = booking.Id,
                Status = booking.Status,
                Notes = "Xe đã được kiểm tra kỹ thuật. Vui lòng giữ xe sạch sẽ và đổ đầy xăng khi trả.",

                Vehicle = new BookingDetailDto.VehicleInfo
                {
                    Id = booking.VehicleId,
                    Name = booking.Vehicle.Name,
                    LicensePlate = booking.Vehicle.LicensePlate,
                    Type = booking.Vehicle.Type,
                    Year = booking.Vehicle.Year,
                    Image = booking.Vehicle.Images[0].Url,
                },

                Owner = new BookingDetailDto.UserInfo
                {
                    Id = booking.Vehicle.OwnerId,
                    Name = booking.Vehicle.Owner.Name,
                    Phone = booking.Vehicle.Owner.Phone,
                    Avatar = booking.Vehicle.Owner.AvatarUrl,
                    Rating = booking.Vehicle.Owner.Rating ?? 4.5f,
                    Address = "123 Nguyễn Huệ, Quận 1, TP.HCM"
                },

                Renter = new BookingDetailDto.UserInfo
                {
                    Name = booking.Renter.Name,
                    Id = booking.RenterId,
                    Phone = booking.Renter.Phone,
                    Avatar = booking.Renter.AvatarUrl,
                    Rating = booking.Renter.Rating ?? 4.5f,
                    Address = "Quận 5, TP.HCM"
                },

                Rental = new BookingDetailDto.RentalInfo
                {
                    StartDate = booking.StartDate,
                    EndDate = booking.EndDate
                },

                Pricing = new BookingDetailDto.PricingInfo
                {
                    DailyRate = booking.Vehicle.PricePerDay,
                    TotalRental = ((booking.EndDate - booking.StartDate).Days) * booking.Vehicle.PricePerDay,
                    ServiceFee = booking.ServiceFee,
                    Insurance = booking.InsuranceFee,
                    Deposit = booking.DepositAmount
                }
            };
        }

        public async Task<ApiResponse<string>> UpdateBookingStatusAsync(int bookingId, string newStatus)
        {
            var booking = await _bookingRepo.Get().FirstOrDefaultAsync(b => b.Id == bookingId);
            if (booking == null)
                return ApiResponse<string>.Failure("Booking not found");

            booking.Status = newStatus;
            await _bookingRepo.SaveChangesAsync();

            return ApiResponse<string>.SuccessResponse("Booking status updated");
        }

        public async Task<BookingDto?> GetByIdAsync(int id)
        {
            return await _bookingRepo.Get().Include(x => x.Renter)
                .Where(b => b.Id == id)
                .Select(b => new BookingDto
                {
                    Id = b.Id,
                    VehicleName = b.Vehicle.Name,
                    RenterName = b.Renter.Name,
                    RenterEmail = b.Renter.Email,
                    RenterId = b.RenterId,
                    OwnerId = b.Vehicle.OwnerId,
                    OwnerName = b.Vehicle.Owner.Name,
                    OwnerEmail = b.Vehicle.Owner.Email,
                    StartDate = b.StartDate,
                    EndDate = b.EndDate,
                    PickupTime = b.PickupTime,
                    ReturnTime = b.ReturnTime,
                    TotalAmount = b.TotalAmount,
                    Status = b.Status
                })
                .FirstOrDefaultAsync();
        }

    }

}
