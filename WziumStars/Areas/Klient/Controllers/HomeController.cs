using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WziumStars.Data;
using WziumStars.Models;
using WziumStars.Utility;

namespace WziumStars.Controllers
{
    [Area("Klient")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if(claim != null)
            {
                var count = _db.Koszyk.Where(c => c.ApplicationUserId == claim.Value).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count);
            }
            else
            {
                string cardId = Request.Cookies["cartId"];
                if(cardId != null)
                {
                    var count = _db.AnonimowyKoszyk.Where(c => c.cartId == cardId).ToList().Count;
                    HttpContext.Session.SetInt32(SD.ssShoppingCartCount, count);
                }
                else
                {
                    HttpContext.Session.SetInt32(SD.ssShoppingCartCount, 0);
                }
                
            }
            return View();
        }

        public IActionResult CheckAnonymousUserOrIdentityUser()
        {
            var claimsIdentity = (ClaimsIdentity)this.User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            if (claim != null)
            {
                return RedirectToAction("Index", "KoszykUzytkownika");
            }
            else
            {
                return RedirectToAction("Index", "Koszyk");
            }
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
