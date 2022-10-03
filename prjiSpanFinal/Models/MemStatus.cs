using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class MemStatus
    {
        public MemStatus()
        {
            MemberAccounts = new HashSet<MemberAccount>();
        }

        public int MemStatusId { get; set; }
        public string MemStatusName { get; set; }

        public virtual ICollection<MemberAccount> MemberAccounts { get; set; }
    }
}
