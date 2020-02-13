using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.ViewModels
{
    public class RequiredMaterialVm
    {
        public int RequiredMaterialId { get; set; }
       
        public int FoodItemId { get; set; }
        public List<MaterialVm> MaterialVms { get; set; } 
    }
}
