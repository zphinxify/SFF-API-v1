using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SFF.Models;

namespace API_v1
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Adds defualt entities to the database
            using (var db = new GlobalDbContext())
            {
                // db.Add( new Movie {Title = "Inglorious Basterds", Genre = "Action", MaxAmount = 10, isRented = false} );
                // db.Add( new Movie {Title = "Ford vs Ferrari", Genre = "Racing", MaxAmount = 9} );
                // db.Add( new Movie {Title = "Kill Bill", Genre = "Action", MaxAmount = 5} );
                db.Add( new Movie {Title = "Catch me if you can", Genre = "Drama/Action", MaxAmount = 6} );
                // db.Add( new Movie {Title = "Wolf of wallstreet", Genre = "Drama", MaxAmount = 3} );
                // db.Add( new MovieStudio {City = "Halmstad", Name = "Röda Kvarn Halmstad"} );
                // db.Add( new MovieStudio {City = "Falkenberg", Name = "F-Studio AB"} );
                // db.Add( new MovieStudio {City = "Borås", Name = "Kvarnstaden Inc."} );
                var movie = db.Movies.OrderBy(m => m.Id)
                                     .First();


                movie.Reviews.Add( new Review { Grade = 3 ,Comment = "Dunder mannen" });
                db.SaveChanges();
            }

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
