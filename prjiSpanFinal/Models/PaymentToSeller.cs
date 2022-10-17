using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class PaymentToSeller
    {
        public int PaymentId { get; set; }
        public int MemberId { get; set; }
        public int PaymentToMemberId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual Payment Payment { get; set; }
    }
}
