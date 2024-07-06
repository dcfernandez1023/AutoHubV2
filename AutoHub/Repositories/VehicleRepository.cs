using AutoHub.Models;
using AutoHub.Data;
using Microsoft.EntityFrameworkCore;
using AutoHub.Repositories.Abstractions;

namespace AutoHub.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vehicle vehicle)
        {
            _context.Vehicles.Add(vehicle);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(string id)
        {
            var vehicle = await _context.Vehicles.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicles.Remove(vehicle);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Vehicle>> GetByUserId(string userId)
        {
            return await _context.Vehicles.Where(v => v.UserId == userId).ToListAsync();
        }

        public async Task<Vehicle> GetByIdAsync(string id)
        {
            return await _context.Vehicles.FindAsync(id);
        }

        public async Task UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicles.Update(vehicle);
            await _context.SaveChangesAsync();
        }
    }
}
