namespace BEBase.Dto
{
    public class BookingDetailDto
    {
        public int Id { get; set; }
        public string Status { get; set; }

        public VehicleInfo Vehicle { get; set; }
        public UserInfo Owner { get; set; }
        public UserInfo Renter { get; set; }

        public RentalInfo Rental { get; set; }
        public PricingInfo Pricing { get; set; }

        public string? Notes { get; set; }

        public class VehicleInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string LicensePlate { get; set; }
            public string Type { get; set; }
            public int Year { get; set; }
            public string Image { get; set; } // Placeholder nếu chưa có
        }

        public class UserInfo
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Avatar { get; set; } // Placeholder nếu chưa có
            public float Rating { get; set; } // Nếu chưa có thì tạm = 4.5
            public string Address { get; set; } // Nếu chưa có thì có thể hardcode hoặc bỏ
        }

        public class RentalInfo
        {
            public int Id { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime EndDate { get; set; }
            public TimeSpan PickupTime { get; set; }
            public TimeSpan ReturnTime { get; set; }
            public string PickupLocation { get; set; }
            public string ReturnLocation { get; set; }
            public int TotalDays => (EndDate.Date - StartDate.Date).Days;
        }

        public class PricingInfo
        {
            public decimal DailyRate { get; set; }
            public decimal TotalRental { get; set; }
            public decimal ServiceFee { get; set; }
            public decimal Insurance { get; set; }
            public decimal Total => TotalRental + ServiceFee + Insurance;
            public decimal Deposit { get; set; }
            public string DepositStatus { get; set; } = "paid";
        }
    }


}
