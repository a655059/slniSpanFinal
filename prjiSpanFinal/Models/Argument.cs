using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Argument
    {
        public Argument()
        {
            ArguePics = new HashSet<ArguePic>();
        }

        public int OrderId { get; set; }
        public int ArgumentId { get; set; }
        public bool ChangeorReturn { get; set; }
        public string Reason { get; set; }
        public int ArgumentTypeId { get; set; }

        public virtual ArgumentType ArgumentType { get; set; }
        public virtual Order Order { get; set; }
        public virtual ICollection<ArguePic> ArguePics { get; set; }
    }
}
