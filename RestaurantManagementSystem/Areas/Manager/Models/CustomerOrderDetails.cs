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
        public int CustomerOrderDetailsID { get; set; }
        public int FoodItemNo { get; set; }
        public int Quantity { get; set; }
        public int CustomerOrderedTableID { get; set; }
        public int OnlineStatus { get; set; }
        public int Discount_ID { get; set; }
    }
}
