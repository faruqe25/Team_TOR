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
        public int CustomerOrderedTableId { get; set; }
        public int TableId { get; set; }
        public DateTime BookTimeFrom { get; set; }
        public DateTime BookTimeTo { get; set; }
        public DateTime Date { get; set; }
        public int CustomerId { get; set; }
        public Boolean ConfirmStatus { get; set; }
    }
}
