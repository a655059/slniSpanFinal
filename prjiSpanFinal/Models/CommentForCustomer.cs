using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class CommentForCustomer
    {
        public int CommentForCustomerId { get; set; }
        public int OrderId { get; set; }
        public string Comment { get; set; }
        public byte CommentStar { get; set; }
        public DateTime CommentTime { get; set; }

        public virtual Order Order { get; set; }
    }
}
