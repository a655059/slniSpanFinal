using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CDeliveryOrderViewModel
    {
        public int sellerID { get; set; }
        public string sellerName { get; set; }
        public byte[] productDetailPic { get; set; }
        public string productName { get; set; }
        public int purchaseQuantity { get; set; }
        public int productQuantity { get; set; }
        public string style { get; set; }
        public int orderDetailID { get; set; }
        public decimal unitPrice { get; set; }
    }
}
