﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class IngredientVm
    {
        public int Serial { get; set; } 
        public int IngredientId { get; set; }
        [Required]
        public string IngredientName { get; set; }
    }
}
