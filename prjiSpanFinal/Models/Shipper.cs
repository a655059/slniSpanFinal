using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            OrderDetails = new HashSet<OrderDetail>();
            ShipperToProducts = new HashSet<ShipperToProduct>();
        }

        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<ShipperToProduct> ShipperToProducts { get; set; }
    }
}
