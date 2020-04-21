using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.Mvc;


namespace SFF.Models
{
    public class RentedMovie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        // This is not needed
        public bool isRented { get; set; } = true;
        public int StudioId { get; set; }
        public MovieStudio Studio { get; set; }

        
    }
}