namespace BEBase.Dto
{
    public class UserRegisterDto
    {
        public string Email { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string Role { get; set; } // renter / owner / admin
        public string Address { get; set;}
    }

}
