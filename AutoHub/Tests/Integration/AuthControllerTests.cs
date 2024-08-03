using AutoHub.Models.RESTAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;
using Xunit;

namespace AutoHub.Tests.Integration
{
    internal class AuthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly HttpClient _client;

        public AuthControllerTests(WebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task POST_TestLogin()
        {
            var requestBody = new LoginModel()
            {
                Email = "test@example.com",
                Password = "Password"
            };
            var jsonContent = JsonContent.Create(requestBody);
            var response = await _client.PostAsync("/login", jsonContent);

            // Ensure success response code
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            // Ensure Response contains access token
            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<JsonElement>(responseString);
            Assert.True(responseObject.TryGetProperty("accessToken", out _));
        }
    }
}
