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
        public List<string> week { get; set; }
        public List<string> time { get; set; }
        public List<string> takebreak { get; set; }

        public string NewProductOnLoad { get; set; }
        public string NewProductCategory { get; set; }
        public string SellerCategory { get; set; }
        public string ServiceAfterBuy { get; set; }
        public string Caution { get; set; }
    }
}
