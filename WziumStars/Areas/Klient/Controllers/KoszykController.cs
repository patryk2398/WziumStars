using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using WziumStars.Data;
using WziumStars.Models;
using WziumStars.Models.ViewModels;
using WziumStars.Utility;

namespace WziumStars.Areas.Klient.Controllers
{
    [Area("Klient")]
    public class KoszykController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public OrderDetailsAnonimowyKoszyk detailsCard { get; set; }

        public KoszykController(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            detailsCard = new OrderDetailsAnonimowyKoszyk()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailsCard.OrderHeader.OrderTotal = 0;

            string cardId = Request.Cookies["cartId"];

            var cart = _db.AnonimowyKoszyk.Where(c => c.cartId == cardId);
            if (cart != null)
            {
                detailsCard.listCart = cart.ToList();

            }

            foreach (var list in detailsCard.listCart)
            {
                list.Produkt = await _db.Produkt.FirstOrDefaultAsync(m => m.Id == list.ProduktId);
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotal + (list.Produkt.Price * list.Count);
            }
            detailsCard.OrderHeader.OrderTotalOriginal = detailsCard.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Kupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }

            if (HttpContext.Session.GetString(SD.ssDelivery) != null)
            {
                detailsCard.OrderHeader.DeliveryMethod = HttpContext.Session.GetString(SD.ssDelivery);
                if (detailsCard.OrderHeader.DeliveryMethod == "poczta")
                {
                    detailsCard.OrderHeader.DeliveryCost = 9;
                }
                else if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
                {
                    detailsCard.OrderHeader.DeliveryCost = 12;
                }
                else
                {
                    detailsCard.OrderHeader.DeliveryCost = 15;
                }
            }
            else
            {
                HttpContext.Session.SetString(SD.ssDelivery, "poczta");
                detailsCard.OrderHeader.DeliveryMethod = "poczta";
                detailsCard.OrderHeader.DeliveryCost = 9;
            }

            if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
            {
                if (HttpContext.Session.GetString(SD.ssPaczkomat) != null)
                {
                    detailsCard.OrderHeader.Paczkomat = HttpContext.Session.GetString(SD.ssPaczkomat);
                }
            }

            detailsCard.OrderHeader.CouponCodeDiscount = detailsCard.OrderHeader.OrderTotalOriginal - detailsCard.OrderHeader.OrderTotal;
            detailsCard.OrderHeader.OrderTotal += detailsCard.OrderHeader.DeliveryCost;
            return View(detailsCard);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPOST()
        {
            if(detailsCard.OrderHeader.Paczkomat != null)
            {
                HttpContext.Session.SetString(SD.ssPaczkomat, detailsCard.OrderHeader.Paczkomat);
            }
            return RedirectToAction("Podsumowanie");
        }

        public async Task<IActionResult> Podsumowanie()
        {
            detailsCard = new OrderDetailsAnonimowyKoszyk()
            {
                OrderHeader = new Models.OrderHeader()
            };
            detailsCard.OrderHeader.OrderTotal = 0;

            string cardId = Request.Cookies["cartId"];

            var cart = _db.AnonimowyKoszyk.Where(c => c.cartId == cardId);
            if (cart != null)
            {
                detailsCard.listCart = cart.ToList();
            }

            foreach (var list in detailsCard.listCart)
            {
                list.Produkt = await _db.Produkt.FirstOrDefaultAsync(m => m.Id == list.ProduktId);
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotal + (list.Produkt.Price * list.Count);
            }
            detailsCard.OrderHeader.OrderTotalOriginal = detailsCard.OrderHeader.OrderTotal;

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Kupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }

