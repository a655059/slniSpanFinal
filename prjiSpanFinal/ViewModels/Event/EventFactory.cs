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
        public EventViewModel fToEvent(int EventID)
        {
            EventViewModel res = new EventViewModel();
            OfficialEventList Event = _db.OfficialEventLists.Where(e => e.OfficialEventListId != 1).FirstOrDefault();
            if (Event == null)
            {
                return res;
            }
            List<CShowItem> EvtShowItem = new List<CShowItem>();
            List<CShowCoupon> evtCoupon = new List<CShowCoupon>();
            EvtShowItem = (new CHomeFactory()).toShowItem(_db.SubOfficialEventToProducts.Where(p => p.SubOfficialEventList.OfficialEventListId == Event.OfficialEventListId).Where(p => p.Product.ProductStatusId == 0).Select(p => p.Product).ToList());
            res.OffcialEvent = Event;
            if (EvtShowItem.Any())
                res.EventProducts = EvtShowItem;
            //List<Coupon> Coupons = _db.Coupons.Where(c=>c.);
            evtCoupon = new List<CShowCoupon>();
            return res;
        }
    }
}
