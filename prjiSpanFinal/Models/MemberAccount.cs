using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class MemberAccount
    {
        public MemberAccount()
        {
            Comments = new HashSet<Comment>();
            FollowFollowedMems = new HashSet<Follow>();
            FollowMembers = new HashSet<Follow>();
            Likes = new HashSet<Like>();
            OfficialCoupons = new HashSet<OfficialCoupon>();
            Orders = new HashSet<Order>();
            Products = new HashSet<Product>();
        }

        public int MemberId { get; set; }
        public string MemberAcc { get; set; }
        public string MemberPw { get; set; }
        public bool? TworNot { get; set; }
        public int RegionId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BackUpEmail { get; set; }
        public string Address { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public string Bio { get; set; }
        public byte[] MemPic { get; set; }
        public int MemStatusId { get; set; }

        public virtual MemStatus MemStatus { get; set; }
        public virtual RegionList Region { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Follow> FollowFollowedMems { get; set; }
        public virtual ICollection<Follow> FollowMembers { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<OfficialCoupon> OfficialCoupons { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
