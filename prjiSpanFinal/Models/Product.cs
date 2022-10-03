using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class Product
    {
        public Product()
        {
            Comments = new HashSet<Comment>();
            Likes = new HashSet<Like>();
            ProductDetails = new HashSet<ProductDetail>();
            ProductPics = new HashSet<ProductPic>();
            ShipperToProducts = new HashSet<ShipperToProduct>();
        }

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int SmallTypeId { get; set; }
        public int MemberId { get; set; }
        public int RegionId { get; set; }
        public decimal AdFee { get; set; }
        public string Description { get; set; }
        public int ProductStatusId { get; set; }

        public virtual MemberAccount Member { get; set; }
        public virtual ProductStatus ProductStatus { get; set; }
        public virtual RegionList Region { get; set; }
        public virtual SmallType SmallType { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ProductPic> ProductPics { get; set; }
        public virtual ICollection<ShipperToProduct> ShipperToProducts { get; set; }
    }
}
