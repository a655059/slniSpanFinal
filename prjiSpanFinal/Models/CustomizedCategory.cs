using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class CustomizedCategory
    {
        public CustomizedCategory()
        {
            Products = new HashSet<Product>();
        }

        public int CustomizedCategoryId { get; set; }
        public int MemberId { get; set; }
        public string CustomizedCategoryName { get; set; }
        public int SortNumber { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
