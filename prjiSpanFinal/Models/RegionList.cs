using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class RegionList
    {
        public RegionList()
        {
            MemberAccounts = new HashSet<MemberAccount>();
            Products = new HashSet<Product>();
        }

        public int RegionId { get; set; }
        public string RegionName { get; set; }
        public int CountryId { get; set; }

        public virtual CountryList Country { get; set; }
        public virtual ICollection<MemberAccount> MemberAccounts { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
