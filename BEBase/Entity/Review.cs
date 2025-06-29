namespace BEBase.Entity
{
    public class Review: IEntity
    {
        public int Id { get; set; }

        public int BookingId { get; set; }
        public Booking Booking { get; set; }

        public int ReviewerId { get; set; }
        public User Reviewer { get; set; }

        public int RevieweeId { get; set; }
        public User Reviewee { get; set; }

        public int Rating { get; set; }
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }

}
