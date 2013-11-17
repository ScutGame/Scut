斗地主源代码

目录构成：

Client：客户端源代码（lua）
更改连接服务器地址两种方式：
1.打开C:\Windows\System32\drivers\etc\hosts文件，增加:127.0.0.1 ddz.36you.net
2.打开Lua文件：Client\lua\lib\NetHelper.lua, 修改67行代码 "ddz.36you.net:9700"改为"127.0.0.1:9700"

Server：服务器源代码
服务器框架使用说明查看《Scut游戏引擎》使用教程，路径：Scut\Document\Tutorials\《Scut游戏引擎》使用教程.doc
修改的地方
1.ZyGames.Doudizhu.HostServer\PyScript\Route.config.xml文件，<lib path="D:\Python\Lib" />改成Python安装时的目录
2.修改App.config里的sa密码，和PayDB_Acount、Snscenter_Acount配置
