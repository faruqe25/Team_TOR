using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Manager.Models
{
    public class CustomerOrderedTable
    {
        [Key]
        public int CustomerOrderedTableID { get; set; }
        public int TableID { get; set; }
        public DateTime BookTimeFrom { get; set; }
        public DateTime BookTimeTo { get; set; }
        public DateTime Date { get; set; }
        public int CustomerID { get; set; }
        public int ConfirmStatus { get; set; }
    }
}
