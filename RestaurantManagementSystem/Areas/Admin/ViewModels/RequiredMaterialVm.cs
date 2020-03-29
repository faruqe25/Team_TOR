using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class RequiredMaterialVm
    {

        public int Serial { get; set; }
        public int RequiredMaterialId { get; set; }

        [Required]
        public int FoodItemId { get; set; }
       
        public string FoodItemNames { get; set; }
        
        public float Price { get; set; }

        [Required]
        public List<MaterialVm> MaterialVms { get; set; } = new List<MaterialVm>();

    }
}