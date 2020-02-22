using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class FoodItemVm 
    {
        public int Serial { get; set; }
        public int FoodItemId { get; set; }
        public string FoodName { get; set; }
        public float Price { get; set; }
        public string Description { get; set; }
        public int MealHourId { get; set; }
        public string MealHourName  { get; set; }
        
    }
}
