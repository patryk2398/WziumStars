using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WziumStars.Models;

namespace WziumStars.Utility
{
    public class SD
    {
        public const string DefaultProduktImage = "default.png";

        public const string AdminEndUser = "Admin";
        public const string CustomerEndUser = "Customer";
        public const string ssShoppingCartCount = "ssCartCount";
        public const string ssCouponCode = "ssCouponCode";
        public const string ssDelivery = "ssDelivery";
        public const string ssPaczkomat = "ssPaczkomat";

        public const string StatusSubmitted = "PRZYJETO";
        public const string StatusInProcess = "W PRZYGOTOWANIU";
        public const string StatusSent = "WYSLANO";
        public const string StatusCompleted = "ODEBRANO";
        public const string StatusCancelled = "ANULOWANO";

        public const string PaymentStatusPending = "OCZEKUJE";
        public const string PaymentStatusApproved = "ZATWIERDZONO";
        public const string PaymentStatusRejected = "ODRZUCONO";

        public static double DiscountedPrice(Kupon couponFromDb, double OriginallOrderTotal)
        {
            if (couponFromDb == null)
            {
                return OriginallOrderTotal;
            }
            else
            {
                if (couponFromDb.MinimumAmount > OriginallOrderTotal)
                {
                    return OriginallOrderTotal;
                }
                else
                {
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Kupon.ECouponType.PLN)
                    {
                        return Math.Round(OriginallOrderTotal - couponFromDb.Discount, 2);
                    }
                    if (Convert.ToInt32(couponFromDb.CouponType) == (int)Kupon.ECouponType.Procent)
                    {
                        return Math.Round(OriginallOrderTotal - (OriginallOrderTotal * couponFromDb.Discount / 100), 2);
                    }
                }
            }
            return OriginallOrderTotal;
        }
    }
}
