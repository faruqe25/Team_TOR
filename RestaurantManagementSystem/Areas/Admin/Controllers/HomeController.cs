﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Admin.ViewModels;
using RestaurantManagementSystem.Database;

namespace RestaurantManagementSystem.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Route("Admin/[controller]/[action]")]
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
        public IActionResult SetMealHour()
        {
            return View();
        }
        [HttpPost]
        public IActionResult SetMealHour(MealHourVm mealHourVm)
        {
            MealHour m = new MealHour() {
                MealHourId = 0,
                MealHourTitle=mealHourVm.MealHourTitle
          
            };
            _context.MealHour.Add(m);
            _context.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public IActionResult MealHourInfo()
        {
            var s = _context.MealHour.AsNoTracking().ToList();
            var a = new List<MealHourVm>();
            int c = 1;
            foreach (var item in s)
            {
                MealHourVm sp = new MealHourVm() {
                Serial=c,
                MealHourId=item.MealHourId,
                MealHourTitle=item.MealHourTitle
                };
                a.Add(sp);
                c++;

            }
            
            return View(a);
        }
        public IActionResult UpdateMealHour(int id)
        {
            var mealhourVm = _context.MealHour.AsNoTracking()
                 .Where(t => t.MealHourId == id).FirstOrDefault();
            MealHourVm m = new MealHourVm()
            {
                MealHourId = mealhourVm.MealHourId,
                MealHourTitle = mealhourVm.MealHourTitle

            };
            return View(m);
        }
        [HttpPost]
        public IActionResult UpdateMealHour(MealHourVm mealHourVm) 
        {
            MealHour m = new MealHour()
            {
                MealHourId = mealHourVm.MealHourId,
                MealHourTitle = mealHourVm.MealHourTitle

            };
            _context.MealHour.Update(m);
            _context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("MealHourInfo");
        }
        public IActionResult DeleteMealHour(int id)
        {
            var mealhourVm = _context.MealHour.AsNoTracking()
                    .Where(t => t.MealHourId == id).FirstOrDefault();
            _context.MealHour.Remove(mealhourVm);
            _context.SaveChanges();
            return RedirectToAction("MealHourInfo");
        } 
        public IActionResult AddFoodItem()
        {
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
                ToList(), "MealHourId", "MealHourTitle");
            return View();
        }
        [HttpPost]
        public IActionResult AddFoodItem(FoodItemVm a)
        {
            FoodItem p = new FoodItem()
            {
                FoodName=a.FoodName,
                Description=a.Description,
                Price=a.Price,
                FoodItemId=a.FoodItemId,
                MealHourId=a.MealHourId,
            };
            _context.FoodItems.Add(p);
            _context.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public IActionResult FoodItemList()
        {
            var a = _context.FoodItems.AsNoTracking().Include(s => s.MealHour).ToList();
            var s = new List<FoodItemVm>();
            int c = 1;
            foreach (var item in a)
            {
                FoodItemVm ab = new FoodItemVm()
                {
                    Serial = c,
                    FoodName = item.FoodName,
                    MealHourName = item.MealHour.MealHourTitle,
                    Price = item.Price,
                    Description = item.Description,
                    FoodItemId = item.FoodItemId

                };
                s.Add(ab);
                c++;            
            }
            return View(s);
        }
        public IActionResult UpdateFoodItem(int id)  
        {
            var foodItem = _context.FoodItems.AsNoTracking()
                 .Where(t => t.FoodItemId == id).FirstOrDefault(); 
            FoodItemVm m = new FoodItemVm()
            {
                FoodItemId = foodItem.FoodItemId,
                Price = foodItem.Price,
                Description=foodItem.Description,
                MealHourId=foodItem.MealHourId,
                FoodName=foodItem.FoodName

            };
            ViewBag.MealHour = new SelectList(_context.MealHour.AsNoTracking().
                ToList(), "MealHourId", "MealHourTitle");
            return View(m);
        }
        [HttpPost]
        public IActionResult UpdateFoodItem(FoodItemVm a) 
        {
            FoodItem p = new FoodItem()
            {
                FoodName = a.FoodName,
                Description = a.Description,
                Price = a.Price,
                FoodItemId = a.FoodItemId,
                MealHourId = a.MealHourId,


            };
            _context.FoodItems.Update(p);
            _context.SaveChanges();
            ModelState.Clear();
            return RedirectToAction("FoodItemList");
        }
        public IActionResult DeleteFoodItem(int id)
        {
            var foodItem = _context.FoodItems.AsNoTracking()
                 .Where(t => t.FoodItemId == id).FirstOrDefault();
            _context.FoodItems.Remove(foodItem);
            _context.SaveChanges();
            return RedirectToAction("FoodItemList");
        }
    }
}