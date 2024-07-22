namespace AutoHub.Models
{
    public class ScheduledServiceType
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required string Name { get; set; }
        public required long DateCreated { get; set; }
    }
}
