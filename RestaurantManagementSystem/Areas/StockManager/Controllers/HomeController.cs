using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
using RestaurantManagementSystem.Areas.StockManager.ViewModels;
using RestaurantManagementSystem.Database;

namespace RestaurantManagementSystem.Areas.StockManager.Controllers
{
    [Area("StockManager")]
    [Route("StockManager/[controller]/[action]")]
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
        public IActionResult AddStock()
        {
            var ingredientlist = _context.Ingredient.ToList();
            ingredientlist.Insert(0, new Ingredient { IngredientId = 0, IngredientName = "Select Raw Ingredient" });
            ViewBag.IngredientList = ingredientlist;
            return View();
        }
        [HttpPost]
        public IActionResult AddStock(StockDetailsVm stockdetailsvm)
        {
            var ingredientlist = _context.Ingredient.ToList();
            ingredientlist.Insert(0, new Ingredient { IngredientId = 0, IngredientName = "Select Raw Ingredient" });
            ViewBag.IngredientList = ingredientlist;

            StockDetails stockdetails = new StockDetails()
            {
                IngredientId = stockdetailsvm.IngredientId,
                Quantity = stockdetailsvm.Quantity,
                StockInDate = DateTime.Now
            };
            _context.StockDetails.Add(stockdetails);
            _context.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public IActionResult StockDetails()
        {
            return View();
        }
    }
}