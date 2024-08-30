using AutoHub.Models.RESTAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading.Tasks;

namespace Tests.Integration.Utilities
{
    internal class AuthHelper
    {
        public static async Task<string> GetAccessToken(HttpClient client, string identifier)
        {
            var requestBody = new LoginModel()
            {
                Email = identifier,
                Password = "Password"
            };
            var jsonContent = JsonContent.Create(requestBody);
            var response = await client.PostAsync("/api/auth/login", jsonContent);

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception(response.Content.ToString());
            }

            var responseString = await response.Content.ReadAsStringAsync();
            var responseObject = JsonNode.Parse(responseString) as JsonObject;
            return responseObject["accessToken"].ToString();
        }
    }
}
