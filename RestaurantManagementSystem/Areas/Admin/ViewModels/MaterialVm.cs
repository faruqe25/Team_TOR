using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class MaterialVm
    {
        [Required]
        public int QuantityInGram { get; set; }
        [Required]
        public int IngredientId { get; set; }

        public string IngredientName { get; set; }

    }
}