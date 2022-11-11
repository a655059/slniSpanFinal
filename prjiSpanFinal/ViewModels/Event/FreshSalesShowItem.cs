using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Event
{
    public class FreshSalesShowItem
    {
        public Product product { get; set; }
        public byte[] pic { get; set; }
        public List<decimal> price { get; set; }
        public decimal discount { get; set; }
        public int sales { get; set; }
        public int stock { get; set; }
        public bool isStart { get; set; }
        public bool isOver { get; set; }
        public decimal Percentage
        {
            get
            {
                decimal SaleD = Convert.ToDecimal(sales);
                return Math.Round(((SaleD / (stock + sales)) * 100), 2);
            }
        }
    }
}
