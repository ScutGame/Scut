/*
参数:
$dbpath 数据库存储路径
*/

/*=========================================================================================*/

IF NOT EXISTS (SELECT * FROM sys.sysdatabases WHERE name = N'snscenter')
BEGIN

    CREATE DATABASE [snscenter] 
        ON  PRIMARY ( NAME = N'snscenter', FILENAME = N'$(dbpath)/snscenter.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
        LOG ON ( NAME = N'snscenter_log', FILENAME = N'$(dbpath)/snscenter_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)

END
go

use snscenter
GO
--权限
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'$(gameuser)')
	CREATE USER $(gameuser) FOR LOGIN $(gameuser) WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_datareader', N'$(gameuser)'
EXEC sp_addrolemember N'db_datawriter', N'$(gameuser)'
EXEC sp_addrolemember N'db_ddladmin', N'$(gameuser)'
GO



--创建表
/*=========================================================================================*/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SnsPassportLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [SnsPassportLog](
	[PassportID] [int] IDENTITY(10000,1) NOT NULL,
	[CreateTime] [datetime] NULL CONSTRAINT [DF_SnsPassportLog_CreateTime]  DEFAULT (getdate()),
	[Mark] [int] NULL,
	[RegPushTime] [datetime] NULL,
	[RegTime] [datetime] NULL,
 CONSTRAINT [PK_SnsPassportLog] PRIMARY KEY CLUSTERED 
(
	[PassportID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SnsPassportLog]') AND name = N'In_mark')
CREATE NONCLUSTERED INDEX [In_mark] ON [SnsPassportLog] 
(
	[Mark] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LimitDevice]') AND type in (N'U'))
BEGIN
CREATE TABLE [LimitDevice](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [varchar](50) NULL,
	[AppTime] [datetime] NULL,
	[Remark] [varchar](200) NULL,
 CONSTRAINT [PK_LimitDevice] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[PassportLoginLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [PassportLoginLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[DeviceID] [varchar](50) NULL,
	[PassportID] [varchar](50) NULL,
	[LoginTime] [datetime] NULL,
 CONSTRAINT [PK_PassportLoginLog] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[SnsUserInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [SnsUserInfo](
	[UserId] [int] IDENTITY(1380000,1) NOT NULL,
	[PassportID] [varchar](32) NULL,
	[PassportPwd] [varchar](50) NULL,
	[DeviceID] [varchar](50) NULL,
	[RegType] [smallint] NULL,
	[RegTime] [datetime] NULL,
	[RetailID] [varchar](50) NULL,
	[RetailUser] [varchar](50) NULL,
	[Mobile] [varchar](12) NULL,
	[Mail] [varchar](50) NULL,
	[PwdType] [tinyint] NULL DEFAULT ((0)),
	[RealName] [varchar](20) NULL,
	[IDCards] [varchar](20) NULL,
	[ActiveCode] [nchar](10) NULL,
	[SendActiveDate] [datetime] NULL,
	[ActiveDate] [datetime] NULL,
	[WeixinCode] varchar(50) NULL,
 CONSTRAINT [PK_SnsUserInfo] PRIMARY KEY CLUSTERED 
(
	[UserId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SnsUserInfo]') AND name = N'In_PassportId')
CREATE NONCLUSTERED INDEX [In_PassportId] ON [SnsUserInfo] 
(
	[PassportID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SnsUserInfo]') AND name = N'In_PassportIdPwd')
CREATE NONCLUSTERED INDEX [In_PassportIdPwd] ON [SnsUserInfo] 
(
	[PassportID] ASC,
	[PassportPwd] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = OFF) ON [PRIMARY]
GO

IF NOT EXISTS (SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID(N'[SnsUserInfo]') AND name = N'In_Retail_User')
CREATE NONCLUSTERED INDEX [In_Retail_User] ON [SnsUserInfo] 
(
	[RetailID] ASC,
	[RetailUser] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = OFF) ON [PRIMARY]
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[userloginlog]') AND type in (N'U'))
BEGIN
CREATE TABLE [userloginlog](
	[SessionID] [bigint] IDENTITY(1000000,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[AddTime] [datetime] NOT NULL,
	[Md5Hash] [varchar](50) NOT NULL,
	[Stat] [bit] NULL,
 CONSTRAINT [PK_userloginlog] PRIMARY KEY CLUSTERED 
(
	[SessionID] DESC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[LogUserLogin]') AND type in (N'U'))
BEGIN
CREATE TABLE [LogUserLogin](
	[LogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [bigint] NOT NULL,
	[LogTime] [datetime] NOT NULL,
	[IPAddr] [varchar](15) NULL,
	[LogType] [smallint] NOT NULL
) ON [PRIMARY]
END

GO