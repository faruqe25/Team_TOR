using RestaurantManagementSystem.Areas.StockManager.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class Ingredient
    {
        public Ingredient()
        {
            StockDetails = new HashSet<StockDetails>();
        }
        [Key]
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public virtual ICollection<StockDetails> StockDetails { get; set; }

    }
}
