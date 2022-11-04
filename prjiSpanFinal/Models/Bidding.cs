using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Bidding
    {
        public Bidding()
        {
            AddBiddingCalendars = new HashSet<AddBiddingCalendar>();
            AutoBiddings = new HashSet<AutoBidding>();
            BiddingDetails = new HashSet<BiddingDetail>();
        }

        public int BiddingId { get; set; }
        public int ProductDetailId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int StartPrice { get; set; }
        public int StepPrice { get; set; }

        public virtual ProductDetail ProductDetail { get; set; }
        public virtual ICollection<AddBiddingCalendar> AddBiddingCalendars { get; set; }
        public virtual ICollection<AutoBidding> AutoBiddings { get; set; }
        public virtual ICollection<BiddingDetail> BiddingDetails { get; set; }
    }
}
