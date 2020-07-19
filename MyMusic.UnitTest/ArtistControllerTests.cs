using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Xunit;
using Moq;
using MyMusic.Core.Services;
using MyMusic.Core.Models;
using MyMusic.API.Resources;
using MyMusic.API.Controllers;
using AutoMapper;
using System.Diagnostics;
using Xunit.Abstractions;

namespace MyMusic.UnitTest
{
    public class ArtistControllerTests
    {
        private List<Artist> allArtists;
        private List<ArtistResource> allArtistsResources;
        private Mock<IArtistService> mockService;
        private Mock<IMapper> mockMapper;
        private ITestOutputHelper output;
        // .Returns(Task.FromResult(default(object)))

        public ArtistControllerTests(ITestOutputHelper output)
        {
            mockService = new Mock<IArtistService>();
            mockMapper = new Mock<IMapper>();
            allArtists = new List<Artist>()
            {
                new Artist { Id = 1, Musics = null, Name = "Artist1" },
                new Artist { Id = 2, Musics = null, Name = "Artist2" },
            };

            allArtistsResources = new List<ArtistResource>()
            {
                new ArtistResource { Id = 1, Name = "Artist1" },
                new ArtistResource { Id = 2, Name = "Artist2" },
            };

            this.output = output;
        }

        [Fact]
        public async Task GetAllArtists_Returns_Ok ()
        {
            mockService.Setup(service => service.GetAllArtists()).ReturnsAsync(allArtists);
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(allArtists))
                .Returns(allArtistsResources);
            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.GetAllArtists();

            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public async Task GetArtistById_Returns_Badrequest ()
        {
            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.GetArtistById(0);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task GetArtistById_Returns_Notfound()
        {
            mockService.Setup(service => service.GetArtistById(684)).ReturnsAsync((Artist)null);
 
            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.GetArtistById(684);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<NotFoundObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist with id: 684 does not exist", value.message);
            Assert.Equal(404, value.status);
        }

        [Fact]
        public async Task GetArtistById_Returns_Artist()
        {
            var artist = allArtists[0];
            mockService.Setup(service => service.GetArtistById(artist.Id)).ReturnsAsync(artist);
            mockMapper.Setup(mapper => mapper.Map<Artist, ArtistResource>(artist))
                .Returns(allArtistsResources[0]);
            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.GetArtistById(artist.Id);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var r = Assert.IsType<OkObjectResult>(result.Result);
            var s = Assert.IsType<Response<ArtistResource>>(r.Value);
            // output.WriteLine($"values: {s.message}");
            Assert.Equal("Artist retrieved successfully", s.message);
            Assert.Equal(artist.Name, s.data.Name);
        }

        [Fact]
        public async Task CreateArtist_Returns_Badrequest()
        {
            var saveArtist = new SaveArtistResource
            {
                Name = ""
            };

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.CreateArtist(saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task CreateArtist_Returns_CreatedAt()
        {
            var artist = allArtists[0];
            var saveArtist = new SaveArtistResource
            {
                Name = artist.Name
            };

            mockMapper.Setup(mapper => mapper.Map<SaveArtistResource, Artist>(saveArtist))
                .Returns(artist);
            mockService.Setup(service => service.CreateArtist(artist)).ReturnsAsync(artist);
            mockMapper.Setup(mapper => mapper.Map<Artist, ArtistResource>(artist))
                .Returns(allArtistsResources[0]);

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.CreateArtist(saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<CreatedAtActionResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist Created Successfully", value.message);
            Assert.Equal(201, value.status);
            Assert.Equal(artist.Name, value.data.Name);
        }

        [Fact]
        public async Task UpdateArtist_Returns_Badrequest()
        {
            var saveArtist = new SaveArtistResource
            {
                Name = ""
            };

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.UpdateArtist(0, saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<BadRequestObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist Id cannot be 0", value.message);
            Assert.Equal(400, value.status);

        }

        [Fact]
        public async Task UpdateArtist_Returns_Badrequest_Invalid_Form()
        {
            var saveArtist = new SaveArtistResource
            {
                Name = ""
            };

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.UpdateArtist(10, saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        [Fact]
        public async Task UpdateArtist_Returns_Notfound()
        {
            var artist = allArtists[0];
            var saveArtist = new SaveArtistResource
            {
                Name = "New name"
            };

            mockService.Setup(service => service.GetArtistById(45)).ReturnsAsync((Artist) null);

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.UpdateArtist(45, saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist with id: 45 does not exist", value.message);
            Assert.Equal(404, value.status);
        }

        [Fact]
        public async Task UpdateArtist_Returns_Ok()
        {
            var artist = allArtists[0];
            var saveArtist = new SaveArtistResource
            {
                Name = artist.Name
            };

            mockMapper.Setup(mapper => mapper.Map<SaveArtistResource, Artist>(saveArtist))
                .Returns(artist);
            mockService.Setup(service => service.UpdateArtist(artist, artist));
            mockService.Setup(service => service.GetArtistById(artist.Id)).ReturnsAsync(artist);
            mockMapper.Setup(mapper => mapper.Map<Artist, ArtistResource>(artist))
                .Returns(allArtistsResources[0]);

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.UpdateArtist(artist.Id, saveArtist);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<OkObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist Updated Successfully", value.message);
            Assert.Equal(200, value.status);
            Assert.Equal(artist.Name, value.data.Name);
        }

        [Fact]
        public async Task DeleteArtist_Returns_Badrequest()
        {
            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.DeleteArtist(0);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<BadRequestObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist id cannot be 0", value.message);
            Assert.Equal(400, value.status);
        }

        [Fact]
        public async Task DeleteArtist_Returns_Notfound()
        {
            mockService.Setup(service => service.GetArtistById(45)).ReturnsAsync((Artist)null);

            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.DeleteArtist(45);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            var res = Assert.IsType<ObjectResult>(result.Result);
            var value = Assert.IsType<Response<ArtistResource>>(res.Value);
            Assert.Equal("Artist with id: 45 does not exist", value.message);
            Assert.Equal(404, value.status);
        }

        [Fact]
        public async Task DeleteArtist_Returns_Nocontent()
        {
            var artist = allArtists[0];
            mockService.Setup(service => service.GetArtistById(artist.Id)).ReturnsAsync(artist);
            mockService.Setup(service => service.DeleteArtist(artist));


            var controller = new ArtistsController(mockService.Object, mockMapper.Object);

            var result = await controller.DeleteArtist(artist.Id);

            Assert.IsType<ActionResult<Response<ArtistResource>>>(result);
            Assert.IsType<NoContentResult>(result.Result);
        }

    }
}
