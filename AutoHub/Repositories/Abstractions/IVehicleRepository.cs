using AutoHub.Models;

namespace AutoHub.Repositories.Abstractions
{
    public interface IVehicleRepository
    {
        Task<IEnumerable<Vehicle>> GetByUserId(Guid userId);
        Task<Vehicle?> GetByIdAsync(Guid id);
        Task<Vehicle> AddAsync(Vehicle vehicle);
        Task<Vehicle> UpdateAsync(Vehicle vehicle);
        Task<Guid?> DeleteAsync(Guid id);
    }
}
