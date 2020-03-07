﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.Admin.ViewModels;
using RestaurantManagementSystem.Areas.Customer.ViewModels;
using RestaurantManagementSystem.Database;
using RestaurantManagementSystem.Helper;
using RestaurantManagementSystem.Models;

namespace RestaurantManagementSystem.Controllers
{
    public class HomeController : Controller
    {
        public readonly DatabaseContext _context;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public HomeController(DatabaseContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole>roleManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;
           
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
                    Price=item.Price,
                    FoodItemId=item.FoodItemId,
                };
                fooditemvmlist.Add(fooditemvm);
            }
            return View(fooditemvmlist);
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public  async Task< IActionResult> Login(CustomerAccount ct)
        {
            
                var user = await userManager.FindByEmailAsync(ct.Email);
                var result = await signInManager.PasswordSignInAsync(user, ct.Password, true, true);
                            
                if (result.Succeeded)
                {
                  
                    return RedirectToAction("Index");
                }
                else
                {
                    return RedirectToAction("Login");
                    //Asd. asd12345
                }
           
           
        }
        public IActionResult CreateAccount()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CustomerAccount ca)
        {
            var rolelist = await roleManager.RoleExistsAsync("Customer");
            if (rolelist == false)
            {
                IdentityRole role = new IdentityRole
                {
                    Name = "Customer"
                 
                };
                await roleManager.CreateAsync(role);
            }
            if (ModelState.IsValid)
            {
                var user = new IdentityUser
                {
                    UserName = ca.Email,
                    Email = ca.Email,
                    
                };
                var result = await userManager.CreateAsync(user, ca.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(user, "Customer");

                   
                    await signInManager.SignInAsync(user, isPersistent: false);

                     return RedirectToAction("Index");
                }
            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [HttpPost]
        public JsonResult SetCartValue(int id) 
        {
            var food = _context.FoodItems.AsNoTracking().Where(s => s.FoodItemId == id).FirstOrDefault();
            if (food != null)
            {
                 
                var List = HttpContext.Session.Get<List<FoodItem>>("FoodS");
                if (List == null)
                {
                    List = new List<FoodItem>();
                }
                List.Add(food);
                HttpContext.Session.Set("FoodS", List);
            }
            var count = (HttpContext.Session.Get<List<FoodItem>>("FoodS")).Count;

            return Json(count);
        }

        public IActionResult Cart() 
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
