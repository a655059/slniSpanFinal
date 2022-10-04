using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Comment
    {
        public Comment()
        {
            CommentPics = new HashSet<CommentPic>();
        }

        public int ProductId { get; set; }
        public int MemberId { get; set; }
        public string Comment1 { get; set; }
        public byte Star { get; set; }
        public int CommentId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<CommentPic> CommentPics { get; set; }
    }
}
