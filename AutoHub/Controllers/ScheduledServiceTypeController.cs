using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AutoHub.Data;
using AutoHub.Models;
using AutoHub.BizLogic.Abstractions;
using AutoHub.Models.RESTAPI;
using AutoHub.Helpers;

namespace AutoHub.Controllers
{
    [Route("api/users/{userId:guid}/[controller]")]
    [ApiController]
    public class ScheduledServiceTypeController : ControllerBase
    {
        private readonly IScheduledServiceTypeBizLogic _scheduledServiceTypeBizLogic;

        public ScheduledServiceTypeController(IScheduledServiceTypeBizLogic scheduledServiceTypeBizLogic)
        {
            _scheduledServiceTypeBizLogic = scheduledServiceTypeBizLogic;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ScheduledServiceTypeResponse>>> GetScheduledServiceTypes([FromRoute] Guid userId)
        {
            try
            {
                IEnumerable<ScheduledServiceTypeResponse> scheduledServiceTypeResponse = await _scheduledServiceTypeBizLogic.GetUserScheduledServiceTypesWithVehicleSchedules(userId);
                return Ok(scheduledServiceTypeResponse);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpPost("{id:guid}/schedules")]
        public async Task<ActionResult<IEnumerable<VehicleSchedule>>> PostVehicleSchedules([FromRoute] Guid userId, Guid id, [FromBody] IList<VehicleScheduleRequest> vehicleScheduleRequest)
        {
            try
            {
                IEnumerable<VehicleSchedule> vehicleSchedules = await _scheduledServiceTypeBizLogic.BatchAddVehicleSchedules(userId, id, vehicleScheduleRequest);
                return Ok(vehicleSchedules);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<ScheduledServiceType>> PostScheduledServiceType([FromRoute] Guid userId, [FromBody] ScheduledServiceTypeRequest request)
        {
            try
            {
                ScheduledServiceType scheduledServiceType = await _scheduledServiceTypeBizLogic.CreateScheduledServiceType(userId, request.Name);
                return Ok(scheduledServiceType);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteScheduledServiceType([FromRoute] Guid userId, Guid id)
        {
            try
            {
                await _scheduledServiceTypeBizLogic.DeleteScheduledServiceType(userId, id);
                return Ok();
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }
    }
}
