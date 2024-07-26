using AutoHub.Models;

namespace AutoHub.Repositories.Abstractions
{
    public interface IVehicleScheduleRepository : IRepositoryBase<VehicleSchedule>
    {
        public Task<IList<VehicleSchedule>> AddManyAsync(IList<VehicleSchedule> vehicleSchedules);
        public Task<IList<Guid>> DeleteManyAsync(IList<Guid> vehicleScheduleIds);
        public Task<IList<VehicleSchedule>> UpdateManyAsync(IList<VehicleSchedule> vehicleSchedules);
        public Task DeleteManyAsyncBySstId (Guid sstId);
        public Task<IList<VehicleSchedule>> GetBySstIdAsync(Guid sstId);
    }
}
