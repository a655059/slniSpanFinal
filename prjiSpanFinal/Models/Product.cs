using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Product
    {
        public Product()
        {
            AdtoProducts = new HashSet<AdtoProduct>();
            Likes = new HashSet<Like>();
            MessageBoards = new HashSet<MessageBoard>();
            ProductDetails = new HashSet<ProductDetail>();
            ProductPics = new HashSet<ProductPic>();
            Reports = new HashSet<Report>();
            SubOfficialEventToProducts = new HashSet<SubOfficialEventToProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SmallTypeId { get; set; }
        public int MemberId { get; set; }
        public int RegionId { get; set; }
        public string Description { get; set; }
        public int ProductStatusId { get; set; }
        public DateTime EditTime { get; set; }
        public int CustomizedCategoryId { get; set; }

        public virtual CustomizedCategory CustomizedCategory { get; set; }
        public virtual MemberAccount Member { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual RegionList Region { get; set; }
        public virtual SmallType SmallType { get; set; }
        public virtual ICollection<AdtoProduct> AdtoProducts { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<MessageBoard> MessageBoards { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductPic> ProductPics { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<SubOfficialEventToProduct> SubOfficialEventToProducts { get; set; }
    }
}
