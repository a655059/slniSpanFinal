using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Follow
    {
        public int MemberId { get; set; }
        public int FollowId { get; set; }
        public int FollowedMemId { get; set; }

        public virtual MemberAccount FollowedMem { get; set; }
        public virtual MemberAccount Member { get; set; }
    }
}
