namespace BEBase.Entity
{
    public class Notification
    {
        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public string Type { get; set; }
        public string Content { get; set; }
        public bool Read { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
