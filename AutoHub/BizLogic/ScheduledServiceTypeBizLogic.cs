using AutoHub.BizLogic.Abstractions;
using AutoHub.Models;
using AutoHub.Models.RESTAPI;
using AutoHub.Repositories.Abstractions;
using AutoHub.Exceptions;

namespace AutoHub.BizLogic
{
    public class ScheduledServiceTypeBizLogic : IScheduledServiceTypeBizLogic
    {
        public static readonly HashSet<string> TIME_UNITS = new HashSet<string> { "day", "week", "month", "year" };

        private IVehicleRepository _vehicleRepository;
        private IVehicleScheduleRepository _vehicleScheduleRepository;
        private IScheduledServiceTypeRepository _scheduledServiceTypeRepository;

        public ScheduledServiceTypeBizLogic(IVehicleRepository vehicleRepository, IVehicleScheduleRepository vehicleScheduleRepository, IScheduledServiceTypeRepository scheduledServiceTypeRepository)
        {
            _vehicleRepository = vehicleRepository;
            _vehicleScheduleRepository = vehicleScheduleRepository;
            _scheduledServiceTypeRepository = scheduledServiceTypeRepository;
        }

        public async Task<IList<VehicleSchedule>> BatchAddVehicleSchedules(Guid userId, Guid sstId, IList<VehicleScheduleRequest> vehicleSchedules)
        {
            ScheduledServiceType? scheduledServiceType = await _scheduledServiceTypeRepository.GetByIdAsync(sstId);
            if (scheduledServiceType == null)
            {
                throw new ResourceNotFoundException(String.Format("ScheduledServiceType {0} does not exist", sstId));
            }

            // Get a set of the vehicle ids that already have a schedule
            HashSet<Guid> vehicleIdSet = new HashSet<Guid>();
            IList<VehicleSchedule> existingVehicleSchedules = await _vehicleScheduleRepository.GetBySstIdAsync(sstId);
            foreach (var existingVehicleSchedule in existingVehicleSchedules)
            {
                vehicleIdSet.Add(existingVehicleSchedule.VehicleId);
            }

            IList<VehicleSchedule> newVehicleSchedules = new List<VehicleSchedule>();
            foreach (VehicleScheduleRequest vehicleScheduleRequest in vehicleSchedules)
            {
                if (vehicleIdSet.Contains(vehicleScheduleRequest.VehicleId))
                {
                    throw new AutoHubServerException(StatusCodes.Status400BadRequest, String.Format("A vehicle schedule for {0} already exists", vehicleScheduleRequest.VehicleId));
                }

                VehicleSchedule newVehicleSchedule = new VehicleSchedule()
                {
                    Id = Guid.NewGuid(),
                    UserId = userId,
                    VehicleId = vehicleScheduleRequest.VehicleId,
                    SstId = sstId,
                    MileInterval = vehicleScheduleRequest.MileInterval,
                    TimeInterval = vehicleScheduleRequest.TimeInterval,
                    TimeUnits = vehicleScheduleRequest.TimeUnits,
                };
                ValidateVehicleSchedule(userId, vehicleScheduleRequest.VehicleId, sstId, newVehicleSchedule);
                newVehicleSchedules.Add(newVehicleSchedule);
            }
            await _vehicleScheduleRepository.AddManyAsync(newVehicleSchedules);
            return newVehicleSchedules;
        }

        public async Task<IList<Guid>> BatchDeleteVehicleSchedules(Guid userId, Guid sstId, IList<Guid> vehicleScheduleIds)
        {
            return await _vehicleScheduleRepository.DeleteManyAsync(vehicleScheduleIds);
        }

        public async Task<IList<VehicleSchedule>> BatchUpdateVehicleSchedules(Guid userId, Guid sstId, IList<VehicleSchedule> vehicleSchedules)
        {
            foreach (var vehicleSchedule in vehicleSchedules)
            {
                ValidateVehicleSchedule(userId, vehicleSchedule.VehicleId, sstId, vehicleSchedule);
            }
            return await _vehicleScheduleRepository.UpdateManyAsync(vehicleSchedules);
        }

        public async Task<ScheduledServiceType> CreateScheduledServiceType(Guid userId, string name)
        {
            ScheduledServiceType scheduledServiceType = new ScheduledServiceType()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Name = name,
                DateCreated = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
            };
            return await _scheduledServiceTypeRepository.AddAsync(scheduledServiceType);
        }

