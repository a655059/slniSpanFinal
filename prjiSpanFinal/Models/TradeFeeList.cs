using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class TradeFeeList
    {
        public int TradeFeeId { get; set; }
        public int OrderId { get; set; }
        public int Total { get; set; }
        public int Fee { get; set; }
        public DateTime Date { get; set; }

        public virtual Order Order { get; set; }
    }
}
