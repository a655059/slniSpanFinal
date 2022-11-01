using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace prjiSpanFinal.ViewModels.seller
{
    public class CSellerNewIndexToViewViewModel
    {
        public int memberid { get; set; }
        public List<string> productName { get; set; }
        public List<List<string>> Style { get; set; }
        public List<List<int>> Quantity { get; set; }
        public List<List<decimal>> UnitPrice { get; set; }
        public List<List<byte[]>> Pic { get; set; }
        public List<int> productId { get; set; }
       public List<int> ProductStatusId { get; set; }
    }
}
