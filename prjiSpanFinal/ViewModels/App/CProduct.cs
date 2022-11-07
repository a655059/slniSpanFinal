using System;
using System.Collections.Generic;


namespace prjiSpanFinal.Models.App
{
    public partial class CProduct
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SmallTypeId { get; set; }
        public int MemberId { get; set; }
        public int RegionId { get; set; }
        public string Description { get; set; }
        public int ProductStatusId { get; set; }
        public DateTime EditTime { get; set; }
        public int CustomizedCategoryId { get; set; }
        public bool IsFeaturedProduct { get; set; }
    }
}
