using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Manager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Customer.Models
{
    public class Customers
    {
        public Customers()
        {
            CustomerOrderedTables = new HashSet<CustomerOrderedTable>();
            
        }
        [Key]
        public int CustomersId { get; set; }
        public string CustomersName { get; set; }
        public string MobileNumber { get; set; }
        
        public virtual ICollection<CustomerOrderedTable> CustomerOrderedTables { get; set; }

    }
}
