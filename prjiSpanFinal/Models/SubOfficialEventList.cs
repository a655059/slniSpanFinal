using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class SubOfficialEventList
    {
        public SubOfficialEventList()
        {
            SubOfficialEventToProducts = new HashSet<SubOfficialEventToProduct>();
        }

        public int SubOfficialEventListId { get; set; }
        public int OfficialEventListId { get; set; }
        public string SubEventName { get; set; }
        public float Discount { get; set; }
        public bool IsFreeDelivery { get; set; }

        public virtual OfficialEventList OfficialEventList { get; set; }
        public virtual ICollection<SubOfficialEventToProduct> SubOfficialEventToProducts { get; set; }
    }
}
