using AutoHub.Models;
using AutoHub.Data;
using Microsoft.EntityFrameworkCore;
using AutoHub.Repositories.Abstractions;
using static System.Runtime.InteropServices.JavaScript.JSType;
using AutoHub.Models.RESTAPI;

namespace AutoHub.Repositories
{
    public class VehicleRepository : IVehicleRepository
    {
        private readonly AppDbContext _context;

        public VehicleRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Vehicle> AddAsync(Vehicle vehicle)
        {
            _context.Vehicle.Add(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }

        public async Task<Guid?> DeleteAsync(Guid id)
        {
            var vehicle = await _context.Vehicle.FindAsync(id);
            if (vehicle != null)
            {
                _context.Vehicle.Remove(vehicle);
                await _context.SaveChangesAsync();
                return id;
            }
            return null;
        }

        public async Task<IEnumerable<Vehicle>> GetByUserId(Guid userId)
        {
            return await _context.Vehicle.Where(v => v.UserId == userId).ToListAsync();
        }

        public async Task<Vehicle?> GetByIdAsync(Guid id)
        {
            return await _context.Vehicle.FindAsync(id);
        }

        public async Task<Vehicle> UpdateAsync(Vehicle vehicle)
        {
            _context.Vehicle.Update(vehicle);
            await _context.SaveChangesAsync();
            return vehicle;
        }
    }
}
