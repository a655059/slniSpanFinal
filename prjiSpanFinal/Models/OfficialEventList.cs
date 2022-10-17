using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class OfficialEventList
    {
        public OfficialEventList()
        {
            Products = new HashSet<Product>();
        }

        public int OfficialEventListId { get; set; }
        public string EventName { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public byte[] EventPic { get; set; }
        public float Discount { get; set; }

        public virtual ICollection<Product> Products { get; set; }
    }
}
