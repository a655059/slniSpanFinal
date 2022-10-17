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
        public List<ProductPic> productPics { get; set; }
        public List<CItemIndexSellerProductViewModel> sellerProducts { get; set; }
    }
}
