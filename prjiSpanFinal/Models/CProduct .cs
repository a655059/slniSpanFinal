using System.Collections.Generic;

namespace prjiSpanFinal.Models
{
    public class CProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SmallType { get; set; }
        public string SellerName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public string ProductStatus { get; set; }
    }
}
