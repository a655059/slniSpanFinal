using prjiSpanFinal.Models;
using prjiSpanFinal.ViewModels.Home;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventViewModel
    {
        public OfficialEventList Event { get; set; }
        public List<CShowItem> EventProducts { get; set; }
        public List<CShowCoupon> EventCoupons { get; set; }
        public MemberAccount LogingMember { get; set; }
        public List<EventSubs> EventSubs { get; set; }
    }
}
