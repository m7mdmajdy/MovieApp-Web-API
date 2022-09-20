using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Dtos;
using MoviesApp.Model;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _genreService;

        public GenresController(IGenreService genreService)
        {
            _genreService = genreService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync() {

            var genres = await _genreService.GetAll();

            return Ok(genres);

        }
        [HttpPost]
        public async Task<IActionResult> CreateGenre(CreateGenreDto dto)
        {
            var genre = new Genre { Name = dto.Name };
            await _genreService.Add(genre);
            return Ok(genre);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(byte id,[FromBody] CreateGenreDto dto)
        {
            var genre=await _genreService.GetGenreById(id);
            if (genre == null)
                return NotFound("Not found");

            _genreService.Update(genre);
            return Ok(genre);

        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(byte id)
        {
            var genre = await _genreService.GetGenreById(id);
            if (genre == null)
                return NotFound("Not Found");

            _genreService.Delete(genre);
            return Ok(genre);
        }
    }
}
