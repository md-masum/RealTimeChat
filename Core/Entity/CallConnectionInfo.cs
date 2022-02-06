namespace Core.Entity
{
    public class CallConnectionInfo : BaseEntity
    {
        public string SenderId { get; set; }
        public string ReceiverId { get; set; }
        public string CallKey { get; set; }
        public bool IsClosed { get; set; }

        public string Offer { get; set; }
        public string Candidate { get; set; }
    }
}
