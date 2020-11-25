USE [master]
GO
/****** Object:  Database [Product_Manager]    Script Date: 11/25/2020 1:14:23 PM ******/
CREATE DATABASE [Product_Manager]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'assignment_1', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\assignment_1.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'assignment_1_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\assignment_1_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [Product_Manager] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [Product_Manager].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [Product_Manager] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [Product_Manager] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [Product_Manager] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [Product_Manager] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [Product_Manager] SET ARITHABORT OFF 
GO
ALTER DATABASE [Product_Manager] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [Product_Manager] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [Product_Manager] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [Product_Manager] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [Product_Manager] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [Product_Manager] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [Product_Manager] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [Product_Manager] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [Product_Manager] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [Product_Manager] SET  DISABLE_BROKER 
GO
ALTER DATABASE [Product_Manager] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [Product_Manager] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [Product_Manager] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [Product_Manager] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [Product_Manager] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [Product_Manager] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [Product_Manager] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [Product_Manager] SET RECOVERY FULL 
GO
ALTER DATABASE [Product_Manager] SET  MULTI_USER 
GO
ALTER DATABASE [Product_Manager] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [Product_Manager] SET DB_CHAINING OFF 
GO
ALTER DATABASE [Product_Manager] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [Product_Manager] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [Product_Manager] SET DELAYED_DURABILITY = DISABLED 
GO
EXEC sys.sp_db_vardecimal_storage_format N'Product_Manager', N'ON'
GO
ALTER DATABASE [Product_Manager] SET QUERY_STORE = OFF
GO
USE [Product_Manager]
GO
/****** Object:  Table [dbo].[Categories]    Script Date: 11/25/2020 1:14:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Categories](
	[CategoryID] [int] IDENTITY(1,1) NOT NULL,
	[CategoryName] [nvarchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[CategoryName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[ProductCategoryRelation]    Script Date: 11/25/2020 1:14:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ProductCategoryRelation](
	[ProductID] [int] NOT NULL,
	[CategoryID] [int] NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC,
	[CategoryID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Products]    Script Date: 11/25/2020 1:14:23 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Products](
	[ProductID] [int] IDENTITY(1,1) NOT NULL,
	[article_number] [varchar](max) NOT NULL,
	[product_name] [varchar](50) NOT NULL,
	[product_description] [varchar](max) NOT NULL,
	[product_price] [int] NOT NULL,
 CONSTRAINT [PK_Products] PRIMARY KEY CLUSTERED 
(
	[ProductID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
ALTER TABLE [dbo].[ProductCategoryRelation]  WITH CHECK ADD  CONSTRAINT [FK_Category] FOREIGN KEY([CategoryID])
REFERENCES [dbo].[Categories] ([CategoryID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductCategoryRelation] CHECK CONSTRAINT [FK_Category]
GO
ALTER TABLE [dbo].[ProductCategoryRelation]  WITH CHECK ADD  CONSTRAINT [FK_Product] FOREIGN KEY([ProductID])
REFERENCES [dbo].[Products] ([ProductID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[ProductCategoryRelation] CHECK CONSTRAINT [FK_Product]
GO
USE [master]
GO
ALTER DATABASE [Product_Manager] SET  READ_WRITE 
GO
