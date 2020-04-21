using System.Collections.Generic;
using System.Linq;
namespace SFF.Models
{
    public class MovieStudio
    {
        public int Id { get; set; }
        public string City { get; set; }
        public string Name { get; set; }

        public ICollection<RentedMovie> RentedMovies { get; set; } = new List<RentedMovie>();

        // Adds a movie as rented
        public void AddMovie(Movie movie)
        {
            if (movie.MaxAmount > 0)
            {
                movie.MaxAmount --;

                RentedMovie rented = new RentedMovie () { Movie = movie};
                RentedMovies.Add(rented);
            }
        }

        // Returns a rented movie
        public RentedMovie ReturnRentedMovie(int id)
        {
            var rentedMovieToReturn = RentedMovies.Where(m => m.MovieId == id).FirstOrDefault();
            var movie = RentedMovies.Select(m => m.Movie).Where(m => m.Id == id).FirstOrDefault();

            if (rentedMovieToReturn != null)
            {
                RentedMovies.Remove(rentedMovieToReturn);
                movie.MaxAmount++;
            }

            return rentedMovieToReturn;
        }
    }
}