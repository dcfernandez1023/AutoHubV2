using AutoHub.Data;
using AutoHub.Models;
using AutoHub.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IList<Guid>> DeleteManyAsync(IList<Guid> vehicleScheduleIds)
        {
            var recordsToDelete = await _dbSet
                .Where(vs => vehicleScheduleIds.Contains(vs.Id))
                .ToListAsync();

            if (recordsToDelete.Count > 0)
            {
                _dbSet.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();
            }

            return recordsToDelete.Select(vs => vs.Id).ToList();
        }

        public async Task<IList<VehicleSchedule>> UpdateManyAsync(IList<VehicleSchedule> vehicleSchedules)
        {
            IList<VehicleSchedule> updatedVehicleSchedules = new List<VehicleSchedule>();
            foreach (var vehicleSchedule in vehicleSchedules)
            {
                var existingVehicleSchedule = await _dbSet.FindAsync(vehicleSchedule.Id);
                if (existingVehicleSchedule != null)
                {
                    existingVehicleSchedule.TimeInterval = vehicleSchedule.TimeInterval;
                    existingVehicleSchedule.TimeUnits = vehicleSchedule.TimeUnits;
                    existingVehicleSchedule.MileInterval = vehicleSchedule.MileInterval;

                    updatedVehicleSchedules.Add(existingVehicleSchedule);
                }
            }
            await _context.SaveChangesAsync();
            return updatedVehicleSchedules;
        }  

        public async Task DeleteManyAsyncBySstId(Guid sstId)
        {
            var recordsToDelete = await _dbSet
                .Where(vs => vs.SstId == sstId)
                .ToListAsync();

            if (recordsToDelete.Count > 0)
            {
                _dbSet.RemoveRange(recordsToDelete);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IList<VehicleSchedule>> GetBySstIdAsync(Guid sstId)
        {
            return await _dbSet.Where(vs => vs.SstId == sstId).ToListAsync();
        }
    }
}
