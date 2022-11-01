using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class BalanceRecord
    {
        public int BalanceRecordId { get; set; }
        public DateTime Record { get; set; }
        public int MemberId { get; set; }
        public int Amount { get; set; }
        public string Reason { get; set; }

        public virtual MemberAccount Member { get; set; }
    }
}
