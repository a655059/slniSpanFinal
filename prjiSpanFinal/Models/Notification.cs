using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Notification
    {
        public int NotificationId { get; set; }
        public int IconTypeId { get; set; }
        public string Text { get; set; }
        public string Link { get; set; }
        public int MemberId { get; set; }
        public bool HaveRead { get; set; }
        public DateTime Time { get; set; }
        public string TextContent { get; set; }

        public virtual IconType IconType { get; set; }
        public virtual MemberAccount Member { get; set; }
    }
}
