using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ChatLog
    {
        public int ChatLogId { get; set; }
        public int SendFrom { get; set; }
        public int SendTo { get; set; }
        public string Msg { get; set; }
        public bool HaveRead { get; set; }

        public virtual MemberAccount SendFromNavigation { get; set; }
        public virtual MemberAccount SendToNavigation { get; set; }
    }
}
