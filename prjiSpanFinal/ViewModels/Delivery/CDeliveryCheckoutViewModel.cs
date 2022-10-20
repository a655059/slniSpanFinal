using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CDeliveryCheckoutViewModel
    {
        public List<CPurchaseItemInfo> purchaseItemInfo { get; set; }
        public List<CDeliverySellerShipperPayment> sellerShipperPayments { get; set; }
    }
}
