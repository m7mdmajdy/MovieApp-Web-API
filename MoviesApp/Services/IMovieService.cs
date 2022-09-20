using MoviesApp.Model;

namespace MoviesApp.Services
{
    public interface IMovieService
    {
        Task<IEnumerable<Movie>> GetAllAsync(byte genreId=0);
        Task<Movie> GetById(int id);
        Task<Movie> Add(Movie movie);
        Movie Update(Movie movie);
        Movie Delete(Movie movie);
    }
}
