using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;
using Vidly.ViewModels;
using System;

namespace Vidly.Models
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext context;
        public MoviesController()
        {
            context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            context.Dispose();
        }

        public ActionResult New()
        {
            var genres = context.Genres.ToList();
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };
            return View("MovieForm", viewModel);
        }

        public ViewResult Index()
        {
            var movies = context.Movies.Include(m => m.Genre).ToList(); ;
            return View(movies);
        }

        public ActionResult Details(int id)
        {
            var movie = context.Movies.Include(m => m.Genre).ToList().SingleOrDefault(m => m.Id == id);
            return View(movie);
        }

        public ActionResult Edit(int id)
        {
            var movie = context.Movies.SingleOrDefault(m => m.Id == id);

            if (movie == null)
                return HttpNotFound();

            var viewModel = new MovieFormViewModel
            {
                Movie = movie,
                Genres=context.Genres.ToList()
            };
            return View("MovieForm", viewModel);
        }

        [HttpPost]
        public ActionResult Save(Movie movie)
        {
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = context.Movies.Single(m => m.Id == movie.Id);
                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;
                movieInDb.DateAdded = movie.DateAdded;
            }

            context.SaveChanges();
            return RedirectToAction("Index", "Movies");
        }

    }
}