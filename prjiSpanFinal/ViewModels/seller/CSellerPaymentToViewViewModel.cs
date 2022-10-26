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
        /// 傳進來的資料庫有的物流資料
        /// </summary>
        public int ShipperIdToShip { get; set; }
        public int PaymentIdToPay { get; set; }
        /// <summary>
        /// 1=true,0=false
        /// </summary>
        public int CheckedOX { get; set; }
        /// <summary>
        /// 為了要給畫面客戶儲存的物流
        /// </summary>
        public List<CSellerPaymentToViewViewModel>x { get; set; }
    }
}
