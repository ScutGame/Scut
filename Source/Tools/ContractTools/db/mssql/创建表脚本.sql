/*
参数:
$dbpath 数据库存储路径
*/

/*=========================================================================================*/
CREATE DATABASE [ContractDB] 
ON  PRIMARY ( NAME = N'ContractDB', FILENAME = N'$(dbpath)/ContractDB.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
 LOG ON ( NAME = N'ContractDB_log', FILENAME = N'$(dbpath)/ContractDB_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO

use ContractDB
GO

--pms
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'$(gameuser)')
	CREATE USER $(gameuser) FOR LOGIN $(gameuser) WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_datareader', N'$(gameuser)'
EXEC sp_addrolemember N'db_datawriter', N'$(gameuser)'
EXEC sp_addrolemember N'db_ddladmin', N'$(gameuser)'
GO


SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Solutions](
	[SlnID] [int] IDENTITY(100,1) NOT NULL,
	[SlnName] [varchar](100) NOT NULL,
	[Namespace] [varchar](200) NULL,
	[RefNamespace] [varchar](200) NULL,
	[Url] [varchar](200) NULL,
	[GameID] [int] NULL,
 CONSTRAINT [PK_Solutions] PRIMARY KEY CLUSTERED 
(
	[SlnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [ParamInfo](
	[ID] [int] IDENTITY(17000,1) NOT NULL,
	[ContractID] [int] NOT NULL,
	[ParamType] [smallint] NOT NULL,
	[Field] [varchar](30) NOT NULL,
	[FieldType] [smallint] NOT NULL,
	[Descption] [varchar](100) NULL,
	[FieldValue] [varchar](100) NULL,
	[Required] [bit] NOT NULL,
	[Remark] [varchar](500) NULL,
	[SortID] [int] NOT NULL,
	[Creator] [int] NOT NULL,
	[CreateDate] [datetime] NOT NULL CONSTRAINT [DF_ParamInfo_CreateDate]  DEFAULT (getdate()),
	[Modifier] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL CONSTRAINT [DF_ParamInfo_ModifyDate]  DEFAULT (getdate()),
	[SlnID] [int] NOT NULL,
	[MinValue] [int] NOT NULL,
	[MaxValue] [int] NOT NULL,
 CONSTRAINT [PK_ParamInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [enuminfo](
	[id] [int] IDENTITY(1000,1) NOT NULL,
	[SlnID] [int] NOT NULL,
	[enumName] [nvarchar](50) NOT NULL,
	[enumDescription] [nvarchar](200) NULL,
	[enumValueInfo] [ntext] NOT NULL,
 CONSTRAINT [PK_enuminfo] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [AgreementClass](
	[AgreementID] [int] IDENTITY(100,1) NOT NULL,
	[GameID] [int] NULL,
	[Title] [varchar](200) NULL,
	[Describe] [varchar](max) NULL,
	[CreateDate] [datetime] NULL CONSTRAINT [DF_AgreementClass_CreateDate]  DEFAULT (getdate()),
 CONSTRAINT [PK_AgreementClass] PRIMARY KEY CLUSTERED 
(
	[AgreementID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [Contract](
	[ID] [int] NOT NULL,
	[Descption] [varchar](100) NOT NULL,
	[ParentID] [int] NOT NULL,
	[SlnID] [int] NOT NULL,
	[Complated] [bit] NOT NULL CONSTRAINT [DF__Contract__Compla__0A688BB1]  DEFAULT ((0)),
	[AgreementID] [int] NULL CONSTRAINT [DF_Contract_AgreementID]  DEFAULT ((0)),
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[SlnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

go
create table ContractVersion
(
	ID	int identity(1,1),
	Title	varchar(100),
	SlnID	int not null,
	primary key(id)
)

go

alter table [Contract] add [VerId] int not null default(0);
alter table [ParamInfo] add [VerId] int not null default(0);
go


alter table [Solutions] add [SerUseScript] varchar(20);
alter table [Solutions] add [CliUseScript] varchar(20);
alter table [Solutions] add [IsDParam] bit not null default(1);
alter table [Solutions] add [RespContentType] int not null default(0);
go
