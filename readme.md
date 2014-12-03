# README

[![Build Status](http://scutgame.com/images/passing.png?branch=6.7.8.7)](http://scutgame.com/download/)


## What's Scut?

Scut is a free, open source, Stable game server framework, which support 
C#/Python/Lua script. Scut's design philosophy is to "Developing online game easier", 
It includes a development framework and no-sql/database storage services, 
and many game system modules. Save a lot of game developers working hours, 
it allows the user to focus on business logic. Scut Game Server Framework 
also provide ScutSDK(eg. for Cocos2d-x) which make communications between 
server and client very easy.


## Git for developers

    $ git clone git://github.com/ScutGame/Scut.git
    or
    $ git clone git://git.oschina.net/scutgame/Scut.git


## Git for samples

If the Chinese downloaded using [oschina git](https://git.oschina.net/scutgame/Scut-samples),
else using [github](https://github.com/ScutGame/Scut-samples)

    $ git clone git://github.com/ScutGame/Scut-samples.git
    or
    $ git clone git://git.oschina.net/scutgame/Scut-samples.git


## Requirements

Scut is only supported on .Net Framework 4.5.1 and up, you need a Redis of NoSQL as well.


## Benefits Features

### Platform

* Windows
* Linux
* Mac


### Data Persistence (DBs)

* SQL: MySQL/MS SQL Server
* NoSQL: Redis


### Protocols: Http/Socket

* Transfer protocol is very lean and slim
* Scut wraps up the networking layer of each client platform
* Communicate cross-platform and cross-protocol
* Code generate automaticly: forget about de-/serialization


### Server Dev Framework
Language:

* C#
* Python
* Lua


### Vast Support of Client Platforms

* All client platforms interoperate(iOS vs Android vs PC)
* Major client platforms supported
* Android(eg. Cocos2d-x)
* iOS(eg. Cocos2d-x)
* Win32
* .NET
* Mac OSX
* Mono
* Xamarin


### Host Scut with major provider

* Microsoft Azure
* Amazon EC2


## Update Log

### Version: 6.7.9.5 (2014-12-3) Beta

* 修改对定时器任务计划简化配置
* 修改支持日期类型的协议参数问题

* 增加Console安全退出方式
* 增加WebSocket通讯支持
* 增加Config配置文件修改生效机制

### Version: 6.7.9.0 (2014-10-24) Stable

* 优化数据加载，是否从Redis的垃圾表加载数据是可配置的，默认不加载
* 优化玩家的请求锁，多个Action可以并发
* 修改聊天中间件显示出错
* 修改Session默认过期时间为2小时
* 修改优化Redis连接池管理
* 修改优化Redis实体数据同步队列
* 修改实体的Change事件通知为同步方式
* 修改实体中Guid类型字段在DB中不能删除
* 修改DB连接会间断连接失败问题
* 修改Linux平台Console更改字体颜色问题

* 增加心跳过期事件
* 增加获取在线玩家方法
* 增加实体在Redis或DB的修改时间字段，可配置DB是是否增加此字段，默认不增加
* 增加动态执行源码字串的功能


old version [more](http://scutgame.com/log).


## Resource

HomePage: http://www.scutgame.com

Github: https://github.com/scutgame/Scut.git

Oschina: https://git.oschina.net/scutgame/Scut.git

QQ Group: 138266675


## License

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
```