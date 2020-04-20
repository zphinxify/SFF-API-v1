namespace SFF.Models
{
    public class RentedMovie
    {
        public int Id { get; set; }
        public int MovieId { get; set; }
        public bool isRented { get; set; } = true;
        public Movie Movie { get; set; }
        public int StudioId { get; set; }
        public MovieStudio Studio { get; set; }
    }
}