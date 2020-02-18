using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class RequiredMaterial
    {
        
        [Key]
        public int RequiredMaterialId { get; set; }
        public int QuantityInGram { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
        public int FoodItemId { get; set; }
        public FoodItem FoodItem { get; set; }
      

    }
}
