using System;
using System.Collections.Generic;


namespace prjiSpanFinal.Models.App
{
    public partial class CComment
    {
        public byte[] memPic { get; set; }
        public int CommentId { get; set; }
        public int OrderDetailId { get; set; }
        public string Comment1 { get; set; }
        public byte CommentStar { get; set; }
        public DateTime CommentTime { get; set; }
        public int ShipperStar { get; set; }
        public string Comment2 { get; set; }
        public string Comment3 { get; set; }

        public List<byte[]> cpics { get; set; }

        public string commentacc { get; set; }
    }
}
