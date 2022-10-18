using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CShowItem
    {
        public Product Product { get; set; }
        public List<decimal> Price { get; set; }
        public byte[] Pic { get; set; }
        public int sells { get; set; }
        public int stars { get; set; }
    }
}