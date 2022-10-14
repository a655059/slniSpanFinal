using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CItemIndexSellerProductViewModel
    {
        public string productName { get; set; }
        public byte[] productPic { get; set; }
        public string price { get; set; }
        public int starCount { get; set; }
    }
}
