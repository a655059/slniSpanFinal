using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class SmallType
    {
        public SmallType()
        {
            Products = new HashSet<Product>();
        }

        public int SmallTypeId { get; set; }
        public string SmallTypeName { get; set; }
        public int BigTypeId { get; set; }

        public virtual BigType BigType { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
