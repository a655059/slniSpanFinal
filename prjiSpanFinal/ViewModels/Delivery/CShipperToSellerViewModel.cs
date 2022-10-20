using prjiSpanFinal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.Delivery
{
    public class CShipperToSellerViewModel
    {
        public int sellerID { get; set; }
        public List<Shipper> shippers { get; set; }
    }
}
