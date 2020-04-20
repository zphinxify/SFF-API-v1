using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SFF.Models;

namespace API_v1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RentedMovieController : ControllerBase
    {
        private readonly GlobalDbContext _context;

        public RentedMovieController(GlobalDbContext context)
        {
            _context = context;
        }

        // GET: api/RentedMovie
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentedMovie>>> GetRentedMovie()
        {
            return await _context.RentedMovies.ToListAsync();
        }

        // GET: api/RentedMovie/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RentedMovie>> GetRentedMovie(int id)
        {
            var rentedMovie = await _context.RentedMovies.FindAsync(id);

            if (rentedMovie == null)
            {
                return NotFound();
            }

            return rentedMovie;
        }

        

        // PUT: api/RentedMovie/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRentedMovie(int id, RentedMovie rentedMovie)
        {
            if (id != rentedMovie.Id)
            {
                return BadRequest();
            }

            _context.Entry(rentedMovie).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RentedMovieExists(id))
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

        // POST: api/RentedMovie
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<RentedMovie>> PostRentedMovie(RentedMovie rentedMovie)
        {
            _context.RentedMovies.Add(rentedMovie);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRentedMovie", new { id = rentedMovie.Id }, rentedMovie);
        }

        // DELETE: api/RentedMovie/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<RentedMovie>> DeleteRentedMovie(int id)
        {
            var rentedMovie = await _context.RentedMovies.FindAsync(id);
            if (rentedMovie == null)
            {
                return NotFound();
            }

            _context.RentedMovies.Remove(rentedMovie);
            await _context.SaveChangesAsync();

            return rentedMovie;
        }

        private bool RentedMovieExists(int id)
        {
            return _context.RentedMovies.Any(e => e.Id == id);
        }
    }
}
