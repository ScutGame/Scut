What's Scut Game Server Framework？
=================
Scut is a free, open source, Stable game server framework, which support C#/Python script. Scut's design philosophy is to "Developing online game easier"，It includes a development framework and no-sql/database storage services, and many game system modules. Save a lot of game developers working hours，it allows the user to focus on business logic. Scut Game Server Framework also provide ScutSDK(eg. for Cocos2d-x) which make communications between server and client very easy.

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
> Language: C#/Python/Lua


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


Resource
----------------
HomePage: http://www.scutgame.com<br />
Github: https://github.com/scutgame<br />
QQ Group: 138266675<br />


Update Log
----------------
###Version: 6.5.8.6 (2014-7-18) Stable
> 1. 增加从Redis中加载数据到Cache可设置筛选条件
> 2. 修改在Web项目中的不能支持自定协议问题
> 3. 修改Share类型的Model在Redis中为空时会尝试从DB中加载数据
> 4. 修改Model命名空间包含下划线字符时在Redis中存取数据错误问题

###Old Version
>http://scutgame.com/log/


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