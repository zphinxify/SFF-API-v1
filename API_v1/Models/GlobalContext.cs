using Microsoft.EntityFrameworkCore;
namespace SFF.Models
{
    public class GlobalDbContext : DbContext
    {

        public GlobalDbContext(DbContextOptions<GlobalDbContext> options) : base(options)
        {

        }
        public GlobalDbContext() { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=myDatabase.db");
        }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<MovieStudio> MovieStudios { get; set; }
        public DbSet<RentedMovie> RentedMovies { get; set; }
    }
}