namespace DemoCRM.Core.Entity
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string? EventType { get; set; } = null;
        public string? Payload { get; set; } = null;
        public DateTime? CreatedDate { get; set; }
        public DateTime? ProcessedDate { get; set; }
        public bool IsProcessed { get; set; } = false;
        public int RetryCount { get; set; }
        public string? LastError { get; set; }
    }
}
