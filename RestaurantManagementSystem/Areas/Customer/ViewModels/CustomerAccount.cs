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
        public string CustomersName { get; set; }
        [Required]
        public string MobileNumber { get; set; } 
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string PaymentMobileNumber { get; set; }

    }
}
