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

        public int CommentId { get; set; }
        public int OrderDetailId { get; set; }
        public string Comment1 { get; set; }
        public byte CommentStar { get; set; }
        public DateTime CommentTime { get; set; }
        public int ShipperStar { get; set; }

        public virtual OrderDetail OrderDetail { get; set; }
        public virtual ICollection<CommentPic> CommentPics { get; set; }
    }
}
