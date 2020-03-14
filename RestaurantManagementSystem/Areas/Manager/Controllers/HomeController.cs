using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Database;

namespace RestaurantManagementSystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/[controller]/[action]")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Receipt()
        {
            return View();
        }
    }
}