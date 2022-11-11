using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CGoOPayCheckoutViewModel
    {
        
        public string productName { get; set; }
        public int productPrice { get; set; }
        public int purchaseCount { get; set; }
        public string couponCode { get; set; }
        public int totalPrice { get; set; }
        public int shipperFee { get; set; }
        public int paymentFee { get; set; }
    }
}
