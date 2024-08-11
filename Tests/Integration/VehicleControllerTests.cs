using AutoHub.Models.RESTAPI;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Tests.Integration.Utilities;
using Xunit;

namespace AutoHub.Tests.Integration
{
    public class VehicleControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        readonly HttpClient _client;

        public VehicleControllerTests(WebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task POST_VehicleTest()
        {
            string accessToken = await AuthHelper.GetAccessToken(_client);

        }
    }
}
