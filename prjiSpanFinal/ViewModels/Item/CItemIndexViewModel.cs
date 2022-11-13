using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CItemIndexViewModel
    {
        public Product product { get; set; }
        public BigType bigType { get; set; }
        public SmallType smallType { get; set; }
        public List<ProductDetail> productDetails { get; set; }
        public List<byte[]> productPics { get; set; }
        public List<int> sellerProductIDs { get; set; }
        public MemberAccount seller { get; set; }
        public string sellerRegion { get; set; }
        public List<CSellerShipperViewModel> sellerShipper { get; set; }
        public List<CSellerPaymentViewModel> sellerPayment { get; set; }
        public double avgCommentStar { get; set; }
        public int commentCount { get; set; }
        public int salesVolume { get; set; }
        public int buyerCount { get; set; }
        public double avgSellerCommentStar { get; set; }
        public int sellerCommentCount { get; set; }
        public Boolean Islike { get; set; }
        public List<ReportType> ReportType { get; set; }
        public bool IsLogin { get; set; }
        public MemberAccount user { get; set; }
        public List<Coupon> sellerCoupons { get; set; }
        public List<CouponWallet> userCouponWallet { get; set; }
        public int remainingQty { get; set; }
        public decimal activeDiscount { get; set; }
    }
}
