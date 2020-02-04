using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Supplier.Models
{
    public class SalesRecord
    {
        [Key]
        public int SalesRecordID { get; set; }
        public int ProvideableProductID { get; set; }
        public int Quantity { get; set; }
        public double TotalPrice { get; set; }
        public DateTime Date { get; set; }
    }
}
