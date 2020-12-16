using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.TestHost;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http;
using System.Net;
using DatingApp;

namespace UnitTest
{
    public class TestValues
    {
        private readonly HttpClient _client;

        public TestValues()
        {
            var server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseStartup<Startup>());
            _client = server.CreateClient();
        }

        [Theory]
        [InlineData("GET")]
        public async Task GetValues(string GetValues)
        {
            var request = new HttpRequestMessage(new HttpMethod(GetValues), "/api/values");

            var response = await _client.SendAsync(request);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }
    }
}
