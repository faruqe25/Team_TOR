using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RestaurantManagementSystem.Areas.Admin.Models
{
    public class FoodItem
    {
        public FoodItem()
        {
            RequiredMaterials = new HashSet<RequiredMaterial>();
        }
        [Key]
        public int FoodItemId { get; set; }
        public string  FoodName { get; set; }
        public float  Price { get; set; }
        public string  Description  { get; set; }
        public int MealHourId { get; set; }
        public MealHour MealHour { get; set; }
        public virtual ICollection<RequiredMaterial> RequiredMaterials { get; set; }
    }
}
