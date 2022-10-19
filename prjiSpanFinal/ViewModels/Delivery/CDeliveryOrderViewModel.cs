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
        public string productName { get; set; }
        public OrderDetail orderDetail { get; set; }
        public ProductDetail productDetail { get; set; } 
    }
}
