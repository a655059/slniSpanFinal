using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class CountryList
    {
        public CountryList()
        {
            RegionLists = new HashSet<RegionList>();
        }

        public int CountryId { get; set; }
        public string CountryName { get; set; }

        public virtual ICollection<RegionList> RegionLists { get; set; }
    }
}
