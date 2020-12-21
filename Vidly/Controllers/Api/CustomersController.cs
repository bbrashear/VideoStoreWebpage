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
    public class CustomersController : ApiController
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            //instantiates ApplicationDbContext to be able to access the database
            _context = new ApplicationDbContext();
        }

        // GET /api/customers
        public IHttpActionResult GetCustomers(string query = null)
        {
            //gets list of customers, inlcudes MembershipType, and stores in variable
            var customersQuery = _context.Customers
               .Include(c => c.MembershipType);

            //if the query is not null, gets the customer(s) that contain the query string and stores in variable
            if (!string.IsNullOrWhiteSpace(query))
                customersQuery = customersQuery.Where(c => c.Name.Contains(query));

            //converts customers to list and maps them to customerDtos
            var customerDtos = customersQuery
                .ToList()
                .Select(Mapper.Map<Customer, CustomerDto>);
            
            //returns the list of customerDtos
            return Ok(customerDtos);
        }

        // GET /api/customers/1
        public IHttpActionResult GetCustomers(int id)
        {
            //gets a specific customer and stores in variable, SingleOrDefault provides some error handling over just Single
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            //error handling
            if (customer == null)
                return NotFound();
            
            //maps customer to customerDto and returns the customerDto
            return Ok(Mapper.Map<Customer, CustomerDto>(customer));
        }

        // POST /api/customers
        [HttpPost]
        public IHttpActionResult CreateCustomer(CustomerDto customerDto)
        {
            //model validation
            if (!ModelState.IsValid)
                return BadRequest();

            //maps customerDto to customer object
            var customer = Mapper.Map<CustomerDto, Customer>(customerDto);

            //adds customer to database and saves changes
            _context.Customers.Add(customer);
            _context.SaveChanges();

            //sets customer Id to customerDto Id since customerDto Id is set to be ignored in MappingProfile
            customerDto.Id = customer.Id;

            //creates api route for the new customer
            return Created(new Uri(Request.RequestUri + "/" + customer.Id), customerDto);
        }

        // PUT /api/customers/1
        [HttpPut]
        public IHttpActionResult UpdateCustomer(int id, CustomerDto customerDto)
        {
            //model validation
            if (!ModelState.IsValid)
                return BadRequest();

            //gets specific customer from database
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            //error handling
            if (customerInDb == null)
                return NotFound();

            //maps customerDto to customerInDb and saves changes
            Mapper.Map(customerDto, customerInDb);

            _context.SaveChanges();

            return Ok();
        }

        // DELETE /api/customers/1
        [HttpDelete]
        public IHttpActionResult DeleteCustomer(int id)
        {
            //gets specific customer in database
            var customerInDb = _context.Customers.SingleOrDefault(c => c.Id == id);

            //error handling
            if (customerInDb == null)
                return NotFound();

            //removes customer from database and saves changes
            _context.Customers.Remove(customerInDb);
            _context.SaveChanges();

            return Ok();
        }
    }
}
