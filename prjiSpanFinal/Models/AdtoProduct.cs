using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class AdtoProduct
    {
        public int AdtoProductId { get; set; }
        public int AdId { get; set; }
        public int ProductId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsSubActive { get; set; }
        public int ExpoTimes { get; set; }
        public int ClickTimes { get; set; }

        public virtual Ad Ad { get; set; }
        public virtual Product Product { get; set; }
    }
}
