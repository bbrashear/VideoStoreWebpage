using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Vidly.Models;

namespace Vidly.ViewModels
{
    //viewModel allows us to pass multiple models to the view, in this case Customers and MembershipTypes
    public class CustomerFormViewModel
    {
        public IEnumerable<MembershipType> MembershipTypes { get; set; }
        public Customer Customer { get; set; }

        //if id is find, title of page will be "Edit Customer", otherwise will be "New Customer"
        public string Title
        {
            get
            {
                return (Customer.Id != 0) ? "Edit Customer" : "New Customer";
            }
        }
    }
}