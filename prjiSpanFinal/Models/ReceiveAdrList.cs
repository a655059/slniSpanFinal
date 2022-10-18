using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ReceiveAdrList
    {
        public int ReceiveAdrList1 { get; set; }
        public int MemberId { get; set; }
        public string ReceiveAdr { get; set; }

        public virtual MemberAccount Member { get; set; }
    }
}
