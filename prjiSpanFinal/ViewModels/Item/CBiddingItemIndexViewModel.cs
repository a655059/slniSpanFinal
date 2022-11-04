using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Item
{
    public class CBiddingItemIndexViewModel
    {
        public Bidding bidding { get; set; }
        public ProductDetail productDetail { get; set; }
        public Product product { get; set; }
        public MemberAccount seller { get; set; }
        public SmallType smallType { get; set; }
        public BigType bigType { get; set; }
        public MemberAccount user { get; set; }
        public RegionList region { get; set; }
        public CountryList country { get; set; }
        public List<byte[]> productPics { get; set; }

        public Boolean isLike { get; set; }
    }
}
