using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ShippingStatus
    {
        public ShippingStatus()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int ShippingStatusId { get; set; }
        public string ShipStatusName { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
