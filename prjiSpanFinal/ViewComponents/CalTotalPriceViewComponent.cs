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
            List<Coupon> buyerCoupons = dbContext.CouponWallets.Where(i => i.MemberId == buyer.MemberId && DateTime.Now >= i.Coupon.StartDate && DateTime.Now < i.Coupon.ExpiredDate && i.Coupon.MemberId == sellerMemberID).Select(i => i.Coupon).ToList();

            CInfoForCalTotalPriceViewModel x = new CInfoForCalTotalPriceViewModel();
            x.isCheckoutPage = id;
            x.buyerCoupons = buyerCoupons;

            return View(x);
        }
    }
}
