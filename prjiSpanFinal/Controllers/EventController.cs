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
        public IActionResult Event(int? Eventid)
        {
            OfficialEventList Event= _db.OfficialEventLists.Where(e => e.OfficialEventListId == Eventid).FirstOrDefault();
            EventViewModel EventVM = new EventViewModel();
            MemberAccount loggedmem = new MemberAccount();
            if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                loggedmem = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
            if (Event != null) {
                EventVM.OffcialEvent = Event;
                EventVM.LogingMember = loggedmem;
                //EventVM.EventCoupons=
                EventVM.EventProducts = _db.SubOfficialEventToProducts.Where(p => p.SubOfficialEventList.OfficialEventListId == Event.OfficialEventListId).Where(p => p.Product.ProductStatusId == 0).Select(p => p.Product).ToList();
                DateTime today = DateTime.Now;
                DateTime startday = Event.StartDate;

                //開始一周前開放看
                if (today.Subtract(startday).TotalDays < 7)
                {
                    return View(EventVM);
                }
                //如果是管理員開放看
                else if (loggedmem != null )
                {
                    if (loggedmem.MemberId == 1)
                        return View(EventVM);
                }
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
