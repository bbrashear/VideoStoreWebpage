using AutoMapper;
using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Vidly.Dtos;
using Vidly.Models;

namespace Vidly.Controllers.Api
{
    public class MoviesController : ApiController
    {
        private ApplicationDbContext _context;

        public MoviesController()
        {
            //instantiates ApplicationDbContext to be able to access the database
            _context = new ApplicationDbContext();
        }

        // GET /api/movies
        public IHttpActionResult GetMovies(string query = null)
        {
            //gets movies form database that are not out of stock and includes genre
            var moviesQuery = _context.Movies
                .Include(m => m.Genre)
                .Where(m => m.NumberAvailable > 0);

            //if query is not null, gets movies that contain the query
            if (!string.IsNullOrWhiteSpace(query))
                moviesQuery = moviesQuery.Where(m => m.Name.Contains(query));

            //converts movies to list and maps them to movieDtos
            var movieDtos = moviesQuery
                .ToList()
                .Select(Mapper.Map<Movie, MovieDto>);

            //returns the list of movieDtos
            return Ok(movieDtos);
        }

        // GET /api/movies/1
        public IHttpActionResult GetMovies(int id)
        {
            //gets specific movie from database, SingleOrDefault add error handling over just Single
            var movie = _context.Movies.SingleOrDefault(m => m.Id == id);

            //error handling
            if (movie == null)
                return NotFound();

            //returns the movie mapped to a movieDto
            return Ok(Mapper.Map<Movie, MovieDto>(movie));
        }

        // POST /api/movies
        [HttpPost]
        //accessible only to users with specified RoleName created in AccountController, Register method with temporary code executed once
        //in order to not give every new registered user that RoleName
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult CreateMovie(MovieDto movieDto)
        {
            //model validation
            if (!ModelState.IsValid)
                return BadRequest();

            //maps movieDto to movie object
            var movie = Mapper.Map<MovieDto, Movie>(movieDto);

            //adds movie to database and saves changes
            _context.Movies.Add(movie);
            _context.SaveChanges();

            //sets movie Id to movieDto Id since movieDto Id is set to be ignored in MappingProfile
            movieDto.Id = movie.Id;

            //creates api route for the new movie
            return Created(new Uri(Request.RequestUri + "/" + movie.Id), movieDto);
        }

        // PUT /api/movies/1
        [HttpPut]
        //accessible only to users with specified RoleName
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult UpdateMovie(int id, MovieDto movieDto)
        {
            //model validation
            if (!ModelState.IsValid)
                return BadRequest();

            //gets specific movie from database
            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            //error handling
            if (movieInDb == null)
                return NotFound();

            //maps movieDto to movieInDb and saves changes
            Mapper.Map(movieDto, movieInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/movies/1
        [HttpDelete]
        //accessible only to users with specified RoleName
        [Authorize(Roles = RoleName.CanManageMovies)]
        public IHttpActionResult DeleteMovie(int id)
        {
            //gets specific movie from database
            var movieInDb = _context.Movies.SingleOrDefault(m => m.Id == id);

            //error handling
            if (movieInDb == null)
                return NotFound();

            //removes movie from database and saves changes
            _context.Movies.Remove(movieInDb);
            _context.SaveChanges();

            return Ok();
        }

    }
}
