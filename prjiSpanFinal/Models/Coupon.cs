using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Coupon
    {
        public Coupon()
        {
            CouponWallets = new HashSet<CouponWallet>();
            Orders = new HashSet<Order>();
        }

        public int CouponId { get; set; }
        public string CouponName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public float Discount { get; set; }
        public string CouponCode { get; set; }
        public int MemberId { get; set; }
        public DateTime ReceiveStartDate { get; set; }
        public DateTime ReceiveEndDate { get; set; }
        public bool IsFreeDelivery { get; set; }
        public int MinimumOrder { get; set; }

        public virtual ICollection<CouponWallet> CouponWallets { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
