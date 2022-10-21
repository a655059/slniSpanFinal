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
        public string bigType { get; set; }
        public string smallType { get; set; }
        public List<ProductDetail> productDetails { get; set; }
        public List<byte[]> productPics { get; set; }
        public List<CItemIndexSellerProductViewModel> sellerProducts { get; set; }
        public List<ReportType> ReportType { get; set; }
        public bool IsLogin { get; set; }
        public MemberAccount user { get; set; }
    }
}
