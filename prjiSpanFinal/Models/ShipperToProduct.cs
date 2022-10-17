using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ShipperToProduct
    {
        public int ShipperToProductId { get; set; }
        public int ShipperId { get; set; }
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }
        public virtual Shipper Shipper { get; set; }
    }
}
