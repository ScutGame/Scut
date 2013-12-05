use SvnDeployment
go
CREATE TABLE SimplePlanInfo(
	Name varchar(100) NOT NULL,
	Value varchar(50) NULL,
	UpdateTime datetime NULL,
	primary key(Name)
)
go
CREATE TABLE [dbo].[SimplePlanInfoLog](
	[LogId] [int] IDENTITY(1,1) NOT NULL,
	[ServerIp] [varchar](50) NULL,
	[LogInfo] [varchar](500) NULL,
	[AddTime] [datetime] NULL  DEFAULT (getdate()),
 CONSTRAINT [PK_SimplePlanInfoLog] PRIMARY KEY CLUSTERED 
(
	[LogId] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

CREATE TABLE [dbo].[SimplePlanManager](
	[AutoID] [int] IDENTITY(1,1) NOT NULL,
	[PlanName] [varchar](50)  NULL,
	[PlanPath] [varchar](300) NULL,
	[PlanStatus] [tinyint] NULL,
	[PlanCommand] [tinyint] NULL,
	[CreateTime] [datetime] NULL,
	[UpdateTime] [datetime] NULL,
	[Remark] [varchar](50) NULL,
	[AutoStart] [tinyint] NULL DEFAULT ((0)),
	[ServerIP] [varchar](50) NULL,
 CONSTRAINT [PK_SimplePlanManager] PRIMARY KEY CLUSTERED 
(
	[AutoID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
go

--drop table DepProject
create table DepProject
(
	Id	Int identity(1,1),
	Name	Varchar(100)  not null,
	Ip	Varchar(100),
	SvnPath	Varchar(500),
	SharePath [varchar](500),
	ExcludeFile [varchar](500),
	CreateDate	DateTime,
	primary key(Id)
)
go


--drop table DepProjectItem
create table DepProjectItem
(
	DepId	Int not null default(0),
	Id	Int identity(1,1),
	Name	Varchar(100)  not null,
	WebSite	Varchar(100)  not null,
	DeployPath	Varchar(500)  not null,
	ExcludeFile Varchar(500),
	CreateDate	DateTime,
	primary key(Id)
)


create table DepProjectAction
(
	Id	Int identity(1,1),
	Ip	Varchar(100),
	Type	Int  not null,
	DepId	Int  not null,
	Revision int  not null,
	Status	Int not null,
	ErrorMsg text, 
	CreateDate	DateTime,
	primary key(Id)

)
--2013-03-05
alter table DepProject add GameId Int default(0) not null;
alter table DepProjectItem add ServerId Int default(0) not null;