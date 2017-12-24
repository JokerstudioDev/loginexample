using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using loginexample.API;
using loginexample.API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json;
using Xunit;

namespace loginexample.IntegrationTest
{
    public class LoginAPITest
    {
        private readonly TestServer server;
        private readonly HttpClient client;

        public LoginAPITest()
        {
            server = new TestServer(new WebHostBuilder()
                                    .UseEnvironment("Development")
                                    .UseStartup<Startup>());
            client = server.CreateClient();
        }

        [Fact]
        public async void ReturnUserInfoGivenLoginInfo()
        {
            var request = "/api/login";
            String jsonData = "{ \"username\": \"ploy\", \"password\": \"1234\"}";
            HttpContent payload = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, payload);
            response.EnsureSuccessStatusCode();

            User results = JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());

            Assert.Equal("ploy", results.Username);
        }

        [Fact]
        public async void ReturnBadRequest()
        {
            var request = "/api/login";
            String jsonData = "{ \"username\": \"ploy\"}";
            HttpContent payload = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, payload);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async void ReturnUnauthorized()
        {
            var request = "/api/login";
            String jsonData = "{ \"username\": \"john\", \"password\": \"1234\"}";
            HttpContent payload = new StringContent(jsonData, Encoding.UTF8, "application/json");
            var response = await client.PostAsync(request, payload);

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }
    }
}
