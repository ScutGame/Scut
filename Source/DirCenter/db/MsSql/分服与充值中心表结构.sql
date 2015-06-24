/*
参数:
$dbpath 数据库存储路径
*/

/*=========================================================================================*/

IF NOT EXISTS (SELECT * FROM sys.sysdatabases WHERE name = N'PayDB')
BEGIN

	CREATE DATABASE [PayDB] 
	ON  PRIMARY ( NAME = N'PayDB', FILENAME = N'$(dbpath)/PayDB.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
	 LOG ON ( NAME = N'PayDB_log', FILENAME = N'$(dbpath)/PayDB_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
END

GO

use PayDB
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[SensitiveWord]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[SensitiveWord](
	[Code] [int] NOT NULL,
	[Word] [varchar](50) NOT NULL,
PRIMARY KEY CLUSTERED 
(
	[Code] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigRetailerInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigRetailerInfo](
	[RetailerId] [varchar](20) NOT NULL,
	[RetailerName] [varchar](50) NOT NULL,
	[Percentage] [decimal](8, 4) NULL,
	[DeveloperID] [int] NULL CONSTRAINT [DF__ConfigRet__Devel__33D4B598]  DEFAULT ((0)),
	[DeveloperName] [varchar](50) NULL,
	[DeveloperDate] [datetime] NULL,
	[MerchandiserID] [int] NULL CONSTRAINT [DF__ConfigRet__Merch__34C8D9D1]  DEFAULT ((0)),
	[MerchandiserName] [varchar](50) NULL,
	[MerchandiserDate] [datetime] NULL,
	[CreateDate] [datetime] NULL,
	[TaxRate] [decimal](8, 4) NULL DEFAULT ((0)),
	[PayPercentage] [decimal](8, 4) NULL,
 CONSTRAINT [PK__ConfigRetailerIn__32E0915F] PRIMARY KEY CLUSTERED 
(
	[RetailerId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayInfo](
	[PayId] [varchar](20) NOT NULL,
	[PayName] [varchar](50) NOT NULL,
	[Percentage] [decimal](8, 4) NOT NULL,
	[ParentID] [int] NOT NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK__PayInfo__29572725] PRIMARY KEY CLUSTERED 
(
	[PayId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Settlement]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Settlement](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[RetailID] [varchar](50) NULL,
	[Amount] [decimal](16, 4) NULL,
	[GameID] [int] NULL,
	[CreatYear] [int] NULL,
	[CreatMouth] [int] NULL,
	[SettlementState] [int] NULL,
	[SettlementTime] [datetime] NULL,
	[Settlementer] [int] NULL,
 CONSTRAINT [PK__Settlement__5BE2A6F2] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ServerInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ServerInfo](
	[ID] [int] NOT NULL,
	[GameID] [int] NOT NULL,
	[ServerName] [varchar](20) NOT NULL,
	[BaseUrl] [varchar](200) NOT NULL,
	[ActiveNum] [int] NULL,
	[Weight] [int] NULL,
	[IsEnable] [bit] NOT NULL DEFAULT ((1)),
	[TargetServer] [nchar](10) NULL,
	[EnableDate] [datetime] NULL,
	[IntranetAddress] [varchar](100) NULL,
 CONSTRAINT [PK_ServerInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[GameID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[OrderInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[OrderInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[OrderNO] [varchar](100) NOT NULL,
	[MerchandiseName] [varchar](100) NULL,
	[PayType] [varchar](20) NOT NULL,
	[Amount] [decimal](16, 4) NOT NULL,
	[Currency] [varchar](10) NOT NULL,
	[Expand] [varchar](200) NULL,
	[SerialNumber] [varchar](200) NULL,
	[PassportID] [varchar](100) NOT NULL,
	[ServerID] [int] NOT NULL,
	[GameID] [int] NOT NULL,
	[gameName] [varchar](100) NULL,
	[ServerName] [varchar](100) NULL,
	[PayStatus] [int] NOT NULL,
	[Signature] [varchar](100) NOT NULL,
	[Remarks] [text] NULL,
	[GameCoins] [int] NOT NULL,
	[SendState] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL,
	[SendDate] [datetime] NULL,
	[RetailID] [varchar](50) NULL CONSTRAINT [DF__OrderInfo__Retai__60A75C0F]  DEFAULT ((0)),
	[DeviceID] [varchar](50) NULL,
 CONSTRAINT [PK_OrderInfo_ID] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [un_OrderNO] UNIQUE NONCLUSTERED 
(
	[OrderNO] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PayOrder]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PayOrder](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[type] [int] NOT NULL CONSTRAINT [DF_PayOrder_type]  DEFAULT ((0)),
	[userid] [bigint] NOT NULL,
	[used] [int] NOT NULL CONSTRAINT [DF_PayOrder_used]  DEFAULT ((0)),
	[orderid] [varchar](128) NOT NULL,
	[flag] [int] NOT NULL CONSTRAINT [DF_PayOrder_flag]  DEFAULT ((0)),
	[createtime] [datetime] NOT NULL CONSTRAINT [DF_PayOrder_createtime]  DEFAULT ((0)),
	[gettime] [int] NOT NULL CONSTRAINT [DF_PayOrder_gettime]  DEFAULT ((0)),
	[itemcount] [int] NOT NULL CONSTRAINT [DF_PayOrder_itemcount]  DEFAULT ((0)),
	[systemtype] [int] NOT NULL CONSTRAINT [DF_PayOrder_systemtype]  DEFAULT ((0)),
 CONSTRAINT [PK_PayOrder] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY],
 CONSTRAINT [IX_PayOrder_orderid] UNIQUE NONCLUSTERED 
(
	[orderid] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GameInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[GameInfo](
	[GameID] [int] NOT NULL,
	[GameName] [varchar](20) NOT NULL,
	[Currency] [varchar](20) NULL,
	[Multiple] [decimal](18, 2) NOT NULL DEFAULT ((0)),
	[GameWord] [varchar](100) NULL,
	[AgentsID] [varchar](20) NOT NULL DEFAULT ('0000'),
	[IsRelease] [bit] NOT NULL DEFAULT ((1)),
	[ReleaseDate] [datetime] NULL,
	[PayStyle] [varchar](500) NULL,
	[SocketServer] [varchar](100) NULL,
	[SocketPort] [int] NULL CONSTRAINT [DF_GameInfo_SocketPort]  DEFAULT ((0)),
 CONSTRAINT [PK_GameInfo] PRIMARY KEY CLUSTERED 
(
	[GameID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END

GO

--Init data
IF NOT EXISTS (SELECT * FROM GameInfo WHERE GameID = 6)
BEGIN
	INSERT [ConfigRetailerInfo] ([RetailerId], [RetailerName], [Percentage], [DeveloperID], [DeveloperName], [DeveloperDate], [MerchandiserID], [MerchandiserName], [MerchandiserDate], [CreateDate], [TaxRate], [PayPercentage]) VALUES (N'0000', N'官网', CAST(0.5000 AS Decimal(8, 4)), 0, N'', NULL, 0, N'', NULL, CAST(0x0000A13900000000 AS DateTime), CAST(0.2000 AS Decimal(8, 4)), CAST(0.1000 AS Decimal(8, 4)))
	INSERT [ServerInfo] ([ID], [GameID], [ServerName], [BaseUrl], [ActiveNum], [Weight], [IsEnable], [TargetServer], [EnableDate], [IntranetAddress]) VALUES (1, 7, N'斗地主一服', N'ddz.scutgame.com:9700', 0, 0, 1, N'1         ', CAST(0xFFFF2E4600000000 AS DateTime), N'ddz.scutgame.com:9700')
	INSERT [ServerInfo] ([ID], [GameID], [ServerName], [BaseUrl], [ActiveNum], [Weight], [IsEnable], [TargetServer], [EnableDate], [IntranetAddress]) VALUES (1, 6, N'口袋一服', N'http://kd1.scutgame.com/Service.aspx', 0, NULL, 1, N'1         ', CAST(0xFFFF2E4600000000 AS DateTime), N'http://kd1.scutgame.com/Service.aspx')
	INSERT [GameInfo] ([GameID], [GameName], [Currency], [Multiple], [GameWord], [AgentsID], [IsRelease], [ReleaseDate], [PayStyle], [SocketServer], [SocketPort]) VALUES (6, N'口袋天界', N'晶石', CAST(10.00 AS Decimal(18, 2)), N'KDTJ', N'0000', 1, CAST(0xFFFF2E4600000000 AS DateTime), N'', N'0', 0)
	INSERT [GameInfo] ([GameID], [GameName], [Currency], [Multiple], [GameWord], [AgentsID], [IsRelease], [ReleaseDate], [PayStyle], [SocketServer], [SocketPort]) VALUES (7, N'斗地主', N'元宝', CAST(10.00 AS Decimal(18, 2)), N'DDZ', N'0000', 1, CAST(0xFFFF2E4600000000 AS DateTime), N'', N'0', 0)
END
GO