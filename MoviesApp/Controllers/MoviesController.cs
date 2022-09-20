using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MoviesApp.Dtos;
using MoviesApp.Model;
using MoviesApp.Services;

namespace MoviesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MoviesController : ControllerBase
    {
        private new List<string> _allowedExtensions =new List<string> { ".png", ".jpg" };
        private long _MaxAllowedPosterSize = 1048576;
        private readonly IMovieService _movieService;
        private readonly IGenreService _genreService;
        
        public MoviesController(IMovieService movieService,IGenreService genreService)
        {
            _movieService = movieService;
            _genreService = genreService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var movies = await _movieService.GetAllAsync();
            return Ok(movies);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var movie = await _movieService.GetById(id);
            if (movie == null)
                return NotFound("Not found message");

            var dto = new MovieDetailsDto
            {
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                Id = movie.Id,
                Poster = movie.Poster,
                Rate = movie.Rate,
                StoreLine = movie.StoreLine,
                Title = movie.Title,
                Year = movie.Year,
            };
            
            return Ok(dto);
        }
        [HttpGet("GetByGenreId")]
        public async Task<IActionResult> GetByGenreIdAsync(byte genreId)
        {
            var movies = await _movieService.GetAllAsync(genreId);
            return Ok(movies);
        }
        [HttpPost]
        public async Task<IActionResult> CreateAsync([FromForm]CreateMovieDto dto)
        {
            if (dto.Poster == null)
                return BadRequest("Poster is needed");

            if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower())) {
                return BadRequest("Bad extension");
            }
            if (dto.Poster.Length>_MaxAllowedPosterSize)
            {
                return BadRequest("Too large");
            }
            var IsValidGenre = await _genreService.IsValidGenre(dto.GenreId);
            if (!IsValidGenre) {
                return BadRequest("Not valid genre id");
            }
            using var datastream = new MemoryStream();
            await dto.Poster.CopyToAsync(datastream);
            var movie = new Movie
            {
                Title = dto.Title,
                Year = dto.Year,
                GenreId = dto.GenreId,
                Poster=datastream.ToArray(),
                Rate = dto.Rate,
                StoreLine = dto.StoreLine,
                
            };
            await _movieService.Add(movie);
            return Ok(movie);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int Id)
        {
            var movie =await _movieService.GetById(Id);

            if (movie == null)
                return NotFound("Not found");
            var dto = new MovieDetailsDto
            {
                GenreId = movie.GenreId,
                GenreName = movie.Genre.Name,
                Id = movie.Id,
                Poster = movie.Poster,
                Rate = movie.Rate,
                StoreLine = movie.StoreLine,
                Title = movie.Title,
                Year = movie.Year,
            };
            _movieService.Delete(movie);
            return Ok(dto);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAsync(int id, [FromForm] CreateMovieDto dto)
        {
            var movie = await _movieService.GetById(id);

            if (movie == null)
                return NotFound("No movie found");
            var IsValidGenre = await _genreService.GetGenreById(dto.GenreId);
            if(dto.Poster != null)
            {
                if (!_allowedExtensions.Contains(Path.GetExtension(dto.Poster.FileName).ToLower()))
                    return BadRequest("bad extension");
                if (dto.Poster.Length > _MaxAllowedPosterSize)
                    return BadRequest("Too large file");
                using var datastream= new MemoryStream();
                await dto.Poster.CopyToAsync(datastream);
                movie.Poster=datastream.ToArray();
            }
            movie.Title = dto.Title;
            movie.StoreLine = dto.StoreLine;
            movie.Year = dto.Year;
            movie.GenreId = dto.GenreId;
            movie.Rate = dto.Rate;
            _movieService.Update(movie);
            return Ok(movie);
        }
    }
}
