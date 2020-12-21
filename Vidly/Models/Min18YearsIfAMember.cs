using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class Min18YearsIfAMember : ValidationAttribute
    {
        //determines whether customer is 18 to be a member
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            //gets object to validate
            var customer = (Customer)validationContext.ObjectInstance;

            //if customer does not have membership or is pay as you go, validation successful
            if (customer.MembershipTypeId == MembershipType.Unknown || 
                customer.MembershipTypeId == MembershipType.PayAsYouGo)
                return ValidationResult.Success;

            //must inlcude birthdate to check
            if (customer.Birthdate == null)
                return new ValidationResult("Birthdate is required.");
            
            //finds age of customer
            var age = DateTime.Today.Year - customer.Birthdate.Value.Year;

            //determines if customer is a valid member
            return (age >= 18) 
                ? ValidationResult.Success 
                : new ValidationResult("Customer should be at least 18 years old to have a membership");
        }
    }
}