using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CPurchaseItemInfo
    {
        public int orderDetailID { get; set; }
        public int productDetailID { get; set; }
        public string productName { get; set; }
        public byte[] productDetailPic { get; set; }
        public string unitPrice { get; set; }
        public string sellerAcc { get; set; }
        public int sellerID { get; set; }
        public int purchaseCount { get; set; }
        public string productStyle { get; set; }
        public decimal eventDiscount { get; set; }
    }
}
