using System.Collections.Generic;

namespace prjiSpanFinal.Models
{
    public class CProductHu
    {
        public int ProductId { get; set; }
        public string ProductName { get { return "蘋果"; } }
        public string SmallType { get { return "食物"; } }
        public string SellerName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Stock { get; set; }
        public string ProductStatus { get; set; }
    }
}
