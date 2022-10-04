using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CItemIndexViewModel
    {
        public CProductViewModel CProduct { get; set; }
        public List<CProductDetailViewModel> CProductDetails { get; set; }
        public List<CProductPicViewModel> CProductPics { get; set; }
        public CMemberAccountViewModel Seller { get; set; }
        public CBigTypeViewModel CBigType { get; set; }
        public CSmallTypeViewModel CSmallType { get; set; }
        public double CommentAvgScore { get; set; }
        public int CommentCount { get; set; }
        public int SellCount { get; set; }
        public List<CProductAllInfoViewModel> SellerProducts { get; set; }

    }
}
