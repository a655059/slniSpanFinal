using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
{
    public class CBuyerIndexViewModel
    {
        public List<CProductViewModel> CProducts { get; set; }
        public List<CProductDetailViewModel> CProductDetails { get; set; }
        public List<CProductPicViewModel> CProductPics { get; set; }
    }
}
