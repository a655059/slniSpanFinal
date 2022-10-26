using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Member
{
    public class CouponFactory
    {
        public List<Coupon> cGetCouponWallet(List<Coupon> listc,int MemID)
        {
            iSpanProjectContext _db = new iSpanProjectContext();
            listc = _db.CouponWallets.Where(c => c.MemberId == MemID).Select(c=>c.Coupon).ToList();
            return listc;
        }
    }
}
