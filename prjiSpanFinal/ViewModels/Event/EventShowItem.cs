using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class EventShowItem
    {

        public Product product { get; set; }
        public byte[] pic { get; set; }
        public List<decimal> price { get; set; }
        public bool isStart { get; set; }
        public decimal discount { get; set; }
        public bool isDeliveryFree { get; set; }


    }
}
