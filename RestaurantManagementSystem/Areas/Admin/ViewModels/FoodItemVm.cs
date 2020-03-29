using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class FoodItemVm 
    {
        public int Serial { get; set; }
        public int FoodItemId { get; set; }
        [Required]
        public string FoodName { get; set; }
        [Required]
        public float Price { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int MealHourId { get; set; }
        public string MealHourName  { get; set; }
        
    }
}
