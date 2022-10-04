using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ArguePic
    {
        public int ArguePicId { get; set; }
        public int ArguementId { get; set; }
        public byte[] ArguePic1 { get; set; }

        public virtual Argument Arguement { get; set; }
    }
}
