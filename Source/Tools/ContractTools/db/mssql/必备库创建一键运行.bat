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
@echo     [dbpath] 数据库存储路径:%dbpath%
@echo ================================================================

MD %dbpath%

Sqlcmd -? 2>nul 1>nul
if errorlevel 1 (
echo 请安装sqlcmd支持。&pause>nul
exit
)

Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i 创建表脚本.sql -v gameuser="%gameuser%" dbpath="%dbpath%" 
@echo 正在创建数据库成功!
@echo 正在导入数据...
Sqlcmd -S %dbServer% -U %dbAcount% -P %dbPass% -d master -i ScutContractData.sql -v gameuser="%gameuser%" dbpath="%dbpath%"
@echo 导入数据成功!
@echo 执行成功
ECHO 运行结束！& PAUSE
