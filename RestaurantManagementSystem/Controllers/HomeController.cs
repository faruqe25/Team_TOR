using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
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
