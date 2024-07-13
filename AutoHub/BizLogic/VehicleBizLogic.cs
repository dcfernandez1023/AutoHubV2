using AutoHub.BizLogic.Abstractions;
using AutoHub.Exceptions;
using AutoHub.Models;
using AutoHub.Models.RESTAPI;
using AutoHub.Repositories.Abstractions;

namespace AutoHub.BizLogic
{
    public class VehicleBizLogic : IVehicleBizLogic
    {
        private IVehicleRepository _vehicleRepository; 

        public VehicleBizLogic(IVehicleRepository vehicleRepository)
        {
            _vehicleRepository = vehicleRepository;
        }

        public async Task<Vehicle> CreateVehicle(Guid userId, VehicleRequest vehicleRequest)
        {
            Vehicle vehicle = new Vehicle
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = vehicleRequest.Name,
                Mileage = vehicleRequest.Mileage,
                Year = vehicleRequest.Year, 
                Make = vehicleRequest.Make,
                Model = vehicleRequest.Model,
                LicensePlate = vehicleRequest.LicensePlate,
                Vin = vehicleRequest.Vin,
                Notes = vehicleRequest.Notes,
                DateCreated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds(),
                SharedWith = new List<Guid>(),
                Base64Image = vehicleRequest.Base64Image
            };
            return await _vehicleRepository.AddAsync(vehicle);
        }

        public async Task DeleteVehicle(Guid vehicleId)
        {
            Guid? deletedId = await _vehicleRepository.DeleteAsync(vehicleId);
            if (deletedId == null)
            {
                throw new ResourceNotFoundException();
            }
        }

        public async Task<IEnumerable<Vehicle>> GetUserVehicles(Guid userId)
        {
            return await _vehicleRepository.GetByUserId(userId);
        }

        public async Task<Vehicle> GetVehicle(Guid userId, Guid vehicleId)
        {
            Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (vehicle == null)
            {
                throw new ResourceNotFoundException();
            }
            if (vehicle.UserId != userId)
            {
                throw new ResourceForbiddenException();
            }
            return vehicle;
        }

        public async Task<Vehicle> UpdateVehicle(Guid userId, Guid vehicleId, VehicleRequest vehicleRequest)
        {
            Vehicle? existingVehicle = await _vehicleRepository.GetByIdAsync(vehicleId);
            if (existingVehicle == null)
            {
                throw new ResourceNotFoundException();
            }

            existingVehicle.Name = vehicleRequest.Name;
            existingVehicle.Mileage = vehicleRequest.Mileage;
            existingVehicle.Year = vehicleRequest.Year;
            existingVehicle.Make = vehicleRequest.Make;
            existingVehicle.Model = vehicleRequest.Model;
            existingVehicle.LicensePlate = vehicleRequest.LicensePlate;
            existingVehicle.Vin = vehicleRequest.Vin;
            existingVehicle.Notes = vehicleRequest.Notes;
            existingVehicle.Base64Image = vehicleRequest.Base64Image;

            Vehicle updatedVehicle = await _vehicleRepository.UpdateAsync(existingVehicle);
            return updatedVehicle;
        }
    }
}
