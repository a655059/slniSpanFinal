using prjiSpanFinal.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.App
{
    public class ShowCartViewModel
    {
        public int qty { get; set; }
        public int oid { get; set; }
        public int odid { get; set; }
        public decimal unitprice { get; set; }
        public string RecieveAdr { get; set; }

        public int ship { get; set; }
        public int pay { get; set; }
        

        public string name { get; set; }
        public string style { get; set; }
        public List<CSellerShipperViewModel> sellerShipper { get; set; }
        public List<CSellerPaymentViewModel> sellerPayment { get; set; }

        public byte[] pic { get; set; }
    }
}
