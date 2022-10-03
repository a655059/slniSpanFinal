using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Payment
    {
        public int PaymentId { get; set; }
        public string PaymentName { get; set; }
        public decimal Fee { get; set; }
    }
}
