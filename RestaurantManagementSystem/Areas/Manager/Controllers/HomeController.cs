using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.Areas.Manager.Models;
using RestaurantManagementSystem.Areas.Manager.ViewModels;
using RestaurantManagementSystem.Database;
using RestaurantManagementSystem.Helper;
using X.PagedList;

namespace RestaurantManagementSystem.Areas.Manager.Controllers
{
    [Area("Manager")]
    [Route("Manager/[controller]/[action]")]
    [Authorize(Roles = "Manager")]
    public class HomeController : Controller
    {
        private readonly DatabaseContext _context;

        public HomeController(DatabaseContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            
            var SellSRecord = from sells in await _context.CustomerOrderDetails.AsNoTracking()
                                    .Include(s=>s.CustomerOrderedTable).Include(s=>s.FoodItem)
                                    .Where(s => s.PaymentStatus == true).
                                    Where(a=>a.CustomerOrderedTable.Date.Month==DateTime.Now.Month)
                                    .ToListAsync()
                                    group sells by
                                    sells.CustomerOrderedTable.Date.Day into p
                                    let temp = (
                                          from val in p
                                          select new
                                          {
                                               Total = p.Sum(s => s.Quantity*s.FoodItem.Price),
                                               Day=p.Key
                                          }
                                           )
                                    select temp;
            List<float> TotalSells = new List<float>();
            List<string> Day = new List<string>();
            var s = new List<TempSell>();

            foreach (var item in SellSRecord)
            {

                foreach (var t in item)
                {
                    var p = new TempSell()
                    {
                        Day = t.Day,
                        Total = t.Total
                    };
                    s.Add(p);
                    break;
                }
            }
            var data = s.OrderBy(s => s.Day);
            for (int i = 0; i < 31; i++)
            {
                TotalSells.Add(0);
            }
            foreach (var item in data)
            {
                var index = item.Day - 1;
                var total = item.Total;
                TotalSells[index] = total;
            }
            

            ViewBag.TotalFoodSells = TotalSells;




            return View();
        }
        public async Task<JsonResult> OrderApproved(int id)
        {
            var Table = await _context.CustomerOrderDetails
                        .AsNoTracking().Where(s => s.CustomerOrderDetailsId == id)
                        .FirstOrDefaultAsync();
            var CustomerOrderTables = await _context.CustomerOrderedTable.AsNoTracking().
                          Where(s => s.CustomerOrderedTableId == Table.CustomerOrderedTableId)
                          .FirstOrDefaultAsync();
            var tb = await _context.Table.AsNoTracking()
                   .Where(s => s.TableId == CustomerOrderTables.TableId)
                   .FirstOrDefaultAsync();
            tb.BookedStatus = true;
            CustomerOrderTables.ConfirmStatus = true;
            _context.Table.Update(tb);
            await _context.SaveChangesAsync();
            _context.CustomerOrderedTable.Update(CustomerOrderTables);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<JsonResult> OrderApprovedWithoutTable(int id, string Seat)
        {
            //HttpContext.Session.Remove("TempTableSeat");
            var list = HttpContext.Session.Get<List<TempTable>>("TempTableSeat");
            if (list == null)
            {
                list = new List<TempTable>();
                TempTable tbl = new TempTable() { Id = id, SeatName = Seat };
                list.Add(tbl);
            }
            else
            {
                TempTable tbl = new TempTable() { Id = id, SeatName = Seat };
                list.Add(tbl);
            }

            HttpContext.Session.Set("TempTableSeat", list);
            var Table = await _context.CustomerOrderDetails
                        .AsNoTracking().Where(s => s.CustomerOrderDetailsId == id)
                        .FirstOrDefaultAsync();
            var CustomerOrderTables = await _context.CustomerOrderedTable.AsNoTracking().
                          Where(s => s.CustomerOrderedTableId == Table.CustomerOrderedTableId)
                          .FirstOrDefaultAsync();
            var tb = await _context.Table.AsNoTracking()
                   .Where(s => s.TableId == CustomerOrderTables.TableId)
                   .FirstOrDefaultAsync();
            tb.BookedStatus = false;
            CustomerOrderTables.ConfirmStatus = true;
            _context.Table.Update(tb);
            await _context.SaveChangesAsync();
            _context.CustomerOrderedTable.Update(CustomerOrderTables);
            await _context.SaveChangesAsync();
            return Json(true);
        }
        public async Task<IActionResult> OrderDetails(int Page = 1)
        {
            var data = await _context.CustomerOrderDetails.AsNoTracking().
                        Include(s => s.CustomerOrderedTable).ThenInclude(a => a.Table)
                        .Include(a => a.CustomerOrderedTable).ThenInclude(a => a.Customers).ToListAsync();
            data = data.GroupBy(p => p.CustomerOrderedTableId)
              .Select(g => g.First())
              .ToList();
            var sent = new List<CustomerOrderVm>();
            int c = 1;
            foreach (var item in data)
            {
                var temp = new CustomerOrderVm()
                {
                    BookFrom = item.CustomerOrderedTable.BookTimeFrom.ToShortTimeString(),
                    BookTo = item.CustomerOrderedTable.BookTimeTo.ToShortTimeString(),
                    CustomerName = item.CustomerOrderedTable.Customers.CustomersName,
                    Mobile = item.CustomerOrderedTable.Customers.MobileNumber,
                    TableName = item.CustomerOrderedTable.Table.TableNumber,
                    CustomerOrderDetailsId = item.CustomerOrderDetailsId,
                    Serial = c,
                    PaymentStatus = item.PaymentStatus,


                };
                if (item.CustomerOrderedTable.ConfirmStatus == false)
                {
                    temp.Approve = false;
                }
                else
                {
                    temp.Approve = true;

                }
                sent.Add(temp);
                c++;


            }
            var list = sent.OrderByDescending(s => s.CustomerOrderDetailsId).ToPagedList(Page, 5);
            return View(list);

        }
        public async Task<IActionResult> Receipt(int id)
        {
            var List = HttpContext.Session.Get<List<TempTable>>("TempTableSeat");
            if (List != null)
            {
                var ShouldDllt = List.Where(s => s.Id == id).FirstOrDefault();
                List.Remove(ShouldDllt);
            }
            HttpContext.Session.Set("TempTableSeat", List);
            var OrderDetails = await _context.CustomerOrderDetails.AsNoTracking()
                .Where(s => s.CustomerOrderDetailsId == id).LastOrDefaultAsync();
            var FinalOrderList = await _context.CustomerOrderDetails.AsNoTracking().
                Where(a => a.CustomerOrderedTableId == OrderDetails.CustomerOrderedTableId).
                Include(p=>p.FoodItem).Include(s=>s.Offer).Include(s=>s.CustomerOrderedTable).ToListAsync();
            var up =await FinalOrderList.Select(s => { s.PaymentStatus = true; return s; }).ToListAsync();

            _context.CustomerOrderDetails.UpdateRange(up);
            await _context.SaveChangesAsync();
            var tableinfo = await _context.CustomerOrderedTable
                .Where(s => s.CustomerOrderedTableId
                == OrderDetails.CustomerOrderedTableId)
                .AsNoTracking().FirstOrDefaultAsync();
            var updateTable = await _context.Table.AsNoTracking().
                Where(s => s.TableId == tableinfo.TableId).FirstOrDefaultAsync();
            updateTable.BookedStatus = false;
            _context.Table.Update(updateTable);
            await _context.SaveChangesAsync();
            List<Invoice> sent = new List<Invoice>();
            foreach (var item in FinalOrderList)
            {
                Invoice a = new Invoice()
                {

                    FoodName = item.FoodItem.FoodName,
                    Quantity = item.Quantity,
                    Price = item.FoodItem.Price,
                    Total = (item.Quantity * item.FoodItem.Price),
                    TablePrice=item.CustomerOrderedTable.Table.BookingPrice,
                };
                if (String.IsNullOrEmpty(item.OfferId.ToString()))
                {
                    a.Discount = 0;
                    a.Coupone = " ";
                }
                else
                {
                    a.Discount = item.Offer.Discount;
                    a.Coupone = item.Offer.Coupon;
                }
               
                sent.Add(a);
            }


            return View(sent);
        }
        public async Task< IActionResult> Sells(int Page=1)
        {
            var se =await  _context.CustomerOrderDetails.AsNoTracking()
                .Include(s => s.CustomerOrderedTable).
                ThenInclude(s => s.Customers).
                Include(s => s.FoodItem).Where(s=>s.PaymentStatus==true).ToListAsync();
            var sent = new List<TempSell>();
            foreach (var item in se.OrderByDescending(aa=>aa.CustomerOrderDetailsId))
            {
                var a = new TempSell()
                {
                    Date=item.CustomerOrderedTable.Date,
                    Total=item.FoodItem.Price*item.Quantity,
                    FoodName=item.FoodItem.FoodName,
                    Quantity=item.Quantity,
                    FoodPrice=item.FoodItem.Price,
                    CustomerName=item.CustomerOrderedTable.Customers.CustomersName
                
                
                };
                sent.Add(a);
            }
            var list = sent.ToPagedList(Page, 5);
            return View(list);
            
        }
    }
}