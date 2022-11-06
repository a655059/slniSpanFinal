using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CShowFSItem
    {
        public Product product { get; set; }
        public List<decimal> price { get; set; }
        public byte[] pic { get; set; }
        public float discount { get; set; }
        public int stock { get; set; }
        public int sale { get; set; }
        public decimal Percentage { 
            get
            {
                return (sale / stock + sale) * 100;
            }
        }
    }
}
