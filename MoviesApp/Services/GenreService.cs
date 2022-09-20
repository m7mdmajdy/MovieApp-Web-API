using Microsoft.EntityFrameworkCore;
using MoviesApp.Model;

namespace MoviesApp.Services
{
    public class GenreService : IGenreService
    {
        private readonly ApplicationDbContext _context;
        public GenreService(ApplicationDbContext context)
        {
                _context = context;
        }
        public async Task<Genre> Add(Genre genre)
        {
            await _context.Genres.AddAsync(genre);
            _context.SaveChanges();
            return genre;
        }

        public Genre Delete(Genre genre)
        {
            _context.Genres.Remove(genre);
            _context.SaveChanges();
            return genre;
        }

        public async Task<IEnumerable<Genre>> GetAll()
        {
            return await _context.Genres.OrderBy(m => m.Name).ToListAsync();
        }

        public async Task<Genre> GetGenreById(byte id)
        {
            return await _context.Genres.SingleOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> IsValidGenre(byte id)
        {
           return await _context.Genres.AnyAsync(m => m.Id ==id);
        }

        public Genre Update(Genre genre)
        {
            _context.Update(genre);
            _context.SaveChanges();
            return genre;
        }

        Genre IGenreService.Delete(Genre genre)
        {
            _context.Remove(genre);
            _context.SaveChanges();
            return genre;
        }
    }
}
