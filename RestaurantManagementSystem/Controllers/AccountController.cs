using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Customer.Models;
using RestaurantManagementSystem.Areas.Customer.ViewModels;
using RestaurantManagementSystem.Database;

namespace RestaurantManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        public readonly DatabaseContext _context;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public AccountController(DatabaseContext context,
            SignInManager<IdentityUser> signInManager,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.roleManager = roleManager;

        }
        [AcceptVerbs("Get", "Post")]
        public async Task<JsonResult> IsEmailInUse(string email)
        {
            var user =await userManager.FindByEmailAsync(email);
            if (user!=null){
                return Json($"Email {email} already in use");
            }
            else
            {
                return Json(true);

            }
        }
        [AcceptVerbs("Get","Post")]
        public async Task<JsonResult> IsNumberInUse(string MobileNumber) 
        {
            var user = await _context.Users.AsNoTracking()
                .Where(s => s.PhoneNumber == MobileNumber).FirstOrDefaultAsync();
            if (user != null)
            {
                return Json($"Number {MobileNumber} already in use");
            }
            else
            {
                return Json(true);

            }
        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]

        public async Task<IActionResult> Login(Login ct)
        {

            var user = await userManager.FindByEmailAsync(ct.Email);
            if (user == null)
            {
                ViewBag.Message = "User name  is wrong";
                return View();
            }
            else
            {
                var result = await signInManager.PasswordSignInAsync(user, ct.Password, true, true);

                if (result.Succeeded)
                {

                    if (await userManager.IsInRoleAsync(user, "Admin") == true)
                    {
                        
                        
                            return RedirectToAction("Index", "Home", new { area = "Admin" });

                       
                    }
                    else if (await userManager.IsInRoleAsync(user, "Manager") == true)
                    {
                        return RedirectToAction("Index", "Home", new { area = "Manager" });
                    }
                    else if (await userManager.IsInRoleAsync(user, "Customer") == true)
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else if (await userManager.IsInRoleAsync(user, "StockManager") == true)
                    {
                        return RedirectToAction("Index", "Home", new { area = "StockManager" });
                    }

                }
                else
                {



                    ViewBag.Message = "Password  is wrong";
                    return View();

                }
            }
            return View();
        }
        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
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
                       
                        CustomersName = ca.CustomersName
                    };

                    await _context.Customers.AddAsync(cs);
                    await _context.SaveChangesAsync();
                    await userManager.AddToRoleAsync(user, "Customer");
                    await signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index","Home");
                }

            }

            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");

        }

    }
}