use Ddz1Data
go

create table GameUser
(
	UserId	Int,
	Pid	Varchar(32) not null,
	NickName	Varchar(50) not null,
	HeadIcon	Varchar(50) not null,
	Sex	bit,
	RetailId	Varchar(50),
	PayGold	Int default(0),
	GiftGold	Int default(0),
	ExtGold	Int default(0),
	UseGold	Int default(0),
	GameCoin	Int default(0),
	UserLv	Smallint default(0),
	VipLv	Int default(0),
	RankId	Int default(0),
	UserStatus	Int default(0),
	MsgState	Bit,
	MobileType	Smallint default(0),
	ScreenX	Smallint default(0),
	ScreenY	Smallint default(0),
	ClientAppVersion	Smallint default(0),
	CreateDate	DateTime,
	LoginDate	DateTime,
	LastLoginDate	DateTime,
	WinNum	Int default(0),
	FailNum	Int default(0),
	primary key (UserId)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '玩家信息[:BaseUser]', N'user', N'dbo', N'table', N'GameUser', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '玩家状态[Enum<UserStatus>]', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'UserStatus' 
EXECUTE sp_addextendedproperty N'MS_Description', '玩家状态[Enum<MobileType>]', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'MobileType' 
go

alter table GameUser add 
	ScoreNum	Int default(0) not null,
	TitleId	Int default(0) not null;
go
EXECUTE sp_addextendedproperty N'MS_Description', '积分', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'ScoreNum' 
EXECUTE sp_addextendedproperty N'MS_Description', '称号ID', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'TitleId' 
go

alter table GameUser add 
	RealName	Varchar(20),
	Birthday	DateTime,
	Hobby	Varchar(20),
	Profession	Varchar(20);
go

EXECUTE sp_addextendedproperty N'MS_Description', '真实姓名', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'RealName' 
EXECUTE sp_addextendedproperty N'MS_Description', '生日', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'Birthday' 
EXECUTE sp_addextendedproperty N'MS_Description', '爱好', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'Hobby' 
EXECUTE sp_addextendedproperty N'MS_Description', '职业', N'SCHEMA', N'dbo',  N'table', N'GameUser', N'column', N'Profession' 
go

create table UserDailyRestrain
(
	UserId	Int,
	RefreshDate	DateTime not null,
	RestrainProperty	Text,
	primary key(UserId)
)
go
EXECUTE sp_addextendedproperty N'MS_Description', '每日限制表', N'user', N'dbo', N'table', N'UserDailyRestrain', NULL, NULL
EXECUTE sp_addextendedproperty N'MS_Description', '玩家限制属性[RestrainProperty]', N'SCHEMA', N'dbo',  N'table', N'UserDailyRestrain', N'column', N'RestrainProperty' 
go

/*玩家物品表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserItemPackage]') AND type in (N'U'))
begin
create table UserItemPackage
(
	UserID	int not null,
	ItemPackage	Text,
	primary key(UserID)
)
end
go

/*玩家任务表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTask]') AND type in (N'U'))
begin
create table UserTask
(
	UserID	int not null,
	TaskPackage	Text,
	primary key(UserID)
)
end
go
/*玩家成就表 */
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserAchieve]') AND type in (N'U'))
begin
create table UserAchieve
(
	UserID	int not null,
	AchievePackage	Text,
	primary key(UserID)
)
end
go


IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[GameNotice]') AND type in (N'U'))
begin
CREATE TABLE GameNotice
(
	[NoticeID] [char](36) NOT NULL,
	[Title] [varchar](200)  NULL,
	[Content] [varchar](2000)  NULL,
	[ExpiryDate] [datetime] NULL,
	[IsTop] [bit]   DEFAULT ((0)),
	[IsBroadcast] [bit] NULL  DEFAULT ((0)),
	[Creater] [varchar](20)  NULL,
	[CreateDate] [datetime] NULL,
	[NoticeType] [int] NULL
	primary key(NoticeID)
)
end
go

--2013-8-1
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[UserTakePrize]') AND type in (N'U'))
begin
create table UserTakePrize
(
	ID	char(36),
	UserID	Int not null,
	GameCoin	Int default(0),
	Gold	Int default(0),
	ItemPackage	Varchar(100),
	MailContent	Varchar(500),
	IsTasked	Bit default(0),
	TaskDate	DateTime,
	OpUserID	Int,
	CreateDate	DateTime,
	primary key(ID)
)
end
go
