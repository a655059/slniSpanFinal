using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CDeliveryShowCartViewModel
    {
        public List<CDeliveryOrderViewModel> cart { get; set; }
        public List<CShipperToSellerViewModel> shipperToSeller { get; set; }
    }
}
