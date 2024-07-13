using AutoHub.Models;
using AutoHub.Models.RESTAPI;

namespace AutoHub.BizLogic.Abstractions
{
    public interface IVehicleBizLogic
    {
        public Task<IEnumerable<Vehicle>> GetUserVehicles(Guid userId);
        public Task<Vehicle> GetVehicle(Guid userId, Guid vehicleId);
        public Task<Vehicle> CreateVehicle(Guid userId, VehicleRequest vehicleRequest);
        public Task<Vehicle> UpdateVehicle(Guid userId, Guid vehicleId, VehicleRequest vehicleRequest);
        public Task DeleteVehicle(Guid vehicleId);
    }
}
