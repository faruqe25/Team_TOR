﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class MealHour
    {
        [Key]
        public int MealHourId { get; set; }
        public int MealHourTitle { get; set; }
        public int Title { get; set; } 
    }
}
