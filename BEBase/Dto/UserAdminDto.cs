namespace BEBase.Dto
{
    namespace BEBase.Dto
    {
        public class UserAdminDto
        {
            public int Id { get; set; }
            public string Email { get; set; }
            public string Name { get; set; }
            public string Role { get; set; }         // renter / owner / admin
            public string Status { get; set; }       // active / blocked
            public int VehicleCount { get; set; }
            public int RentalCount { get; set; }
            public DateTime JoinDate { get; set; }
            public DateTime? LastActive { get; set; }
        }
    }

}
