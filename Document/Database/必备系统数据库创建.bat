@echo off

if "%1" == "" (
@echo 出错:
@echo     执行命令缺少"[dbServer]"参数
goto error
)
if "%2" == "" (
@echo 出错:
@echo     执行命令缺少"[dbAcount]"参数
goto error
)
if "%3" == "" (
@echo 出错:
@echo     执行命令缺少"[dbPass]"参数
goto error
)
if "%4" == "" (
@echo 出错:
@echo     执行命令缺少"[gamepass]"参数
goto error
)
if "%5" == "" (
@echo 出错:
@echo     执行命令缺少"[dbpath]"参数
goto error
) else (

@echo 正在创建数据库登录帐号...
Sqlcmd -S %1 -U %2 -P %3 -d master -i 创建游戏帐号.sql -v loginPass="%4"

@echo 正在创建用户中心数据库...
Sqlcmd -S %1 -U %2 -P %3 -d master -i 用户中心表结构.sql -v dbpath="%5" 

@echo 正在创建分服与充值中心数据库...
Sqlcmd -S %1 -U %2 -P %3 -d master -i 分服与充值中心表结构.sql -v dbpath="%5" 

goto success
)

:error
@echo 命令参数：
@echo     [dbServer] 数据库服务器
@echo     [dbAcount] 数据库登录帐号 
@echo     [dbPass] 数据库登录密码 
@echo     [gamepass] 游戏登录帐号game_user的密码 
@echo     [dbpath] 数据库存储路径 

:success
@echo 执行成功

ECHO 运行结束！& PAUSE
