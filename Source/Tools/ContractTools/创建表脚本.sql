
GO
/****** Object:  Table [dbo].[Solutions] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Solutions](
	[SlnID] [int] IDENTITY(1,1) NOT NULL,
	[SlnName] [varchar](100) NOT NULL,
	[Namespace] [varchar](200) NULL,
	[RefNamespace] [varchar](200) NULL,
	[Url] [varchar](200) NULL,
 CONSTRAINT [PK_Solutions] PRIMARY KEY CLUSTERED 
(
	[SlnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ParamInfo]******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[ParamInfo](
	[ID] [int] IDENTITY(1,1) NOT NULL,
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
	[CreateDate] [datetime] NOT NULL,
	[Modifier] [int] NOT NULL,
	[ModifyDate] [datetime] NOT NULL,
	[SlnID] [int] NOT NULL,
	[MinValue] [int] NOT NULL,
	[MaxValue] [int] NOT NULL,
 CONSTRAINT [PK_ParamInfo] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Enuminfo] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[enuminfo](
	[id] [int] IDENTITY(1,1) NOT NULL,
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
/****** Object:  Table [dbo].[Contract] ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Contract](
	[ID] [int] NOT NULL,
	[Descption] [varchar](100) NOT NULL,
	[ParentID] [int] NOT NULL,
	[SlnID] [int] NOT NULL,
	[Complated] [bit] NOT NULL,
 CONSTRAINT [PK_Contract] PRIMARY KEY CLUSTERED 
(
	[ID] ASC,
	[SlnID] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
SET ANSI_PADDING OFF
GO
/****** Object:  Default [DF__Contract__Compla__0A688BB1]    Script Date: 08/16/2013 14:34:11 ******/
ALTER TABLE [dbo].[Contract] ADD  CONSTRAINT [DF__Contract__Compla__0A688BB1]  DEFAULT ((0)) FOR [Complated]
GO
/****** Object:  Default [DF_ParamInfo_CreateDate]    Script Date: 08/16/2013 14:34:11 ******/
ALTER TABLE [dbo].[ParamInfo] ADD  CONSTRAINT [DF_ParamInfo_CreateDate]  DEFAULT (getdate()) FOR [CreateDate]
GO
/****** Object:  Default [DF_ParamInfo_ModifyDate]    Script Date: 08/16/2013 14:34:11 ******/
ALTER TABLE [dbo].[ParamInfo] ADD  CONSTRAINT [DF_ParamInfo_ModifyDate]  DEFAULT (getdate()) FOR [ModifyDate]
GO
