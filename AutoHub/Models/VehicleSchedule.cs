namespace AutoHub.Models
{
    public class VehicleSchedule
    {
        public required Guid Id { get; set; }
        public required Guid UserId { get; set; }
        public required Guid VehicleId { get; set; }
        public required Guid SstId { get; set; }
        public required int MileInterval { get; set; }
        public required int TimeInterval { get; set; }
        public required string TimeUnits { get; set; }
    }
}
