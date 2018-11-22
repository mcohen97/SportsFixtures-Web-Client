USE [master]
GO
/****** Object:  Database [ObligatorioDA2]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE DATABASE [ObligatorioDA2]
 CONTAINMENT = NONE
 ON  PRIMARY 
( NAME = N'ObligatorioDA2', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER_R14\MSSQL\DATA\ObligatorioDA2.mdf' , SIZE = 3264KB , MAXSIZE = UNLIMITED, FILEGROWTH = 1024KB )
 LOG ON 
( NAME = N'ObligatorioDA2_log', FILENAME = N'C:\Program Files\Microsoft SQL Server\MSSQL12.SQLSERVER_R14\MSSQL\DATA\ObligatorioDA2_log.ldf' , SIZE = 832KB , MAXSIZE = 2048GB , FILEGROWTH = 10%)
GO
ALTER DATABASE [ObligatorioDA2] SET COMPATIBILITY_LEVEL = 120
GO
IF (1 = FULLTEXTSERVICEPROPERTY('IsFullTextInstalled'))
begin
EXEC [ObligatorioDA2].[dbo].[sp_fulltext_database] @action = 'enable'
end
GO
ALTER DATABASE [ObligatorioDA2] SET ANSI_NULL_DEFAULT OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET ANSI_NULLS OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET ANSI_PADDING OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET ANSI_WARNINGS OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET ARITHABORT OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET AUTO_CLOSE ON 
GO
ALTER DATABASE [ObligatorioDA2] SET AUTO_SHRINK OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET AUTO_UPDATE_STATISTICS ON 
GO
ALTER DATABASE [ObligatorioDA2] SET CURSOR_CLOSE_ON_COMMIT OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET CURSOR_DEFAULT  GLOBAL 
GO
ALTER DATABASE [ObligatorioDA2] SET CONCAT_NULL_YIELDS_NULL OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET NUMERIC_ROUNDABORT OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET QUOTED_IDENTIFIER OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET RECURSIVE_TRIGGERS OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET  ENABLE_BROKER 
GO
ALTER DATABASE [ObligatorioDA2] SET AUTO_UPDATE_STATISTICS_ASYNC OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET DATE_CORRELATION_OPTIMIZATION OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET TRUSTWORTHY OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET ALLOW_SNAPSHOT_ISOLATION OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET PARAMETERIZATION SIMPLE 
GO
ALTER DATABASE [ObligatorioDA2] SET READ_COMMITTED_SNAPSHOT ON 
GO
ALTER DATABASE [ObligatorioDA2] SET HONOR_BROKER_PRIORITY OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET RECOVERY SIMPLE 
GO
ALTER DATABASE [ObligatorioDA2] SET  MULTI_USER 
GO
ALTER DATABASE [ObligatorioDA2] SET PAGE_VERIFY CHECKSUM  
GO
ALTER DATABASE [ObligatorioDA2] SET DB_CHAINING OFF 
GO
ALTER DATABASE [ObligatorioDA2] SET FILESTREAM( NON_TRANSACTED_ACCESS = OFF ) 
GO
ALTER DATABASE [ObligatorioDA2] SET TARGET_RECOVERY_TIME = 0 SECONDS 
GO
ALTER DATABASE [ObligatorioDA2] SET DELAYED_DURABILITY = DISABLED 
GO
USE [ObligatorioDA2]
GO
/****** Object:  Table [dbo].[Comments]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Comments](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[MakerUserName] [nvarchar](450) NULL,
	[Text] [nvarchar](max) NULL,
	[EncounterEntityId] [int] NULL,
 CONSTRAINT [PK_Comments] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Encounters]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Encounters](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[Date] [datetime2](7) NOT NULL,
	[SportEntityName] [nvarchar](450) NULL,
	[HasResult] [bit] NOT NULL,
 CONSTRAINT [PK_Encounters] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EncounterTeams]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EncounterTeams](
	[TeamNumber] [int] NOT NULL,
	[EncounterId] [int] NOT NULL,
	[Position] [int] NOT NULL,
 CONSTRAINT [PK_EncounterTeams] PRIMARY KEY CLUSTERED 
(
	[EncounterId] ASC,
	[TeamNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Sports]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Sports](
	[Name] [nvarchar](450) NOT NULL,
	[IsTwoTeams] [bit] NOT NULL,
 CONSTRAINT [PK_Sports] PRIMARY KEY CLUSTERED 
(
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Teams]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Teams](
	[TeamNumber] [int] IDENTITY(1,1) NOT NULL,
	[Name] [nvarchar](450) NOT NULL,
	[Photo] [nvarchar](max) NULL,
	[SportEntityName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_Teams] PRIMARY KEY CLUSTERED 
(
	[TeamNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [AK_Teams_SportEntityName_Name] UNIQUE NONCLUSTERED 
(
	[SportEntityName] ASC,
	[Name] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[Users]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[Users](
	[Name] [nvarchar](max) NULL,
	[Surname] [nvarchar](max) NULL,
	[UserName] [nvarchar](450) NOT NULL,
	[Password] [nvarchar](max) NULL,
	[Email] [nvarchar](max) NULL,
	[IsAdmin] [bit] NOT NULL,
 CONSTRAINT [PK_Users] PRIMARY KEY CLUSTERED 
(
	[UserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[UserTeams]    Script Date: 11/22/2018 8:44:08 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[UserTeams](
	[UserEntityUserName] [nvarchar](450) NOT NULL,
	[TeamNumber] [int] NULL,
	[TeamEntityName] [nvarchar](450) NOT NULL,
	[TeamEntitySportEntityName] [nvarchar](450) NOT NULL,
 CONSTRAINT [PK_UserTeams] PRIMARY KEY CLUSTERED 
(
	[TeamEntityName] ASC,
	[TeamEntitySportEntityName] ASC,
	[UserEntityUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Index [IX_Comments_EncounterEntityId]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_EncounterEntityId] ON [dbo].[Comments]
(
	[EncounterEntityId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Comments_MakerUserName]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_Comments_MakerUserName] ON [dbo].[Comments]
(
	[MakerUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_Encounters_SportEntityName]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_Encounters_SportEntityName] ON [dbo].[Encounters]
(
	[SportEntityName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_EncounterTeams_TeamNumber]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_EncounterTeams_TeamNumber] ON [dbo].[EncounterTeams]
(
	[TeamNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
/****** Object:  Index [IX_UserTeams_TeamNumber]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserTeams_TeamNumber] ON [dbo].[UserTeams]
(
	[TeamNumber] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
SET ANSI_PADDING ON

GO
/****** Object:  Index [IX_UserTeams_UserEntityUserName]    Script Date: 11/22/2018 8:44:08 PM ******/
CREATE NONCLUSTERED INDEX [IX_UserTeams_UserEntityUserName] ON [dbo].[UserTeams]
(
	[UserEntityUserName] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Encounters_EncounterEntityId] FOREIGN KEY([EncounterEntityId])
