using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class WebAd
    {
        public int WebAdid { get; set; }
        public byte[] WebAdimage { get; set; }
        public int MemberId { get; set; }
        public int WebAdimageTypeId { get; set; }
        public bool IsPublishing { get; set; }
        public string Path { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual WebAdimageType WebAdimageType { get; set; }
    }
}
