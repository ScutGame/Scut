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
@echo     执行命令缺少"[gameName]"参数
goto error
)
if "%5" == "" (
@echo 出错:
@echo     执行命令缺少"[dbpath]"参数
goto error
) else (
@echo 正在创建游戏%4数据库...
Sqlcmd -S %1 -U %2 -P %3 -d master -i 创建游戏数据库.sql -v gameName="%4" dbpath="%5"
goto success
)

:error
@echo 命令参数：
@echo     [dbServer] 数据库服务器
@echo     [dbAcount] 数据库登录帐号
@echo     [dbPass]   数据库登录密码
@echo     [gameName] 游戏项目名称
@echo     [dbpath]   数据库存储路径

:success
@echo 执行成功

ECHO 运行结束！& PAUSE

