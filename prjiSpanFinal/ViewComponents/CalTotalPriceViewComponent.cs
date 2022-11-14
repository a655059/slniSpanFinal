using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Delivery;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewComponents
{
    public class CalTotalPriceViewComponent : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync(int id, int? sellerIDIndex,  int? sellerMemberID)
        {
            iSpanProjectContext dbContext = new iSpanProjectContext();
            string buyerString = HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER);
            MemberAccount buyer = JsonSerializer.Deserialize<MemberAccount>(buyerString);
            List<Coupon> buyerCoupons = dbContext.CouponWallets.Where(i => i.MemberId == buyer.MemberId && DateTime.Now >= i.Coupon.StartDate && DateTime.Now < i.Coupon.ExpiredDate && (i.Coupon.MemberId == sellerMemberID || i.Coupon.MemberId == 1) && i.IsExpired == false).Select(i => i.Coupon).ToList();
            CInfoForCalTotalPriceViewModel x = new CInfoForCalTotalPriceViewModel();
            x.isCheckoutPage = id;
            x.buyerCoupons = buyerCoupons;
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT))
            {
                string jsonString = HttpContext.Session.GetString(CDictionary.SK_ALL_INFO_TO_SHOW_CHECKOUT);
                CDeliveryCheckoutViewModel cDeliveryCheckout = JsonSerializer.Deserialize<CDeliveryCheckoutViewModel>(jsonString);
                foreach (var a in cDeliveryCheckout.sellerShipperPayments)
                {
                    if (a.seller.MemberId == sellerMemberID && a.savedShipperPaymentCoupon != null)
                    {
                        x.savedShipperPaymentCoupon = a.savedShipperPaymentCoupon;
                    }
                }
                if (x.savedShipperPaymentCoupon != null && x.savedShipperPaymentCoupon.couponID > 0)
                {
                    x.selectedCoupon = dbContext.Coupons.Where(i => i.CouponId == x.savedShipperPaymentCoupon.couponID).FirstOrDefault();
                }
            }
            


            return View(x);
        }
    }
}