        public async Task DeleteScheduledServiceType(Guid userId, Guid id)
        {
            Guid? deletedId = await _scheduledServiceTypeRepository.DeleteAsync(id);
            if (deletedId == null)
            {
                throw new ResourceNotFoundException();
            }

            await _vehicleScheduleRepository.DeleteManyAsyncBySstId(id);
        }

        public async Task<IList<ScheduledServiceTypeResponse>> GetUserScheduledServiceTypesWithVehicleSchedules(Guid userId)
        {
            // Get scheduled service types and vehicle schedules
            IList<ScheduledServiceType> scheduledServiceTypes = await _scheduledServiceTypeRepository.GetByUserId(userId);
            IList<VehicleSchedule> vehicleSchedules = await _vehicleScheduleRepository.GetByUserId(userId);

            // Construct a dictionary where vehicle schedules are grouped by scheduled service type
            Dictionary<Guid, IList<VehicleSchedule>> vehicleSchedulesBySstId = new Dictionary<Guid, IList<VehicleSchedule>>();
            foreach (var vehicleSchedule in vehicleSchedules)
            {
                Guid sstId = vehicleSchedule.SstId;
                if (!vehicleSchedulesBySstId.TryGetValue(sstId, out var schedules))
                {
                    schedules = new List<VehicleSchedule>();
                    vehicleSchedulesBySstId[sstId] = schedules;
                }

                vehicleSchedulesBySstId[sstId].Add(vehicleSchedule);
            }

            // For each ScheduledServiceType, construct a ScheduledServiceTypeResponse containing the vehicle schedules and vehicle name for each schedule
            IList<ScheduledServiceTypeResponse> result = new List<ScheduledServiceTypeResponse>();
            foreach (var scheduledServiceType in scheduledServiceTypes)
            {
                ScheduledServiceTypeResponse scheduledServiceTypeResponse = new ScheduledServiceTypeResponse()
                {
                    Id = scheduledServiceType.Id,
                    UserId = scheduledServiceType.UserId,
                    Name = scheduledServiceType.Name,
                    DateCreated = scheduledServiceType.DateCreated,
                    VehicleSchedules = new List<VehicleScheduleResponse>()
                };

                if (vehicleSchedulesBySstId.TryGetValue(scheduledServiceType.Id, out var schedules))
                {
                    foreach (var schedule in schedules)
                    {
                        // TODO: Figure out a way to avoid making multiple database reads for the vehicle
                        Vehicle? vehicle = await _vehicleRepository.GetByIdAsync(schedule.VehicleId);
                        if (vehicle == null)
                        {
                            throw new ResourceNotFoundException(String.Format("Could not find vehicle {0} when getting scheduled service types with vehicle schedules", schedule.VehicleId));
                        }
                        scheduledServiceTypeResponse.VehicleSchedules.Add(new VehicleScheduleResponse()
                        {
                            Id = schedule.Id,
                            UserId = schedule.UserId,
                            VehicleId = schedule.VehicleId,
                            SstId = schedule.SstId,
                            MileInterval = schedule.MileInterval,
                            TimeInterval = schedule.TimeInterval,
                            TimeUnits = schedule.TimeUnits,
                            VehicleName = vehicle.Name
                        });
                    }
                }

                result.Add(scheduledServiceTypeResponse);
            }

            return result;
        }

        public async Task<ScheduledServiceType> UpdateScheduledServiceType(Guid id, Guid userId, string Name)
        {
            ScheduledServiceType? scheduledServiceType = await _scheduledServiceTypeRepository.GetByIdAsync(id);
            if (scheduledServiceType == null)
            {
                throw new ResourceNotFoundException();
            }
            scheduledServiceType.Name = Name;
            return await _scheduledServiceTypeRepository.UpdateAsync(scheduledServiceType);
        }

        private void ValidateVehicleSchedule(Guid userId, Guid vehicleId, Guid sstId, VehicleSchedule vehicleSchedule)
        {
            if (!TIME_UNITS.Contains(vehicleSchedule.TimeUnits))
            {
                throw new AutoHubServerException(StatusCodes.Status400BadRequest, "Invalid value provided for time units");
            }
            if (vehicleSchedule.UserId != userId || vehicleSchedule.VehicleId != vehicleId || vehicleSchedule.SstId != sstId)
            {
                throw new ResourceForbiddenException();
            }
        }
    }
}
