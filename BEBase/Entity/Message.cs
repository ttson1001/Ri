namespace BEBase.Entity
{
    public class Message : IEntity
    {
        public int Id { get; set; }

        public int SenderId { get; set; }
        public User Sender { get; set; }

        public int ReceiverId { get; set; }
        public User Receiver { get; set; }

        public string Text { get; set; }

        public DateTime Timestamp { get; set; }
    }


}
