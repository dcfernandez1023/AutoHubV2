using AutoHub.Models;

namespace AutoHub.Repositories.Abstractions
{
    public interface IVehicleScheduleRepository : IRepositoryBase<VehicleSchedule>
    {
        public async Task<IList<VehicleSchedule>> AddManyAsync(IList<VehicleSchedule> vehicleSchedules);
    }
}
