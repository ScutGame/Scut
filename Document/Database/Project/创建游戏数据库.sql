/*
参数:
$gameName 游戏项目名称
$dbpath 数据库存储路径
*/

/*=========================================================================================*/

--config
CREATE DATABASE [$(gameName)Config] 
ON  PRIMARY ( NAME = N'$(gameName)Config', FILENAME = N'$(dbpath)/$(gameName)Config.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
 LOG ON ( NAME = N'$(gameName)Config_log', FILENAME = N'$(dbpath)/$(gameName)Config_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [$(gameName)Config] SET RECOVERY SIMPLE 
go

use $(gameName)Config
GO
--权限
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'$(gameuser)')
	CREATE USER $(gameuser) FOR LOGIN $(gameuser) WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_datareader', N'$(gameuser)'
EXEC sp_addrolemember N'db_datawriter', N'$(gameuser)'
EXEC sp_addrolemember N'db_ddladmin', N'$(gameuser)'
GO

--data
CREATE DATABASE [$(gameName)1Data] 
ON  PRIMARY ( NAME = N'$(gameName)1Data', FILENAME = N'$(dbpath)/$(gameName)1Data.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
 LOG ON ( NAME = N'$(gameName)1Data_log', FILENAME = N'$(dbpath)/$(gameName)1Data_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [$(gameName)1Data] SET RECOVERY FULL
go

use $(gameName)1Data
GO
--权限
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'$(gameuser)')
	CREATE USER $(gameuser) FOR LOGIN $(gameuser) WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_datareader', N'$(gameuser)'
EXEC sp_addrolemember N'db_datawriter', N'$(gameuser)'
EXEC sp_addrolemember N'db_ddladmin', N'$(gameuser)'
GO

--log
CREATE DATABASE [$(gameName)1Log] 
ON  PRIMARY ( NAME = N'$(gameName)1Log', FILENAME = N'$(dbpath)/$(gameName)1Log.mdf' , SIZE = 3072KB , FILEGROWTH = 1024KB )
 LOG ON ( NAME = N'$(gameName)1Log_log', FILENAME = N'$(dbpath)/$(gameName)1Log_log.ldf' , SIZE = 1024KB , FILEGROWTH = 10%)
GO
ALTER DATABASE [$(gameName)1Log] SET RECOVERY BULK_LOGGED 
go

use $(gameName)1Log
GO
--权限
IF NOT EXISTS (SELECT * FROM sys.database_principals WHERE name = N'$(gameuser)')
	CREATE USER $(gameuser) FOR LOGIN $(gameuser) WITH DEFAULT_SCHEMA=[dbo]
GO
EXEC sp_addrolemember N'db_datareader', N'$(gameuser)'
EXEC sp_addrolemember N'db_datawriter', N'$(gameuser)'
EXEC sp_addrolemember N'db_ddladmin', N'$(gameuser)'
GO
