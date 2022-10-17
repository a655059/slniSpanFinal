using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Shipper
    {
        public Shipper()
        {
            Orders = new HashSet<Order>();
            ShipperToProducts = new HashSet<ShipperToProduct>();
            ShipperToSellers = new HashSet<ShipperToSeller>();
        }

        public int ShipperId { get; set; }
        public string ShipperName { get; set; }
        public string Phone { get; set; }

        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<ShipperToProduct> ShipperToProducts { get; set; }
        public virtual ICollection<ShipperToSeller> ShipperToSellers { get; set; }
    }
}
