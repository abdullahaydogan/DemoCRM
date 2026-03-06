namespace DemoCRM.Application.Events
{
    public record StudentCreatedEvent
    {
        public int StudentId { get; init; }
        public string Name { get; init; }
        public string Email { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}
