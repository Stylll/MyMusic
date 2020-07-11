using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.Core.Services;
using MyMusic.Core.Models;
using MyMusic.API.Resources;
using MyMusic.API.Validators;
using AutoMapper;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArtistsController : ControllerBase
    {
        public readonly IArtistService _artistService;
        public readonly IMapper _mapper;

        public ArtistsController(IArtistService artistService, IMapper mapper)
        {
            _artistService = artistService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult> GetAllArtists()
        {
            var artists = await _artistService.GetAllArtists();
            var artistsResource = _mapper.Map<IEnumerable<Artist>, IEnumerable<ArtistResource>>(artists);
            var response = new Response<IEnumerable<ArtistResource>>
            {
                status = 200,
                message = "Artists retrieved successfully",
                data = artistsResource
            };

            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetArtistById(int id)
        {
            var response = new Response<ArtistResource>();
            if (id == 0)
            {
                response.status = 400;
                response.message = "Artist Id cannot be 0";

                return BadRequest(response);
            }

            var artist = await _artistService.GetArtistById(id);
            if (artist == null)
            {
                response.status = 404;
                response.message = $"Artist with id: {id} does not exist";

                return NotFound(response);
            }

            var artistResource = _mapper.Map<Artist, ArtistResource>(artist);
            response.status = 200;
            response.message = "Artist retrieved successfully";
            response.data = artistResource;

            return Ok(response);
        }

        [HttpPost("")]
        public async Task<ActionResult> CreateArtist([FromBody] SaveArtistResource saveArtist)
        {
            var validator = new SaveArtistResourceValidator();
            var validatorResult = await validator.ValidateAsync(saveArtist);
            var errorResponse = new ErrorResponse<IList<FluentValidation.Results.ValidationFailure>>();

            if (!validatorResult.IsValid)
            {
                errorResponse.status = 400;
                errorResponse.message = "Bad Request";
                errorResponse.errors = validatorResult.Errors;

                return BadRequest(errorResponse);
            }

            var artist = _mapper.Map<SaveArtistResource, Artist>(saveArtist);
            var savedArtist = await _artistService.CreateArtist(artist);
            var artistResponse = _mapper.Map<Artist, ArtistResource>(savedArtist);
            var response = new Response<ArtistResource>
            {
                status = 201,
                message = "Artist Created Successfully",
                data = artistResponse
            };

            return CreatedAtAction(nameof(GetArtistById), new { id = artistResponse.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateArtist(int id, [FromBody] SaveArtistResource saveArtist)
        {
            var response = new Response<ArtistResource>();
            if (id == 0)
            {
                response.status = 400;
                response.message = "Artist Id cannot be 0";

                return BadRequest(response);
            }

            var validator = new SaveArtistResourceValidator();
            var validatorResult = await validator.ValidateAsync(saveArtist);
            var errorResponse = new ErrorResponse<IList<FluentValidation.Results.ValidationFailure>>();

            if (!validatorResult.IsValid)
            {
                errorResponse.status = 400;
                errorResponse.message = "Bad Request";
                errorResponse.errors = validatorResult.Errors;

                return BadRequest(errorResponse);
            }

            var artistToUpdate = await _artistService.GetArtistById(id);
            if (artistToUpdate == null)
            {
                response.status = 404;
                response.message = string.Format("Artist with id: {0} does not exist", id);

                return StatusCode(404, response);
            }

            var artist = _mapper.Map<SaveArtistResource, Artist>(saveArtist);
            await _artistService.UpdateArtist(artistToUpdate, artist);
            var updatedArtist = await _artistService.GetArtistById(artistToUpdate.Id);
            var artistResource = _mapper.Map<Artist, ArtistResource>(updatedArtist);

            response.status = 200;
            response.message = "Artist Updated Successfully";
            response.data = artistResource;

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteArtist(int id)
        {
            var response = new Response<ArtistResource>();
            if (id == 0)
            {
                response.status = 400;
                response.message = "Artist id cannot be 0";

                return BadRequest(response);
            }
            var artist = await _artistService.GetArtistById(id);
            if (artist == null)
            {
                response.status = 404;
                response.message = string.Format("Artist with id: {0} does not exist", id);

                return StatusCode(404, response);
            }

            await _artistService.DeleteArtist(artist);

            return NoContent();
        }

    }
}