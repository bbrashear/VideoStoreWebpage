using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Vidly.Models
{
    public class MembershipType
    {
        public byte Id { get; set; }
        [Required]
        public string Name { get; set; }
        public short SignUpFee { get; set; }
        public byte DurationInMonths { get; set; }
        public byte DiscountRate { get; set; }


        //these properties are used for the Min18YearsIfAMember custom validation 
        //and are readonly so they do not accidentally get changed elsewhere
        public static readonly byte Unknown = 0;
        public static readonly byte PayAsYouGo = 1;
    }
}