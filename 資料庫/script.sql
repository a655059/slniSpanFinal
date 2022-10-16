USE [master]
GO
/****** Object:  Database [iSpanProject]    Script Date: 2022/10/15 下午 11:24:13 ******/
CREATE DATABASE [iSpanProject]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'iSpanProject', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\iSpanProject.mdf' , SIZE = 73728KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'iSpanProject_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\iSpanProject_log.ldf' , SIZE = 73728KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [iSpanProject] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [iSpanProject].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [iSpanProject] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [iSpanProject] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [iSpanProject] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [iSpanProject] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [iSpanProject] SET ARITHABORT OFF 
GO
ALTER DATABASE [iSpanProject] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [iSpanProject] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [iSpanProject] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [iSpanProject] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [iSpanProject] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [iSpanProject] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [iSpanProject] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [iSpanProject] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [iSpanProject] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [iSpanProject] SET  ENABLE_BROKER 
GO
ALTER DATABASE [iSpanProject] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [iSpanProject] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [iSpanProject] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [iSpanProject] SET ALLOW_SNAPSHOT_ISOLATION ON 
GO
ALTER DATABASE [iSpanProject] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [iSpanProject] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [iSpanProject] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [iSpanProject] SET RECOVERY FULL 
GO
ALTER DATABASE [iSpanProject] SET  MULTI_USER 
GO
ALTER DATABASE [iSpanProject] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [iSpanProject] SET DB_CHAINING OFF 
GO
ALTER DATABASE [iSpanProject] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [iSpanProject] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [iSpanProject] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'iSpanProject', N'ON'
GO
ALTER DATABASE [iSpanProject] SET QUERY_STORE = ON
GO
ALTER DATABASE [iSpanProject] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 100, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [iSpanProject]
GO
ALTER DATABASE SCOPED CONFIGURATION SET MAXDOP = 8;
GO
USE [iSpanProject]
GO
/****** Object:  Table [dbo].[AD]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[AD](
	[AdID] [int] IDENTITY(1,1) NOT NULL,
	[AdName] [nvarchar](50) NOT NULL,
	[AdFee] [money] NOT NULL,
	[AdPeriod] [int] NOT NULL,
 CONSTRAINT [PK_AD] PRIMARY KEY CLUSTERED 
(
	[AdID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ADToProduct]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ADToProduct](
	[ADToProductID] [int] IDENTITY(1,1) NOT NULL,
	[AdID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[SubActive] [bit] NOT NULL,
	[ExpoTimes] [int] NOT NULL,
	[ClickTimes] [int] NOT NULL,
 CONSTRAINT [PK_ADToProduct] PRIMARY KEY CLUSTERED 
(
	[ADToProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArguePic]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArguePic](
	[ArguePicID] [int] IDENTITY(1,1) NOT NULL,
	[ArguementID] [int] NOT NULL,
	[ArguePic] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_ArguePic] PRIMARY KEY CLUSTERED 
(
	[ArguePicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Argument]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Argument](
	[OrderID] [int] NOT NULL,
	[ArgumentID] [int] IDENTITY(1,1) NOT NULL,
	[ChangeorReturn] [bit] NOT NULL,
	[Reason] [nvarchar](max) NOT NULL,
	[ArgumentTypeID] [int] NOT NULL,
 CONSTRAINT [PK_Argument] PRIMARY KEY CLUSTERED 
(
	[ArgumentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ArgumentType]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ArgumentType](
	[ArgumentTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ArgumentTypeName] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_ArgumentType] PRIMARY KEY CLUSTERED 
(
	[ArgumentTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[BigType]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BigType](
	[BigTypeID] [int] NOT NULL,
	[BigTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ProductType] PRIMARY KEY CLUSTERED 
(
	[BigTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Comment]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comment](
	[OrderDetailID] [int] NOT NULL,
	[MemberID] [int] NOT NULL,
	[Comment] [nvarchar](500) NOT NULL,
	[CommentStar] [tinyint] NOT NULL,
	[CommentID] [int] IDENTITY(1,1) NOT NULL,
	[CommentTime] [datetime] NOT NULL,
	[ShipperStar] [int] NOT NULL,
 CONSTRAINT [PK_Comment] PRIMARY KEY CLUSTERED 
(
	[CommentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CommentPic]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CommentPic](
	[CommentPicID] [int] IDENTITY(1,1) NOT NULL,
	[CommentID] [int] NOT NULL,
	[CommentPic] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_CommentPic] PRIMARY KEY CLUSTERED 
(
	[CommentPicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CountryList]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CountryList](
	[CountryID] [int] IDENTITY(1,1) NOT NULL,
	[CountryName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_CountryList] PRIMARY KEY CLUSTERED 
(
	[CountryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Coupons]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Coupons](
	[CouponID] [int] IDENTITY(1,1) NOT NULL,
	[CouponName] [nvarchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[ExpiredDate] [datetime] NOT NULL,
	[Discount] [real] NOT NULL,
	[CouponCode] [nvarchar](10) NOT NULL,
	[MemberID] [int] NOT NULL,
	[ReceiveStartDate] [datetime] NOT NULL,
	[ReceiveEndDate] [datetime] NOT NULL,
 CONSTRAINT [PK_Coupons] PRIMARY KEY CLUSTERED 
(
	[CouponID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CouponWallet]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CouponWallet](
	[MemberID] [int] NOT NULL,
	[CouponWalletID] [int] IDENTITY(1,1) NOT NULL,
	[CouponID] [int] NOT NULL,
	[ExpireN_A] [bit] NOT NULL,
 CONSTRAINT [PK_Wallet] PRIMARY KEY CLUSTERED 
(
	[CouponWalletID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[CustomizedCategory]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[CustomizedCategory](
	[CustomizedCategoryID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NOT NULL,
	[CustomizedCategoryName] [nchar](50) NOT NULL,
	[SortNumber] [int] NOT NULL,
 CONSTRAINT [PK_CustomizedCategory] PRIMARY KEY CLUSTERED 
(
	[CustomizedCategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAQ]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQ](
	[FAQID] [int] IDENTITY(1,1) NOT NULL,
	[Answer] [nvarchar](500) NOT NULL,
	[Question] [nvarchar](500) NOT NULL,
	[FAQTypeID] [int] NOT NULL,
 CONSTRAINT [PK_FAQ] PRIMARY KEY CLUSTERED 
(
	[FAQID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FAQType]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FAQType](
	[FAQTypeID] [int] NOT NULL,
	[FAQTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_FAQType] PRIMARY KEY CLUSTERED 
(
	[FAQTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Follows]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Follows](
	[MemberID] [int] NOT NULL,
	[FollowID] [int] IDENTITY(1,1) NOT NULL,
	[FollowedMemID] [int] NOT NULL,
 CONSTRAINT [PK_Follows] PRIMARY KEY CLUSTERED 
(
	[FollowID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Likes]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Likes](
	[MemberID] [int] NOT NULL,
	[LikeID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
 CONSTRAINT [PK_Likes] PRIMARY KEY CLUSTERED 
(
	[LikeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemberAccount]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemberAccount](
	[MemberID] [int] IDENTITY(1,1) NOT NULL,
	[MemberAcc] [nvarchar](10) NOT NULL,
	[MemberPw] [nvarchar](10) NOT NULL,
	[TWorNOT] [bit] NOT NULL,
	[RegionID] [int] NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
	[Email] [nvarchar](50) NOT NULL,
	[BackUpEmail] [nvarchar](50) NOT NULL,
	[Address] [nvarchar](50) NOT NULL,
	[NickName] [nvarchar](50) NOT NULL,
	[Name] [nvarchar](50) NOT NULL,
	[Birthday] [date] NOT NULL,
	[MemPic] [varbinary](max) NULL,
	[MemStatusID] [int] NOT NULL,
	[Gender] [int] NOT NULL,
	[Balance] [int] NOT NULL,
	[ServiceTime] [nvarchar](500) NOT NULL,
	[SellerCaution] [nvarchar](500) NOT NULL,
	[AfterSales] [nvarchar](500) NOT NULL,
	[RenewProduct] [nvarchar](500) NOT NULL,
	[SellerType] [nvarchar](500) NOT NULL,
 CONSTRAINT [PK_MemberAccount] PRIMARY KEY CLUSTERED 
(
	[MemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [U_Email] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [U_MemberAcc] UNIQUE NONCLUSTERED 
(
	[MemberAcc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
 CONSTRAINT [U_Phone] UNIQUE NONCLUSTERED 
(
	[Phone] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[MemStatus]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[MemStatus](
	[MemStatusID] [int] IDENTITY(1,1) NOT NULL,
	[MemStatusName] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_MemStatus] PRIMARY KEY CLUSTERED 
(
	[MemStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OfficialEventList]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OfficialEventList](
	[OfficialEventListID] [int] IDENTITY(1,1) NOT NULL,
	[EventName] [nvarchar](50) NOT NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[EventPic] [varbinary](max) NOT NULL,
	[Discount] [real] NOT NULL,
 CONSTRAINT [PK_OfficialEventList] PRIMARY KEY CLUSTERED 
(
	[OfficialEventListID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderDetails]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderDetails](
	[OrderDetailID] [int] IDENTITY(1,1) NOT NULL,
	[OrderID] [int] NOT NULL,
	[ProductDetailID] [int] NOT NULL,
	[Quantity] [int] NOT NULL,
	[ReceiveDate] [datetime] NOT NULL,
	[ShippingStatusID] [int] NOT NULL,
 CONSTRAINT [PK_OrderDetails_1] PRIMARY KEY CLUSTERED 
(
	[OrderDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Orders]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Orders](
	[OrderID] [int] IDENTITY(1,1) NOT NULL,
	[MemberID] [int] NOT NULL,
	[OrderDatetime] [datetime] NOT NULL,
	[RecieveAdr] [nvarchar](50) NOT NULL,
	[FinishDate] [date] NOT NULL,
	[CouponID] [int] NOT NULL,
	[StatusID] [int] NOT NULL,
	[ShipperID] [int] NOT NULL,
	[PaymentDate] [datetime] NOT NULL,
	[ShippingDate] [datetime] NOT NULL,
	[ReceiveDate] [datetime] NOT NULL,
	[PaymentID] [int] NOT NULL,
	[OrderMessage] [nvarchar](300) NOT NULL,
	[OutAdr] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_OrderDetails] PRIMARY KEY CLUSTERED 
(
	[OrderID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[OrderStatus]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[OrderStatus](
	[OrderStatusID] [int] IDENTITY(1,1) NOT NULL,
	[OrderStatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Statuses] PRIMARY KEY CLUSTERED 
(
	[OrderStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Payment]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Payment](
	[PaymentID] [int] IDENTITY(1,1) NOT NULL,
	[PaymentName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_Payment] PRIMARY KEY CLUSTERED 
(
	[PaymentID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentToProduct]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentToProduct](
	[PaymentID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[PaymentToProductID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_PaymentToProduct] PRIMARY KEY CLUSTERED 
(
	[PaymentToProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[PaymentToSeller]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[PaymentToSeller](
	[PaymentID] [int] NOT NULL,
	[MemberID] [int] NOT NULL,
	[PaymentToMemberID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_PaymentToSeller] PRIMARY KEY CLUSTERED 
(
	[PaymentToMemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Product]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Product](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[ProductName] [nvarchar](200) NOT NULL,
	[SmallTypeID] [int] NOT NULL,
	[MemberID] [int] NOT NULL,
	[RegionID] [int] NOT NULL,
	[Description] [nvarchar](600) NOT NULL,
	[ProductStatusID] [int] NOT NULL,
	[EditTime] [datetime] NOT NULL,
	[CustomizedCategoryID] [int] NOT NULL,
	[OfficialEventListID] [int] NOT NULL,
 CONSTRAINT [PK_Saler] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductDetail]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductDetail](
	[ProductDetailID] [int] IDENTITY(1,1) NOT NULL,
	[ProductID] [int] NOT NULL,
	[Style] [nvarchar](10) NOT NULL,
	[Quantity] [int] NOT NULL,
	[UnitPrice] [money] NOT NULL,
	[Pic] [varbinary](max) NULL,
 CONSTRAINT [PK_Product] PRIMARY KEY CLUSTERED 
(
	[ProductDetailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductPic]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductPic](
	[ProductID] [int] NOT NULL,
	[ProductPicID] [int] IDENTITY(1,1) NOT NULL,
	[Pic] [varbinary](max) NOT NULL,
 CONSTRAINT [PK_ProductPic] PRIMARY KEY CLUSTERED 
(
	[ProductPicID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductStatus]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductStatus](
	[ProductStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ProductStatusName] [nvarchar](10) NOT NULL,
 CONSTRAINT [PK_ProductStatus] PRIMARY KEY CLUSTERED 
(
	[ProductStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[RegionList]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RegionList](
	[RegionID] [int] NOT NULL,
	[RegionName] [nvarchar](50) NOT NULL,
	[CountryID] [int] NOT NULL,
 CONSTRAINT [PK_RegionList] PRIMARY KEY CLUSTERED 
(
	[RegionID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Report]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Report](
	[ReportID] [int] IDENTITY(1,1) NOT NULL,
	[ReporterID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[ReportTypeID] [int] NOT NULL,
	[Reason] [nvarchar](100) NOT NULL,
	[ReportPic] [varbinary](max) NULL,
 CONSTRAINT [PK_Report] PRIMARY KEY CLUSTERED 
(
	[ReportID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ReportType]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReportType](
	[ReportTypeID] [int] IDENTITY(1,1) NOT NULL,
	[ReportTypeName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ReportType] PRIMARY KEY CLUSTERED 
(
	[ReportTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Shipper]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Shipper](
	[ShipperID] [int] IDENTITY(1,1) NOT NULL,
	[ShipperName] [nvarchar](50) NOT NULL,
	[Phone] [nvarchar](20) NOT NULL,
 CONSTRAINT [PK_Shipper] PRIMARY KEY CLUSTERED 
(
	[ShipperID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipperToProduct]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipperToProduct](
	[ShipperID] [int] NOT NULL,
	[ProductID] [int] NOT NULL,
	[ShipperToProductID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ShipperToProduct] PRIMARY KEY CLUSTERED 
(
	[ShipperToProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShipperToSeller]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShipperToSeller](
	[ShipperID] [int] NOT NULL,
	[MemberID] [int] NOT NULL,
	[ShipperToMemberID] [int] IDENTITY(1,1) NOT NULL,
 CONSTRAINT [PK_ShipperToSeller] PRIMARY KEY CLUSTERED 
(
	[ShipperToMemberID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ShippingStatus]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ShippingStatus](
	[ShippingStatusID] [int] IDENTITY(1,1) NOT NULL,
	[ShipStatusName] [nvarchar](50) NOT NULL,
 CONSTRAINT [PK_ShippingStatuses] PRIMARY KEY CLUSTERED 
(
	[ShippingStatusID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[SmallType]    Script Date: 2022/10/15 下午 11:24:13 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[SmallType](
	[SmallTypeID] [int] IDENTITY(1,1) NOT NULL,
	[SmallTypeName] [nvarchar](50) NOT NULL,
	[BigTypeID] [int] NOT NULL,
 CONSTRAINT [PK_SmallType] PRIMARY KEY CLUSTERED 
(
	[SmallTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UX_MemberAccount]    Script Date: 2022/10/15 下午 11:24:13 ******/
