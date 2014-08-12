What's Scut Game Server Framework?
=================
Scut is a free, open source, Stable game server framework, which support C#/Python/Lua script. Scut's design philosophy is to "Developing online game easier", It includes a development framework and no-sql/database storage services, and many game system modules. Save a lot of game developers working hours, it allows the user to focus on business logic. Scut Game Server Framework also provide ScutSDK(eg. for Cocos2d-x) which make communications between server and client very easy.

Benefits Features
----------------
###Platform
<ul>
<li>Windows
<li>Linux
<li>Mac
</ul>

###Data Persistence (DBs)
<ul>
<li>SQL: MySQL/MS SQL Server
<li>NoSQL: Redis
</ul>

###Protocols: Http/Socket
<ul>
<li>Transfer protocol is very lean and slim
<li>Scut wraps up the networking layer of each client platform
<li>Communicate cross-platform and cross-protocol
<li>Code generate automaticly: forget about de-/serialization
</ul>

###Server Dev Framework
Language:
<ul>
<li>C#
<li>Python
<li>Lua
</ul>


###Vast Support of Client Platforms
<ul>
<li>All client platforms interoperate(iOS vs Android vs PC)
<li>Major client platforms supported
<li>Android(eg. Cocos2d-x)
<li>iOS(eg. Cocos2d-x)
<li>Win32
<li>.NET
<li>Mac OSX
<li>Mono
<li>Xamarin
</ul>

###Host Scut with major provider
<ul>
<li>Microsoft Azure
<li>Amazon EC2
</ul>


Demo
----------------
https://github.com/ScutGame/Scut-samples


Resource
----------------
HomePage: http://www.scutgame.com<br />
Github: https://github.com/scutgame<br />
Oschina: https://git.oschina.net/scutgame/Scut<br />
QQ Group: 138266675<br />


Update Log
----------------
###Version: 6.7.8.7 (2014-8-15) Stable
```
增加服务器之间http/socket通讯支持
增加请求支持单向模式，不输出响应
增加ulong、uint、ushort的通讯参数类型
增加脚本加密发布，提供ScutSecurity类库支持
增加EventNotifier类管理异步与超时处理事件
增加数据库表名支持DateTime格式化生成规则
增加数据库字段支持blob类型存储
增加Model字段支持ulong,ushort,uint类型

修改优化Session与User对象
修改Redis多个Key时加载数据的问题
```
old version [more](http://scutgame.com/log).


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
```