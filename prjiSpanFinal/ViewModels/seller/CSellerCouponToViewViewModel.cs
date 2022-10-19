using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerCouponToViewViewModel
    {
        public List<int> CouponId { get; set; }
        public List<string> CouponName { get; set; }
        public List<DateTime> StartDate { get; set; }
        public List<DateTime> ExpiredDate { get; set; }
        public List<float> Discount { get; set; }
        public List<string> CouponCode { get; set; }
        public List<DateTime> ReceiveStartDate { get; set; }
        public List<DateTime> ReceiveEndDate { get; set; }
        public List<bool> IsFreeDelivery { get; set; }
        public List<int> MinimumOrder { get; set; }
    }
}
