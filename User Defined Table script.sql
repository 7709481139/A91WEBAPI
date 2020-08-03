/*USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[User]    Script Date: 1/29/2020 12:54:43 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[User](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserName] [nvarchar](max) NULL,
	[PasswordHash] [varbinary](max) NULL,
	[PasswordSalt] [varbinary](max) NULL,
	[Role] [nvarchar](max) NULL,
	[CardCode] [nvarchar](max) NULL,
	[email] [nvarchar](250) NULL,
	[mobile] [nvarchar](20) NULL,
	[temppass] [nvarchar](250) NULL,
 CONSTRAINT [PK_User] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[UserRole]    Script Date: 1/29/2020 12:55:05 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[UserRole](
	[Id] [nvarchar](450) NOT NULL,
	[Role] [nvarchar](max) NULL,
	[IsA91] [nvarchar](max) NULL,
 CONSTRAINT [PK_UserRole] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
*/

USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[OSS_MIS_Monthly]    Script Date: 1/29/2020 12:55:30 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSS_MIS_Monthly](
	[FY_Year] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Card_Code] [nvarchar](50) NOT NULL,
	[GL_Code] [nvarchar](50) NOT NULL,
	[Amt] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OSS_MIS_Monthly] PRIMARY KEY CLUSTERED 
(
	[FY_Year] ASC,
	[Period] ASC,
	[Card_Code] ASC,
	[GL_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[OSS_MIS_Quaterly]    Script Date: 1/29/2020 12:55:51 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSS_MIS_Quaterly](
	[FY_Year] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Card_Code] [nvarchar](50) NOT NULL,
	[GL_Code] [nvarchar](50) NOT NULL,
	[Amt] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OSS_MIS_Quaterly] PRIMARY KEY CLUSTERED 
(
	[FY_Year] ASC,
	[Period] ASC,
	[Card_Code] ASC,
	[GL_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[OSS_MIS_Yearly]    Script Date: 1/29/2020 12:56:07 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSS_MIS_Yearly](
	[FY_Year] [int] NOT NULL,
	[Period] [int] NOT NULL,
	[Card_Code] [nvarchar](50) NOT NULL,
	[GL_Code] [nvarchar](50) NOT NULL,
	[Amt] [decimal](18, 2) NULL,
 CONSTRAINT [PK_OSS_MIS_Yearly] PRIMARY KEY CLUSTERED 
(
	[FY_Year] ASC,
	[Period] ASC,
	[Card_Code] ASC,
	[GL_Code] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO


USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[OSS_VotingHead]    Script Date: 1/29/2020 12:56:22 PM ******/
SET ANSI_NULLS ON
GO USE [A91_Emerging_Fund]
GO

/****** Object:  Table [dbo].[OSS_VotingResults]    Script Date: 1/29/2020 12:56:40 PM ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSS_VotingResults](
	[VotingID] [int] NOT NULL,
	[UserId] [nvarchar](150) NOT NULL,
	[Reason] [nvarchar](max) NULL,
	[prediscussionrate] [int] NULL,
	[postdiscussionrate] [int] NULL,
	[prediscussionncommnets] [nvarchar](max) NULL,
	[postdiscussioncommnets] [nvarchar](max) NULL,
	[CreatedBy] [nvarchar](150) NULL,
	[CreatedDateTS] [datetime] NULL,
	[UpdateDateTS] [datetime] NULL,
 CONSTRAINT [PK_OSS_VotingResults] PRIMARY KEY CLUSTERED 
(
	[VotingID] ASC,
	[UserId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO




SET QUOTED_IDENTIFIER ON
GO

CREATE TABLE [dbo].[OSS_VotingHead](
	[VotingID] [int] IDENTITY(1,1) NOT NULL,
	[CardCode] [nvarchar](50) NOT NULL,
	[VotingStartDtTS] [datetime] NOT NULL,
	[VotingEndDtTS] [datetime] NOT NULL,
	[MeetingID] [int] NOT NULL,
	[Agenda] [nvarchar](max) NULL,
	[VotType] [nvarchar](50) NULL,
	[CreatedDateTS] [datetime] NULL,
	[UpdateDateTS] [datetime] NULL,
	[CreatedBy] [nvarchar](50) NULL,
	[UpdatedBy] [nvarchar](50) NULL
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO


