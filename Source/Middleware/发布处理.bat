@echo off
xcopy /y ZyGames.Framework.Game\bin\ZyGames.Framework.Game.* ..\..\Release\6.7.9.11\Lib\ /EXCLUDE:copyfilter.txt
xcopy /y ScutSecurity\bin\*.dll ..\..\Release\6.7.9.11\Lib\ /EXCLUDE:copyfilter.txt

xcopy /y GameServer\bin\GameServer.* ..\..\Release\6.7.9.11\Console\ /EXCLUDE:copyfilter.txt
xcopy /y GameServer\bin\RNLog.config ..\..\Release\6.7.9.11\Console\
xcopy /y /s GameServer\Script\*.* ..\..\Release\6.7.9.11\Console\Script\

ECHO ·¢²¼Íê±Ï£¡& PAUSE