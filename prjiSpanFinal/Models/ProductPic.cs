using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class ProductPic
    {
        public int ProductPicId { get; set; }
        public int ProductId { get; set; }
        public byte[] Pic { get; set; }

        public virtual Product Product { get; set; }
    }
}
