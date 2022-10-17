using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ArgumentReason
    {
        public ArgumentReason()
        {
            Arguments = new HashSet<Argument>();
        }

        public int ArgumentReasonId { get; set; }
        public string ArgumentReasonName { get; set; }

        public virtual ICollection<Argument> Arguments { get; set; }
    }
}
