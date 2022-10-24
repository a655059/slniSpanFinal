using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerCreateToViewViewModel
    {
        public List<string> bigType { get; set; }
        public List<string> smallType { get; set; }
        public List<string> memship { get; set; }
        public List<int> shipID { get; set; }

        //傳出去的屬性
        public int MemberID { get; set; }
        public string ProductName { get; set; }
        public string smalltype { get; set; }
        public List<string> Style { get; set; }
        public List<int> Quantity { get; set; }
        public List<decimal> UnitPrice { get; set; }
        public byte HeadPic { get; set; }
        public List<byte> BodyPic { get; set; }
        public string Description { get; set; }
        public List<string> ShiperID { get; set; }
        public int ProductID { get; set; }
        public int PaymentID { get; set; }
        public List<CSellerCreateToViewViewModel> 暫存規格 { get; set; }
        public List<int> SaveCount { get; set; }



        public string StyleStr { get; set; }
        public string QuantityStr { get; set; }
        public string UnitPriceStr { get; set; }
        public string BodyPicStr { get; set; }

    }
}
