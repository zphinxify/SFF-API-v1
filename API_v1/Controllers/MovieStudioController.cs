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

    // Rents a movie to a studio, this method is also able to set a movie as not rented anymore
    // This is acheived by passing in either 0 or 1 in the alternative
    [HttpPost("RentMovieToStudio/{studioId}/{movieId}/{alternative}")]
    public async Task<ActionResult<Movie>> RentMovieToStudio(int studioId, int movieId, int alternative)
    {
      if (alternative == 1)
      {
        var movieStudio = await _context.MovieStudios.Where(m => m.Id == studioId).FirstOrDefaultAsync();
        var movie = await _context.Movies.Where(m => m.Id == movieId).FirstOrDefaultAsync();

        movieStudio.AddMovie(movie);
        await _context.SaveChangesAsync();
        return StatusCode(201);
      }

      else if (alternative == 0)
      {
        var movieToReturn = await _context.MovieStudios.Where(movie => movie.Id == studioId)
                                                       .Include(x => x.RentedMovies)
                                                       .ThenInclude(x => x.Movie)
                                                       .FirstOrDefaultAsync();
        movieToReturn.ReturnRentedMovie(movieId);
        await _context.SaveChangesAsync();
        return StatusCode(201);
      }
      else
      {
        return BadRequest();
      }
    }


    // GET: api/MovieStudio/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieStudio>> GetMovieStudios(int id)
    {
      var movieStudio = await _context.MovieStudios.FindAsync(id);

      if (movieStudio == null)
      {
        return NotFound();
      }

      return movieStudio;
    }

    [HttpPut("editmoviestudio/{id}")]
    public async Task<IActionResult> EditMovieStudio(int id, MovieStudio movieStudio)
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

    // Create new MovieStudio object
    [HttpPost]
    public async Task<ActionResult<MovieStudio>> PostMovieStudio(MovieStudio movieStudio)
    {
      _context.MovieStudios.Add(movieStudio);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetMovieStudio", new { id = movieStudio.Id }, movieStudio);
    }

    // DELETE a moviestudio 
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

