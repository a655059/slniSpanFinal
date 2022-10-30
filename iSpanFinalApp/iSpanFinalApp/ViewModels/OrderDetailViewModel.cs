using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace iSpanFinalApp.ViewModels
{
    public class OrderDetailViewModel
    {
        public int OrderId { get; set; }
        public string SellerAcc { get; set; }
        public string SellerPhone { get; set; }
        public string SellerEmail { get; set; }
        public string SellerName { get; set; }
        public string BuyerPhone { get; set; }
        public string BuyerEmail { get; set; }
        public string BuyerName { get; set; }
        public string BuyerAcc { get; set; }
        public DateTime OrderDatetime { get; set; }
        public string OrderDatetimeCool
        {
            get
            {
                return OrderDatetime.ToString("yyyy-MM-dd HH:mm:ss");
            }
        }
        public string RecieveAdr { get; set; }
        public string CouponName { get; set; }
        public bool IsFreeDelivery { get; set; }
        public string OrderStatusName { get; set; }
        public int ShipperStatusId { get; set; }
        public string ShipperName { get; set; }
        public string ShipperPhone { get; set; }
        public int ShipperFee { get; set; }
        public DateTime PaymentDate { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public string PaymentName { get; set; }
        public int PaymentFee { get; set; }
        public int SmallTotal 
        { 
            get 
            {
                int a = 0;
                for(int i=0;i<Pic.Count;i++)
                {
                    a += Quantity[i] * Convert.ToInt32(Unitprice[i]);
                }
                return a;
            } 
        }
        public int BigTotal
        {
            get
            {
                int a = 0;
                for (int i = 0; i < Pic.Count; i++)
                {
                    a += Quantity[i] * Convert.ToInt32(Unitprice[i]);
                }
                if(!IsFreeDelivery)
                {
                    a += ShipperFee;
                }
                a += PaymentFee;
                return a;
            }
        }
        public int shipperfeecount
        {
            get
            {
                if(IsFreeDelivery)
                {
                    return 0;
                }
                else
                {
                    return ShipperFee;
                }
            }
        }
        public string OrderMessage { get; set; }
        public List<int> Quantity { get; set; }
        public List<int> OrderDetailId { get; set; }
        public List<string> ShipStatusName { get; set; }
        public List<decimal> Unitprice { get; set; }
        public List<string> Style { get; set; }
        public List<byte[]> Pic { get; set; }

        public List<ImageSource> PicCool { get { 
            
                var list = new List<ImageSource>();
                foreach(var item in Pic)
                {
                    list.Add(ImageSource.FromStream(() => new MemoryStream(item)));
                }
                return list;
            
            } }

        public List<string> ProductName { get; set; }

    }
}
