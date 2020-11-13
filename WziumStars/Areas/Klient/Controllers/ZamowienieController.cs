using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WziumStars.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WziumStars.Models;
using WziumStars.Models.ViewModels;
using WziumStars.Utility;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace WziumStars.Areas.Klient.Controllers
{
    [Area("Klient")]
    public class ZamowienieController : Controller
    {
        private ApplicationDbContext _db;
        private int PageSize = 15;
        private readonly IEmailSender _emailSender;
        public ZamowienieController(ApplicationDbContext db, IEmailSender emailSender)
        {
            _db = db;
            _emailSender = emailSender;
        }

        [Authorize]
        public async Task<IActionResult> PotwierdzenieLog(int id)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderHeader OrderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value);

            string PayUId = OrderHeader.PayUId;

            string jsonTokenString = await PayU.GetAccessToken();
            JToken jsonToken = JObject.Parse(jsonTokenString);
            string accessToken = jsonToken.Value<string>("access_token");
            string tokenType = jsonToken.Value<string>("token_type");

            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", tokenType + " " + accessToken);

                using (var response = await httpClient.GetAsync("api/v2_1/orders/" + PayUId + ""))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseData);
                    string status = obj.status.statusCode;

                    if (status != "SUCCESS")
                    {
                        OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                    }
                    else
                    {
                        await _emailSender.SendEmailAsync(OrderHeader.Email, "Wzium Stars - zamówienie nr" + OrderHeader.Id.ToString() + " przyjęte ", "Zamówienie zostało opłacone oraz przyjęte do realizacji.");

                        OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                        OrderHeader.Status = SD.StatusSubmitted;
                    }
                    await _db.SaveChangesAsync();
                }
            }

            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = await _db.OrderHeader.Include(o => o.ApplicationUser).FirstOrDefaultAsync(o => o.Id == id && o.UserId == claim.Value),
                OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == id).ToListAsync()
            };
            return View(orderDetailsViewModel);
        }

        public async Task<IActionResult> Potwierdzenie(int id)
        {
            OrderHeader OrderHeader = await _db.OrderHeader.FirstOrDefaultAsync(o => o.Id == id);

            string PayUId = OrderHeader.PayUId;

            string jsonTokenString = await PayU.GetAccessToken();
            JToken jsonToken = JObject.Parse(jsonTokenString);
            string accessToken = jsonToken.Value<string>("access_token");
            string tokenType = jsonToken.Value<string>("token_type");

            var baseAddress = new Uri("https://secure.snd.payu.com/");

            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("authorization", tokenType + " " + accessToken);

                using (var response = await httpClient.GetAsync("api/v2_1/orders/" + PayUId + ""))
                {
                    string responseData = await response.Content.ReadAsStringAsync();

                    dynamic obj = JsonConvert.DeserializeObject<dynamic>(responseData);
                    string status = obj.status.statusCode;

                    if (status != "SUCCESS")
                    {
                        OrderHeader.PaymentStatus = SD.PaymentStatusRejected;
                    }
                    else
                    {
                        await _emailSender.SendEmailAsync(OrderHeader.Email, "Wzium Stars - zamówienie nr" + OrderHeader.Id.ToString() + " przyjęte ", "Zamówienie zostało opłacone oraz przyjęte do realizacji.");

                        OrderHeader.PaymentStatus = SD.PaymentStatusApproved;
                        OrderHeader.Status = SD.StatusSubmitted;
                    }
                    await _db.SaveChangesAsync();
                }
            }
            return View();
        }

        public IActionResult Index()
        {
            return View();
        }

        [Authorize]
        public async Task<IActionResult> HistoriaZamowien(int productPage = 1)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };

            List<OrderHeader> OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.UserId == claim.Value).ToListAsync();

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderListVM.Orders.Add(individual);
            }
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(p => p.OrderHeader.Id).Skip((productPage - 1) * PageSize).Take(PageSize).ToList();

            orderListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParm = "/Klient/Zamowienie/HistoriaZamowien?productPage=:"
            };

            return View(orderListVM);
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> Zarzadzaj(int productPage = 1, string searchEmail = null, string searchPhone = null, string searchId = null)
        {
            OrderListViewModel orderListVM = new OrderListViewModel()
            {
                Orders = new List<OrderDetailsViewModel>()
            };

            StringBuilder parm = new StringBuilder();
            parm.Append("/Klient/Zamowienie/Zarzadzaj?productPage=:");
            parm.Append("&searchId=");
            if (searchId != null)
            {
                parm.Append(searchId);
            }
            parm.Append("&searchEmail=");
            if (searchEmail != null)
            {
                parm.Append(searchEmail);
            }
            parm.Append("&searchPhone=");
            if (searchPhone != null)
            {
                parm.Append(searchPhone);
            }
            List<OrderHeader> OrderHeaderList = new List<OrderHeader>();
            if (searchId != null || searchEmail != null || searchPhone != null)
            {
                var user = new ApplicationUser();

                if (searchId != null)
                {
                    int searchIdInt = Convert.ToInt32(searchId);
                    OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.Id == searchIdInt).OrderByDescending(o => o.OrderDate).ToListAsync();
                }
                else
                {
                    if (searchEmail != null)
                    {
                        user = await _db.ApplicationUser.Where(u => u.Email.ToLower().Contains(searchEmail.ToLower())).FirstOrDefaultAsync();
                        OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(o => o.UserId == user.Id).OrderByDescending(o => o.OrderDate).ToListAsync();
                    }
                    else
                    {
                        if (searchPhone != null)
                        {
                            OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.PhoneNumber.
                            Contains(searchPhone)).OrderByDescending(o => o.OrderDate).ToListAsync();
                        }
                    }
                }
            }
            else
            {
                OrderHeaderList = await _db.OrderHeader.Include(o => o.ApplicationUser).Where(u => u.Status == SD.PaymentStatusPending || u.Status == SD.StatusInProcess || u.Status == SD.StatusSent).ToListAsync();
            }

            foreach (OrderHeader item in OrderHeaderList)
            {
                OrderDetailsViewModel individual = new OrderDetailsViewModel
                {
                    OrderHeader = item,
                    OrderDetails = await _db.OrderDetails.Where(o => o.OrderId == item.Id).ToListAsync()
                };
                orderListVM.Orders.Add(individual);
            }
            var count = orderListVM.Orders.Count;
            orderListVM.Orders = orderListVM.Orders.OrderByDescending(p => p.OrderHeader.Id).Skip((productPage - 1) * PageSize).Take(PageSize).ToList();

            orderListVM.PagingInfo = new PagingInfo
            {
                CurrentPage = productPage,
                ItemsPerPage = PageSize,
                TotalItem = count,
                urlParm = "/Klient/Zamowienie/Zarzadzaj?productPage=:"
            };

            return View(orderListVM);
        }


        public async Task<IActionResult> GetOrderDetails(int Id)
        {
            OrderDetailsViewModel orderDetailsViewModel = new OrderDetailsViewModel()
            {
                OrderHeader = await _db.OrderHeader.FirstOrDefaultAsync(m => m.Id == Id),
                OrderDetails = await _db.OrderDetails.Where(m => m.OrderId == Id).ToListAsync()
            };
            orderDetailsViewModel.OrderHeader.ApplicationUser = await _db.ApplicationUser.FirstOrDefaultAsync(u => u.Id == orderDetailsViewModel.OrderHeader.UserId);
            return PartialView("_IndividualOrderDetails", orderDetailsViewModel);
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderPrepare(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusInProcess;
            await _db.SaveChangesAsync();
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().
                  Email, "Wzium Stars - Zamówienie nr " + orderHeader.Id.ToString() + " w produkcji",
                  "Twoje zamówienie własnie jest przygotowywane.");
            return RedirectToAction("Zarzadzaj", "Zamowienie");
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderSent(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusSent;
            await _db.SaveChangesAsync();
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().
                   Email, "Wzium Stars - Zamówienie nr " + orderHeader.Id.ToString() + " zostało wysłane",
                   "Twoje zamówienie zostało wysłane.");
            return RedirectToAction("Zarzadzaj", "Zamowienie");
        }

        [Authorize(Roles = SD.AdminEndUser)]
        public async Task<IActionResult> OrderCancel(int OrderId)
        {
            OrderHeader orderHeader = await _db.OrderHeader.FindAsync(OrderId);
            orderHeader.Status = SD.StatusCancelled;
            await _db.SaveChangesAsync();
            await _emailSender.SendEmailAsync(_db.Users.Where(u => u.Id == orderHeader.UserId).FirstOrDefault().
                    Email, "Wzium Stars - Anulowanie zamówienia nr " + orderHeader.Id.ToString(),
                    "Twoje zamówienie zostało anulowane.");
            return RedirectToAction("Zarzadzaj", "Zamowienie");
        }
    }
}
