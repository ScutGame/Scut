use DdzConfig
go
create table PokerInfo
(
	Id	Int,
	Name	Varchar(20) not null,
	Color	Smallint not null,
	Value	Smallint not null,
	HeadIcon	Varchar(50),
	primary key(Id)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '扑克牌配置', N'user', N'dbo', N'table', N'PokerInfo', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '牌花色[Enum<CardColor>]', N'SCHEMA', N'dbo',  N'table', N'PokerInfo', N'column', N'Color' 
EXECUTE sp_addextendedproperty N'MS_Description', '牌大小值', N'SCHEMA', N'dbo',  N'table', N'PokerInfo', N'column', N'Value' 
go

create table ChatInfo
(
	Id	Int,
	Content	Varchar(100),
	primary key(Id)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '聊天语言配置', N'user', N'dbo', N'table', N'ChatInfo', NULL, NULL
go

create table TitleInfo
(
	Id	Int,
	Name	Varchar(20) not null,
	TargetNum	Int not null default(0),
	primary key(Id)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '称号配置', N'user', N'dbo', N'table', N'TitleInfo', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '达到值', N'SCHEMA', N'dbo',  N'table', N'TitleInfo', N'column', N'TargetNum' 
go


create table AchievementInfo
(
	Id	Int,
	Name	Varchar(20) not null,
	Type	Smallint not null default(0),
	TargetNum	Int not null default(0),
	HeadIcon	Varchar(50),
	Description	Varchar(500),
	primary key(Id)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '成就配置', N'user', N'dbo', N'table', N'AchievementInfo', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '成就类型', N'SCHEMA', N'dbo',  N'table', N'AchievementInfo', N'column', N'Type' 
EXECUTE sp_addextendedproperty N'MS_Description', '成就达到值', N'SCHEMA', N'dbo',  N'table', N'AchievementInfo', N'column', N'TargetNum' 
go

create table RoomInfo
(
	Id	Int,
	Name	Varchar(20) not null,
	AnteNum	Int not null default(0),
	MultipleNum	SmallInt not null default(0),
	MinGameCion	Int not null default(0),
	GiffCion	Int not null default(0),
	Description	Varchar(500),
	primary key(Id)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '房间配置', N'user', N'dbo', N'table', N'RoomInfo', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '底注分', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'AnteNum' 
EXECUTE sp_addextendedproperty N'MS_Description', '房间倍数', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'MultipleNum' 
EXECUTE sp_addextendedproperty N'MS_Description', '金豆数限制', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'MinGameCion' 
EXECUTE sp_addextendedproperty N'MS_Description', '每日赠豆,0:不赠送', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'GiffCion' 
go

alter table RoomInfo add 
	PlayerNum	Int not null default(0),
	CardPackNum	Int not null default(0);

EXECUTE sp_addextendedproperty N'MS_Description', '玩家人数', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'PlayerNum' 
EXECUTE sp_addextendedproperty N'MS_Description', '卡牌几副', N'SCHEMA', N'dbo',  N'table', N'RoomInfo', N'column', N'CardPackNum' 
go

create table ConfigEnvSet
(
	EnvKey	Varchar(50),
	EnvValue	Varchar(50) not null,
	EnvDesc	Varchar(200),
	primary key(EnvKey)
)
go

/*商店配置表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[ShopInfo]') AND type in (N'U'))
begin
create table ShopInfo
(
	ShopID	Int not null,
	ShopName	Varchar(20) not null,
	HeadID	Varchar(100),
	ShopType	Smallint not null,
	SeqNO	Smallint not null,
	Price	Int not null,
	VipPrice	Int not null,
	GameCoin	Int not null,
	ShopDesc	Varchar(200),
	primary key(ShopID)
)
end
go

/*转盘奖励配置表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DialInfo]') AND type in (N'U'))
begin
create table DialInfo
(
	ID	Int not null,
	GameCoin	Int not null,
	HeadID	Varchar(100),
	Probability	Decimal(8,4) not null,
	ItemDesc	Varchar(500),
	primary key(ID)
)
end
go

/*任务配置表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[TaskInfo]') AND type in (N'U'))
begin
create table TaskInfo
(
	TaskID	Int not null,
	TaskName	Varchar(50)  not null,
	TaskType	Smallint  not null,
	TaskClass	Smallint  not null,
	RestraintNum	Int  not null,
	AchieveID	Int  not null,
	GameCoin	Int  not null,
	TaskDesc	Varchar(500),
	primary key(TaskID)
)
end
go
