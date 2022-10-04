using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ProductPic
    {
        public int ProductId { get; set; }
        public int PicId { get; set; }
        public byte[] Picture { get; set; }

        public virtual Product Product { get; set; }
    }
}
