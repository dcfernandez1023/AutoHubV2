using AutoHub.Models;
using AutoHub.Models.RESTAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Web;
using Tests.Integration.Utilities;

namespace AutoHub.Tests.Integration
{
    public class ScheduledServiceTypeControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly HttpClient _client;
        static readonly string _userId = "5cd55172-5617-410a-9ff6-cc17303c4598";
        static readonly int _numVehicleSchedules = 3;

        public ScheduledServiceTypeControllerTests(WebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task ScheduledServiceTypeControllerTest()
        {
            // Access token
            string accessToken = await AuthHelper.GetAccessToken(_client, _userId);

            // POST ScheduledServiceType
            var postScheduledServiceTypeResponse = await PostScheduledServiceType(accessToken);
            Assert.Equal(HttpStatusCode.OK, postScheduledServiceTypeResponse.StatusCode);
            var scheduledServiceTypePost = await postScheduledServiceTypeResponse.Content.ReadFromJsonAsync<ScheduledServiceType>();
            Assert.NotNull(scheduledServiceTypePost);
            Assert.Equal("Test ScheduledServiceType", scheduledServiceTypePost.Name);

            // POST VehicleSchedules
            IList<Vehicle> vehicles = new List<Vehicle>();
            for (int i = 0; i < _numVehicleSchedules; i++)
            {
                var postVehicleResponse = await PostVehicle(accessToken);
                Assert.Equal(HttpStatusCode.OK, postVehicleResponse.StatusCode);
                var vehicle = await postVehicleResponse.Content.ReadFromJsonAsync<Vehicle>();
                Assert.NotNull(vehicle);
                vehicles.Add(vehicle);
            }

            var postVehicleSchedulesResponse = await PostVehicleSchedules(scheduledServiceTypePost.Id.ToString(), vehicles, accessToken);
            Assert.NotNull(postVehicleSchedulesResponse);
            Assert.Equal(HttpStatusCode.OK, postVehicleSchedulesResponse.StatusCode);
            IList<VehicleSchedule>? vehicleSchedulesPost = await postVehicleSchedulesResponse.Content.ReadFromJsonAsync<IList<VehicleSchedule>>();
            Assert.NotNull(vehicleSchedulesPost);
            Assert.Equal(vehicleSchedulesPost.Count, _numVehicleSchedules);

            // PUT VehicleSchedules
            var putVehicleSchedulesResponse = await PutVehicleSchedules(scheduledServiceTypePost.Id.ToString(), vehicleSchedulesPost, accessToken);
            Assert.NotNull(putVehicleSchedulesResponse);
            Assert.Equal(HttpStatusCode.OK, putVehicleSchedulesResponse.StatusCode);
            IList<VehicleSchedule>? vehicleSchedulesPut = await putVehicleSchedulesResponse.Content.ReadFromJsonAsync<IList<VehicleSchedule>>();
            Assert.NotNull(vehicleSchedulesPut);
            Assert.Equal(vehicleSchedulesPut.Count, _numVehicleSchedules);

            // GET ScheduledServiceType
            var getScheduledServiceTypeResponse = await GetScheduledServiceTypes(accessToken);
            var scheduledServiceTypeGet = await getScheduledServiceTypeResponse.Content.ReadFromJsonAsync<IList<ScheduledServiceTypeResponse>>();
            Assert.NotNull(scheduledServiceTypeGet);
            var scheduledServiceType = scheduledServiceTypeGet.Where(x => x.Id == scheduledServiceTypePost.Id).FirstOrDefault();
            Assert.NotNull(scheduledServiceType);
            Assert.Equal("Test ScheduledServiceType", scheduledServiceType.Name);
            foreach (var schedule in scheduledServiceType.VehicleSchedules)
            {
                Assert.NotNull(schedule);
                Assert.Equal(_userId, schedule.UserId.ToString());
                Assert.Equal(scheduledServiceType.Id, schedule.SstId);
                Assert.Equal(0, schedule.MileInterval);
                Assert.Equal(0, schedule.TimeInterval);
                Assert.Equal("day", schedule.TimeUnits);
            }

            // PUT ScheduledServiceType
            var putScheduledServiceTypeResponse = await PutScheduledServiceType(scheduledServiceTypePost.Id.ToString(), accessToken);
            Assert.Equal(HttpStatusCode.OK, putScheduledServiceTypeResponse.StatusCode);
            var scheduledServiceTypePut = await putScheduledServiceTypeResponse.Content.ReadFromJsonAsync<ScheduledServiceType>();
            Assert.NotNull(scheduledServiceTypePut);
            Assert.Equal("Test Put ScheduledServiceType", scheduledServiceTypePut.Name);

            // DELETE ScheduledServiceType
            var deleteScheduledServiceTypeResponse = await DeleteScheduledServiceType(scheduledServiceType.Id.ToString(), accessToken);
            Assert.Equal(HttpStatusCode.OK, deleteScheduledServiceTypeResponse.StatusCode);

            for (int i = 0; i < _numVehicleSchedules; i++)
            {
                var deleteVehicleResponse = await DeleteVehicle(vehicles[i].Id.ToString(), accessToken);
                Assert.Equal(HttpStatusCode.OK, deleteVehicleResponse.StatusCode);
            }
        }

        private async Task<HttpResponseMessage> PostVehicleSchedules(string scheduledServiceTypeId, IList<Vehicle> vehicles, string accessToken)
        {
            if (vehicles.Count != _numVehicleSchedules)
            {
                throw new Exception("Vehicle count does not match specified nuber of vehicle schedules");
            }

            IList<VehicleScheduleRequest> requestBody = new List<VehicleScheduleRequest>();
            for (int i = 0; i < _numVehicleSchedules; i++)
            {
                var vehicleScheduleRequest = new VehicleScheduleRequest()
                {
                    VehicleId = vehicles[i].Id,
                    MileInterval = 5000,
                    TimeInterval = 6,
                    TimeUnits = "month"
                };
                requestBody.Add(vehicleScheduleRequest);
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonString = JsonSerializer.Serialize(requestBody, options);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.PostAsync($"/api/users/{_userId}/scheduledServiceType/{scheduledServiceTypeId}/schedules", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> PutVehicleSchedules(string scheduledServiceTypeId, IList<VehicleSchedule> vehicleSchedules, string accessToken)
        {
            if (vehicleSchedules.Count != _numVehicleSchedules)
            {
                throw new Exception("Vehicle count does not match specified nuber of vehicle schedules");
            }

            for (int i = 0; i < vehicleSchedules.Count; i++)
            {
                var schedule = vehicleSchedules[i];
                schedule.MileInterval = 0;
                schedule.TimeInterval = 0;
                schedule.TimeUnits = "day";
            }

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
            var jsonString = JsonSerializer.Serialize(vehicleSchedules, options);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.PutAsync($"/api/users/{_userId}/scheduledServiceType/{scheduledServiceTypeId}/schedules", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> PostVehicle(string accessToken)
        {
            var requestBody = new VehicleRequest()
            {
                Name = "Test Vehicle",
                Mileage = 50000,
                Year = 2022,
                Make = "Test Make",
                Model = "Test Model",
                LicensePlate = "Test License Plate",
                Vin = "1234567890",
                Notes = "Test Notes",
                Base64Image = "https://picsum.photos/200/300"
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonString = JsonSerializer.Serialize(requestBody, options);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.PostAsync($"/api/users/{_userId}/vehicles", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> DeleteVehicle(string vehicleId, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.DeleteAsync($"/api/users/{_userId}/vehicles/{vehicleId}");
            return response;
        }

        private async Task<HttpResponseMessage> PostScheduledServiceType(string accessToken)
        {
            var requestBody = new ScheduledServiceTypeRequest()
            {
                Name = "Test ScheduledServiceType"
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonString = JsonSerializer.Serialize(requestBody, options);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.PostAsync($"/api/users/{_userId}/scheduledServiceType", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> PutScheduledServiceType(string scheduledServiceTypeId, string accessToken)
        {
            var requestBody = new ScheduledServiceTypeRequest()
            {
                Name = "Test Put ScheduledServiceType"
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            var jsonString = JsonSerializer.Serialize(requestBody, options);
            var stringContent = new StringContent(jsonString, Encoding.UTF8, "application/json");
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.PutAsync($"/api/users/{_userId}/scheduledServiceType/{scheduledServiceTypeId}", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> GetScheduledServiceTypes(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"/api/users/{_userId}/scheduledServiceType");
            return response;
        }

        private async Task<HttpResponseMessage> DeleteScheduledServiceType(string scheduledServiceTypeId, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.DeleteAsync($"/api/users/{_userId}/scheduledServiceType/{scheduledServiceTypeId}");
            if (response.Content != null)
            {
                string jsonResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Response Body as JSON:");
                Console.WriteLine(jsonResponse);
            }
            return response;
        }
    }
}
