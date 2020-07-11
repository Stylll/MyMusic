using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MyMusic.Core.Models;
using MyMusic.Core.Services;
using MyMusic.API.Resources;
using MyMusic.API.Validators;
using AutoMapper;

namespace MyMusic.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MusicsController : ControllerBase
    {
        private readonly IMusicService _musicService;
        private readonly IMapper _mapper;

        public MusicsController(IMusicService musicService, IMapper mapper)
        {
            _musicService = musicService;
            _mapper = mapper;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<MusicResource>>> GetAllMusics()
        {
            var musics = await _musicService.GetAllWithArtist();
            var musicsResources = _mapper.Map<IEnumerable<Music>, IEnumerable<MusicResource>>(musics);
            var resp = new Response<IEnumerable<MusicResource>>
            {
                data = musicsResources,
                status = 200,
                message = "Musics retrieved successfully"
            };

            return Ok(resp);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MusicResource>> GetByMusicId(int id)
        {
            System.Console.WriteLine($"music id: {id}");
            var music = await _musicService.GetMusicById(id);
            System.Console.WriteLine($"music object: {music}");
            var response = new Response<MusicResource>();

            if (music == null)
            {
                response.status = 404;
                response.message = "Music not found";

                return NotFound(response);
            }


            var musicResource = _mapper.Map<Music, MusicResource>(music);
            response.data = musicResource;
            response.status = 200;
            response.message = "Music Retrieved Successfully";

            return Ok(musicResource);
        }

        [HttpPost("")]
        public async Task<ActionResult<Response<MusicResource>>> CreateMusic([FromBody]SaveMusicResource saveMusic)
        {
            var validator = new SaveMusicResourceValidator();
            var validatorResult = await validator.ValidateAsync(saveMusic);
            var errorResponse = new ErrorResponse<IList<FluentValidation.Results.ValidationFailure>>();

            if (!validatorResult.IsValid)
            {
                errorResponse.status = 400;
                errorResponse.errors = validatorResult.Errors;
                errorResponse.message = "Bad Request";
                return BadRequest(errorResponse);
            }

            var music = _mapper.Map<SaveMusicResource, Music>(saveMusic);
            var savedMusic = await _musicService.CreateMusic(music);
            var musicWithArtist = await _musicService.GetMusicById(savedMusic.Id);
            var response = new Response<MusicResource>(); 
            if (musicWithArtist == null)
            {
                response.status = 500;
                response.message = "Failed to save. An error occurred";

                return StatusCode(500, response);
            }

            var musicResource = _mapper.Map<Music, MusicResource>(musicWithArtist);
            response.data = musicResource;
            response.status = 201;
            response.message = "Music Created Successfully";

            return CreatedAtAction(nameof(GetByMusicId), new { id = musicResource.Id }, response);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<MusicResource>> UpdateMusic(int id, [FromBody] SaveMusicResource saveMusic)
        {
            var validator = new SaveMusicResourceValidator();
            var validationResult = await validator.ValidateAsync(saveMusic);
            var errorResponse = new ErrorResponse<IList<FluentValidation.Results.ValidationFailure>>();

            if (!validationResult.IsValid)
            {
                errorResponse.status = 400;
                errorResponse.errors = validationResult.Errors;
                errorResponse.message = "Bad Request";
                return BadRequest(errorResponse);
            }

            var savedMusicFromDb = await _musicService.GetMusicById(id);
            var response = new Response<MusicResource>();
            if (savedMusicFromDb == null)
            {
                response.status = 404;
                response.message = string.Format("Music with id: {0} does not exist", id);

                return StatusCode(404, response);
            }

            var mappedSavedMusic = _mapper.Map<SaveMusicResource, Music>(saveMusic);
            await _musicService.UpdateMusic(savedMusicFromDb, mappedSavedMusic);

            var musicWithArtist = await _musicService.GetMusicById(savedMusicFromDb.Id);
            var musicResource = _mapper.Map<Music, MusicResource>(musicWithArtist);
            response.data = musicResource;
            response.status = 200;
            response.message = "Music Updated Successfully";

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteMusic(int id)
        {
            var response = new Response<MusicResource>();
            if (id == 0)
            {
                response.status = 400;
                response.message = "Id cannot be 0";

                return BadRequest(response);
            }
            var music = await _musicService.GetMusicById(id);
            if (music == null)
            {
                response.status = 404;
                response.message = string.Format("Music with id: {0} does not exist", id);

                return StatusCode(404, response);
            }

            await _musicService.DeleteMusic(music);

            return NoContent();
        }
    }
}