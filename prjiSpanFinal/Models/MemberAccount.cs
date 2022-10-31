using System;
using System.Collections.Generic;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class MemberAccount
    {
        public MemberAccount()
        {
            ChatLogSendFromNavigations = new HashSet<ChatLog>();
            ChatLogSendToNavigations = new HashSet<ChatLog>();
            CouponWallets = new HashSet<CouponWallet>();
            CustomizedCategories = new HashSet<CustomizedCategory>();
            FollowFollowedMems = new HashSet<Follow>();
            FollowMembers = new HashSet<Follow>();
            Likes = new HashSet<Like>();
            MessageBoards = new HashSet<MessageBoard>();
            Notifications = new HashSet<Notification>();
            Orders = new HashSet<Order>();
            PaymentToSellers = new HashSet<PaymentToSeller>();
            Products = new HashSet<Product>();
            ReceiveAdrLists = new HashSet<ReceiveAdrList>();
            Reports = new HashSet<Report>();
            ShipperToSellers = new HashSet<ShipperToSeller>();
            WebAds = new HashSet<WebAd>();
        }

        public int MemberId { get; set; }
        public string MemberAcc { get; set; }
        public string MemberPw { get; set; }
        public bool? IsTw { get; set; }
        public int RegionId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string BackUpEmail { get; set; }
        public string Address { get; set; }
        public string NickName { get; set; }
        public string Name { get; set; }
        public DateTime Birthday { get; set; }
        public byte[] MemPic { get; set; }
        public int MemStatusId { get; set; }
        public int Gender { get; set; }
        public int Balance { get; set; }
        public string Description { get; set; }
        public string ServiceTime { get; set; }
        public string SellerCaution { get; set; }
        public string AfterSales { get; set; }
        public string RenewProduct { get; set; }
        public string SellerType { get; set; }
        public int ReportedTime { get; set; }
        public bool IsAcceptAd { get; set; }

        public virtual MemStatus MemStatus { get; set; }
        public virtual RegionList Region { get; set; }
        public virtual ICollection<ChatLog> ChatLogSendFromNavigations { get; set; }
        public virtual ICollection<ChatLog> ChatLogSendToNavigations { get; set; }
        public virtual ICollection<CouponWallet> CouponWallets { get; set; }
        public virtual ICollection<CustomizedCategory> CustomizedCategories { get; set; }
        public virtual ICollection<Follow> FollowFollowedMems { get; set; }
        public virtual ICollection<Follow> FollowMembers { get; set; }
        public virtual ICollection<Like> Likes { get; set; }
        public virtual ICollection<MessageBoard> MessageBoards { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
        public virtual ICollection<PaymentToSeller> PaymentToSellers { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual ICollection<ReceiveAdrList> ReceiveAdrLists { get; set; }
        public virtual ICollection<Report> Reports { get; set; }
        public virtual ICollection<ShipperToSeller> ShipperToSellers { get; set; }
        public virtual ICollection<WebAd> WebAds { get; set; }
    }
}
