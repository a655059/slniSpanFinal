using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CItemIndexSellerProductViewModel
    {
        public int productID { get; set; }
        public string productName { get; set; }
        public byte[] productPic { get; set; }
        public string price { get; set; }
        public double starCount { get; set; }
        public int salesVolume { get; set; }
        public bool isBiddingItem { get; set; }
        public int biddingID { get; set; }
    }
}
