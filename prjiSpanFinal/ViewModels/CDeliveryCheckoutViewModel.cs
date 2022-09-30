using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CDeliveryCheckoutViewModel
    {
        public CProductDetailViewModel CProductDetail { get; set; }
        public CProductViewModel CProduct { get; set; }
        public int PurchaseCount { get; set; }
        public CMemberAccountViewModel Seller { get; set; }
    }
}
