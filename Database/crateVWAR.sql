USE [master]
GO

/****** Object:  Database [vwar]    Script Date: 03/08/2010 09:32:24 ******/
CREATE DATABASE [vwar] ON  PRIMARY 
( NAME = N'FormsAuthenticationTemplate', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\vwar.mdf' , SIZE = 3072KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'FormsAuthenticationTemplate_log', FILENAME = N'c:\Program Files\Microsoft SQL Server\MSSQL10.SQLEXPRESS\MSSQL\DATA\vwar.ldf' , SIZE = 1024KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO

ALTER DATABASE [vwar] SET COMPATIBILITY_LEVEL = 90
GO

IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [vwar].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO

ALTER DATABASE [vwar] SET ANSI_NULL_DEFAULT OFF 
GO

ALTER DATABASE [vwar] SET ANSI_NULLS OFF 
GO

ALTER DATABASE [vwar] SET ANSI_PADDING OFF 
GO

ALTER DATABASE [vwar] SET ANSI_WARNINGS OFF 
GO

ALTER DATABASE [vwar] SET ARITHABORT OFF 
GO

ALTER DATABASE [vwar] SET AUTO_CLOSE OFF 
GO

ALTER DATABASE [vwar] SET AUTO_CREATE_STATISTICS ON 
GO

ALTER DATABASE [vwar] SET AUTO_SHRINK OFF 
GO

ALTER DATABASE [vwar] SET AUTO_UPDATE_STATISTICS ON 
GO

ALTER DATABASE [vwar] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO

ALTER DATABASE [vwar] SET CURSOR_DEFAULT  GLOBAL 
GO

ALTER DATABASE [vwar] SET CONCAT_NULL_YIELDS_NULL OFF 
GO

ALTER DATABASE [vwar] SET NUMERIC_ROUNDABORT OFF 
GO

ALTER DATABASE [vwar] SET QUOTED_IDENTIFIER OFF 
GO

ALTER DATABASE [vwar] SET RECURSIVE_TRIGGERS OFF 
GO

ALTER DATABASE [vwar] SET  DISABLE_BROKER 
GO

ALTER DATABASE [vwar] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO

ALTER DATABASE [vwar] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO

ALTER DATABASE [vwar] SET TRUSTWORTHY OFF 
GO

ALTER DATABASE [vwar] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO

ALTER DATABASE [vwar] SET PARAMETERIZATION SIMPLE 
GO

ALTER DATABASE [vwar] SET READ_COMMITTED_SNAPSHOT OFF 
GO

ALTER DATABASE [vwar] SET HONOR_BROKER_PRIORITY OFF 
GO

ALTER DATABASE [vwar] SET  READ_WRITE 
GO

ALTER DATABASE [vwar] SET RECOVERY SIMPLE 
GO

ALTER DATABASE [vwar] SET  MULTI_USER 
GO

ALTER DATABASE [vwar] SET PAGE_VERIFY CHECKSUM  
GO

ALTER DATABASE [vwar] SET DB_CHAINING OFF 
GO

SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[ContentObject](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Title] [nvarchar](50) NOT NULL,
	[Description] [nvarchar](50) NOT NULL,
	[Keywords] [nvarchar](50) NULL,
	[DescriptionWebsiteURL] [nvarchar](50) NULL,
	[SubmitterEmail] [varchar](50) NULL,
	[SubmitterLogoImageFilePath] [nvarchar](50) NULL,
	[CollectionName] [nchar](10) NULL,
	[Location] [nvarchar](50) NOT NULL,
	[Views] [int] NOT NULL,
	[ScreenShot] [nvarchar](50) NULL,
	[LastViewed] [datetime] NOT NULL,
	[UploadedDate] [datetime] NOT NULL,
	[LastModified] [datetime] NOT NULL,
 CONSTRAINT [PK_ContentObject] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[ContentObject] ADD  CONSTRAINT [DF_ContentObject_LastViewed]  DEFAULT (getdate()) FOR [LastViewed]
GO

ALTER TABLE [dbo].[ContentObject] ADD  CONSTRAINT [DF_ContentObject_UploadedDate]  DEFAULT (getdate()) FOR [UploadedDate]
GO

ALTER TABLE [dbo].[ContentObject] ADD  CONSTRAINT [DF_ContentObject_LastModified]  DEFAULT (getdate()) FOR [LastModified]
GO
GO

/****** Object:  Table [dbo].[Reviews]    Script Date: 03/08/2010 09:34:57 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[Reviews](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Rating] [int] NOT NULL,
	[ReviewText] [nvarchar](50) NOT NULL,
	[ContentObjectId] [int] NOT NULL,
 CONSTRAINT [PK_Reviews] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

ALTER TABLE [dbo].[Reviews]  WITH CHECK ADD  CONSTRAINT [FK_Reviews_ContentObject] FOREIGN KEY([ContentObjectId])
REFERENCES [dbo].[ContentObject] ([Id])
GO

ALTER TABLE [dbo].[Reviews] CHECK CONSTRAINT [FK_Reviews_ContentObject]
GO