using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public int ShipperId { get; set; }
        public int Quantity { get; set; }
        public DateTime ShippingDate { get; set; }
        public DateTime RecieveDate { get; set; }
        public string OutAdr { get; set; }
        public int ShippingStatusId { get; set; }

        public virtual Order Order { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public virtual Shipper Shipper { get; set; }
        public virtual ShippingStatus ShippingStatus { get; set; }
    }
}
