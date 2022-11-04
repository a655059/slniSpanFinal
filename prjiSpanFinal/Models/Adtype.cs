using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Adtype
    {
        public Adtype()
        {
            Ads = new HashSet<Ad>();
        }

        public int AdTypeId { get; set; }
        public string AdType1 { get; set; }
        public string AdTyepDescription { get; set; }

        public virtual ICollection<Ad> Ads { get; set; }
    }
}
