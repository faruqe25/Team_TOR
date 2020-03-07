using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class MealHourVm 
    {
        public int Serial { get; set; } 
        public int MealHourId { get; set; }
        [Required]
        public string MealHourTitle { get; set; }
        
    }
}
