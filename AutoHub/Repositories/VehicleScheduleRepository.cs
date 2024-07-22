using AutoHub.Data;
using AutoHub.Models;
using AutoHub.Repositories.Abstractions;

namespace AutoHub.Repositories
{
    public class VehicleScheduleRepository : AbstractRepository<VehicleSchedule>, IVehicleScheduleRepository
    {
        public VehicleScheduleRepository(AppDbContext context) : base(context) { }

        public async Task<IList<VehicleSchedule>> AddManyAsync(IList<VehicleSchedule> vehicleSchedules)
        {
            _dbSet.AddRange(vehicleSchedules);
            await _context.SaveChangesAsync();
            return vehicleSchedules;
        }
    }
}