            if (HttpContext.Session.GetString(SD.ssDelivery) != null)
            {
                detailsCard.OrderHeader.DeliveryMethod = HttpContext.Session.GetString(SD.ssDelivery);
                if (detailsCard.OrderHeader.DeliveryMethod == "poczta")
                {
                    detailsCard.OrderHeader.DeliveryCost = 9;
                }
                else if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
                {
                    detailsCard.OrderHeader.DeliveryCost = 12;
                }
                else
                {
                    detailsCard.OrderHeader.DeliveryCost = 15;
                }
            }
            else
            {
                HttpContext.Session.SetString(SD.ssDelivery, "poczta");
                detailsCard.OrderHeader.DeliveryMethod = "poczta";
                detailsCard.OrderHeader.DeliveryCost = 9;
            }

            if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
            {
                if (HttpContext.Session.GetString(SD.ssPaczkomat) != null)
                {
                    detailsCard.OrderHeader.Paczkomat = HttpContext.Session.GetString(SD.ssPaczkomat);
                }
            }

            detailsCard.OrderHeader.CouponCodeDiscount = detailsCard.OrderHeader.OrderTotalOriginal - detailsCard.OrderHeader.OrderTotal;
            detailsCard.OrderHeader.OrderTotal += detailsCard.OrderHeader.DeliveryCost;
            return View(detailsCard);
        }

        public IActionResult AddCoupon()
        {
            if (detailsCard.OrderHeader.CouponCode == null)
            {
                detailsCard.OrderHeader.CouponCode = "";
            }
            HttpContext.Session.SetString(SD.ssCouponCode, detailsCard.OrderHeader.CouponCode);
            return RedirectToAction("Index");
        }

        public IActionResult RemoveCoupon()
        {
            HttpContext.Session.SetString(SD.ssCouponCode, string.Empty);
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Plus(int cartId)
        {
            var cart = await _db.AnonimowyKoszyk.FirstOrDefaultAsync(c => c.Id == cartId);
            cart.Count += 1;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Minus(int cartId)
        {
            string cardId = Request.Cookies["cartId"];
            var cart = await _db.AnonimowyKoszyk.FirstOrDefaultAsync(c => c.Id == cartId);
            if (cart.Count == 1)
            {
                _db.AnonimowyKoszyk.Remove(cart);
                await _db.SaveChangesAsync();
                var cnt = _db.AnonimowyKoszyk.Where(c => c.cartId == cardId).ToList().Count;
                HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);
            }
            else
            {
                cart.Count -= 1;
                await _db.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Remove(int cartId)
        {
            string cardId = Request.Cookies["cartId"];
            var cart = await _db.AnonimowyKoszyk.FirstOrDefaultAsync(c => c.Id == cartId);

            _db.AnonimowyKoszyk.Remove(cart);
            await _db.SaveChangesAsync();

            var cnt = _db.AnonimowyKoszyk.Where(c => c.cartId == cardId).ToList().Count;
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, cnt);

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Poczta()
        {
            HttpContext.Session.SetString(SD.ssDelivery, "poczta");
            return RedirectToAction("Index");
        }

        public IActionResult Paczkomat()
        {
            HttpContext.Session.SetString(SD.ssDelivery, "paczkomat");
            return RedirectToAction("Index");
        }

        public IActionResult Kurier()
        {
            HttpContext.Session.SetString(SD.ssDelivery, "kurier");
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Podsumowanie")]
        public async Task<IActionResult> PodsumowaniePOST()
        {
            string payMethod = Request.Form["payMethod"].ToString();
            if (payMethod == "PayU")
            {
                detailsCard.OrderHeader.PayMethod = "PayU";
            }
            else
            {
                detailsCard.OrderHeader.PayMethod = "Stripe";
            }
            var ip = "192.168.0.0";
            string cardId = Request.Cookies["cartId"];

            detailsCard.listCart = await _db.AnonimowyKoszyk.Where(c => c.cartId == cardId).ToListAsync();


            detailsCard.OrderHeader.PaymentStatus = SD.PaymentStatusPending;
            detailsCard.OrderHeader.OrderDate = DateTime.Now;
            detailsCard.OrderHeader.Status = SD.PaymentStatusPending;
            detailsCard.OrderHeader.Country = "Polska";
            detailsCard.OrderHeader.DeliveryCountry = "Polska";

            List<OrderDetails> orderDetailsList = new List<OrderDetails>();
            _db.OrderHeader.Add(detailsCard.OrderHeader);
            await _db.SaveChangesAsync();

            detailsCard.OrderHeader.OrderTotalOriginal = 0;


            foreach (var item in detailsCard.listCart)
            {
                item.Produkt = await _db.Produkt.FirstOrDefaultAsync(m => m.Id == item.ProduktId);
                OrderDetails orderDetails = new OrderDetails
                {
                    ProduktId = item.ProduktId,
                    OrderId = detailsCard.OrderHeader.Id,
                    Name = item.Produkt.Name,
                    Price = item.Produkt.Price,
                    Count = item.Count
                };
                detailsCard.OrderHeader.OrderTotalOriginal += orderDetails.Count * orderDetails.Price;
                _db.OrderDetails.Add(orderDetails);
            }

            if (HttpContext.Session.GetString(SD.ssCouponCode) != null)
            {
                detailsCard.OrderHeader.CouponCode = HttpContext.Session.GetString(SD.ssCouponCode);
                var couponFromDb = await _db.Kupon.Where(c => c.Name.ToLower() == detailsCard.OrderHeader.CouponCode.ToLower()).FirstOrDefaultAsync();
                detailsCard.OrderHeader.OrderTotal = SD.DiscountedPrice(couponFromDb, detailsCard.OrderHeader.OrderTotalOriginal);
            }
            else
            {
                detailsCard.OrderHeader.OrderTotal = detailsCard.OrderHeader.OrderTotalOriginal;
            }

            if (HttpContext.Session.GetString(SD.ssDelivery) != null)
            {
                detailsCard.OrderHeader.DeliveryMethod = HttpContext.Session.GetString(SD.ssDelivery);
                if (detailsCard.OrderHeader.DeliveryMethod == "poczta")
                {
                    detailsCard.OrderHeader.DeliveryCost = 9;
                }
                else if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
                {
                    detailsCard.OrderHeader.DeliveryCost = 12;
                }
                else
                {
                    detailsCard.OrderHeader.DeliveryCost = 15;
                }
            }
            else
            {
                HttpContext.Session.SetString(SD.ssDelivery, "poczta");
                detailsCard.OrderHeader.DeliveryMethod = "poczta";
                detailsCard.OrderHeader.DeliveryCost = 9;
            }

            if (detailsCard.OrderHeader.DeliveryMethod == "paczkomat")
            {
                if (HttpContext.Session.GetString(SD.ssPaczkomat) != null)
                {
                    detailsCard.OrderHeader.Paczkomat = HttpContext.Session.GetString(SD.ssPaczkomat);
                }
            }

            detailsCard.OrderHeader.CouponCodeDiscount = detailsCard.OrderHeader.OrderTotalOriginal - detailsCard.OrderHeader.OrderTotal;
            detailsCard.OrderHeader.OrderTotal += detailsCard.OrderHeader.DeliveryCost;
            await _db.SaveChangesAsync();
            _db.AnonimowyKoszyk.RemoveRange(detailsCard.listCart);
            HttpContext.Session.SetInt32(SD.ssShoppingCartCount, 0);
            await _db.SaveChangesAsync();

            if (detailsCard.OrderHeader.PayMethod == "PayU")
            {
                string jsonTokenString = await PayU.GetAccessToken();
                JToken jsonToken = JObject.Parse(jsonTokenString);
                string accessToken = jsonToken.Value<string>("access_token");
                string tokenType = jsonToken.Value<string>("token_type");

                string jsonUrlString = await AnonimowyPayU.CreateNewOrder(accessToken, tokenType, detailsCard, ip);
                string[] array = jsonUrlString.Split('?', '&', '=');
                string PayUId = array[2];

                detailsCard.OrderHeader.PayUId = PayUId;
                await _db.SaveChangesAsync();

                return Redirect(jsonUrlString);
            }
            else
            {
                return NotFound();
            }
        }
    }
}
