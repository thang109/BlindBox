USE [master]
GO
/****** Object:  Database [BlindBox]    Script Date: 9/4/2024 1:49:20 PM ******/
CREATE DATABASE [BlindBox]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'BlindBox', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER02\MSSQL\DATA\BlindBox.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'BlindBox_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL16.MSSQLSERVER02\MSSQL\DATA\BlindBox_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT, LEDGER = OFF
GO
ALTER DATABASE [BlindBox] SET COMPATIBILITY_LEVEL = 160
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [BlindBox].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [BlindBox] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [BlindBox] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [BlindBox] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [BlindBox] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [BlindBox] SET ARITHABORT OFF 
GO
ALTER DATABASE [BlindBox] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [BlindBox] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [BlindBox] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [BlindBox] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [BlindBox] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [BlindBox] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [BlindBox] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [BlindBox] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [BlindBox] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [BlindBox] SET  DISABLE_BROKER 
GO
ALTER DATABASE [BlindBox] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [BlindBox] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [BlindBox] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [BlindBox] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [BlindBox] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [BlindBox] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [BlindBox] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [BlindBox] SET RECOVERY FULL 
GO
ALTER DATABASE [BlindBox] SET  MULTI_USER 
GO
ALTER DATABASE [BlindBox] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [BlindBox] SET DB_CHAINING OFF 
GO
ALTER DATABASE [BlindBox] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [BlindBox] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [BlindBox] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [BlindBox] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'BlindBox', N'ON'
GO
ALTER DATABASE [BlindBox] SET QUERY_STORE = ON
GO
ALTER DATABASE [BlindBox] SET QUERY_STORE (OPERATION_MODE = READ_WRITE, CLEANUP_POLICY = (STALE_QUERY_THRESHOLD_DAYS = 30), DATA_FLUSH_INTERVAL_SECONDS = 900, INTERVAL_LENGTH_MINUTES = 60, MAX_STORAGE_SIZE_MB = 1000, QUERY_CAPTURE_MODE = AUTO, SIZE_BASED_CLEANUP_MODE = AUTO, MAX_PLANS_PER_QUERY = 200, WAIT_STATS_CAPTURE_MODE = ON)
GO
USE [BlindBox]
GO
/****** Object:  Table [dbo].[ads]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ads](
	[ad_id] [int] IDENTITY(1,1) NOT NULL,
	[image_url] [nvarchar](255) NULL,
	[link_url] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[ad_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[blind_boxes]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[blind_boxes](
	[blind_box_id] [int] IDENTITY(1,1) NOT NULL,
	[name] [nvarchar](255) NOT NULL,
	[description] [nvarchar](max) NULL,
	[price] [decimal](10, 2) NOT NULL,
	[stock] [int] NOT NULL,
	[image_url] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[blind_box_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[commissions]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[commissions](
	[commission_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[shopee_product_id] [nvarchar](255) NULL,
	[commission_amount] [decimal](10, 2) NOT NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[commission_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[order_items]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[order_items](
	[order_item_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[blind_box_id] [int] NOT NULL,
	[quantity] [int] NOT NULL,
	[price] [decimal](10, 2) NOT NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_item_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[orders]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[orders](
	[order_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[total_price] [decimal](10, 2) NOT NULL,
	[status] [nvarchar](50) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[order_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[payments]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[payments](
	[payment_id] [int] IDENTITY(1,1) NOT NULL,
	[order_id] [int] NOT NULL,
	[payment_method] [nvarchar](50) NULL,
	[amount] [decimal](10, 2) NOT NULL,
	[status] [nvarchar](50) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[payment_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[quizzes]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[quizzes](
	[quiz_id] [int] IDENTITY(1,1) NOT NULL,
	[user_id] [int] NOT NULL,
	[quiz_result] [nvarchar](max) NULL,
	[suggested_products] [nvarchar](max) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[quiz_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[users]    Script Date: 9/4/2024 1:49:20 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[users](
	[user_id] [int] IDENTITY(1,1) NOT NULL,
	[username] [nvarchar](255) NOT NULL,
	[password] [nvarchar](255) NOT NULL,
	[email] [nvarchar](255) NOT NULL,
	[full_name] [nvarchar](255) NULL,
	[address] [nvarchar](255) NULL,
	[phone_number] [nvarchar](255) NULL,
	[created_at] [datetime] NULL,
	[updated_at] [datetime] NULL,
PRIMARY KEY CLUSTERED 
(
	[user_id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING ON
GO
/****** Object:  Index [UQ__users__AB6E61649AE3039E]    Script Date: 9/4/2024 1:49:20 PM ******/
ALTER TABLE [dbo].[users] ADD UNIQUE NONCLUSTERED 
(
	[email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO
ALTER TABLE [dbo].[ads] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[ads] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[blind_boxes] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[blind_boxes] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[commissions] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[commissions] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[order_items] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[order_items] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[orders] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[orders] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[payments] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[payments] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[quizzes] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[quizzes] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [created_at]
GO
ALTER TABLE [dbo].[users] ADD  DEFAULT (getdate()) FOR [updated_at]
GO
ALTER TABLE [dbo].[commissions]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([order_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[order_items]  WITH CHECK ADD FOREIGN KEY([blind_box_id])
REFERENCES [dbo].[blind_boxes] ([blind_box_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[order_items]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([order_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[orders]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([user_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[payments]  WITH CHECK ADD FOREIGN KEY([order_id])
REFERENCES [dbo].[orders] ([order_id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[quizzes]  WITH CHECK ADD FOREIGN KEY([user_id])
REFERENCES [dbo].[users] ([user_id])
ON DELETE CASCADE
GO
USE [master]
GO
ALTER DATABASE [BlindBox] SET  READ_WRITE 
GO
