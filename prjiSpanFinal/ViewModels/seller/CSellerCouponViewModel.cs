using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerCouponViewModel
    {
        public string CouponName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime ExpiredDate { get; set; }
        public float Discount { get; set; }
        public string CouponCode { get; set; }
        public DateTime ReceiveStartDate { get; set; }
        public DateTime ReceiveEndDate { get; set; }
        public bool IsFreeDelivery { get; set; }
        public int MinimumOrder { get; set; }
    }
}
