ScutSDK
=================


ScutSDK简介
----------------
Scut游戏服务器引擎开发的宗旨是为了能让网游开发和单机一样简单，以便于降低有志于游戏开发的小伙伴们进入网络游戏开发的门槛。ScutSDK是和Scut游戏服务器引擎，简化客户端开发的配套SDK，她彻底打通了Scut开源游戏服务器引擎与客户端引擎（如Cocos2d-x/Quick-x/Unity3D）项目间的通信，进而实现整套的网络游戏解决方案。

资源
----------------
官网地址：http://www.scutgame.com<br />
Github代码库：https://github.com/scutgame/scut<br />
OSChina代码库：https://git.oschina.net/scutgame/Scut<br />
博客园地址：http://www.cnblogs.com/scut/<br />

技术交流QQ群：138266675

更新日志
----------------
###版本：0.95 (2013-12-24)
> 1. 增加对cocos2d-x的Android平台的支持

###版本：0.9 (2013-12-21)
> 1. 支持cocos2d-x C++/Lua语言最新版本2.2.1和quick-x最新版本2.2.1
> 2. 支持Win32/iOS平台


What's Scut Game Server Framework？
=================
Scut is a free, open source, stable game server framework, which support C#/Python script. Scut's design philosophy is to "Developing online game easier"，It includes a development framework and no-sql/database storage services, and many game system modules. Save a lot of game developers working hours，it allows the user to focus on business logic. Scut Game Server Framework also provide ScutSDK(eg. for Cocos2d-x) which make communications between server and client very easy.

Benefits Features
----------------
###Platform
> Windows
> Linux
###Data Persistence (DBs)
> SQL: MySQL/MS SQL Server
> NoSQL: Redis
###Protocols: http/socket
> Transfer protocol is very lean and slim
> Scut wraps up the networking layer of each client platform
> Communicate cross-platform and cross-protocol
> Code generate automaticly: forget about de-/serialization
###Server Dev Framework
> Language: C#/Python
###Vast Support of Client Platforms
> All client platforms interoperate(iOS vs Android vs PC)
> Major client platforms supported
> Android(eg. Cocos2d-x)
> iOS(eg. Cocos2d-x)
> Win32
> .NET
> Mac OSX
> Mono
> Xamarin
###Host Scut with major provider
> PaaS: Platform as a Service
>Selected providers
>> Microsoft Azure
>> Amazon EC2

Resource
----------------
HomePage: http://www.scutgame.com
Github: https://github.com/scutgame/scut
QQ Group: 138266675


License
--------------
FreeBSD License
```
Copyright (c) 2013-2015 scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.