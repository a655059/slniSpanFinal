using System;
using System.Collections.Generic;


namespace prjiSpanFinal.Models.App
{
    public partial class CProductDetail
    {
        public int ProductDetailId { get; set; }
        public int ProductId { get; set; }
        public string Style { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public byte[] Pic { get; set; }
    }
}
