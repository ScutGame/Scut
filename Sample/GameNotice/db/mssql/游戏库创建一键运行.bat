@echo off

cd %cd%

set dbServer=.
set dbAcount=sa
set dbPass=123
set gameuser=game_user
set gamepass=123
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

Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i create_db.sql -v gameuser="%gameuser%" loginPass="%gamepass%" dbpath="%dbpath%"
@echo 创建数据库登录帐号成功!

@echo 执行成功

ECHO 运行结束！& PAUSE

