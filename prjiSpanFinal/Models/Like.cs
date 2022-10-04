using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Like
    {
        public int MemberId { get; set; }
        public int LikeId { get; set; }
        public int ProductId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual Product Product { get; set; }
    }
}
