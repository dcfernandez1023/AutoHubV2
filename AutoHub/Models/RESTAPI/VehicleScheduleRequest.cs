namespace AutoHub.Models.RESTAPI
{
    public class VehicleScheduleRequest
    {
        public Guid? Id { get; set; }
        public Guid VehicleId { get; set; }
        public required int MileInterval { get; set; }
        public required int TimeInterval { get; set; }
        public required string TimeUnits { get; set; }
    }
}
