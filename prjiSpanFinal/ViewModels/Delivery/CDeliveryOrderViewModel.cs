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
        public string sellerAcc { get; set; }
        public Product product { get; set; }
        public OrderDetail orderDetail { get; set; }
        public ProductDetail productDetail { get; set; } 
        public string shipperName { get; set; }
        public decimal shipperFee { get; set; }
        public decimal eventDiscount { get; set; }
    }
}
