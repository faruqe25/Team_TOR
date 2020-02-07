using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class Ingredient
    {
        [Key]
        public int IngredientId { get; set; }
        public int IngredientName { get; set; }

    }
}
