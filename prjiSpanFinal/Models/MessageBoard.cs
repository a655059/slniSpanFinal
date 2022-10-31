using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class MessageBoard
    {
        public int MessageBoardId { get; set; }
        public int MemberId { get; set; }
        public int ProductId { get; set; }
        public string Message { get; set; }
        public int Parent { get; set; }
        public DateTime PostTime { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual Product Product { get; set; }
    }
}
