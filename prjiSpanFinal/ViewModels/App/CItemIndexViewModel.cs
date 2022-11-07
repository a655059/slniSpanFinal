using prjiSpanFinal.Models.App;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.App
{
    public class CItemIndexViewModel
    {
        public CProduct product { get; set; }
        public List<CProductDetail> productDetails { get; set; }
        public List<byte[]> productPics { get; set; }
        public CMemberAccount seller { get; set; }
        public CMemberAccount buyer { get; set; }
        public List<CSellerShipperViewModel> sellerShipper { get; set; }
        public List<CSellerPaymentViewModel> sellerPayment { get; set; }
        public double avgCommentStar { get; set; }
        public int commentCount { get; set; }
        public List<CComment> comments { get; set; }
        public int salesVolume { get; set; }
        public Boolean Islike { get; set; }
        public int cartcount { get; set; }

    }
}
