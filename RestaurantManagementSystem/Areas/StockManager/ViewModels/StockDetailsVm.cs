using RestaurantManagementSystem.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.StockManager.ViewModels
{
    public class StockDetailsVm
    {
        public int Serial { get; set; }
        public int StockDetailsId { get; set; }
        public int Quantity { get; set; }
        public DateTime StockInDate { get; set; }
        public DateTime AvailableStock { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
