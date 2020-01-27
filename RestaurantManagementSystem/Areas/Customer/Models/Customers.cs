using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Customer.Models
{
    public class Customers
    {   [Key]
        public int CustomersId { get; set; }
        public string CustomersName { get; set; }
        public Int64 MobileNumber { get; set; }
        public Int64 PaymentMobileNumber { get; set; }

    }
}
