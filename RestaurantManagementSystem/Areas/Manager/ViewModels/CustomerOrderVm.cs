using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.ViewModels
{
    public class CustomerOrderVm
    {
        public int CustomerOrderDetailsId { get; set; }
        public int Serial { get; set; } 
        public Boolean Approve { get; set; } 
        public Boolean PaymentStatus { get; set; }  
        public string BookFrom { get; set; }
        public string BookTo { get; set; }
        public string TableName { get; set; } 
        public string CustomerName { get; set; } 
        public string Mobile { get; set; } 
        
    }
}
