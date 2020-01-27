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
        public DateTime StockOutDate { get; set; }
        public DateTime AvailableStock { get; set; }
        public int ProvideableProductId { get; set; }  
        public int EmployeeId { get; set; }
    }
}
