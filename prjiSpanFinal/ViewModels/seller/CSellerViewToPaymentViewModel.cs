using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerViewToPaymentViewModel
    {
        public int MemberID { get; set; }
        public List<bool> check{ get; set; }
        public List<bool> paycheck { get; set; }
        public List<bool> shipcheck { get; set; }
    }
}
