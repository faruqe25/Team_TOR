using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Customer.ViewModels
{
    public class TableResevationCart
    {
        public int TableId { get; set; } 
        public DateTime BookTimeFrom { get; set; }
        public DateTime BookTimeTo { get; set; }
        public DateTime Date { get; set; }
        public Boolean ConfirmStatus { get; set; }
        public int CustomersId { get; set; }
    }
}