CREATE UNIQUE NONCLUSTERED INDEX [UX_MemberAccount] ON [dbo].[MemberAccount]
(
	[MemberAcc] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[AD] ADD  CONSTRAINT [DF_AD_AdName]  DEFAULT ('廣告1') FOR [AdName]
GO
ALTER TABLE [dbo].[AD] ADD  CONSTRAINT [DF_AD_AdFee]  DEFAULT ((0)) FOR [AdFee]
GO
ALTER TABLE [dbo].[ADToProduct] ADD  CONSTRAINT [DF_ADToProduct_ExpoTimes]  DEFAULT ((0)) FOR [ExpoTimes]
GO
ALTER TABLE [dbo].[ADToProduct] ADD  CONSTRAINT [DF_ADToProduct_ClickTimes]  DEFAULT ((0)) FOR [ClickTimes]
GO
ALTER TABLE [dbo].[Argument] ADD  CONSTRAINT [DF_Argument_ChangeorReturn]  DEFAULT ((0)) FOR [ChangeorReturn]
GO
ALTER TABLE [dbo].[Argument] ADD  CONSTRAINT [DF_Argument_Reason]  DEFAULT ('未輸入') FOR [Reason]
GO
ALTER TABLE [dbo].[ArgumentType] ADD  CONSTRAINT [DF_ArgumentType_ArgumentTypeName]  DEFAULT ('糾紛1') FOR [ArgumentTypeName]
GO
ALTER TABLE [dbo].[BigType] ADD  CONSTRAINT [DF_BigType_BigTypeName]  DEFAULT ('大類別1') FOR [BigTypeName]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_Comment]  DEFAULT ('未輸入') FOR [Comment]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_Star]  DEFAULT ((0)) FOR [CommentStar]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_CommentTime]  DEFAULT (((2000)-(1))-(1)) FOR [CommentTime]
GO
ALTER TABLE [dbo].[Comment] ADD  CONSTRAINT [DF_Comment_ShipperStar]  DEFAULT ((0)) FOR [ShipperStar]
GO
ALTER TABLE [dbo].[Coupons] ADD  CONSTRAINT [DF_Coupons_CouponName]  DEFAULT ('折價券1') FOR [CouponName]
GO
ALTER TABLE [dbo].[Coupons] ADD  CONSTRAINT [DF_Coupons_Discount]  DEFAULT ((1)) FOR [Discount]
GO
ALTER TABLE [dbo].[Coupons] ADD  CONSTRAINT [DF_Coupons_DiscountCode]  DEFAULT (N'XXXXXXXX') FOR [CouponCode]
GO
ALTER TABLE [dbo].[CouponWallet] ADD  CONSTRAINT [DF_OfficialCoupons_ExpireN_A]  DEFAULT ((0)) FOR [ExpireN_A]
GO
ALTER TABLE [dbo].[FAQ] ADD  CONSTRAINT [DF_FAQ_Answer]  DEFAULT ('A1') FOR [Answer]
GO
ALTER TABLE [dbo].[FAQ] ADD  CONSTRAINT [DF_FAQ_Question]  DEFAULT ('Q1:') FOR [Question]
GO
ALTER TABLE [dbo].[FAQType] ADD  CONSTRAINT [DF_FAQType_FAQTypeName]  DEFAULT ('FAQType1') FOR [FAQTypeName]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_MemberAcc]  DEFAULT ('Acc1') FOR [MemberAcc]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_MemberPw]  DEFAULT ('PW1') FOR [MemberPw]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_TWorNOT]  DEFAULT ((1)) FOR [TWorNOT]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Phone]  DEFAULT ('09xxxxxxxx') FOR [Phone]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Email]  DEFAULT ('Email') FOR [Email]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_BackUpEmail]  DEFAULT ('') FOR [BackUpEmail]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Address]  DEFAULT ('Add') FOR [Address]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_NickName]  DEFAULT ('') FOR [NickName]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Name]  DEFAULT ('Name') FOR [Name]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Birthday]  DEFAULT ('2000-01-01') FOR [Birthday]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Gender]  DEFAULT ((0)) FOR [Gender]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_Balance]  DEFAULT ((0)) FOR [Balance]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_ServiceTime]  DEFAULT ('') FOR [ServiceTime]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_SellerCaution]  DEFAULT ('') FOR [SellerCaution]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_AfterSales]  DEFAULT ('') FOR [AfterSales]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_RenewProduct]  DEFAULT ('') FOR [RenewProduct]
GO
ALTER TABLE [dbo].[MemberAccount] ADD  CONSTRAINT [DF_MemberAccount_SellerType]  DEFAULT ('') FOR [SellerType]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_Quantity]  DEFAULT ((0)) FOR [Quantity]
GO
ALTER TABLE [dbo].[OrderDetails] ADD  CONSTRAINT [DF_OrderDetails_RecieveDate]  DEFAULT ('2000-01-01') FOR [ReceiveDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_OrderDatetime]  DEFAULT ('2000-01-01') FOR [OrderDatetime]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_RecieveAdr]  DEFAULT ('收貨地址') FOR [RecieveAdr]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_FinishDate]  DEFAULT ('9999-12-31') FOR [FinishDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_ProductID]  DEFAULT ((0)) FOR [ShipperID]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_PaymentDate]  DEFAULT (((2000)-(1))-(1)) FOR [PaymentDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_ShippingDate]  DEFAULT (((2000)-(1))-(1)) FOR [ShippingDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_ReceiveDate]  DEFAULT (((2000)-(1))-(1)) FOR [ReceiveDate]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_OrderMessage]  DEFAULT (N'給賣家的話') FOR [OrderMessage]
GO
ALTER TABLE [dbo].[Orders] ADD  CONSTRAINT [DF_Orders_OutAdr]  DEFAULT ('') FOR [OutAdr]
GO
ALTER TABLE [dbo].[OrderStatus] ADD  CONSTRAINT [DF_OrderStatuses_StatusName]  DEFAULT ('訂單狀態') FOR [OrderStatusName]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_ProductName]  DEFAULT ('產品名稱') FOR [ProductName]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_Description]  DEFAULT ('這個賣家太怠惰了，什麼都沒有寫') FOR [Description]
GO
ALTER TABLE [dbo].[Product] ADD  CONSTRAINT [DF_Product_EditTime]  DEFAULT (((2000)-(1))-(1)) FOR [EditTime]
GO
ALTER TABLE [dbo].[Shipper] ADD  CONSTRAINT [DF_Shipper_ShipperName]  DEFAULT ('Shipper1') FOR [ShipperName]
GO
ALTER TABLE [dbo].[Shipper] ADD  CONSTRAINT [DF_Shipper_Phone]  DEFAULT ('09xxxxxxxx') FOR [Phone]
GO
ALTER TABLE [dbo].[ShippingStatus] ADD  CONSTRAINT [DF_ShippingStatuses_ShipStatusName]  DEFAULT ('未出貨') FOR [ShipStatusName]
GO
ALTER TABLE [dbo].[SmallType] ADD  CONSTRAINT [DF_SmallType_SmallTypeName]  DEFAULT ('小類別名稱') FOR [SmallTypeName]
GO
ALTER TABLE [dbo].[ADToProduct]  WITH CHECK ADD  CONSTRAINT [FK_ADToProduct_AD] FOREIGN KEY([AdID])
REFERENCES [dbo].[AD] ([AdID])
GO
ALTER TABLE [dbo].[ADToProduct] CHECK CONSTRAINT [FK_ADToProduct_AD]
GO
ALTER TABLE [dbo].[ADToProduct]  WITH CHECK ADD  CONSTRAINT [FK_ADToProduct_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ADToProduct] CHECK CONSTRAINT [FK_ADToProduct_Product]
GO
ALTER TABLE [dbo].[ArguePic]  WITH CHECK ADD  CONSTRAINT [FK_ArguePic_Argument] FOREIGN KEY([ArguementID])
REFERENCES [dbo].[Argument] ([ArgumentID])
GO
ALTER TABLE [dbo].[ArguePic] CHECK CONSTRAINT [FK_ArguePic_Argument]
GO
ALTER TABLE [dbo].[Argument]  WITH CHECK ADD  CONSTRAINT [FK_Argument_ArgumentType] FOREIGN KEY([ArgumentTypeID])
REFERENCES [dbo].[ArgumentType] ([ArgumentTypeID])
GO
ALTER TABLE [dbo].[Argument] CHECK CONSTRAINT [FK_Argument_ArgumentType]
GO
ALTER TABLE [dbo].[Argument]  WITH CHECK ADD  CONSTRAINT [FK_Argument_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[Argument] CHECK CONSTRAINT [FK_Argument_Orders]
GO
ALTER TABLE [dbo].[Comment]  WITH CHECK ADD  CONSTRAINT [FK_Comment_OrderDetails] FOREIGN KEY([OrderDetailID])
REFERENCES [dbo].[OrderDetails] ([OrderDetailID])
GO
ALTER TABLE [dbo].[Comment] CHECK CONSTRAINT [FK_Comment_OrderDetails]
GO
ALTER TABLE [dbo].[CommentPic]  WITH CHECK ADD  CONSTRAINT [FK_CommentPic_Comment] FOREIGN KEY([CommentID])
REFERENCES [dbo].[Comment] ([CommentID])
GO
ALTER TABLE [dbo].[CommentPic] CHECK CONSTRAINT [FK_CommentPic_Comment]
GO
ALTER TABLE [dbo].[CouponWallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_Coupons] FOREIGN KEY([CouponID])
REFERENCES [dbo].[Coupons] ([CouponID])
GO
ALTER TABLE [dbo].[CouponWallet] CHECK CONSTRAINT [FK_Wallet_Coupons]
GO
ALTER TABLE [dbo].[CouponWallet]  WITH CHECK ADD  CONSTRAINT [FK_Wallet_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[CouponWallet] CHECK CONSTRAINT [FK_Wallet_MemberAccount]
GO
ALTER TABLE [dbo].[CustomizedCategory]  WITH CHECK ADD  CONSTRAINT [FK_CustomizedCategory_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[CustomizedCategory] CHECK CONSTRAINT [FK_CustomizedCategory_MemberAccount]
GO
ALTER TABLE [dbo].[FAQ]  WITH CHECK ADD  CONSTRAINT [FK_FAQ_FAQType] FOREIGN KEY([FAQTypeID])
REFERENCES [dbo].[FAQType] ([FAQTypeID])
GO
ALTER TABLE [dbo].[FAQ] CHECK CONSTRAINT [FK_FAQ_FAQType]
GO
ALTER TABLE [dbo].[Follows]  WITH CHECK ADD  CONSTRAINT [FK_Follows_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Follows] CHECK CONSTRAINT [FK_Follows_MemberAccount]
GO
ALTER TABLE [dbo].[Follows]  WITH CHECK ADD  CONSTRAINT [FK_Follows_MemberAccount1] FOREIGN KEY([FollowedMemID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Follows] CHECK CONSTRAINT [FK_Follows_MemberAccount1]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_MemberAccount]
GO
ALTER TABLE [dbo].[Likes]  WITH CHECK ADD  CONSTRAINT [FK_Likes_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Likes] CHECK CONSTRAINT [FK_Likes_Product]
GO
ALTER TABLE [dbo].[MemberAccount]  WITH CHECK ADD  CONSTRAINT [FK_MemberAccount_MemStatus] FOREIGN KEY([MemStatusID])
REFERENCES [dbo].[MemStatus] ([MemStatusID])
GO
ALTER TABLE [dbo].[MemberAccount] CHECK CONSTRAINT [FK_MemberAccount_MemStatus]
GO
ALTER TABLE [dbo].[MemberAccount]  WITH CHECK ADD  CONSTRAINT [FK_MemberAccount_RegionList] FOREIGN KEY([RegionID])
REFERENCES [dbo].[RegionList] ([RegionID])
GO
ALTER TABLE [dbo].[MemberAccount] CHECK CONSTRAINT [FK_MemberAccount_RegionList]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Orders] FOREIGN KEY([OrderID])
REFERENCES [dbo].[Orders] ([OrderID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Orders]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_Product] FOREIGN KEY([ProductDetailID])
REFERENCES [dbo].[ProductDetail] ([ProductDetailID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_Product]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [FK_OrderDetails_ShippingStatuses] FOREIGN KEY([ShippingStatusID])
REFERENCES [dbo].[ShippingStatus] ([ShippingStatusID])
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [FK_OrderDetails_ShippingStatuses]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Coupons] FOREIGN KEY([CouponID])
REFERENCES [dbo].[Coupons] ([CouponID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Coupons]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_MemberAccount]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Payment] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([PaymentID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Payment]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Shipper] FOREIGN KEY([ShipperID])
REFERENCES [dbo].[Shipper] ([ShipperID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Shipper]
GO
ALTER TABLE [dbo].[Orders]  WITH CHECK ADD  CONSTRAINT [FK_Orders_Statuses] FOREIGN KEY([StatusID])
REFERENCES [dbo].[OrderStatus] ([OrderStatusID])
GO
ALTER TABLE [dbo].[Orders] CHECK CONSTRAINT [FK_Orders_Statuses]
GO
ALTER TABLE [dbo].[PaymentToProduct]  WITH CHECK ADD  CONSTRAINT [FK_PaymentToProduct_Payment] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([PaymentID])
GO
ALTER TABLE [dbo].[PaymentToProduct] CHECK CONSTRAINT [FK_PaymentToProduct_Payment]
GO
ALTER TABLE [dbo].[PaymentToProduct]  WITH CHECK ADD  CONSTRAINT [FK_PaymentToProduct_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[PaymentToProduct] CHECK CONSTRAINT [FK_PaymentToProduct_Product]
GO
ALTER TABLE [dbo].[PaymentToSeller]  WITH CHECK ADD  CONSTRAINT [FK_PaymentToSeller_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[PaymentToSeller] CHECK CONSTRAINT [FK_PaymentToSeller_MemberAccount]
GO
ALTER TABLE [dbo].[PaymentToSeller]  WITH CHECK ADD  CONSTRAINT [FK_PaymentToSeller_Payment] FOREIGN KEY([PaymentID])
REFERENCES [dbo].[Payment] ([PaymentID])
GO
ALTER TABLE [dbo].[PaymentToSeller] CHECK CONSTRAINT [FK_PaymentToSeller_Payment]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_CustomizedCategory] FOREIGN KEY([CustomizedCategoryID])
REFERENCES [dbo].[CustomizedCategory] ([CustomizedCategoryID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_CustomizedCategory]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_OfficialEventList] FOREIGN KEY([OfficialEventListID])
REFERENCES [dbo].[OfficialEventList] ([OfficialEventListID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_OfficialEventList]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_ProductStatus1] FOREIGN KEY([ProductStatusID])
REFERENCES [dbo].[ProductStatus] ([ProductStatusID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_ProductStatus1]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_Product_SmallType] FOREIGN KEY([SmallTypeID])
REFERENCES [dbo].[SmallType] ([SmallTypeID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_Product_SmallType]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_SalesCourt_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_SalesCourt_MemberAccount]
GO
ALTER TABLE [dbo].[Product]  WITH CHECK ADD  CONSTRAINT [FK_SalesCourt_RegionList] FOREIGN KEY([RegionID])
REFERENCES [dbo].[RegionList] ([RegionID])
GO
ALTER TABLE [dbo].[Product] CHECK CONSTRAINT [FK_SalesCourt_RegionList]
GO
ALTER TABLE [dbo].[ProductDetail]  WITH CHECK ADD  CONSTRAINT [FK_Product_SalesCourt] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductDetail] CHECK CONSTRAINT [FK_Product_SalesCourt]
GO
ALTER TABLE [dbo].[ProductPic]  WITH CHECK ADD  CONSTRAINT [FK_ProductPic_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ProductPic] CHECK CONSTRAINT [FK_ProductPic_Product]
GO
ALTER TABLE [dbo].[RegionList]  WITH CHECK ADD  CONSTRAINT [FK_RegionList_CountryList] FOREIGN KEY([CountryID])
REFERENCES [dbo].[CountryList] ([CountryID])
GO
ALTER TABLE [dbo].[RegionList] CHECK CONSTRAINT [FK_RegionList_CountryList]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_MemberAccount] FOREIGN KEY([ReporterID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_MemberAccount]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_Product]
GO
ALTER TABLE [dbo].[Report]  WITH CHECK ADD  CONSTRAINT [FK_Report_ReportType] FOREIGN KEY([ReportTypeID])
REFERENCES [dbo].[ReportType] ([ReportTypeID])
GO
ALTER TABLE [dbo].[Report] CHECK CONSTRAINT [FK_Report_ReportType]
GO
ALTER TABLE [dbo].[ShipperToProduct]  WITH CHECK ADD  CONSTRAINT [FK_ShipperToProduct_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Product] ([ProductID])
GO
ALTER TABLE [dbo].[ShipperToProduct] CHECK CONSTRAINT [FK_ShipperToProduct_Product]
GO
ALTER TABLE [dbo].[ShipperToProduct]  WITH CHECK ADD  CONSTRAINT [FK_ShipperToProduct_Shipper] FOREIGN KEY([ShipperID])
REFERENCES [dbo].[Shipper] ([ShipperID])
GO
ALTER TABLE [dbo].[ShipperToProduct] CHECK CONSTRAINT [FK_ShipperToProduct_Shipper]
GO
ALTER TABLE [dbo].[ShipperToSeller]  WITH CHECK ADD  CONSTRAINT [FK_ShipperToSeller_MemberAccount] FOREIGN KEY([MemberID])
REFERENCES [dbo].[MemberAccount] ([MemberID])
GO
ALTER TABLE [dbo].[ShipperToSeller] CHECK CONSTRAINT [FK_ShipperToSeller_MemberAccount]
GO
ALTER TABLE [dbo].[ShipperToSeller]  WITH CHECK ADD  CONSTRAINT [FK_ShipperToSeller_Shipper] FOREIGN KEY([ShipperID])
REFERENCES [dbo].[Shipper] ([ShipperID])
GO
ALTER TABLE [dbo].[ShipperToSeller] CHECK CONSTRAINT [FK_ShipperToSeller_Shipper]
GO
ALTER TABLE [dbo].[SmallType]  WITH CHECK ADD  CONSTRAINT [FK_SmallType_BigType] FOREIGN KEY([BigTypeID])
REFERENCES [dbo].[BigType] ([BigTypeID])
GO
ALTER TABLE [dbo].[SmallType] CHECK CONSTRAINT [FK_SmallType_BigType]
GO
ALTER TABLE [dbo].[AD]  WITH CHECK ADD  CONSTRAINT [CK_AD] CHECK  (([AdFee]>=(0)))
GO
ALTER TABLE [dbo].[AD] CHECK CONSTRAINT [CK_AD]
GO
ALTER TABLE [dbo].[OrderDetails]  WITH CHECK ADD  CONSTRAINT [CK_Quantuty] CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[OrderDetails] CHECK CONSTRAINT [CK_Quantuty]
GO
ALTER TABLE [dbo].[ProductDetail]  WITH CHECK ADD  CONSTRAINT [CK_ProductQqt0] CHECK  (([Quantity]>=(0)))
GO
ALTER TABLE [dbo].[ProductDetail] CHECK CONSTRAINT [CK_ProductQqt0]
GO
ALTER TABLE [dbo].[ProductDetail]  WITH CHECK ADD  CONSTRAINT [CK_ProductUPgt0] CHECK  (([UnitPrice]>=(0)))
GO
ALTER TABLE [dbo].[ProductDetail] CHECK CONSTRAINT [CK_ProductUPgt0]
GO
USE [master]
GO
ALTER DATABASE [iSpanProject] SET  READ_WRITE 
GO
