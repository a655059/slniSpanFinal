using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class CouponWallet
    {
        public int CouponWalletId { get; set; }
        public int MemberId { get; set; }
        public int CouponId { get; set; }
        public bool IsExpired { get; set; }

        public virtual Coupon Coupon { get; set; }
        public virtual MemberAccount Member { get; set; }
    }
}
