namespace BEBase.Dto
{
    public class UserDto
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AvatarUrl { get; set; }
        public string Phone { get; set; }
        public DateTime JoinDate { get; set; }
        public float? Rating { get; set; }
        public string Role { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime? LastActiveDate { get; set; }
    }
}
