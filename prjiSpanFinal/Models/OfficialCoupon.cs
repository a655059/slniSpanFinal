using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OfficialCoupon
    {
        public int MemberId { get; set; }
        public int OfficialCouponsId { get; set; }
        public int CouponId { get; set; }
        public bool ExpireNA { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual MemberAccount Member { get; set; }
    }
}
