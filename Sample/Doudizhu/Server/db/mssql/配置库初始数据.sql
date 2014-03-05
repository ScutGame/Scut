USE [$(gameName)Config]
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[AchievementInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[AchievementInfo](
	[Id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Type] [int] NOT NULL DEFAULT ((0)),
	[TargetNum] [int] NOT NULL DEFAULT ((0)),
	[HeadIcon] [varchar](50) NULL,
	[Description] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AchievementInfo', N'COLUMN',N'Type'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成就类型' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AchievementInfo', @level2type=N'COLUMN',@level2name=N'Type'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AchievementInfo', N'COLUMN',N'TargetNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成就达到值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AchievementInfo', @level2type=N'COLUMN',@level2name=N'TargetNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'AchievementInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'成就配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'AchievementInfo'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[RoomInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[RoomInfo](
	[Id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[AnteNum] [int] NOT NULL DEFAULT ((0)),
	[MultipleNum] [smallint] NOT NULL DEFAULT ((0)),
	[MinGameCion] [int] NOT NULL DEFAULT ((0)),
	[GiffCion] [int] NOT NULL DEFAULT ((0)),
	[Description] [varchar](500) NULL,
	[PlayerNum] [int] NOT NULL DEFAULT ((0)),
	[CardPackNum] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'AnteNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'底注分' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'AnteNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'MultipleNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'房间倍数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'MultipleNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'MinGameCion'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'金豆数限制' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'MinGameCion'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'GiffCion'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'每日赠豆,0:不赠送' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'GiffCion'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'PlayerNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'玩家人数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'PlayerNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', N'COLUMN',N'CardPackNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'卡牌几副' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo', @level2type=N'COLUMN',@level2name=N'CardPackNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'RoomInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'房间配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'RoomInfo'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ConfigEnvSet]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ConfigEnvSet](
	[EnvKey] [varchar](50) NOT NULL,
	[EnvValue] [varchar](50) NOT NULL,
	[EnvDesc] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[EnvKey] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShopInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ShopInfo](
	[ShopID] [int] NOT NULL,
	[ShopName] [varchar](20) NOT NULL,
	[HeadID] [varchar](100) NULL,
	[ShopType] [int] NOT NULL,
	[SeqNO] [smallint] NOT NULL,
	[Price] [int] NOT NULL,
	[VipPrice] [int] NOT NULL,
	[GameCoin] [int] NOT NULL,
	[ShopDesc] [varchar](200) NULL,
PRIMARY KEY CLUSTERED 
(
	[ShopID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DialInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[DialInfo](
	[ID] [int] NOT NULL,
	[GameCoin] [int] NOT NULL,
	[HeadID] [varchar](100) NULL,
	[Probability] [decimal](8, 4) NOT NULL,
	[ItemDesc] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
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
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TaskInfo](
	[TaskID] [int] NOT NULL,
	[TaskName] [varchar](50) NOT NULL,
	[TaskType] [int] NOT NULL,
	[TaskClass] [smallint] NOT NULL,
	[RestraintNum] [int] NOT NULL,
	[AchieveID] [int] NOT NULL,
	[GameCoin] [int] NOT NULL,
	[TaskDesc] [varchar](500) NULL,
PRIMARY KEY CLUSTERED 
(
	[TaskID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[PokerInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[PokerInfo](
	[Id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[Color] [int] NOT NULL,
	[Value] [smallint] NOT NULL,
	[HeadIcon] [varchar](50) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PokerInfo', N'COLUMN',N'Color'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'牌花色[Enum<CardColor>]' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PokerInfo', @level2type=N'COLUMN',@level2name=N'Color'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PokerInfo', N'COLUMN',N'Value'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'牌大小值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PokerInfo', @level2type=N'COLUMN',@level2name=N'Value'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'PokerInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'扑克牌配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'PokerInfo'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ChatInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[ChatInfo](
	[Id] [int] NOT NULL,
	[Content] [varchar](100) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'ChatInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'聊天语言配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'ChatInfo'
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TitleInfo]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[TitleInfo](
	[Id] [int] NOT NULL,
	[Name] [varchar](20) NOT NULL,
	[TargetNum] [int] NOT NULL DEFAULT ((0)),
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TitleInfo', N'COLUMN',N'TargetNum'))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'达到值' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TitleInfo', @level2type=N'COLUMN',@level2name=N'TargetNum'
GO
IF NOT EXISTS (SELECT * FROM ::fn_listextendedproperty(N'MS_Description' , N'SCHEMA',N'dbo', N'TABLE',N'TitleInfo', NULL,NULL))
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'称号配置' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TitleInfo'
go


GO
IF NOT EXISTS (SELECT * FROM RoomInfo WHERE Id =1001 )
BEGIN
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1001, N'萌妹铜牌', 2, 50, N'icon_1034', N'成就描述：赢得50场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1002, N'萌妹银牌', 2, 150, N'icon_1035', N'成就描述：赢得150场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1003, N'萌妹金牌', 2, 300, N'icon_1036', N'成就描述：赢得300场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1004, N'萌妹白金', 2, 600, N'icon_1037', N'成就描述：赢得600场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1005, N'萌妹铂金', 2, 1000, N'icon_1038', N'成就描述：赢得1000场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1006, N'萌妹钻石', 2, 2000, N'icon_1039', N'成就描述：赢得2000场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1007, N'萌妹大神', 2, 3500, N'icon_1040', N'成就描述：赢得3500场萌妹斗地主胜利')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1008, N'一星至尊', 5, 500, N'icon_1041', N'成就描述：累计充值500元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1009, N'二星至尊', 5, 1500, N'icon_1042', N'成就描述：累计充值1500元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1010, N'三星至尊', 5, 3000, N'icon_1043', N'成就描述：累计充值3000元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1011, N'四星至尊', 5, 6000, N'icon_1044', N'成就描述：累计充值6000元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1012, N'五星至尊', 5, 8000, N'icon_1045', N'成就描述：累计充值8000元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1013, N'六星至尊', 5, 10000, N'icon_1046', N'成就描述：累计充值10000元宝')
	INSERT [AchievementInfo] ([Id], [Name], [Type], [TargetNum], [HeadIcon], [Description]) VALUES (1014, N'七星至尊', 5, 20000, N'icon_1047', N'成就描述：累计充值20000元宝')
	INSERT [RoomInfo] ([Id], [Name], [AnteNum], [MultipleNum], [MinGameCion], [GiffCion], [Description], [PlayerNum], [CardPackNum]) VALUES (1001, N'一倍积分房', 400, 1, 1000, 1000, N'', 3, 1)
	INSERT [RoomInfo] ([Id], [Name], [AnteNum], [MultipleNum], [MinGameCion], [GiffCion], [Description], [PlayerNum], [CardPackNum]) VALUES (1002, N'二倍积分房', 400, 2, 10000, 1000, N'', 3, 1)
	INSERT [RoomInfo] ([Id], [Name], [AnteNum], [MultipleNum], [MinGameCion], [GiffCion], [Description], [PlayerNum], [CardPackNum]) VALUES (1003, N'四倍积分房', 600, 4, 60000, 1000, N'', 3, 1)
	INSERT [RoomInfo] ([Id], [Name], [AnteNum], [MultipleNum], [MinGameCion], [GiffCion], [Description], [PlayerNum], [CardPackNum]) VALUES (1004, N'十倍积分房', 800, 10, 200000, 1000, N'', 3, 1)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1001, N'萌妹青铜组I级', 49)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1002, N'萌妹青铜组II级', 99)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1003, N'萌妹青铜组III级', 199)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1004, N'萌妹白银组I级', 399)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1005, N'萌妹白银组II级', 599)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1006, N'萌妹白银组III级', 999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1007, N'萌妹黄金组I级', 1999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1008, N'萌妹黄金组II级', 3999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1009, N'萌妹黄金组III级', 5999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1010, N'萌妹铂金组I级', 9999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1011, N'萌妹铂金组II级', 29999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1012, N'萌妹铂金组III级', 99999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1013, N'萌妹钻石组I级', 199999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1014, N'萌妹钻石组II级', 499999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1015, N'萌妹钻石组III级', 999999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1016, N'萌妹大神组I级', 2999999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1017, N'萌妹大神组II级', 4999999)
	INSERT [TitleInfo] ([Id], [Name], [TargetNum]) VALUES (1018, N'萌妹大神组III级', 9999999)
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (1, N'白鼠', N'head_1006', 1, 1, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (2, N'水牛', N'head_1007', 1, 2, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (3, N'蠢虎', N'head_1008', 1, 3, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (4, N'玉兔', N'head_1009', 1, 4, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (5, N'神龙', N'head_1010', 1, 5, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (6, N'青蛇', N'head_1011', 1, 6, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (7, N'憨马', N'head_1012', 1, 7, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (8, N'羊羊', N'head_1013', 1, 8, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (9, N'灵猴', N'head_1014', 1, 9, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (10, N'雄鸡', N'head_1015', 1, 10, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (11, N'小哈', N'head_1016', 1, 11, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (12, N'睡猪', N'head_1017', 1, 12, 20, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (13, N'小丑', N'head_1018', 1, 13, 50, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (14, N'公主', N'head_1019', 1, 14, 50, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (15, N'王子', N'head_1020', 1, 15, 50, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (16, N'小金豆', N'head_1021', 2, 16, 10, 0, 1000, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (17, N'中金豆', N'head_1022', 2, 17, 100, 0, 10000, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (18, N'大金豆', N'head_1023', 2, 18, 600, 0, 60000, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (19, N'特大金豆', N'head_1024', 2, 19, 2000, 0, 200000, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (20, N'至尊金豆', N'head_1025', 2, 20, 5000, 0, 500000, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (21, N'女王', N'head_1026', 1, 21, 70, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (22, N'精灵', N'head_1027', 1, 22, 70, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (23, N'女神', N'head_1028', 1, 23, 70, 0, 0, N'')
	INSERT [ShopInfo] ([ShopID], [ShopName], [HeadID], [ShopType], [SeqNO], [Price], [VipPrice], [GameCoin], [ShopDesc]) VALUES (24, N'学生妹', N'head_1029', 1, 24, 70, 0, 0, N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (1, 100, N'', CAST(0.3000 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (2, 200, N'', CAST(0.3000 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (3, 400, N'', CAST(0.2000 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (4, 600, N'', CAST(0.1000 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (5, 900, N'', CAST(0.0500 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (6, 1200, N'', CAST(0.0500 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (7, 2400, N'', CAST(0.0400 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (8, 4000, N'', CAST(0.0300 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (9, 6000, N'', CAST(0.0200 AS Decimal(8, 4)), N'')
	INSERT [DialInfo] ([ID], [GameCoin], [HeadID], [Probability], [ItemDesc]) VALUES (10, 10000, N'', CAST(0.0100 AS Decimal(8, 4)), N'')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (1001, N'萌妹首胜', 1, 2, 1, 0, 800, N'初出茅庐赢得首场萌妹斗地主胜利')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (1002, N'正常发挥', 1, 2, 3, 0, 1200, N'赢得3场萌妹斗地主的胜利')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (1003, N'胜利旗帜', 1, 2, 10, 0, 2000, N'赢得10场萌妹斗地主的胜利')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (1004, N'胜利喜悦', 1, 2, 15, 0, 2500, N'赢得15场萌妹斗地主的胜利')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (1005, N'斗战胜佛', 1, 2, 30, 0, 4500, N'赢得30场萌妹斗地主的胜利')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2001, N'萌妹铜牌', 1, 2, 50, 1001, 200, N'获得萌妹铜牌成就（需赢得50胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2002, N'萌妹银牌', 1, 2, 150, 1002, 400, N'获得萌妹银牌成就（需赢得150胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2003, N'萌妹金牌', 1, 2, 300, 1003, 600, N'获得萌妹金牌成就（需赢得300胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2004, N'萌妹白金', 1, 2, 600, 1004, 800, N'获得萌妹白金成就（需赢得600胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2005, N'萌妹铂金', 1, 2, 1000, 1005, 1000, N'获得萌妹铂金成就（需赢得1000胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2006, N'萌妹钻石', 1, 2, 2000, 1006, 1200, N'获得萌妹钻石成就（需赢得2000胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2007, N'萌妹大神', 1, 2, 3500, 1007, 1400, N'获得萌妹大神成就（需赢得3500胜利）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2008, N'一星至尊', 1, 5, 500, 1008, 2000, N'获得一星至尊成就（需累充值500元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2009, N'二星至尊', 1, 5, 1500, 1009, 3000, N'获得二星至尊成就（需累充值1500元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2010, N'三星至尊', 1, 5, 3000, 1010, 4000, N'获得三星至尊成就（需累充值3000元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2011, N'四星至尊', 1, 5, 6000, 1011, 5000, N'获得四星至尊成就（需累充值6000元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2012, N'五星至尊', 1, 5, 8000, 1012, 7000, N'获得五星至尊成就（需累充值8000元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2013, N'六星至尊', 1, 5, 10000, 1013, 8000, N'获得六星至尊成就（需累充值10000元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (2014, N'七星至尊', 1, 5, 20000, 1014, 9000, N'获得七星至尊成就（需累充值20000元宝）')
	INSERT [TaskInfo] ([TaskID], [TaskName], [TaskType], [TaskClass], [RestraintNum], [AchieveID], [GameCoin], [TaskDesc]) VALUES (3001, N'每日一胜', 2, 2, 1, 0, 400, N'赢得今日第一次胜利')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Game.FleeMultipleNum', N'10', N'游戏逃跑最低倍数')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Game.Table.AIFirstOutCardTime', N'15000', N'机器人一手出牌间隔时间')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Game.Table.AIIntoTime', N'10000', N'机器人加入桌子时间间隔(毫秒)')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Game.Table.AIOutCardTime', N'5000', N'机器人出牌间隔5000秒')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Game.Table.MinTableCount', N'10', N'初始开出空桌数')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'Ranking.OfficeNumber', N'100', N'进胜率榜的总场数')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.DailyFreeNum', N'3', N'每日转盘活动免费次数')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.DailyGiffCoinTime', N'1', N'每是赠送金豆次数')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.GameCoin', N'10000', N'建角初始金豆')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.GiftGold', N'50', N'建角初始赠送元宝')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.Level', N'1', N'建角初始等级')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.MaxLength', N'12', N'昵称最大长度')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.MinLength', N'4', N'昵称最小长度')
	INSERT [ConfigEnvSet] ([EnvKey], [EnvValue], [EnvDesc]) VALUES (N'User.VipLv', N'0', N'建角初始VIP等级')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (103, N'黑桃3', 1, 3, N'card_1066')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (104, N'黑桃4', 1, 4, N'card_1070')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (105, N'黑桃5', 1, 5, N'card_1074')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (106, N'黑桃6', 1, 6, N'card_1078')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (107, N'黑桃7', 1, 7, N'card_1082')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (108, N'黑桃8', 1, 8, N'card_1086')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (109, N'黑桃9', 1, 9, N'card_1090')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (110, N'黑桃10', 1, 10, N'card_1094')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (111, N'黑桃J', 1, 11, N'card_1098')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (112, N'黑桃Q', 1, 12, N'card_1102')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (113, N'黑桃K', 1, 13, N'card_1106')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (114, N'黑桃A', 1, 14, N'card_1110')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (115, N'黑桃2', 1, 15, N'card_1062')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (203, N'梅花3', 2, 3, N'card_1068')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (204, N'梅花4', 2, 4, N'card_1072')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (205, N'梅花5', 2, 5, N'card_1076')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (206, N'梅花6', 2, 6, N'card_1080')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (207, N'梅花7', 2, 7, N'card_1084')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (208, N'梅花8', 2, 8, N'card_1088')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (209, N'梅花9', 2, 9, N'card_1092')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (210, N'梅花10', 2, 10, N'card_1096')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (211, N'梅花J', 2, 11, N'card_1100')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (212, N'梅花Q', 2, 12, N'card_1104')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (213, N'梅花K', 2, 13, N'card_1108')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (214, N'梅花A', 2, 14, N'card_1112')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (215, N'梅花2', 2, 15, N'card_1064')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (303, N'方片3', 3, 3, N'card_1065')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (304, N'方片4', 3, 4, N'card_1069')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (305, N'方片5', 3, 5, N'card_1073')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (306, N'方片6', 3, 6, N'card_1077')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (307, N'方片7', 3, 7, N'card_1081')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (308, N'方片8', 3, 8, N'card_1085')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (309, N'方片9', 3, 9, N'card_1089')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (310, N'方片10', 3, 10, N'card_1093')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (311, N'方片J', 3, 11, N'card_1097')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (312, N'方片Q', 3, 12, N'card_1101')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (313, N'方片K', 3, 13, N'card_1105')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (314, N'方片A', 3, 14, N'card_1109')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (315, N'方片2', 3, 15, N'card_1061')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (403, N'红桃3', 4, 3, N'card_1067')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (404, N'红桃4', 4, 4, N'card_1071')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (405, N'红桃5', 4, 5, N'card_1075')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (406, N'红桃6', 4, 6, N'card_1079')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (407, N'红桃7', 4, 7, N'card_1083')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (408, N'红桃8', 4, 8, N'card_1087')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (409, N'红桃9', 4, 9, N'card_1091')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (410, N'红桃10', 4, 10, N'card_1095')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (411, N'红桃J', 4, 11, N'card_1099')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (412, N'红桃Q', 4, 12, N'card_1103')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (413, N'红桃K', 4, 13, N'card_1107')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (414, N'红桃A', 4, 14, N'card_1111')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (415, N'红桃2', 4, 15, N'card_1063')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (518, N'小王', 5, 18, N'card_1114')
	INSERT [PokerInfo] ([Id], [Name], [Color], [Value], [HeadIcon]) VALUES (619, N'大王', 6, 19, N'card_1113')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1001, N'苍天啊，给我一个出牌的机会吧！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1002, N'哎，我说你丫能快点吗？')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1003, N'出啊，好牌都留着下蛋啊！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1004, N'地主，我和你拼了!')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1005, N'和你搭档，真是伤不起啊！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1006, N'和你搭档，真是太愉快了！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1007, N'农民兄弟，不要欺负地主啊！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1008, N'兄弟，你打得这叫什么牌呀！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1009, N'咱们老百姓啊，今儿个真高兴！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1010, N'这副牌我赢定了！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1011, N'这牌发的，真是太郁闷了！')
	INSERT [ChatInfo] ([Id], [Content]) VALUES (1012, N'这牌一好，心情就好！')
END
GO