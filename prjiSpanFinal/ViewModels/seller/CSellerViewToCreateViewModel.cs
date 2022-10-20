using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerViewToCreateViewModel
    {
        public int MemberID { get; set; }
        public string ProductName { get; set; }
        public List<string> smallType { get; set; }
        public List<string> Style { get; set; }
        public List<int> Quantity { get; set; }
        public List<decimal> UnitPrice { get; set; }
        public byte HeadPic { get; set; }
        public List<byte> BodyPic { get; set; }
        public string Description { get; set; }
        public List<int> ShiperID { get; set; }
        public int ProductID { get; set; }
        public int PaymentID { get; set; }

    }
}
