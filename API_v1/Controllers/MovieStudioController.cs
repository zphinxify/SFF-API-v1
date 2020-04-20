using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFF;
using SFF.Models;

namespace API_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MovieStudioController : ControllerBase
    {
        private readonly GlobalDbContext _context;

        public MovieStudioController(GlobalDbContext context)
        {
            _context = context;
        }

        // GET: api/MovieStudio
        [HttpGet]
        public async Task<ActionResult<IEnumerable<MovieStudio>>> GetStudio()
        {
            return await _context.MovieStudios.ToListAsync();
        }

        [HttpPut("changestudioname/{id}")]
        public async Task<ActionResult<MovieStudio>> ChangeStudioName(int id, MovieStudio studio)
        {
            var filmStudio = _context.MovieStudios.Find(id);
            filmStudio.Name = studio.Name;
            await _context.SaveChangesAsync();
            return filmStudio;
        }

        [HttpGet("{id}/movies")]
        public async Task<ActionResult<IEnumerable<Movie>>> GetRentedMovies(int id)
        {
            var movieStudio = await _context.MovieStudios.FindAsync(id);

            var rentals = _context.RentedMovies
                                  .Where(r => r.StudioId == movieStudio.Id && r.isRented == true);
            var movies = new List<Movie>();

            foreach (var rental in rentals)
            {
                var movie = _context.Movies.Where(m => m.Id == rental.MovieId);
                movies.AddRange(movie);
            }

            return movies;
        }

        // GET: api/MovieStudio/5
        [HttpGet("{id}")]
        public async Task<ActionResult<MovieStudio>> GetMovieStudio(int id)
        {
            var movieStudio = await _context.MovieStudios.FindAsync(id);

            if (movieStudio == null)
            {
                return NotFound();
            }

            return movieStudio;
        }
        
        // PUT: api/MovieStudio/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutMovieStudio(int id, MovieStudio movieStudio)
        {
            if (id != movieStudio.Id)
            {
                return BadRequest();
            }

            _context.Entry(movieStudio).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieStudioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/MovieStudio
        [HttpPost]
        public async Task<ActionResult<MovieStudio>> PostMovieStudio(MovieStudio movieStudio)
        {
            _context.MovieStudios.Add(movieStudio);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovieStudio", new { id = movieStudio.Id }, movieStudio);
        }

        // DELETE: api/MovieStudio/5
        [HttpDelete("deletestudio/{id}")]
        public async Task<ActionResult<MovieStudio>> DeleteMovieStudio(int id)
        {
            var movieStudio = await _context.MovieStudios.FindAsync(id);
            if (movieStudio == null)
            {
                return NotFound();
            }

            _context.MovieStudios.Remove(movieStudio);
            await _context.SaveChangesAsync();

            return movieStudio;
        }

        private bool MovieStudioExists(int id)
        {
            return _context.MovieStudios.Any(e => e.Id == id);
        }
    }
}
