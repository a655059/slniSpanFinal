using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels;
using prjiSpanFinal.ViewModels.Event;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace prjiSpanFinal.Controllers
{
    public class EventController : Controller
    {
        iSpanProjectContext _db = new iSpanProjectContext();
        public IActionResult Discount()
        {
            List<Coupon> listCoupon = (new EventFactory()).listTheCoupons(1);
            List<CShowCoupon> listShowCoupon = new List<CShowCoupon>();
            CouponViewModel CouponVM = new CouponViewModel();

            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                CouponVM.Member = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));

            foreach (Coupon coupons in listCoupon)
            {
                CShowCoupon showCoupon = new CShowCoupon
                {
                    coupon = coupons,
                    couponEventTitle = "優惠券",
                };
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                    showCoupon.loggeduser = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                listShowCoupon.Add(showCoupon);
            }
            CouponVM.cListShowCoupon = listShowCoupon;           
            
            return View(CouponVM);
        }
        public IActionResult GetCoupon(int couponid)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return Content("0", "text/plain", Encoding.UTF8);
            }
            MemberAccount loggedmem  = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            if (_db.CouponWallets.Where(w => w.MemberId == loggedmem.MemberId && w.CouponId == couponid).Any()) {
                return Content("1", "text/plain", Encoding.UTF8);
            }
            else { 
            CouponWallet CW = new CouponWallet()
            {
                CouponId = couponid,
                MemberId = loggedmem.MemberId,
                IsExpired = false
            };
            _db.CouponWallets.Add(CW);
            _db.SaveChanges();

            return Content("2", "text/plain", Encoding.UTF8);
            }
        }
    }
}
