using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.ViewModels;
using RestaurantManagementSystem.Database;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public readonly DatabaseContext _context;
        public HomeController(DatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var fooditem = _context.FoodItems.AsNoTracking().Include(q=>q.MealHour).ToList();
            var fooditemvmlist=new List<FoodItemVm>();
            foreach (var item in fooditem)
            {
                FoodItemVm fooditemvm = new FoodItemVm()
                {
                    FoodName = item.FoodName,
                    MealHourName=item.MealHour.MealHourTitle,
                    Description=item.Description,
                    Price=item.Price
                };
                fooditemvmlist.Add(fooditemvm);
            }
            return View(fooditemvmlist);
        }
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
