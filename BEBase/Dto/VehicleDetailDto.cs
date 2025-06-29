namespace BEBase.Dto
{
    // Application/Dto/VehicleDetailDto.cs  (như bạn yêu cầu đầy đủ)
    namespace BEBase.Dto
    {
        public class VehicleDetailDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Owner { get; set; }
            public DateTime SubmittedDate { get; set; }
            public string Status { get; set; }
            public string Image { get; set; }
            public string Type { get; set; }
            public string Price { get; set; }
            public int DocumentsCount { get; set; }
            public int ImagesCount { get; set; }
            public int Completeness { get; set; }
            public string Description { get; set; }
            public string Location { get; set; }
            public string Year { get; set; }
            public string Brand { get; set; }
            public string Model { get; set; }
            public string LicensePlate { get; set; }
            public string EngineSize { get; set; }
            public string FuelType { get; set; }
            public string Transmission { get; set; }
            public List<string> Features { get; set; } = [];
            public List<string> Images { get; set; } = [];
            public Dictionary<string, string> Documents { get; set; } = [];
            public OwnerInfoDto OwnerInfo { get; set; }
        }

        public class OwnerInfoDto
        {
            public string Name { get; set; }
            public string Phone { get; set; }
            public string Email { get; set; }
            public string IdCard { get; set; }
            public string Address { get; set; }
            public DateTime JoinDate { get; set; }
            public double Rating { get; set; }
            public int TotalVehicles { get; set; }
        }
    }

}
