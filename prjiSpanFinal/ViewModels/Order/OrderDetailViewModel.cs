using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels
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
        public string OrderMessage { get; set; }
        public List<int> Quantity { get; set; }
        public List<DateTime> OrderDetailReceiveDate { get; set; }
        public List<string> ShipStatusName { get; set; }
        public List<decimal> Unitprice { get; set; }
        public List<string> Style { get; set; }
        public List<byte[]> Pic { get; set; }
        public List<string> ProductName { get; set; }

    }
}
