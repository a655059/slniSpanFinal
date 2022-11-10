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
            OfficialEventList Event = _db.OfficialEventLists.Where(e => e.OfficialEventListId != 1&&e.OfficialEventListId==EventID).FirstOrDefault();
            if (Event == null)
            {
                return res;
            }

            List<CShowCoupon> evtShowCoupon = new List<CShowCoupon>();
            //優惠券為 本次活動 可收券時間(早>晚)排序
            var Coupons = _db.Coupons.Where(c => c.OfficialEventListId == EventID).OrderBy(e => e.ReceiveStartDate);
            if (Coupons.Any())
                evtShowCoupon = fCouponToShowCoupon(Coupons.ToList());

            List<EventSubs> evtSubs = new List<EventSubs>();
            //子活動為本次活動 折價排序(低>高)
            var Subs = _db.SubOfficialEventLists.Where(s => s.OfficialEventListId == EventID).OrderByDescending(s=>s.Discount).ToList();
            if (Subs.Any()) { 
                foreach(var item in Subs)
                {
                    List<EventShowItem> PRODS =ftoEvtShowItem(_db.SubOfficialEventToProducts.Where(S => S.SubOfficialEventListId == item.SubOfficialEventListId && S.VerifyId == 2).Select(p => p.Product).ToList());                    
                    EventSubs es = new EventSubs()
                    {
                        SubEvent = item,
                        SubEventProducts=PRODS,
                    };
                    evtSubs.Add(es);
                }
            }

            res.Event = Event;
            res.EventSubs = evtSubs;
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
        public List<EventShowItem> ftoEvtShowItem(List<Product> list)
        {
            List<EventShowItem> res = new List<EventShowItem>();
            if (list == null)
            {
                return res;
            }
            foreach (var item in list)
            {
                if (item.ProductStatusId == 1 || item.ProductStatusId == 2)
                {
                    continue;
                }
                var price = _db.ProductDetails.Where(p => p.ProductId == item.ProductId).OrderBy(p => p.UnitPrice).Select(p => p.UnitPrice);
                decimal x = price.Min();
                decimal y = price.Max();
                byte[] pic = _db.ProductPics.Where(p => p.ProductId == item.ProductId).Select(p => p.Pic).FirstOrDefault();
                decimal discount=1;
                bool isDeliveryFree = false;
                bool isStart = false;
                SubOfficialEventToProduct prodstatus = new SubOfficialEventToProduct();
                if (_db.SubOfficialEventToProducts.Where(p => p.ProductId == item.ProductId).Where(p => p.VerifyId == 2).FirstOrDefault()!=null)
                {
                    prodstatus = _db.SubOfficialEventToProducts.Where(p => p.ProductId == item.ProductId).Where(p => p.VerifyId == 2).FirstOrDefault();
                    discount = Convert.ToDecimal(prodstatus.SubOfficialEventList.Discount);
                    isDeliveryFree = prodstatus.SubOfficialEventList.IsFreeDelivery;
                    if (DateTime.Now.CompareTo(prodstatus.SubOfficialEventList.OfficialEventList.StartDate) >= 0 && DateTime.Now.CompareTo(prodstatus.SubOfficialEventList.OfficialEventList.EndDate) <= 0)
                        isStart = true;
                }


                List<decimal> dlist = new List<decimal>();
                if (x == y)
                    dlist.Add(x);
                else
                {
                    dlist.Add(x);
                    dlist.Add(y);
                }
                EventShowItem a = new EventShowItem()
                {
                    product = item,
                    price = dlist,
                    discount= discount,
                    isDeliveryFree=isDeliveryFree,
                    isStart=isStart,                    
                };
                if (pic != null)
                    a.pic = pic;

                res.Add(a);
            }
            return res;

        }
    }
}
