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
            List<Coupon> lc= (new EventFactory()).listTheCoupons(1);
            List<CShowCoupon> ls = new List<CShowCoupon>();
            foreach(Coupon c in lc)
            {
                CShowCoupon sc = new CShowCoupon
                {
                    coupon = c,
                    couponEventTitle = "優惠券",
                };
                ls.Add(sc);
            }
            CouponViewModel cv = new CouponViewModel();
            cv.cListShowCoupon = ls;
            if(HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                cv.Member= JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            
            return View(cv);
        }
        public IActionResult GetCoupon(int couponid)
        {
            if (!HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
            {
                return Content("0", "text/plain", Encoding.UTF8);   //如果沒有登入則要求登入
            }
            MemberAccount loggedmem  = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            if (_db.CouponWallets.Where(w => w.MemberId == loggedmem.MemberId && w.CouponId == couponid).Any()) {
                return Content("1", "text/plain", Encoding.UTF8);
            }
            else { 
            CouponWallet cw = new CouponWallet()
            {
                CouponId = couponid,
                MemberId = loggedmem.MemberId,
                IsExpired = false
            };
            _db.CouponWallets.Add(cw);
            _db.SaveChanges();

            return Content("2", "text/plain", Encoding.UTF8);
            }
        }
    }
}
