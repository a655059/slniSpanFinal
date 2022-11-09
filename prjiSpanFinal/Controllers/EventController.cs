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
        public IActionResult Event(int Eventid)
        {
            if (_db.OfficialEventLists.Where(e => e.OfficialEventListId == Eventid&&e.OfficialEventTypeId==1).Any())
            {
                EventViewModel EventVM = (new EventFactory()).fToEvent(Eventid);
                MemberAccount evtLoggedAcc = new MemberAccount();
                if (HttpContext.Session.Keys.Contains(CDictionary.SK_LOGINED_USER))
                {
                    evtLoggedAcc = JsonSerializer.Deserialize<MemberAccount>(HttpContext.Session.GetString(CDictionary.SK_LOGINED_USER));
                }
                EventVM.LogingMember = evtLoggedAcc;
                //開始一周前開放看
                double evtPublishDay = (DateTime.Now).Subtract(EventVM.Event.StartDate).TotalDays;
                //如果是管理員開放看
                if (evtLoggedAcc!=null && evtLoggedAcc.MemberId == 1)
                {
                    return View(EventVM);
                }
                //七天前開放瀏覽
                else if (evtPublishDay > -7)
                {
                    return View(EventVM);
                }
            }
            return RedirectToAction("Index", "Home");
        }
        public IActionResult FlashSales(int Eventid)
        {
            if (_db.OfficialEventLists.Where(e => e.OfficialEventListId == Eventid && e.OfficialEventTypeId == 2).Any())
            {

            }
            return RedirectToAction("Index", "Home");
        }
    }
}

