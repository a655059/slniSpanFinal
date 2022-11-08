using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CBiddingDetailWithMemberViewModel
    {
        public BiddingDetail biddingDetail { get; set; }
        public MemberAccount member { get; set; }
        public BiddingType biddingType { get; set; }
    }
}
