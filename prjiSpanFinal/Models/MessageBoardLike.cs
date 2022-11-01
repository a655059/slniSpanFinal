using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class MessageBoardLike
    {
        public int MessageBoardLikeId { get; set; }
        public int MessageBoardId { get; set; }
        public int MemberId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual MessageBoard MessageBoard { get; set; }
    }
}
