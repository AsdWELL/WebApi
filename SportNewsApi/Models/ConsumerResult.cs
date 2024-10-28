namespace SportNewsWebApi.Models
{
    public class ConsumerResult
    {
        public bool Confirmed { get; set; } = false;

        public int ObjectId { get; set; }

        public int UserId { get; set; }

        public DateTime ConfirmationTime { get; set; }
    }
}
