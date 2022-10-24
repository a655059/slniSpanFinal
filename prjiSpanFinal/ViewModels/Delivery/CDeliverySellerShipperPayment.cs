using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CDeliverySellerShipperPayment
    {
        public MemberAccount seller { get; set; }
        public List<Shipper> shippers { get; set; }
        public List<Payment> payments { get; set; }
        public CSaveShipperPaymentCoupon savedShipperPaymentCoupon { get; set; }
    }
}
