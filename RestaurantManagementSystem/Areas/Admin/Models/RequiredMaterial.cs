using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class RequiredMaterial
    {
        [Key]
        public int RequiredMaterialId { get; set; }
        public string RequiredMaterialName { get; set; }
    }
}
