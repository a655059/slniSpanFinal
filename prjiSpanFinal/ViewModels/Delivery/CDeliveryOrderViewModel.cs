using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CDeliveryOrderViewModel
    {
        public Order order { get; set; }
        public List<OrderDetail> orderDetails { get; set; }
    }
}
