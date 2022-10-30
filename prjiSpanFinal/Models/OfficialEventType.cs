using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OfficialEventType
    {
        public OfficialEventType()
        {
            OfficialEventLists = new HashSet<OfficialEventList>();
        }

        public int OfficialEventTypeId { get; set; }
        public string OfficialEventTypeName { get; set; }

        public virtual ICollection<OfficialEventList> OfficialEventLists { get; set; }
    }
}
