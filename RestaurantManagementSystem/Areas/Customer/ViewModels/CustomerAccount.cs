using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Customer.ViewModels
{
    public class CustomerAccount
    {
        [Required]
        [Display(Name = "Customer Name")]
        [RegularExpression(@"([\s]*[A-Za-z]+[\s]*)*", ErrorMessage = "Please provide valid Name")]

        public string CustomersName { get; set; }
        [Required]
        [Display(Name = "Mobile Number")]
        [Remote(action:"IsNumberInUse",controller:"Account")]
        [RegularExpression(@"[0][1][3-9]{1}[0-9]{8}", ErrorMessage ="Please provide valid Mobile Number")]
        public string MobileNumber { get; set; } 
        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email Address")]
        [Remote(action: "IsEmailInUse", controller: "Account")]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
       

    }
}
