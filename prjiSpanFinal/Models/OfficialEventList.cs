using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OfficialEventList
    {
        public OfficialEventList()
        {
            SubOfficialEventLists = new HashSet<SubOfficialEventList>();
        }

        public int OfficialEventListId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime JoinStartDate { get; set; }
        public DateTime JoinEndDate { get; set; }
        public byte[] EventPic { get; set; }

        public virtual ICollection<SubOfficialEventList> SubOfficialEventLists { get; set; }
    }
}
