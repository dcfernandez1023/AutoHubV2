using AutoHub.Models.RESTAPI;
using AutoHub.Models;

namespace AutoHub.BizLogic.Abstractions
{
    public interface IScheduledServiceTypeBizLogic
    {
        public Task<IEnumerable<ScheduledServiceTypeResponse>> GetUserScheduledServiceTypesWithVehicleSchedules(Guid userId);
        public Task<ScheduledServiceType> CreateScheduledServiceType(Guid userId, string Name);
        public Task<ScheduledServiceType> UpdateScheduledServiceType(Guid userId, string Name);
        public Task<IEnumerable<VehicleSchedule>> BatchAddVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules);
        public Task<IEnumerable<VehicleSchedule>> BatchUpdateVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules);
        public Task<IEnumerable<VehicleSchedule>> BatchDeleteVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<Guid> vehicleScheduleIds);
        public Task<Guid> DeleteScheduledServiceType(Guid userId, Guid id);
    }
}