REFERENCES [dbo].[Encounters] ([Id])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Encounters_EncounterEntityId]
GO
ALTER TABLE [dbo].[Comments]  WITH CHECK ADD  CONSTRAINT [FK_Comments_Users_MakerUserName] FOREIGN KEY([MakerUserName])
REFERENCES [dbo].[Users] ([UserName])
GO
ALTER TABLE [dbo].[Comments] CHECK CONSTRAINT [FK_Comments_Users_MakerUserName]
GO
ALTER TABLE [dbo].[Encounters]  WITH CHECK ADD  CONSTRAINT [FK_Encounters_Sports_SportEntityName] FOREIGN KEY([SportEntityName])
REFERENCES [dbo].[Sports] ([Name])
GO
ALTER TABLE [dbo].[Encounters] CHECK CONSTRAINT [FK_Encounters_Sports_SportEntityName]
GO
ALTER TABLE [dbo].[EncounterTeams]  WITH CHECK ADD  CONSTRAINT [FK_EncounterTeams_Encounters_EncounterId] FOREIGN KEY([EncounterId])
REFERENCES [dbo].[Encounters] ([Id])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EncounterTeams] CHECK CONSTRAINT [FK_EncounterTeams_Encounters_EncounterId]
GO
ALTER TABLE [dbo].[EncounterTeams]  WITH CHECK ADD  CONSTRAINT [FK_EncounterTeams_Teams_TeamNumber] FOREIGN KEY([TeamNumber])
REFERENCES [dbo].[Teams] ([TeamNumber])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[EncounterTeams] CHECK CONSTRAINT [FK_EncounterTeams_Teams_TeamNumber]
GO
ALTER TABLE [dbo].[Teams]  WITH CHECK ADD  CONSTRAINT [FK_Teams_Sports_SportEntityName] FOREIGN KEY([SportEntityName])
REFERENCES [dbo].[Sports] ([Name])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[Teams] CHECK CONSTRAINT [FK_Teams_Sports_SportEntityName]
GO
ALTER TABLE [dbo].[UserTeams]  WITH CHECK ADD  CONSTRAINT [FK_UserTeams_Teams_TeamNumber] FOREIGN KEY([TeamNumber])
REFERENCES [dbo].[Teams] ([TeamNumber])
GO
ALTER TABLE [dbo].[UserTeams] CHECK CONSTRAINT [FK_UserTeams_Teams_TeamNumber]
GO
ALTER TABLE [dbo].[UserTeams]  WITH CHECK ADD  CONSTRAINT [FK_UserTeams_Users_UserEntityUserName] FOREIGN KEY([UserEntityUserName])
REFERENCES [dbo].[Users] ([UserName])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[UserTeams] CHECK CONSTRAINT [FK_UserTeams_Users_UserEntityUserName]
GO
USE [master]
GO
ALTER DATABASE [ObligatorioDA2] SET  READ_WRITE 
GO
