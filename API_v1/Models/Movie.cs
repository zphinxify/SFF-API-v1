using System.Collections.Generic;

namespace SFF.Models
{
    public class Movie 
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Genre {get; set; }
        public int MaxAmount { get; set; }
        public bool isRented { get; set; } = false;

        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}