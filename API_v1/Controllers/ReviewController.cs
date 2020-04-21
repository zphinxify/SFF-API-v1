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
    public class ReviewController : ControllerBase
    {
        private readonly GlobalDbContext _context;

        public ReviewController(GlobalDbContext context)
        {
            _context = context;
        }

        // GET: api/Review
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        {
            return await _context.Reviews.ToListAsync();
        }

        // GET: api/Review/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Review>> GetReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        // PUT: api/Review/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.Id)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
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

        // POST: api/Review/studioid/movieid 
        [HttpPost("postreview/{studioid}/{movieid}")]
        public async Task<ActionResult<Review>> PostReview(Review review, int studioId, int movieId)
        {
            var movieStudioToSetGrade = await _context.MovieStudios.Where(ms => ms.Id == studioId).FirstOrDefaultAsync();
            var movieToGrade = await _context.Movies.Where(m => m.Id == movieId).FirstOrDefaultAsync();
            if (movieToGrade != null && movieStudioToSetGrade != null)
            {
                if (review.Grade > 5 || review.Grade < 0)
                {
                    return StatusCode(400);
                }
                movieToGrade.AddReview(review, movieStudioToSetGrade);
                await _context.SaveChangesAsync();
                return StatusCode(201);
            }
            return StatusCode(400);
        }

        [HttpGet("Label/{movieid}/{studioid}")]
        [Produces("application/xml")]
        public async Task<Label> test(int movieId, int studioId)
        {
            var label = new Label();
            var labelData = await label.CreateLabel(_context, movieId, studioId);
            var XMLdata = label.LabelData(labelData);
            return XMLdata;
        }

        // DELETE: api/Review/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Review>> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return review;
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.Id == id);
        }
    }
}
