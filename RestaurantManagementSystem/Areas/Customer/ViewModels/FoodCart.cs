using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Customer.ViewModels
{
    public class FoodCart
    {
        public int Quantity { get; set; }
        public int FoodItemId { get; set; }
        public int DiscountId { get; set; }
        public int CustomerOrderedTableId { get; set; }
    
    }
}
