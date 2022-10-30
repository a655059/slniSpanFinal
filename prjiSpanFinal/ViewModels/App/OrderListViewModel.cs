using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.App
{
    public class OrderListViewModel
    {
        public int OrderId { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string OrderStatusName { get; set; }
        public int Quantity { get; set; }
        public decimal Unitprice { get; set; }
        public string Style { get; set; }
        public byte[] Pic { get; set; }
        public string ProductName { get; set; }

    }
}
