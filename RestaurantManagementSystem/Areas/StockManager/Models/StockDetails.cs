using RestaurantManagementSystem.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.StockManager.Models
{
    public class StockDetails
    {
        [Key]
        public int StockDetailsId { get; set; }
        public int Quantity { get; set; }

        public DateTime StockInDate { get; set; }
        public Double AvailableStock { get; set; }
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; } 
    }
}
