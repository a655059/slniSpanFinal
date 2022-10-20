using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventFactory
    {
        iSpanProjectContext _db = new iSpanProjectContext();
        public List<Coupon> listTheCoupons(int? memberid)
        {
            List<Coupon> res = new List<Coupon>();
            res = _db.Coupons.Where(p => p.CouponId != 1 &&p.MemberId== memberid).ToList();
            return res;
        }
    }
}
