using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CProductAllInfoViewModel
    {
        public CProductViewModel CProduct { get; set; }
        public List<CProductDetailViewModel> CProductDetails { get; set; }
        public List<CProductPicViewModel> CProductPics { get; set; }
        public double CommentAvgScore { get; set; }
        public int SellCount { get; set; }
    }
}
