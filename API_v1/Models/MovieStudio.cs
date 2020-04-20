using System.Collections.Generic;
namespace SFF.Models
{
    public class MovieStudio
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Name { get; set; }

        public ICollection<Movie> Movies { get; set; } = new List<Movie>();
    }
}