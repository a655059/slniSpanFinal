using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ProductDetail
    {
        public ProductDetail()
        {
            Biddings = new HashSet<Bidding>();
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string Style { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[] Pic { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<Bidding> Biddings { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
