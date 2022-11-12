using prjiSpanFinal.Models;
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
        public List<string> Category { get; set; }
        public List<int> CustomizedCategoryID { get; set; }
        public List<string> memship { get; set; }
        public List<Payment> mempay { get; set; }
        public List<int> shipID { get; set; }
        public List<int> PaymentID { get; set; }

        public string bigTypealone { get; set; }
        public int bigTypeIDalone { get; set; }
        public string smallTypealone { get; set; }
        public string Categoryalone { get; set; }

        //傳出去的屬性
        public string ProductName { get; set; }
        public string smalltype { get; set; }
        public List<int> DetailID { get; set; }
        public List<string> Style { get; set; }
        public List<int> Quantity { get; set; }
        public List<decimal> UnitPrice { get; set; }
        public List<int> BodyPicID { get; set; }
        public List<byte[]> BodyPic { get; set; }

        public List<int> Dataphotoindex { get; set; }
        public string Description { get; set; }
        public int ProductID { get; set; }
        public int CategoryID { get; set; }
        public List<int> ShiperID { get; set; }
        public List<CSellerCreateToViewViewModel> 暫存規格 { get; set; }
        public List<byte[]> DBtoPic { get; set; }

        //AD區
        public List<string> ADName { get; set; }
        public List<List<int>> ADId { get; set; }
        public List<string> AdTyepDescription { get; set; }
        public List<List<decimal>> AdFee { get; set; }
        public List<List<int>> AdPeriod { get; set; }
        //存入AD的ID
        public List<int> SelectADId { get; set; }

        //規格區
        public string StyleStr { get; set; }
        public string QuantityStr { get; set; }
        public string UnitPriceStr { get; set; }
        public byte[] BodyPicStr { get; set; }
        public int productDetailIDStr { get; set; }

    }
}
