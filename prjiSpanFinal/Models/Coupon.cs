using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Coupon
    {
        public Coupon()
        {
            OfficialCoupons = new HashSet<OfficialCoupon>();
            Orders = new HashSet<Order>();
        }

        public int CouponId { get; set; }
        public string CouponName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public float Discount { get; set; }

        public virtual ICollection<OfficialCoupon> OfficialCoupons { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
