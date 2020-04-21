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
    [Route("api/Movies")]
    [ApiController]
    public class MovieController : ControllerBase
    {
        private readonly GlobalDbContext _context;

        public MovieController(GlobalDbContext context)
        {
            _context = context;
        }

        // GET: api/Movie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Movie>>> GetMovies()
        {
            return await _context.Movies.ToListAsync();
        }

        // PUT: Change amount of movies a studio can rent
        [HttpPut("changeamount/{id}")]
        public async Task<ActionResult<Movie>> ChangeRentQuote(int id, Movie movie)
        {
            var movieToChange = _context.Movies.Find(id);
            movieToChange.MaxAmount = movie.MaxAmount;
            if (movieToChange.MaxAmount > 20)
            {
                return BadRequest();
            }
            await _context.SaveChangesAsync();
            return movieToChange;
        }

        // GET: api/Movie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Movie>> GetMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);

            if (movie == null)
            {
                return NotFound();
            }

            return movie;
        }

        // Create new movie
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE a movie from the database
        [HttpDelete("{id}")]
        public async Task<ActionResult<Movie>> DeleteMovie(int id)
        {
            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }

            _context.Movies.Remove(movie);
            await _context.SaveChangesAsync();

            return movie;
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
    }
}
