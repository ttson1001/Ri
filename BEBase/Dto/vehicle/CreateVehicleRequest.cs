namespace BEBase.Dto.vehicle
{
    public class CreateVehicleRequest
    {
        // Vehicle info
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Type { get; set; }
        public int Year { get; set; }
        public string LicensePlate { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
        public string LicenseRequired { get; set; }
        public int MinAge { get; set; }
        public int Quantity { get; set; }
        public int OwnerId { get; set; }
        public List<string> Images { get; set; }
        public string IdCardFrontURL { get; set; }
        public string IdCardBackURL { get; set; }
        public string VehicleRegistrationURL { get; set; }
        public string? AuthorizationURL { get; set; }
    }

}
