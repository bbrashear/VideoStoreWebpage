using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using Vidly.ViewModels;
using System.Data.Entity;

namespace Vidly.Controllers
{
    public class MoviesController : Controller
    {
        private ApplicationDbContext _context;
        public MoviesController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        //limits access to only specific users
        [Authorize(Roles = RoleName.CanManageMovies)]
        public ViewResult New()
        {
            //gets genres from database
            var genres = _context.Genres.ToList();

            //creates viewModel for movie
            var viewModel = new MovieFormViewModel
            {
                Genres = genres
            };

            //returns MovieForm view with viewModel in the fields
            return View("MovieForm", viewModel);
        }

        [HttpPost]
        //AntiForgeryToken prevents Cross-Site Request Forgery for sensitive actions
        [ValidateAntiForgeryToken]
        public ActionResult Save(Movie movie)
        {
            //if ModelState is valid, creates new viewModel for movie and returns MovieForm View

            if (!ModelState.IsValid)
            {
                var viewModel = new MovieFormViewModel(movie)
                {
                    Genres = _context.Genres.ToList()
                };

                return View("MovieForm", viewModel);
            }

            //adds new movie to database if Id not found and sets DateAdded, otherwise updates movie information edited and saves changes to database
            if (movie.Id == 0)
            {
                movie.DateAdded = DateTime.Now;
                _context.Movies.Add(movie);
            }
            else
            {
                var movieInDb = _context.Movies.Single(m => m.Id == movie.Id);

                movieInDb.Name = movie.Name;
                movieInDb.ReleaseDate = movie.ReleaseDate;
                movieInDb.GenreId = movie.GenreId;
                movieInDb.NumberInStock = movie.NumberInStock;

            }

            _context.SaveChanges();

            //returns "home page" of movies
            return RedirectToAction("Index", "Movies");
        }

        // GET: Movies
        public ViewResult Index()
        {
            //List view allows edit and new actions for approved users
            if (User.IsInRole(RoleName.CanManageMovies))
                return View("List");

            //ReadOnlyList view only provides list of movies with details action
            return View("ReadOnlyList");
        }

        public ActionResult Details(int id)
        {
            //gets movie from database
            var movie = _context.Movies.Include(m => m.Genre).SingleOrDefault(m => m.Id == id);

            //error handling
            if (movie == null)
                return HttpNotFound();

            return View(movie);
        }
        //GET: Movies/Random
        //creates a hard coded "random" movie, more of a practice action
        public ActionResult Random()
        {
            var movie = new Movie { Name = "Inception" };
            var customers = new List<Customer>
            {
                new Customer{Name = "Customer 1"},
                new Customer{Name = "Customer 2"}
            };

            var viewModel = new RandomMovieViewModel
            {
                Movie = movie,
                Customers = customers
            };
            return View(viewModel);
        }

        //limits access to only specific users

        [Authorize(Roles = RoleName.CanManageMovies)]
        public ActionResult Edit(int id)
        {
            //gets specific movie from database
            var movie = _context.Movies.SingleOrDefault(c => c.Id == id);

            //error handling
            if (movie == null)
                return HttpNotFound();

            //creates new viewModel for movie
            var viewModel = new MovieFormViewModel(movie)
            {
                Genres = _context.Genres.ToList()
            };

            //returns MovieForm view with viewModel in the fields
            return View("MovieForm", viewModel);
        }
    }
}