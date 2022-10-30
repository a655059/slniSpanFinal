using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventFactory
    {
        iSpanProjectContext _db;

        public EventFactory()
        {
            _db = new iSpanProjectContext();
        }
        public List<Coupon> listTheCoupons(int? memberid)
        {
            List<Coupon> res = new List<Coupon>();
            res = _db.Coupons.Where(p => p.CouponId != 1 &&p.MemberId== memberid).ToList();
            return res;
        }
        //取得EventID轉成ViewModel
        public EventViewModel fToEvent(int EventID)
        {
            EventViewModel res = new EventViewModel();
            //活動為非預設活動
            OfficialEventList Event = _db.OfficialEventLists.Where(e => e.OfficialEventListId != 1).FirstOrDefault();
            if (Event == null)
            {
                return res;
            }
            //List<CShowItem> evtShowItem = new List<CShowItem>();
            ////產品為 本次活動 且為上架狀態
            //var Prods = _db.SubOfficialEventToProducts.Where(p => p.SubOfficialEventList.OfficialEventListId == Event.OfficialEventListId).Where(p => p.Product.ProductStatusId == 0).Select(p => p.Product);
            //    evtShowItem = (new CHomeFactory()).toShowItem(Prods.ToList());

            List<CShowCoupon> evtShowCoupon = new List<CShowCoupon>();
            //優惠券為 本次活動 可收券時間(早>晚)排序
            var Coupons = _db.Coupons.Where(c => c.OfficialEventListId == EventID).OrderBy(e => e.ReceiveStartDate);
            if (Coupons.Any())
                evtShowCoupon = fCouponToShowCoupon(Coupons.ToList());

            List<EventSubs> evtSubs = new List<EventSubs>();
            //子活動為本次活動 折價排序(低>高)
            var Subs = _db.SubOfficialEventLists.Where(s => s.OfficialEventListId == EventID).OrderByDescending(s=>s.Discount);
            if (Subs.Any()) { 
                foreach(var item in Subs)
                {
                    EventSubs es = new EventSubs()
                    {
                        SubEvent = item
                    };
                    evtSubs.Add(es);
                }
            }

            res.Event = Event;
            res.EventSubs = evtSubs;
            //res.EventProducts = evtShowItem;
            res.EventCoupons = evtShowCoupon;
            return res;
        }
        //List<Coupon> 轉成List<CShowCoupon>
        public List<CShowCoupon> fCouponToShowCoupon(List<Coupon> coupon)
        {
            List<CShowCoupon> res = new List<CShowCoupon>();
            if (coupon.Any())
            {
                foreach(Coupon c in coupon)
                {
                    CShowCoupon sc = new CShowCoupon()
                    {
                        coupon = c
                    };
                    res.Add(sc);
                }
            }
            return res;
        }
    }
}
