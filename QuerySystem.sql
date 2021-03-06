USE [master]
GO
/****** Object:  Database [QuerySystem]    Script Date: 2022/5/12 下午 07:17:08 ******/
CREATE DATABASE [QuerySystem]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'QuerySystem', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\QuerySystem.mdf' , SIZE = 8192KB , MAXSIZE = UNLIMITED, FILEGROWTH = 65536KB )
 LOG ON 
( NAME = N'QuerySystem_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL15.MSSQLSERVER\MSSQL\DATA\QuerySystem_log.ldf' , SIZE = 8192KB , MAXSIZE = 2048GB , FILEGROWTH = 65536KB )
 WITH CATALOG_COLLATION = DATABASE_DEFAULT
GO
ALTER DATABASE [QuerySystem] SET COMPATIBILITY_LEVEL = 150
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [QuerySystem].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [QuerySystem] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [QuerySystem] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [QuerySystem] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [QuerySystem] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [QuerySystem] SET ARITHABORT OFF 
GO
ALTER DATABASE [QuerySystem] SET AUTO_CLOSE OFF 
GO
ALTER DATABASE [QuerySystem] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [QuerySystem] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [QuerySystem] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [QuerySystem] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [QuerySystem] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [QuerySystem] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [QuerySystem] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [QuerySystem] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [QuerySystem] SET  DISABLE_BROKER 
GO
ALTER DATABASE [QuerySystem] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [QuerySystem] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [QuerySystem] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [QuerySystem] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [QuerySystem] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [QuerySystem] SET READ_COMMITTED_SNAPSHOT OFF 
GO
ALTER DATABASE [QuerySystem] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [QuerySystem] SET RECOVERY FULL 
GO
ALTER DATABASE [QuerySystem] SET  MULTI_USER 
GO
ALTER DATABASE [QuerySystem] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [QuerySystem] SET DB_CHAINING OFF 
GO
ALTER DATABASE [QuerySystem] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [QuerySystem] SET TARGET_RECOVERY_TIME = 60 SECONDS 
GO
ALTER DATABASE [QuerySystem] SET DELAYED_DURABILITY = DISABLED 
GO
ALTER DATABASE [QuerySystem] SET ACCELERATED_DATABASE_RECOVERY = OFF  
GO
EXEC sys.sp_db_vardecimal_storage_format N'QuerySystem', N'ON'
GO
ALTER DATABASE [QuerySystem] SET QUERY_STORE = OFF
GO
USE [QuerySystem]
GO
/****** Object:  Table [dbo].[Answers]    Script Date: 2022/5/12 下午 07:17:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Answers](
	[AnswerID] [uniqueidentifier] NOT NULL,
	[QueID] [uniqueidentifier] NOT NULL,
	[UserID] [uniqueidentifier] NOT NULL,
	[Answer] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Commonlys]    Script Date: 2022/5/12 下午 07:17:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Commonlys](
	[QuestionID] [uniqueidentifier] NOT NULL,
	[Type] [int] NOT NULL,
	[QueTitle] [nvarchar](max) NOT NULL,
	[QueAns] [nvarchar](max) NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questionnaires]    Script Date: 2022/5/12 下午 07:17:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questionnaires](
	[Number] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[QueID] [uniqueidentifier] NOT NULL,
	[StartTime] [datetime] NOT NULL,
	[EndTime] [datetime] NOT NULL,
	[QueContent] [nvarchar](max) NOT NULL,
	[State] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Questions]    Script Date: 2022/5/12 下午 07:17:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Questions](
	[QueID] [uniqueidentifier] NOT NULL,
	[QuestionID] [uniqueidentifier] NOT NULL,
	[QuestionNumber] [int] IDENTITY(1,1) NOT NULL,
	[Type] [int] NOT NULL,
	[QueTitle] [nvarchar](max) NOT NULL,
	[QueAns] [nvarchar](max) NOT NULL,
	[Necessary] [bit] NOT NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[Users]    Script Date: 2022/5/12 下午 07:17:09 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[UserID] [uniqueidentifier] NOT NULL,
	[QueID] [uniqueidentifier] NOT NULL,
	[UserName] [nvarchar](50) NOT NULL,
	[UserMail] [nvarchar](50) NOT NULL,
	[UserPhone] [nvarchar](50) NOT NULL,
	[UserAge] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Answers] ADD  CONSTRAINT [DF_Answers_AnswerID]  DEFAULT (newid()) FOR [AnswerID]
GO
ALTER TABLE [dbo].[Commonlys] ADD  CONSTRAINT [DF_Commonlys_QuestionID]  DEFAULT (newid()) FOR [QuestionID]
GO
ALTER TABLE [dbo].[Questionnaires] ADD  CONSTRAINT [DF_Table_1_ID]  DEFAULT (newid()) FOR [QueID]
GO
ALTER TABLE [dbo].[Users] ADD  CONSTRAINT [DF_Users_UserID]  DEFAULT (newid()) FOR [UserID]
GO
USE [master]
GO
ALTER DATABASE [QuerySystem] SET  READ_WRITE 
GO
