using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class BiddingType
    {
        public BiddingType()
        {
            BiddingDetails = new HashSet<BiddingDetail>();
        }

        public int BiddingTypeId { get; set; }
        public string BiddingTypeName { get; set; }

        public virtual ICollection<BiddingDetail> BiddingDetails { get; set; }
    }
}
