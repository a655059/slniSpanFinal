using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ArgumentType
    {
        public ArgumentType()
        {
            Arguments = new HashSet<Argument>();
        }

        public int ArgumentTypeId { get; set; }
        public string ArgumentTypeName { get; set; }

        public virtual ICollection<Argument> Arguments { get; set; }
    }
}
