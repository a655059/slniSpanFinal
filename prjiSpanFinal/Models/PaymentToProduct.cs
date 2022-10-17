using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class PaymentToProduct
    {
        public int PaymentToProductId { get; set; }
        public int PaymentId { get; set; }
        public int ProductId { get; set; }

        public virtual Payment Payment { get; set; }
        public virtual Product Product { get; set; }
    }
}
