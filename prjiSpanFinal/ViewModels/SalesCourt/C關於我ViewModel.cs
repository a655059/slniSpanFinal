using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.SalesCourt
{
    public class C關於我ViewModel
    {
        
        public int Memberid { get; set; }
        public string SalesCourtServiceTime { get; set; }
        public string weekDown { get; set; }
        public string weekUp { get; set; }
        public string timeDown { get; set; }
        public string timeUp { get; set; }
        public string takebreak { get; set; }

        public string NewProductOnLoad { get; set; }
        public string NewProductCategory { get; set; }
        public string SellerCategory { get; set; }
        public string ServiceAfterBuy { get; set; }
        public string Caution { get; set; }
    }
}
