using RestaurantManagementSystem.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.Models
{
    public class CustomerOrderDetails
    {
        [Key]
        public int CustomerOrderDetailsId { get; set; }
        public int Quantity { get; set; }
        public Boolean PaymentStatus { get; set; } 
        public int FoodItemId { get; set; } 
        public FoodItem FoodItem { get; set; }
        public int? OfferId { get; set; }
        public Offer Offer { get; set; }
        public int? CustomerOrderedTableId { get; set; }
        public CustomerOrderedTable CustomerOrderedTable { get; set; } 
    }
}
