using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Verify
    {
        public Verify()
        {
            SubOfficialEventToProducts = new HashSet<SubOfficialEventToProduct>();
        }

        public int VerifyId { get; set; }
        public string VerifyName { get; set; }

        public virtual ICollection<SubOfficialEventToProduct> SubOfficialEventToProducts { get; set; }
    }
}
