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
    public class VehiclesController : ControllerBase
    {
        private readonly IVehicleBizLogic _vehicleBizLogic;

        public VehiclesController(IVehicleBizLogic vehicleBizLogic)
        {
            _vehicleBizLogic = vehicleBizLogic;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehicle>>> GetVehicles([FromRoute] Guid userId)
        {
            try 
            {
                IEnumerable<Vehicle> vehicles = await _vehicleBizLogic.GetUserVehicles(userId);
                return Ok(vehicles);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Vehicle>> GetVehicle([FromRoute] Guid userId, Guid id)
        {
            try
            {
                Vehicle vehicle = await _vehicleBizLogic.GetVehicle(userId, id);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Vehicle>> PutVehicle([FromRoute] Guid userId, Guid id, [FromBody] VehicleRequest vehicleRequest)
        {
            try
            {
                Vehicle vehicle = await _vehicleBizLogic.UpdateVehicle(userId, id, vehicleRequest);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Vehicle>> PostVehicle([FromRoute] Guid userId, [FromBody] VehicleRequest vehicleRequest)
        {
            try
            {
                Vehicle vehicle = await _vehicleBizLogic.CreateVehicle(userId, vehicleRequest);
                return Ok(vehicle);
            }
            catch (Exception ex)
            {
                var (statusCode, message) = ControllerExceptionHelper.HandleControllerException(ex);
                return StatusCode(statusCode, message);
            }

        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteVehicle([FromRoute] Guid userId, Guid id)
        {
            try
            {
                await _vehicleBizLogic.DeleteVehicle(id);
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
