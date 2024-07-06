using AutoHub.Models;

namespace AutoHub.Repositories.Abstractions
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetByUserId(string userId);
        Task<Vehicle> GetByIdAsync(string id);
        Task AddAsync(Vehicle vehicle);
        Task UpdateAsync(Vehicle vehicle);
        Task DeleteAsync(string id);
    }
}
