using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OrderDetail
    {
        public OrderDetail()
        {
            Comments = new HashSet<Comment>();
        }

        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public int ProductDetailId { get; set; }
        public int Quantity { get; set; }
        public int ShippingStatusId { get; set; }
        public decimal Unitprice { get; set; }

        public virtual Order Order { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
        public virtual ShippingStatus ShippingStatus { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
    }
}
