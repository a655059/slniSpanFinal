using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class CouponViewModel
    {
        public List<CShowCoupon> cListShowCoupon { get; set; }
        public MemberAccount Member{ get; set; }
    }
}
