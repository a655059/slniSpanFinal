using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Ad
    {
        public Ad()
        {
            AdtoProducts = new HashSet<AdtoProduct>();
        }

        public int AdId { get; set; }
        public int AdTypeId { get; set; }
        public decimal AdFee { get; set; }
        public int AdPeriod { get; set; }

        public virtual Adtype AdType { get; set; }
        public virtual ICollection<AdtoProduct> AdtoProducts { get; set; }
    }
}
