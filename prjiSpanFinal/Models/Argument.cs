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

        public int ArgumentId { get; set; }
        public int OrderdetailId { get; set; }
        public int ArgumentTypeId { get; set; }
        public int ArgumentReasonId { get; set; }
        public string ReasonText { get; set; }

        public virtual ArgumentReason ArgumentReason { get; set; }
        public virtual ArgumentType ArgumentType { get; set; }
        public virtual OrderDetail Orderdetail { get; set; }
        public virtual ICollection<ArguePic> ArguePics { get; set; }
    }
}
