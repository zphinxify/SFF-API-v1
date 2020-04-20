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
        
        // PUT: Update maxAmount and set isRented = true 
        [HttpPut("movieisrented/{id}")]
        public async Task<ActionResult<Movie>> RentMovieToStudio(int id, Movie movie)
        {
            var movieToRent = _context.Movies.Find(id);
            if (movieToRent.MaxAmount == 0)
            {
                return BadRequest();
            }
            movieToRent.MaxAmount --;
            movieToRent.isRented = true;
            await _context.SaveChangesAsync();
            return movieToRent;
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

        // PUT: Register a new movie to the database
        [HttpPut("{id}")]
        public async Task<IActionResult> CreateNewMovie(int id, Movie movie)
        {
            if (id != movie.Id)
            {
                return BadRequest();
            }

            _context.Entry(movie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(id))
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

        // POST: api/Movie
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Movie>> PostMovie(Movie movie)
        {
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetMovie", new { id = movie.Id }, movie);
        }

        // DELETE: api/Movie/5
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
