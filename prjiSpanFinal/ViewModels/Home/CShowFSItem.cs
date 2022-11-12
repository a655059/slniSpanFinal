using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Home
{
    public class CShowFSItem
    {
        public int eventid { get; set; }
        public Product product { get; set; }
        public List<decimal> price { get; set; }
        public byte[] pic { get; set; }
        public float discount { get; set; }
        public int stock { get; set; }
        public int sale { get; set; }
        public decimal Percentage {
            get {
                decimal SaleD = Convert.ToDecimal(sale);
                return  Math.Round(((SaleD / (stock + sale)) * 100), 2);
            }
        }
        public List<int> effects { get; set; }
        //public bool isDeliveryFree
        //{
        //    get;
        //    set;
        //}
    }
}
