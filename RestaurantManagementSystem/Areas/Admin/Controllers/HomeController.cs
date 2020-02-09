using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RestaurantManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult SetMealHour()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult SetMealHour()
        //{
        //    return View();
        //}
        public IActionResult MealHourInfo()
        {
            return View();
        }
        public IActionResult AddFoodItem()
        {
            return View();
        }
        //[HttpPost]
        //public IActionResult AddFoodItem()
        //{
        //    return View();
        //}
        public IActionResult FoodItemList()
        {
            return View();
        }
    }
}