using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Supplier.Models
{
    public class ProvideableProduct
    {
        [Key]
        public int ProvideableProductID { get; set; }
        public int SupplierID { get; set; }
        public int RequiredMaterialID { get; set; }
        public double Rate { get; set; }
    }
}
