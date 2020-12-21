using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class NewRentalController : ApiController
    {
        private ApplicationDbContext _context;

        public NewRentalController()
        {
            _context = new ApplicationDbContext();
        }

        [HttpPost]
        public IHttpActionResult CreateNewRentals(NewRentalDto newRental)
        {
            //gets specific customer from database and sets the customer Id to the newRental CustomerId property
            var customer = _context.Customers.Single(c => c.Id == newRental.CustomerId);

            //gets the movies from the database the Id selected and converts ToList to prevent multiple round trips to database
            var movies = _context.Movies.Where(m => newRental.MovieIds.Contains(m.Id)).ToList();

            foreach (var movie in movies)
            {
                //if the movie has 0 copies available return error
                if (movie.NumberAvailable == 0)
                    return BadRequest("Movie is not available.");

                //decrease the NumberAvailable as the movie is being rented
                movie.NumberAvailable--;

                //create a rental for each movie in list and add to rentals table in database
                var rental = new Rental
                {
                    Customer = customer,
                    Movie = movie,
                    DateRented = DateTime.Now
                };

                _context.Rentals.Add(rental);
            }

            //save changes to database after all rentals have been created
            _context.SaveChanges();

            return Ok();
        }
    }
}
