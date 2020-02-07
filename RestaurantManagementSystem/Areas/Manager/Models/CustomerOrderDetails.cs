using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.Models
{
    public class CustomerOrderDetails
    {
        [Key]
        public int CustomerOrderDetailsId { get; set; }
        public int FoodItemNo { get; set; }
        public int Quantity { get; set; }
        public int CustomerOrderedTableId { get; set; }
        public Boolean OnlineStatus { get; set; }
        public int DiscountId { get; set; }
    }
}
