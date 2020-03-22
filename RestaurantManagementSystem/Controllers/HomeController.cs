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

using RestaurantManagementSystem.Areas.Customer.Models;

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
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;

        }

        public IActionResult Index()
        {
            var fooditem = _context.FoodItems.AsNoTracking().Include(q => q.MealHour).ToList();
            var fooditemvmlist = new List<FoodItemVm>();
            foreach (var item in fooditem)
            {

                FoodItemVm fooditemvm = new FoodItemVm()
                {
                    FoodName = item.FoodName,
                    MealHourName = item.MealHour.MealHourTitle,
                    Description = item.Description,
                    Price = item.Price,
                    FoodItemId = item.FoodItemId,
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

        public async Task<IActionResult> Login(CustomerAccount ct)
        {

            var user = await userManager.FindByEmailAsync(ct.Email);
            var result = await signInManager.PasswordSignInAsync(user, ct.Password, true, true);

            if (result.Succeeded)
            {

                if (await userManager.IsInRoleAsync(user, "Admin") == true)
                {
                    return RedirectToAction("Index", "Home", new { area = "Admin" });
                }
                else if (await userManager.IsInRoleAsync(user, "Customer") == true)
                {
                    return RedirectToAction("Index");
                }

            }

            return RedirectToAction("Login");



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
                    PhoneNumber = ca.MobileNumber

                };
                var result = await userManager.CreateAsync(user, ca.Password);
                if (result.Succeeded)
                {


                    Customers cs = new Customers
                    {
                        CustomersId = 0,
                        MobileNumber = ca.MobileNumber,
                        PaymentMobileNumber = ca.PaymentMobileNumber,
                        CustomersName = ca.CustomersName
                    };

                    await _context.Customers.AddAsync(cs);
                    await _context.SaveChangesAsync();
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
            var foodDetails = _context.FoodItems.AsNoTracking().Where(s => s.FoodItemId == id).FirstOrDefault();
            var food = new FoodCart
            {
                FoodItemId = id,
                Quantity = 1,
                FoodName = foodDetails.FoodName,
                FoodPrice = foodDetails.Price,
                FoodDescription = foodDetails.Description
            };

            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            if (List == null)
            {
                List = new List<FoodCart>();
                List.Add(food);
            }
            else
            {

                var exist = List.Where(a => a.FoodItemId == id).FirstOrDefault();
                if (exist != null)
                {

                    food.Quantity = exist.Quantity + 1;
                    food.FoodItemId = id;
                    food.FoodPrice = food.FoodPrice;
                    List.Remove(exist);
                    List.Add(food);

                }
                else
                {
                    List.Add(food);
                }

            }


            HttpContext.Session.Set("FoodS", List);

            var count = HttpContext.Session.Get<List<FoodCart>>("FoodS").Sum(t => t.Quantity);

            return Json(count);
        }
        public JsonResult SetCartValueUpdated(int id, int Quantity)
        {


            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var up = List.Where(s => s.FoodItemId == id).FirstOrDefault();
            var food = new FoodCart
            {
                FoodItemId = id,
                Quantity = Quantity,
                FoodName = up.FoodName,
                FoodPrice = up.FoodPrice,
                FoodDescription = up.FoodDescription
            };
            List.Remove(up);
            List.Add(food);

            HttpContext.Session.Set("FoodS", List);
            var total = List.Sum(s => s.FoodPrice * s.Quantity);

            return Json(total);
        }

        public JsonResult DeleteCart(int id)
        {
            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var up = List.Where(s => s.FoodItemId == id).FirstOrDefault();
            List.Remove(up);
            HttpContext.Session.Set("FoodS", List);
            if (List != null)
            {
                var total = List.Sum(s => s.FoodPrice * s.Quantity);

                return Json(total);

            }
            return Json(0);
        }
        public JsonResult GetCartValueTotal()
        {
            var List = HttpContext.Session.Get<List<FoodCart>>("FoodS");
            var count = 0;
            if (List == null)
            {
                count = 0;
            }
            else
            {
                count = List.Count();
            }

            return Json(count);
        }

        public async Task<IActionResult> Cart()
        {
            var Exists = HttpContext.Session.Get<List<TableResevationCart>>("AvailableTable");
            if (Exists != null)
            {
                HttpContext.Session.Remove("AvailableTable");
            }
            var TableAvailableList = await _context.Table.AsNoTracking().Where(a => a.TableId != 1).ToListAsync();

            if (TableAvailableList.Count() != 0)
            {
                var Tables = new List<TableResevationCart>();
                foreach (var item in TableAvailableList)
                {
                    TableResevationCart tableResevationCart = new TableResevationCart()
                    {
                        TableId = item.TableId,
                        BookedStatus = item.BookedStatus,
                        TableName = item.TableNumber
                    };
                    Tables.Add(tableResevationCart);
                }
                HttpContext.Session.Set("AvailableTable", Tables);
            }



            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}