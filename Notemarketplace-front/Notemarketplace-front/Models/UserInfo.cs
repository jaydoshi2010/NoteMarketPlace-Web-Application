using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Notemarketplace_front.Models
{
    public class UserInfo
    {
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use alphabets only")]
        public string FirstName { get; set; }
        [Required]
        [RegularExpression(@"^[a-zA-Z]+$", ErrorMessage = "Use alphabets only")]
        public string LastName { get; set; }
        [Required]
        public string EmailID { get; set; }
        [Required]
        public string CountryCode { get; set; }
        [RegularExpression("[1-9]{1}[0-9]{9}", ErrorMessage = "Please provide valid phone number")]
        public string PhnNo { get; set; }
    }
}