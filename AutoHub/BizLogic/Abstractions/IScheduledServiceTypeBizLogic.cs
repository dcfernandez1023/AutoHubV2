using AutoHub.Models.RESTAPI;
using AutoHub.Models;

namespace AutoHub.BizLogic.Abstractions
{
    public interface IScheduledServiceTypeBizLogic
    {
        public Task<IList<ScheduledServiceTypeResponse>> GetUserScheduledServiceTypesWithVehicleSchedules(Guid userId);
        public Task<ScheduledServiceType> CreateScheduledServiceType(Guid userId, string name);
        public Task<ScheduledServiceType> UpdateScheduledServiceType(Guid id, Guid userId, string name);
        public Task<IList<VehicleSchedule>> BatchAddVehicleSchedules(Guid userId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules);
        public Task<IList<VehicleSchedule>> BatchUpdateVehicleSchedules(Guid userId, Guid sstId, IList<VehicleSchedule> vehicleSchedules);
        public Task<IList<Guid>> BatchDeleteVehicleSchedules(Guid userId, Guid sstId, IList<Guid> vehicleScheduleIds);
        public Task DeleteScheduledServiceType(Guid userId, Guid id);
    }
}
