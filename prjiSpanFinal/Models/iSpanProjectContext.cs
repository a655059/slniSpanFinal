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
        public virtual DbSet<ArguePic> ArguePics { get; set; }
        public virtual DbSet<Argument> Arguments { get; set; }
        public virtual DbSet<ArgumentType> ArgumentTypes { get; set; }
        public virtual DbSet<BigType> BigTypes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<CommentPic> CommentPics { get; set; }
        public virtual DbSet<CountryList> CountryLists { get; set; }
        public virtual DbSet<Coupon> Coupons { get; set; }
        public virtual DbSet<Faq> Faqs { get; set; }
        public virtual DbSet<Faqtype> Faqtypes { get; set; }
        public virtual DbSet<Follow> Follows { get; set; }
        public virtual DbSet<Like> Likes { get; set; }
        public virtual DbSet<MemStatus> MemStatuses { get; set; }
        public virtual DbSet<MemberAccount> MemberAccounts { get; set; }
        public virtual DbSet<OfficialCoupon> OfficialCoupons { get; set; }
        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderStatus> OrderStatuses { get; set; }
        public virtual DbSet<Payment> Payments { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ProductDetail> ProductDetails { get; set; }
        public virtual DbSet<ProductPic> ProductPics { get; set; }
        public virtual DbSet<ProductStatus> ProductStatuses { get; set; }
        public virtual DbSet<RegionList> RegionLists { get; set; }
        public virtual DbSet<Shipper> Shippers { get; set; }
        public virtual DbSet<ShipperToProduct> ShipperToProducts { get; set; }
        public virtual DbSet<ShippingStatus> ShippingStatuses { get; set; }
        public virtual DbSet<SmallType> SmallTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
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

                entity.Property(e => e.ArgumentTypeId).HasColumnName("ArgumentTypeID");

                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasDefaultValueSql("('未輸入')");

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

            modelBuilder.Entity<ArgumentType>(entity =>
            {
                entity.ToTable("ArgumentType");

                entity.Property(e => e.ArgumentTypeId).HasColumnName("ArgumentTypeID");

                entity.Property(e => e.ArgumentTypeName)
                    .IsRequired()
                    .HasMaxLength(500)
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

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.CommentId).HasColumnName("CommentID");

                entity.Property(e => e.Comment1)
                    .IsRequired()
                    .HasMaxLength(500)
                    .HasColumnName("Comment")
                    .HasDefaultValueSql("('未輸入')");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_MemberAccount");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Comment_Product");
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

                entity.Property(e => e.CouponName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('折價券1')");

                entity.Property(e => e.Discount).HasDefaultValueSql("((1))");

                entity.Property(e => e.ExpiredDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('9999-12-31')");

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('1000-01-01')");
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

                entity.Property(e => e.FaqtypeId)
                    .ValueGeneratedNever()
                    .HasColumnName("FAQTypeID");

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
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Add')");

                entity.Property(e => e.BackUpEmail).HasMaxLength(50);

                entity.Property(e => e.Bio).HasDefaultValueSql("('這個使用者太怠惰了，現在還沒有介紹')");

                entity.Property(e => e.Birthday)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('Email')");

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

                entity.Property(e => e.NickName).HasMaxLength(50);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('09xxxxxxxx')");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.TworNot)
                    .IsRequired()
                    .HasColumnName("TWorNOT")
                    .HasDefaultValueSql("((1))");

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

            modelBuilder.Entity<OfficialCoupon>(entity =>
            {
                entity.HasKey(e => e.OfficialCouponsId)
                    .HasName("PK_Wallet");

                entity.Property(e => e.OfficialCouponsId).HasColumnName("OfficialCouponsID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.ExpireNA).HasColumnName("ExpireN_A");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.HasOne(d => d.Coupon)
                    .WithMany(p => p.OfficialCoupons)
                    .HasForeignKey(d => d.CouponId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_Coupons");

                entity.HasOne(d => d.Member)
                    .WithMany(p => p.OfficialCoupons)
                    .HasForeignKey(d => d.MemberId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Wallet_MemberAccount");
            });

            modelBuilder.Entity<Order>(entity =>
            {
                entity.Property(e => e.OrderId).HasColumnName("OrderID");

                entity.Property(e => e.CouponId).HasColumnName("CouponID");

                entity.Property(e => e.FinishDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('9999-12-31')");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.OrderDatetime)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.RecieveAdr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('收貨地址')");

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

                entity.Property(e => e.OutAdr)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasDefaultValueSql("('出貨地址')");

                entity.Property(e => e.ProductDetailId).HasColumnName("ProductDetailID");

                entity.Property(e => e.RecieveDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('9999-12-31')");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.Property(e => e.ShippingDate)
                    .HasColumnType("date")
                    .HasDefaultValueSql("('2000-01-01')");

                entity.Property(e => e.ShippingStatusId).HasColumnName("ShippingStatusID");

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

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ShipperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_Shipper");

                entity.HasOne(d => d.ShippingStatus)
                    .WithMany(p => p.OrderDetails)
                    .HasForeignKey(d => d.ShippingStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_OrderDetails_ShippingStatuses");
            });

            modelBuilder.Entity<OrderStatus>(entity =>
            {
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

                entity.Property(e => e.Fee).HasColumnType("money");

                entity.Property(e => e.PaymentName)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("Product");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.AdFee).HasColumnType("money");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasDefaultValueSql("('這個賣家太怠惰了，什麼都沒有寫')");

                entity.Property(e => e.MemberId).HasColumnName("MemberID");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasDefaultValueSql("('產品名稱')");

                entity.Property(e => e.ProductStatusId).HasColumnName("ProductStatusID");

                entity.Property(e => e.RegionId).HasColumnName("RegionID");

                entity.Property(e => e.SmallTypeId).HasColumnName("SmallTypeID");

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

                entity.Property(e => e.Style).IsRequired();

                entity.Property(e => e.UnitPrice).HasColumnType("money");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ProductDetails)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_SalesCourt");
            });

            modelBuilder.Entity<ProductPic>(entity =>
            {
                entity.HasKey(e => e.PicId);

                entity.ToTable("ProductPic");

                entity.Property(e => e.PicId).HasColumnName("PicID");

                entity.Property(e => e.Picture)
                    .IsRequired()
                    .HasColumnName("picture");

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

            modelBuilder.Entity<RegionList>(entity =>
            {
                entity.HasKey(e => e.RegionId);

                entity.ToTable("RegionList");

                entity.Property(e => e.RegionId)
                    .ValueGeneratedNever()
                    .HasColumnName("RegionID");

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

            modelBuilder.Entity<ShipperToProduct>(entity =>
            {
                entity.HasKey(e => e.ProductToShipperId);

                entity.ToTable("ShipperToProduct");

                entity.Property(e => e.ProductToShipperId).HasColumnName("ProductToShipperID");

                entity.Property(e => e.ProductId).HasColumnName("ProductID");

                entity.Property(e => e.ShipperId).HasColumnName("ShipperID");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShipperToProducts)
                    .HasForeignKey(d => d.ProductId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipperToProduct_Product");

                entity.HasOne(d => d.Shipper)
                    .WithMany(p => p.ShipperToProducts)
                    .HasForeignKey(d => d.ShipperId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ShipperToProduct_Shipper");
            });

            modelBuilder.Entity<ShippingStatus>(entity =>
            {
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

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
