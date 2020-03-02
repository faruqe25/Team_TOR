﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Admin.Models;
using RestaurantManagementSystem.Areas.StockManager.Models;
using RestaurantManagementSystem.Areas.StockManager.ViewModels;
using RestaurantManagementSystem.Database;
using X.PagedList;

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
            var stck = _context.StockDetails.AsNoTracking().Where(s => s.IngredientId == stockdetailsvm.IngredientId).LastOrDefault();
            if (stck != null)
            {
                
                stockdetailsvm.AvailableStock = stockdetailsvm.Quantity * 1000 + stck.AvailableStock;
            }
            else
            {
                stockdetailsvm.AvailableStock = stockdetailsvm.Quantity * 1000;
            }
            var ingredientlist = _context.Ingredient.ToList();
            ingredientlist.Insert(0, new Ingredient { IngredientId = 0, IngredientName = "Select Raw Ingredient" });
            ViewBag.IngredientList = ingredientlist;

            StockDetails stockdetails = new StockDetails()
            {
                IngredientId = stockdetailsvm.IngredientId,
                Quantity = stockdetailsvm.Quantity,
                StockInDate = DateTime.Now,
                AvailableStock= stockdetailsvm.AvailableStock,
            };
           
            _context.StockDetails.Add(stockdetails);
            _context.SaveChanges();
            ModelState.Clear();
            return View();
        }
        public IActionResult StockDetails(int Page = 1)
        {
            var list = _context.StockDetails.AsNoTracking().Include(a=>a.Ingredient).ToList();
            var sent = new List<StockDetailsVm>();
            int c = 1;
            foreach (var item in list)
            {
                StockDetailsVm a = new StockDetailsVm()
                {
                    StockDetailsId = item.StockDetailsId,
                    Serial = c,
                    Quantity=item.Quantity,
                    AvailableStock=item.AvailableStock/1000,
                    IngredientName=item.Ingredient.IngredientName,
                    StockInDate=item.StockInDate
                
                };
                sent.Add(a);
                c++;

                
            }
            var list1 = sent.ToPagedList(Page, 5);
            return View(list1);
        }
        public IActionResult UpdateStock(int id)
        {
            var item = _context.StockDetails. 
                AsNoTracking().Where(a => a.StockDetailsId==id).FirstOrDefault();

            ViewBag.IngredientList = new SelectList(_context.Ingredient.ToList(), "IngredientId", "IngredientName");
            StockDetailsVm data = new StockDetailsVm()
            {
                StockDetailsId = item.StockDetailsId,
               
                Quantity = item.Quantity,
               
                IngredientId = item.IngredientId,
                StockInDate = item.StockInDate

            };
            return View(data);
        }
        [HttpPost]
        public IActionResult UpdateStock(StockDetailsVm stockdetailsvm)
        {
            StockDetails stockdetails = new StockDetails()
            {   
                StockDetailsId=stockdetailsvm.StockDetailsId,
                IngredientId = stockdetailsvm.IngredientId,
                Quantity = stockdetailsvm.Quantity,
                StockInDate = DateTime.Now,
                AvailableStock = stockdetailsvm.Quantity * 1000,
            };
            _context.StockDetails.Update(stockdetails);
            _context.SaveChanges();
            return RedirectToAction("StockDetails");
        }
    }
}