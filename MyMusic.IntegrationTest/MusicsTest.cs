using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using Xunit;
using Xunit.Abstractions;
using MyMusic.API.Resources;

namespace MyMusic.IntegrationTest
{
    public class MusicsTest: IClassFixture<CustomWebApplicationFactory<MyMusic.API.Startup>>
    {
        private readonly CustomWebApplicationFactory<MyMusic.API.Startup> factory;
        private readonly ITestOutputHelper output;

        public MusicsTest(CustomWebApplicationFactory<MyMusic.API.Startup> factory, ITestOutputHelper output)
        {
            this.factory = factory;
            this.output = output;
        }

        [Fact]
        public async Task Get_EndpointReturnsMusics()
        {
            var client = factory.CreateClient();

            var response = await client.GetAsync("/api/musics");
            var value = await response.Content.ReadAsStringAsync();
            output.WriteLine($"response output: ", response.StatusCode);
            response.EnsureSuccessStatusCode();

            var sResponse = JsonConvert.DeserializeObject<Response<IEnumerable<MusicResource>>>(value);
            output.WriteLine($"sResponse: ", sResponse.message);
            Assert.Equal("Musics retrieved successfully", sResponse.message);
            Assert.Equal(200, sResponse.status);

        }
    }
}
