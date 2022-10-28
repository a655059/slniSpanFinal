using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class CShowItem
    {
        public Product Product { get; set; }
        public List<decimal> Price { get; set; }
        public byte[] Pic { get; set; }
        public int salesVolume { get; set; }
        public double starCount { get; set; }
    }
}
