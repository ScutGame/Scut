@echo off

cd %cd%

set dbServer=.
set dbAcount=sa
set dbPass=123
set gameuser=game_user
set gamepass=123
set gameName=Ddz
set dbpath=%cd%\Data

@echo 配置参数如下：
@echo     [dbServer] 数据库服务器:%dbServer%
@echo     [dbAcount] 可创建数据库的帐号(sa):%dbAcount%
@echo     [dbPass]   可创建数据库的密码(sa):%dbPass%
@echo     [gameuser] 游戏登录帐号:%gameuser%
@echo     [gamepass] 游戏登录密码:%gamepass%
@echo     [gameName] 游戏项目名称:%gameName%
@echo     [dbpath] 数据库存储路径:%dbpath%
@echo ================================================================

MD %dbpath%

Sqlcmd -? 2>nul 1>nul
if errorlevel 1 (
echo 请安装sqlcmd支持。&pause>nul
exit
)

Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 创建游戏帐号.sql -v gameuser="%gameuser%" loginPass="%gamepass%"
@echo 创建数据库登录帐号成功!

Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 用户中心表结构.sql -v gameuser="%gameuser%" dbpath="%dbpath%" 
@echo 正在创建用户中心数据库成功!

Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 分服与充值中心表结构.sql -v gameuser="%gameuser%" dbpath="%dbpath%" 
@echo 创建分服中心数据库成功!

@echo 正在创建游戏%4数据库...
Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 创建游戏数据库.sql -v gameuser="%gameuser%" gameName="%gameName%" dbpath="%dbpath%"


@echo 正在初始化游戏配置数据...
Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 配置库初始数据.sql -v gameName="%gameName%"

@echo 执行成功

ECHO 运行结束！& PAUSE

