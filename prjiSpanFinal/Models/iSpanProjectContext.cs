using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace prjiSpanFinal.Models
{
    public partial class iSpanProjectContext : DbContext
    {
        public iSpanProjectContext()
        {
        }

        public iSpanProjectContext(DbContextOptions<iSpanProjectContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Ad> Ads { get; set; }
        public virtual DbSet<AdtoProduct> AdtoProducts { get; set; }
        public virtual DbSet<ArguePic> ArguePics { get; set; }
        public virtual DbSet<Argument> Arguments { get; set; }
        public virtual DbSet<ArgumentReason> ArgumentReasons { get; set; }
        public virtual DbSet<ArgumentType> ArgumentTypes { get; set; }
        public virtual DbSet<BigType> BigTypes { get; set; }
        public virtual DbSet<ChatLog> ChatLogs { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentForCustomer> CommentForCustomers { get; set; }
        public virtual DbSet<CommentPic> CommentPics { get; set; }
        public virtual DbSet<CountryList> CountryLists { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<CouponWallet> CouponWallets { get; set; }
        public virtual DbSet<CustomizedCategory> CustomizedCategories { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Faqtype> Faqtypes { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<IconType> IconTypes { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<MemStatus> MemStatuses { get; set; }
        public virtual DbSet<MemberAccount> MemberAccounts { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<OfficialEventList> OfficialEventLists { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<PaymentToSeller> PaymentToSellers { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductPic> ProductPics { get; set; }
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; }
        public virtual DbSet<ReceiveAdrList> ReceiveAdrLists { get; set; }
        public virtual DbSet<RegionList> RegionLists { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<ReportStatus> ReportStatuses { get; set; }
        public virtual DbSet<ReportType> ReportTypes { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<ShipperToSeller> ShipperToSellers { get; set; }
        public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; }
        public virtual DbSet<SmallType> SmallTypes { get; set; }
        public virtual DbSet<SubOfficialEventList> SubOfficialEventLists { get; set; }
        public virtual DbSet<SubOfficialEventToProduct> SubOfficialEventToProducts { get; set; }
        public virtual DbSet<WebAd> WebAds { get; set; }
        public virtual DbSet<WebAdimageType> WebAdimageTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=iSpanProject;Integrated Security=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Chinese_Taiwan_Stroke_CI_AS");

            modelBuilder.Entity<Ad>(entity =>
            {
                entity.ToTable("AD");

                entity.Property(e => e.AdId).HasColumnName("AdID");

                entity.Property(e => e.AdFee).HasColumnType("money");

                entity.Property(e => e.AdName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('廣告1')");
            });

            modelBuilder.Entity<AdtoProduct>(entity =>
            {
                entity.ToTable("ADToProduct");

                entity.Property(e => e.AdtoProductId).HasColumnName("ADToProductID");

                entity.Property(e => e.AdId).HasColumnName("AdID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.StartDate).HasColumnType("datetime");

                entity.HasOne(d => d.Ad)
                    .WithMany(p => p.AdtoProducts)
                    .HasForeignKey(d => d.AdId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ADToProduct_AD");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.AdtoProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ADToProduct_Product");
            });

            modelBuilder.Entity<ArguePic>(entity =>
            {
                entity.ToTable("ArguePic");

                entity.Property(e => e.ArguePicId).HasColumnName("ArguePicID");

                entity.Property(e => e.ArguePic1)
                    .IsRequired()
                    .HasColumnName("ArguePic");

                entity.Property(e => e.ArguementId).HasColumnName("ArguementID");

                entity.HasOne(d => d.Arguement)
                    .WithMany(p => p.ArguePics)
                    .HasForeignKey(d => d.ArguementId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ArguePic_Argument");
            });

            modelBuilder.Entity<Argument>(entity =>
            {
                entity.ToTable("Argument");

                entity.Property(e => e.ArgumentId).HasColumnName("ArgumentID");

                entity.Property(e => e.ArgumentReasonId).HasColumnName("ArgumentReasonID");

                entity.Property(e => e.ArgumentTypeId).HasColumnName("ArgumentTypeID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ReasonText)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('未輸入')");

                entity.HasOne(d => d.ArgumentReason)
                    .WithMany(p => p.Arguments)
                    .HasForeignKey(d => d.ArgumentReasonId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Argument_ArgumentReason");

                entity.HasOne(d => d.ArgumentType)
                    .WithMany(p => p.Arguments)
                    .HasForeignKey(d => d.ArgumentTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Argument_ArgumentType");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.Arguments)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Argument_Orders");
            });

            modelBuilder.Entity<ArgumentReason>(entity =>
            {
                entity.ToTable("ArgumentReason");

                entity.Property(e => e.ArgumentReasonId).HasColumnName("ArgumentReasonID");

                entity.Property(e => e.ArgumentReasonName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<ArgumentType>(entity =>
            {
                entity.ToTable("ArgumentType");

                entity.Property(e => e.ArgumentTypeId).HasColumnName("ArgumentTypeID");

                entity.Property(e => e.ArgumentTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('糾紛1')");
            });

            modelBuilder.Entity<BigType>(entity =>
            {
                entity.ToTable("BigType");

                entity.Property(e => e.BigTypeId).HasColumnName("BigTypeID");

                entity.Property(e => e.BigTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('大類別1')");
            });

            modelBuilder.Entity<ChatLog>(entity =>
            {
                entity.ToTable("ChatLog");

                entity.Property(e => e.ChatLogId).HasColumnName("ChatLogID");

                entity.Property(e => e.Msg).IsRequired();

                entity.HasOne(d => d.SendFromNavigation)
                    .WithMany(p => p.ChatLogSendFromNavigations)
                    .HasForeignKey(d => d.SendFrom)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatLog_MemberAccount");

                entity.HasOne(d => d.SendToNavigation)
                    .WithMany(p => p.ChatLogSendToNavigations)
                    .HasForeignKey(d => d.SendTo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChatLog_MemberAccount1");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Comment")
                    .HasDefaultValueSql("('未輸入')");

                entity.Property(e => e.Comment2)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('未輸入')");

                entity.Property(e => e.Comment3)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('未輸入')");

                entity.Property(e => e.CommentTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((2000)-(1))-(1))");

                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.HasOne(d => d.OrderDetail)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.OrderDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_OrderDetails");
            });

            modelBuilder.Entity<CommentForCustomer>(entity =>
            {
                entity.ToTable("CommentForCustomer");

                entity.Property(e => e.CommentForCustomerId).HasColumnName("CommentForCustomerID");

                entity.Property(e => e.Comment)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.CommentTime).HasColumnType("datetime");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.CommentForCustomers)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentForCustomer_Orders");
            });

            modelBuilder.Entity<CommentPic>(entity =>
            {
                entity.ToTable("CommentPic");

                entity.Property(e => e.CommentPicId).HasColumnName("CommentPicID");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.CommentPic1)
                    .IsRequired()
                    .HasColumnName("CommentPic");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.CommentPics)
                    .HasForeignKey(d => d.CommentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CommentPic_Comment");
            });

            modelBuilder.Entity<CountryList>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("CountryList");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Coupon>(entity =>
            {
                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.CouponCode)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("(N'XXXXXXXX')");

                entity.Property(e => e.CouponName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('折價券1')");

                entity.Property(e => e.Discount).HasDefaultValueSql("((1))");

                entity.Property(e => e.ExpiredDate).HasColumnType("datetime");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OfficialEventListId).HasColumnName("OfficialEventListID");

                entity.Property(e => e.ReceiveEndDate).HasColumnType("datetime");

                entity.Property(e => e.ReceiveStartDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<CouponWallet>(entity =>
            {
                entity.ToTable("CouponWallet");

                entity.Property(e => e.CouponWalletId).HasColumnName("CouponWalletID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.CouponWallets)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_Coupons");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CouponWallets)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_MemberAccount");
            });

            modelBuilder.Entity<CustomizedCategory>(entity =>
            {
                entity.ToTable("CustomizedCategory");

                entity.Property(e => e.CustomizedCategoryId).HasColumnName("CustomizedCategoryID");

                entity.Property(e => e.CustomizedCategoryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.CustomizedCategories)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_CustomizedCategory_MemberAccount");
            });

            modelBuilder.Entity<Faq>(entity =>
            {
                entity.ToTable("FAQ");

                entity.Property(e => e.Faqid).HasColumnName("FAQID");

                entity.Property(e => e.Answer)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('A1')");

                entity.Property(e => e.FaqtypeId).HasColumnName("FAQTypeID");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('Q1:')");

                entity.HasOne(d => d.Faqtype)
                    .WithMany(p => p.Faqs)
                    .HasForeignKey(d => d.FaqtypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_FAQ_FAQType");
            });

            modelBuilder.Entity<Faqtype>(entity =>
            {
                entity.ToTable("FAQType");

                entity.Property(e => e.FaqtypeId).HasColumnName("FAQTypeID");

                entity.Property(e => e.FaqtypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("FAQTypeName")
                    .HasDefaultValueSql("('FAQType1')");
            });

            modelBuilder.Entity<Follow>(entity =>
            {
                entity.Property(e => e.FollowId).HasColumnName("FollowID");

                entity.Property(e => e.FollowedMemId).HasColumnName("FollowedMemID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.FollowedMem)
                    .WithMany(p => p.FollowFollowedMems)
                    .HasForeignKey(d => d.FollowedMemId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Follows_MemberAccount1");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.FollowMembers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Follows_MemberAccount");
            });

            modelBuilder.Entity<IconType>(entity =>
            {
                entity.ToTable("IconType");

                entity.Property(e => e.IconTypeId).HasColumnName("IconTypeID");

                entity.Property(e => e.IconPic).IsRequired();

                entity.Property(e => e.IconTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Like>(entity =>
            {
                entity.Property(e => e.LikeId).HasColumnName("LikeID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_MemberAccount");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Likes)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Likes_Product");
            });

            modelBuilder.Entity<MemStatus>(entity =>
            {
                entity.ToTable("MemStatus");

                entity.Property(e => e.MemStatusId).HasColumnName("MemStatusID");

                entity.Property(e => e.MemStatusName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<MemberAccount>(entity =>
            {
                entity.HasKey(e => e.MemberId);

                entity.ToTable("MemberAccount");

                entity.HasIndex(e => e.MemberAcc, "UX_MemberAccount")
                    .IsUnique();

                entity.HasIndex(e => e.Email, "U_Email")
                    .IsUnique();

                entity.HasIndex(e => e.MemberAcc, "U_MemberAcc")
                    .IsUnique();

                entity.HasIndex(e => e.Phone, "U_Phone")
                    .IsUnique();

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasDefaultValueSql("('Add')");

                entity.Property(e => e.AfterSales)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.BackUpEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Email')");

                entity.Property(e => e.IsTw)
                    .IsRequired()
                    .HasColumnName("IsTW")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.MemStatusId).HasColumnName("MemStatusID");

                entity.Property(e => e.MemberAcc)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('Acc1')");

                entity.Property(e => e.MemberPw)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('PW1')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Name')");

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('09xxxxxxxx')");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.RenewProduct)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SellerCaution)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.SellerType)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ServiceTime)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.MemStatus)
                    .WithMany(p => p.MemberAccounts)
                    .HasForeignKey(d => d.MemStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberAccount_MemStatus");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.MemberAccounts)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MemberAccount_RegionList");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.ToTable("Notification");

                entity.Property(e => e.NotificationId).HasColumnName("NotificationID");

                entity.Property(e => e.IconTypeId).HasColumnName("IconTypeID");

                entity.Property(e => e.Link)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.Time).HasColumnType("datetime");

                entity.HasOne(d => d.IconType)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.IconTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_IconType");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Notification_MemberAccount");
            });

            modelBuilder.Entity<OfficialEventList>(entity =>
            {
                entity.ToTable("OfficialEventList");

                entity.Property(e => e.OfficialEventListId).HasColumnName("OfficialEventListID");

                entity.Property(e => e.EndDate).HasColumnType("datetime");

                entity.Property(e => e.EventName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EventPic).IsRequired();

                entity.Property(e => e.JoinEndDate).HasColumnType("datetime");

                entity.Property(e => e.JoinStartDate).HasColumnType("datetime");

                entity.Property(e => e.StartDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.FinishDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.OrderMessage)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasDefaultValueSql("(N'給賣家的話')");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.ReceiveDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.RecieveAdr)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('收貨地址')");

                entity.Property(e => e.RecieveEmail)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecieveName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecievePhone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.StatusId).HasColumnName("StatusID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Coupons");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_MemberAccount");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Payment");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.ShipperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Shipper");

                entity.HasOne(d => d.Status)
                    .WithMany(p => p.Orders)
                    .HasForeignKey(d => d.StatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Orders_Statuses");
            });

            modelBuilder.Entity<OrderDetail>(entity =>
            {
                entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.ProductDetailId).HasColumnName("ProductDetailID");

                entity.Property(e => e.ShippingStatusId).HasColumnName("ShippingStatusID");

                entity.Property(e => e.Unitprice).HasColumnType("money");

                entity.HasOne(d => d.Order)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.OrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Orders");

                entity.HasOne(d => d.ProductDetail)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ProductDetailId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Product");

                entity.HasOne(d => d.ShippingStatus)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ShippingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_ShippingStatuses");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
                entity.ToTable("OrderStatus");

                entity.Property(e => e.OrderStatusId).HasColumnName("OrderStatusID");

                entity.Property(e => e.OrderStatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('訂單狀態')");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.ToTable("Payment");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.Property(e => e.PaymentName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<PaymentToSeller>(entity =>
            {
                entity.HasKey(e => e.PaymentToMemberId);

                entity.ToTable("PaymentToSeller");

                entity.Property(e => e.PaymentToMemberId).HasColumnName("PaymentToMemberID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.PaymentId).HasColumnName("PaymentID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.PaymentToSellers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentToSeller_MemberAccount");

                entity.HasOne(d => d.Payment)
                    .WithMany(p => p.PaymentToSellers)
                    .HasForeignKey(d => d.PaymentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PaymentToSeller_Payment");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.CustomizedCategoryId).HasColumnName("CustomizedCategoryID");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasDefaultValueSql("('這個賣家太怠惰了，什麼都沒有寫')");

                entity.Property(e => e.EditTime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(((2000)-(1))-(1))");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasDefaultValueSql("('產品名稱')");

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SmallTypeId).HasColumnName("SmallTypeID");

                entity.HasOne(d => d.CustomizedCategory)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.CustomizedCategoryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_CustomizedCategory");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesCourt_MemberAccount");

                entity.HasOne(d => d.ProductStatus)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.ProductStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_ProductStatus1");

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SalesCourt_RegionList");

                entity.HasOne(d => d.SmallType)
                    .WithMany(p => p.Products)
                    .HasForeignKey(d => d.SmallTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_SmallType");
            });

            modelBuilder.Entity<ProductDetail>(entity =>
            {
                entity.ToTable("ProductDetail");

                entity.Property(e => e.ProductDetailId).HasColumnName("ProductDetailID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Style)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_SalesCourt");
            });

            modelBuilder.Entity<ProductPic>(entity =>
            {
                entity.ToTable("ProductPic");

                entity.Property(e => e.ProductPicId).HasColumnName("ProductPicID");

                entity.Property(e => e.Pic).IsRequired();

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductPics)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProductPic_Product");
            });

            modelBuilder.Entity<ProductStatus>(entity =>
            {
                entity.ToTable("ProductStatus");

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.ProductStatusName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<ReceiveAdrList>(entity =>
            {
                entity.HasKey(e => e.ReceiveAdrList1);

                entity.ToTable("ReceiveAdrList");

                entity.Property(e => e.ReceiveAdrList1).HasColumnName("ReceiveAdrList");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ReceiveAdr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ReceiveAdrLists)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ReceiveAdrList_MemberAccount");
            });

            modelBuilder.Entity<RegionList>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("RegionList");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.CountryId).HasColumnName("CountryID");

                entity.Property(e => e.RegionName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.RegionLists)
                    .HasForeignKey(d => d.CountryId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_RegionList_CountryList");
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("Report");

                entity.Property(e => e.ReportId).HasColumnName("ReportID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ReportStatusId).HasColumnName("ReportStatusID");

                entity.Property(e => e.ReportTypeId).HasColumnName("ReportTypeID");

                entity.Property(e => e.ReporterId).HasColumnName("ReporterID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_Product");

                entity.HasOne(d => d.ReportStatus)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReportStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_ReportStatus");

                entity.HasOne(d => d.ReportType)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReportTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_ReportType");

                entity.HasOne(d => d.Reporter)
                    .WithMany(p => p.Reports)
                    .HasForeignKey(d => d.ReporterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Report_MemberAccount");
            });

            modelBuilder.Entity<ReportStatus>(entity =>
            {
                entity.ToTable("ReportStatus");

                entity.Property(e => e.ReportStatusId).HasColumnName("ReportStatusID");

                entity.Property(e => e.ReportStatusName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<ReportType>(entity =>
            {
                entity.ToTable("ReportType");

                entity.Property(e => e.ReportTypeId).HasColumnName("ReportTypeID");

                entity.Property(e => e.ReportTypeName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Shipper>(entity =>
            {
                entity.ToTable("Shipper");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('09xxxxxxxx')");

                entity.Property(e => e.ShipperName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Shipper1')");
            });

            modelBuilder.Entity<ShipperToSeller>(entity =>
            {
                entity.HasKey(e => e.ShipperToMemberId);

                entity.ToTable("ShipperToSeller");

                entity.Property(e => e.ShipperToMemberId).HasColumnName("ShipperToMemberID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.ShipperToSellers)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipperToSeller_MemberAccount");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.ShipperToSellers)
                    .HasForeignKey(d => d.ShipperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipperToSeller_Shipper");
            });

            modelBuilder.Entity<ShippingStatus>(entity =>
            {
                entity.ToTable("ShippingStatus");

                entity.Property(e => e.ShippingStatusId).HasColumnName("ShippingStatusID");

                entity.Property(e => e.ShipStatusName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('未出貨')");
            });

            modelBuilder.Entity<SmallType>(entity =>
            {
                entity.ToTable("SmallType");

                entity.Property(e => e.SmallTypeId).HasColumnName("SmallTypeID");

                entity.Property(e => e.BigTypeId).HasColumnName("BigTypeID");

                entity.Property(e => e.SmallTypeName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('小類別名稱')");

                entity.HasOne(d => d.BigType)
                    .WithMany(p => p.SmallTypes)
                    .HasForeignKey(d => d.BigTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SmallType_BigType");
            });

            modelBuilder.Entity<SubOfficialEventList>(entity =>
            {
                entity.ToTable("SubOfficialEventList");

                entity.Property(e => e.SubOfficialEventListId).HasColumnName("SubOfficialEventListID");

                entity.Property(e => e.OfficialEventListId).HasColumnName("OfficialEventListID");

                entity.Property(e => e.SubEventName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.OfficialEventList)
                    .WithMany(p => p.SubOfficialEventLists)
                    .HasForeignKey(d => d.OfficialEventListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubOfficialEventList_OfficialEventList");
            });

            modelBuilder.Entity<SubOfficialEventToProduct>(entity =>
            {
                entity.ToTable("SubOfficialEventToProduct");

                entity.Property(e => e.SubOfficialEventToProductId).HasColumnName("SubOfficialEventToProductID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.SubOfficialEventListId).HasColumnName("SubOfficialEventListID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.SubOfficialEventToProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubOfficialEventToProduct_Product");

                entity.HasOne(d => d.SubOfficialEventList)
                    .WithMany(p => p.SubOfficialEventToProducts)
                    .HasForeignKey(d => d.SubOfficialEventListId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SubOfficialEventToProduct_SubOfficialEventList");
            });

            modelBuilder.Entity<WebAd>(entity =>
            {
                entity.ToTable("WebAD");

                entity.Property(e => e.WebAdid).HasColumnName("WebADID");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.WebAdimage)
                    .IsRequired()
                    .HasColumnName("WebADImage");

                entity.Property(e => e.WebAdimageTypeId).HasColumnName("WebADImageTypeID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.WebAds)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebAD_MemberAccount");

                entity.HasOne(d => d.WebAdimageType)
                    .WithMany(p => p.WebAds)
                    .HasForeignKey(d => d.WebAdimageTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WebAD_WebADImageType");
            });

            modelBuilder.Entity<WebAdimageType>(entity =>
            {
                entity.ToTable("WebADImageType");

                entity.Property(e => e.WebAdimageTypeId).HasColumnName("WebADImageTypeID");

                entity.Property(e => e.ImageType)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
