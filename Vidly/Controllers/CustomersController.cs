using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Vidly.Models;
using System.Data.Entity;
using Vidly.ViewModels;
using System.Runtime.Caching;

namespace Vidly.Controllers
{
    public class CustomersController : Controller
    {
        private ApplicationDbContext _context;

        public CustomersController()
        {
            _context = new ApplicationDbContext();
        }

        protected override void Dispose(bool disposing)
        {
            //disposing connection to database at end of using statement
            _context.Dispose();
        }

        public ActionResult New()
        {
            //gets MembershipTypes from database and converts to list to prevent multiple round trips to database (eager loading)
            var membershipTypes = _context.MembershipTypes.ToList();

            //creates new viewModel for the customer to be used in CustomerForm View page 
            var viewModel = new CustomerFormViewModel
            {
                Customer = new Customer(),
                MembershipTypes = membershipTypes
            };

            //returns CustomerForm View page with viewModel in the fields
            return View("CustomerForm", viewModel);
        }

        [HttpPost]
        //AntiForgeryToken prevents Cross-Site Request Forgery for sensitive actions
        [ValidateAntiForgeryToken]
        public ActionResult Save(Customer customer)
        {
            //if ModelState is valid, creates new viewModel for customer and returns CustomerForm View
            if(!ModelState.IsValid)
            {
                var viewModel = new CustomerFormViewModel
                {
                    Customer = customer,
                    MembershipTypes = _context.MembershipTypes.ToList()
                };

                return View("CustomerForm", viewModel);
            }

            //adds new customer to database if Id not found, otherwise updates customer information edited and saves changes to database
            if(customer.Id == 0)
                _context.Customers.Add(customer);
            else
            {
                var customerInDb = _context.Customers.Single(c => c.Id == customer.Id);

                customerInDb.Name = customer.Name;
                customerInDb.Birthdate = customer.Birthdate;
                customerInDb.MembershipTypeId = customer.MembershipTypeId;
                customerInDb.IsSubscribedToNewsletter = customer.IsSubscribedToNewsletter;
            }

            _context.SaveChanges();

            //redirects to "home page" of customers
            return RedirectToAction("Index", "Customers");
        }

        // GET: Customers
        public ViewResult Index()
        {
            //returns "home page" for customers
            return View();
        }

        public ActionResult Details(int id)
        {
            //gets customer from database and includes MembershipType
            var customer = _context.Customers.Include(c => c.MembershipType).SingleOrDefault(c => c.Id == id);

            //error handling
            if (customer == null)
                return HttpNotFound();

            //returns details of customer
            return View(customer);
        }

        public ActionResult Edit(int id)
        {
            //gets specific customer from database
            var customer = _context.Customers.SingleOrDefault(c => c.Id == id);

            //error handling
            if (customer == null)
                return HttpNotFound();

            //creates new viewModel for edits made to customer
            var viewModel = new CustomerFormViewModel
            {
                Customer = customer,
                MembershipTypes = _context.MembershipTypes.ToList()
            };

            //returns CustomerForm view
            return View("CustomerForm", viewModel);
        }
    }
}