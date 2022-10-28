using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventViewModel
    {
        public OfficialEventList OffcialEvent { get; set; }
        public List<Product> EventProducts { get; set; }
        public List<CShowCoupon> EventCoupons { get; set; }
        public MemberAccount LogingMember { get; set; }

    }
}
