namespace AutoHub.Models.RESTAPI
{
    public class ScheduledServiceTypeResponse : ScheduledServiceType
    {
        public required IList<VehicleScheduleResponse> VehicleSchedules { get; set; }
    }
}
