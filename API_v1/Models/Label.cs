using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
namespace SFF.Models
{
    public class Label
    {
        public string MovieName { get; set; }
        public string City { get; set; }
        public DateTime Date { get; set; }

        // Retrieves the data relevant to the Label
        public async Task<Label> CreateLabel(GlobalDbContext _context, int movieId, int studioId)
        {
            var movie = await _context.Movies.FindAsync(movieId);
            var studio = await _context.MovieStudios.FindAsync(studioId);

            var label = new Label() { MovieName = movie.Title, City = studio.City, Date = DateTime.Now};

            return label;
        }
        
        // used to get the proper type for label
       public Label LabelData(Label label)
       {
           return label;
       }
    }
}
