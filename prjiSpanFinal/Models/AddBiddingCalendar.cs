using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class AddBiddingCalendar
    {
        public int AddBiddingCalendarId { get; set; }
        public int BiddingId { get; set; }
        public int MemberId { get; set; }

        public virtual Bidding Bidding { get; set; }
        public virtual MemberAccount Member { get; set; }
    }
}
