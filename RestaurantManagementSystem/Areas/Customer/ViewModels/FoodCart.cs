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
        public string FoodName { get; set; }
        public string FoodDescription { get; set; }
        public float FoodPrice { get; set; }
      

    }
}
