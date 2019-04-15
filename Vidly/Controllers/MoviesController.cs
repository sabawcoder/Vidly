using System.Web.Mvc;
using System.Data.Entity;
using System.Linq;

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
    }
}