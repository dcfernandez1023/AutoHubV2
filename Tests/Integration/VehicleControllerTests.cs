using AutoHub.Models;
using AutoHub.Models.RESTAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using NuGet.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using Tests.Integration.Utilities;
using Xunit;

namespace AutoHub.Tests.Integration
{
    public class VehicleControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly HttpClient _client;
        static readonly int _numVehicles = 2;
        static readonly string _userId = "5cd55172-5617-410a-9ff6-cc17303c4598";

        public VehicleControllerTests(WebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task VehicleControllerTest()
        {
            // Access token
            string accessToken = await AuthHelper.GetAccessToken(_client, _userId);

            // POST
            List<Vehicle> createdVehicles = new List<Vehicle>();
            for (int i = 0; i < _numVehicles; i++)
            {
                var response = await PostVehicle(accessToken);
                var vehicle = await DeserializeAndAssert(response);
                createdVehicles.Add(vehicle);
            }

            Assert.Equal(_numVehicles, createdVehicles.Count);

            // GET
            Guid vehicleId = createdVehicles[0].Id;
            var getVehicleResponse = await GetVehicle(vehicleId.ToString(), accessToken);
            var vehicleGet = await DeserializeAndAssert(getVehicleResponse);

            var getUserVehiclesResponse = await GetUserVehicles(accessToken);
            var vehiclesGet = await DeserializeListAndAssert(getUserVehiclesResponse);

            // PUT
            var putVehicleResponse = await PutVehicle(vehicleId.ToString(), accessToken);
            var vehiclePut = await putVehicleResponse.Content.ReadFromJsonAsync<Vehicle>();
            Assert.NotNull(vehiclePut);
            Assert.Equal("Test Vehicle Put", vehiclePut.Name);

            // DELETE
            foreach (var vehicle in vehiclesGet)
            {
                var deleteVehicleResponse = await DeleteVehicle(vehicle.Id.ToString(), accessToken);
                Assert.Equal(HttpStatusCode.OK, deleteVehicleResponse.StatusCode);
            }
        }

        private async Task<Vehicle> DeserializeAndAssert(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var vehicle = await response.Content.ReadFromJsonAsync<Vehicle>();
            Assert.NotNull(vehicle);
            VehicleAssert(vehicle);
            return vehicle;
        }

        private async Task<IList<Vehicle>> DeserializeListAndAssert(HttpResponseMessage response)
        {
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var vehicles = await response.Content.ReadFromJsonAsync<IList<Vehicle>>();
            Assert.NotNull(vehicles);
            Assert.IsAssignableFrom<IList<Vehicle>>(vehicles);
            for (int i = 0; i < vehicles.Count; i++)
            {
                VehicleAssert(vehicles[i]);
            }
            return vehicles;
        }

        private void VehicleAssert(Vehicle vehicle)
        {
            Assert.True(Guid.TryParse(vehicle.Id.ToString(), out var _), "Vehicle Id is not a valid GUID.");
            Assert.True(Guid.TryParse(vehicle.UserId.ToString(), out var _), "UserId is not a valid GUID.");
            Assert.Equal(_userId, vehicle.UserId.ToString());
            Assert.Equal("Test Vehicle", vehicle.Name);
            Assert.Equal(50000, vehicle.Mileage);
            Assert.Equal("Test Make", vehicle.Make);
            Assert.Equal("Test Model", vehicle.Model);
            Assert.Equal("Test License Plate", vehicle.LicensePlate);
            Assert.Equal("1234567890", vehicle.Vin);
            Assert.Equal("Test Notes", vehicle.Notes);
            Assert.Empty(vehicle.SharedWith);
            Assert.Equal("https://picsum.photos/200/300", vehicle.Base64Image);
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

        private async Task<HttpResponseMessage>GetVehicle(string vehicleId, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"/api/users/{_userId}/vehicles/{vehicleId}");
            return response;
        }

        private async Task<HttpResponseMessage> GetUserVehicles(string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.GetAsync($"/api/users/{_userId}/vehicles");
            return response;
        }

        private async Task<HttpResponseMessage> PutVehicle(string vehicleId, string accessToken)
        {
            var requestBody = new VehicleRequest()
            {
                Name = "Test Vehicle Put",
                Mileage = 0,
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
            var response = await _client.PutAsync($"/api/users/{_userId}/vehicles/{vehicleId}", stringContent);
            return response;
        }

        private async Task<HttpResponseMessage> DeleteVehicle(string vehicleId, string accessToken)
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            var response = await _client.DeleteAsync($"/api/users/{_userId}/vehicles/{vehicleId}");
            return response;
        }
    }
}
