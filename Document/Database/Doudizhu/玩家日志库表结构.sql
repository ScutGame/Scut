use Ddz1Log
go
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[MallItemLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[MallItemLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[ItemID] [int] NULL,
	[Uid] [int] NULL,
	[Num] [int] NULL,
	[CurrencyType] [int] NULL,
	[Amount] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_MallItemLog] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserItemLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserItemLog](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[Uid] [int] NULL,
	[UserItemID] [varchar](50) NULL,
	[OpType] [int] NULL,
	[ItemID] [int] NULL,
	[Num] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK__UserItemSellLog__117F9D94] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserGoldLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserGoldLog](
	[ID] [bigint] NOT NULL,
	[UserID] [int] IDENTITY(1,1) NOT NULL,
	[BalanceType] [smallint] NULL,
	[CurrencyType] [smallint] NULL,
	[OpType] [smallint] NULL,
	[DetailID] [int] NULL,
	[Num] [int] NULL,
	[Gold] [int] NULL,
	[BanGold] [int] NULL,
	[SurplusGold] [int] NULL,
	[SurplusBanGold] [int] NULL,
	[CreateDate] [datetime] NULL,
 CONSTRAINT [PK_UserUseGoldLog] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChatLog](
	[ChatID] [int] IDENTITY(1,1) NOT NULL,
	[ChatType] [int] NOT NULL,
	[FromUserID] [int] NOT NULL,
	[FromUserName] [varchar](50) NOT NULL,
	[ToUserID] [int] NULL,
	[ToUserName] [varchar](50) NULL,
	[Content] [varchar](1000) NOT NULL,
	[SendDate] [datetime] NOT NULL,
 CONSTRAINT [PK_ChatLog] PRIMARY KEY CLUSTERED 
(
	[ChatID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserLoginLog]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[UserLoginLog](
	[ID] [int] IDENTITY(1,1) NOT NULL,
	[SessionID] [varchar](50) NULL,
	[MobileType] [smallint] NULL,
	[ScreenX] [smallint] NULL,
	[ScreenY] [smallint] NULL,
	[RetailId] [varchar](100) NULL,
	[Model] [varchar](50) NULL,
	[NetWork] [smallint] NULL,
	[UserId] [varchar](50) NULL,
	[AddTime] [datetime] NULL,
	[State] [int] NULL,
	[DeviceID] [varchar](50) NULL CONSTRAINT [DF__UserLogin__Devic__014935CB]  DEFAULT ((0)),
	[Ip] [varchar](50) NULL,
	[Pid] [varchar](20) NULL,
	[UserLv] [smallint] NULL,
 CONSTRAINT [PK_UserLoginLog] PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[LogActionVisitor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[LogActionVisitor](
	[LogID] [bigint] IDENTITY(1,1) NOT NULL,
	[UserID] [varchar](50) NOT NULL,
	[ActionID] [int] NOT NULL,
	[ActionStat] [int] NOT NULL,
	[VisitUrl] [varchar](1024) NOT NULL,
	[RespCont] [ntext] NULL,
	[IP] [varchar](32) NOT NULL,
	[VisitBeginTime] [datetime] NOT NULL,
	[VisitEndTime] [datetime] NOT NULL,
	[Addtime] [datetime] NOT NULL,
 CONSTRAINT [PK_LogActionVisitor] PRIMARY KEY CLUSTERED 
(
	[LogID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
CREATE TABLE OAOperationLog(
	ID char(36) NOT NULL,
	UserID varchar(20)  NULL,
	OpType smallint NULL,
	Reason varchar(1000) NULL,
	EndDate datetime NULL,
	OpUserID int NULL,
	CreateDate datetime NULL,
	primary key(ID)
)

/*商城道具购买log  */
alter table MallItemLog add RetailID varchar(50); 
alter table MallItemLog add MobileType smallint; 
alter table MallItemLog add Pid varchar(50); 
alter table MallItemLog add ItemName varchar(50); 
