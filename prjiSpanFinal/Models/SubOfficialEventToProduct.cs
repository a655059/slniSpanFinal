using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class SubOfficialEventToProduct
    {
        public int SubOfficialEventToProductId { get; set; }
        public int SubOfficialEventListId { get; set; }
        public int ProductId { get; set; }
        public int VerifyId { get; set; }

        public virtual Product Product { get; set; }
        public virtual SubOfficialEventList SubOfficialEventList { get; set; }
        public virtual Verify Verify { get; set; }
    }
}
