using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class BigType
    {
        public BigType()
        {
            SmallTypes = new HashSet<SmallType>();
        }

        public int BigTypeId { get; set; }
        public string BigTypeName { get; set; }

        public virtual ICollection<SmallType> SmallTypes { get; set; }
    }
}
