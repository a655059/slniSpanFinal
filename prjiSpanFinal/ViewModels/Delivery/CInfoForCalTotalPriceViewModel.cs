using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CInfoForCalTotalPriceViewModel
    {
        public int isCheckoutPage { get; set; }
        public List<Coupon> buyerCoupons { get; set; }
        public CSaveShipperPaymentCoupon savedShipperPaymentCoupon { get; set; }

        public Coupon selectedCoupon { get; set; }

    }
}
