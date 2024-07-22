using AutoHub.BizLogic.Abstractions;
using AutoHub.Models;
using AutoHub.Models.RESTAPI;
using AutoHub.Repositories.Abstractions;
using AutoHub.Exceptions;

namespace AutoHub.BizLogic
{
    public class ScheduledServiceBizLogic : IScheduledServiceTypeBizLogic
    {
        public static readonly HashSet<string> TIME_UNITS = new HashSet<string> { "day", "week", "month", "year" };

        private IVehicleScheduleRepository _vehicleScheduleRepository;
        private IScheduledServiceTypeRepository _scheduledServiceTypeRepository;

        public ScheduledServiceBizLogic(IVehicleScheduleRepository vehicleScheduleRepository, IScheduledServiceTypeRepository scheduledServiceTypeRepository)
        {
            _vehicleScheduleRepository = vehicleScheduleRepository;
            _scheduledServiceTypeRepository = scheduledServiceTypeRepository;
        }

        public async Task<IEnumerable<VehicleSchedule>> BatchAddVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules)
        {
            IList<VehicleSchedule> newVehicleSchedules = new List<VehicleSchedule>();
            foreach (VehicleScheduleRequest vehicleScheduleRequest in vehicleSchedules)
            {
                if (!TIME_UNITS.Contains(vehicleScheduleRequest.TimeUnits))
                {
                    throw new AutoHubServerException(StatusCodes.Status400BadRequest, "Invalid value provided for time units");
                }
                VehicleSchedule newVehicleSchedule = new VehicleSchedule()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    VehicleId = vehicleId,
                    SstId = sstId,
                    MileInterval = vehicleScheduleRequest.MileInterval,
                    TimeInterval = vehicleScheduleRequest.TimeInterval,
                    TimeUnits = vehicleScheduleRequest.TimeUnits,
                };
                newVehicleSchedules.Add(newVehicleSchedule);
            }
            await _vehicleScheduleRepository.AddM
            return newVehicleSchedules;
        }

        public Task<IEnumerable<VehicleSchedule>> BatchDeleteVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<Guid> vehicleScheduleIds)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<VehicleSchedule>> BatchUpdateVehicleSchedules(Guid userId, Guid vehicleId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduledServiceType> CreateScheduledServiceType(Guid userId, string Name)
        {
            throw new NotImplementedException();
        }

        public Task<Guid> DeleteScheduledServiceType(Guid userId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ScheduledServiceTypeResponse>> GetUserScheduledServiceTypesWithVehicleSchedules(Guid userId)
        {
            throw new NotImplementedException();
        }

        public Task<ScheduledServiceType> UpdateScheduledServiceType(Guid userId, string Name)
        {
            throw new NotImplementedException();
        }
    }
}
