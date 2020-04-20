namespace SFF.Models
{
    public class RentingHandler 
    {
        public int Id {get; set;}
        public int MovieId { get; set; }
        public Movie Movie { get; set; }
        public int StudioId { get; set; }
        public MovieStudio Studio { get; set; }
        public int ReviewId {get; set;}
        public Review Reviews {get; set;}
    }
}