using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ShipperToSeller
    {
        public int ShipperId { get; set; }
        public int MemberId { get; set; }
        public int ShipperToMemberId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual Shipper Shipper { get; set; }
    }
}
