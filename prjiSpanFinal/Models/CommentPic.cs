using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class CommentPic
    {
        public int CommentPicId { get; set; }
        public int CommentId { get; set; }
        public byte[] CommentPic1 { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
