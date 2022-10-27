using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerPaymentToViewViewModel
    {
        public List<int> PaymentId { get; set; }
        public List<string> PaymentName { get; set; }
        public int MemberID { get; set; }
        public List<int> ProductID { get; set; }
        public List<int> ShipperId { get; set; }
        public List<string> ShipperName { get; set; }
        /// <summary>
        /// 1=true,0=false
        /// </summary>
        public List<int> ShipCheckedOX { get; set; }
        public List<int> PayCheckedOX { get; set; }
    }
}
