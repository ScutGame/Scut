use SvnDeployment
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