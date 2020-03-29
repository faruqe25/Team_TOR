using RestaurantManagementSystem.Areas.Admin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.StockManager.ViewModels
{
    public class StockDetailsVm
    {
        public int Serial { get; set; }
       
        public int StockDetailsId { get; set; }
        [Required]
        public int Quantity { get; set; }
        public DateTime StockInDate { get; set; }
        public double AvailableStock { get; set; }
        [Required]
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
     
    }
}
