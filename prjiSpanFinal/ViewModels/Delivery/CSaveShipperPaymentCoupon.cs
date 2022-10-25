using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CSaveShipperPaymentCoupon
    {
        public int sellerID { get; set; }
        public string recipient { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public int shipperID { get; set; }
        public string shipperName { get; set; }
        public int shipperFee { get; set; }
        public int paymentID { get; set; }
        public string paymentName { get; set; }
        public int paymentFee { get; set; }
        public int couponID { get; set; }
        public string couponName { get; set; }
        public string wordToSeller { get; set; }
    }
}
