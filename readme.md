Scut开源游戏服务器引擎
=================


简介
----------------
Scut游戏服务器引擎使用C#语言开发，特别适用于手机网络游戏，支持使用C#/Python/Lua脚本进行游戏开发；支持MSSQL/Mysql数据库；支持HTTP/Socket协议同时接入；采用实体类对象建模，自动构造数据库表结构生成和修改语句；提供了丰富的类库和API接口，极大降低了开发人员的技术要求。

多种脚本支持
----------------
使用脚本开发可以支持热更新，目前支持的脚本包括如下：<br />
> CSharp脚本：运行时编译执行，性能高（推荐）<br />
> Python脚本<br />
> Lua脚本：从版本6.5.8.0开始支持<br />



游戏中间件
----------------
在游戏的开发过程中，积累了大量的例程，提供和开放大量的中间件：<br />
登录系统<br /> 
建角系统<br />
充值系统<br />
排行榜系统<br />
任务系统<br />
聊天系统<br />
邮件系统<br />
公告系统<br />
广播系统<br />
GM命令系统<br />
新手引导系统<br />
问答系统<br />
媒体礼包系统<br />
新手卡系统<br />
商城系统<br />
等成熟系统，只需简单使用脚本就可以马上快速完成相应系统功能。<br />

框架性能
----------------
在架构上，可以支持多服务器耦合架构，可以分离业务到不同服务器，以提升在线人数，如战斗服务器、聊天服务器分别部署至不同物理服务器；根据业务等级，实现不同等级的写库需求，默认前提下，数据更新将延迟10分钟写入数据库，提供预读机制，极大降低对数据库的依赖，哪怕数据库网络短期中断也不会形成脏读；
服务器内存和CPU占用低，对象在一定时间内（24小时）未登陆将会被交换至数据库，并从内存卸下，节省内存利用率（16核服务器，20个游服平均CPU低于10%，峰值低于30%）；

数据库支持
----------------
Windows：支持MSSQL/Mysql<br />
Linux：支持Mysql<br />
从5.5.3.5版开始，Scut游戏服务器引擎将Redis作为必备的存储，数据库是可选存储


资源
----------------
官网地址：http://www.scutgame.com<br />
Github代码库：https://github.com/scutgame/scut.git<br />
OSChina代码库：https://git.oschina.net/scutgame/Scut<br />
博客园地址：http://www.cnblogs.com/scut/<br />

技术交流QQ群：138266675


更新日志
----------------
###版本：6.5.8.0 (2014-5-30) Unstable
> 1. 增加Lua脚本语言支持
> 2. 增加游戏服务启动进度
> 3. 增加Server启动时检测网络是否可以访问
> 4. 修改Server正在关闭时Socket连接出错问题

###版本：6.3.7.5 (2014-5-9) Release
> 1. 增加客户端与服务端通讯支持自定义的消息结构，支持Unity3d对象接入
> 2. 增加Http操作工具类
> 3. 修改实体对象修改锁不能重入问题
> 4. 修改Action封包出异常时的下发Push类型包的问题


###版本：6.3.7.3 (2014-4-30) Release
> 1. 修改动态脚本使用实体继承的方式编译出错
> 2. 修改使用实体继承的方式不能Protobuf序列化问题
> 3. 修改加载旧版存储结构的Redis数据问题
> 4. 增加ScutSMS工具的帮助文档（含游戏服参数配置说明）


###版本：6.3.7.2 (2014-4-25) Release
> 1. 修改缓存更新与删除一起处理时未能删除数据问题
> 2. 修改实体数据在Redis中以Hash方式存储
> 3. 修改使用Redis自带的连接池管理类
> 4. 修改Redis内存回收表（Temp_EntityHistory）的Value列的存储超出范围问题，手动删除表引擎自动重建
> 5. 增加多个线程同步缓存变更数据到Redis和Db库，可配置开启线程数（默认2个）
> 6. 增加Entity类的ModifyLocked方法，多次修改属性时只会通知一次变更事件，降低缓存数据同步的次数
> 7. 增加CacheStruct类LoadingStatus属性，在缓存中查找Key时是对象为空，还是由于从Redis取数据异常导致为空


###版本：6.2.7.0 (2014-4-4) Release
> 1. 升级底层类库到.Net Framework 4.5.1版本
> 2. 增加Action=2与服务端Socket断开接口
> 3. 修改更新Python脚本时ReferenceLib.py文件被占用问题


###版本：6.1.6.5 (2014-3-14) Release
> 1. 增加CSharp脚本不需要验证访问Action设置
> 2. 增加脚本引擎可以执行py/C# Source脚本片段
> 3. 修改Change Key写入Redis有丢失的问题
> 4. 修改Changed Eventg事件通知使用异步调用


###版本：6.1.6.2 (2014-3-7) Release
> 1. 增加输入Log文件可划分目录
> 2. 修改GameSession初始加载Redis数据出错问题
> 3. 修改Language使用非脚本无法配置Type问题
> 4. 修改渠道不能登录问题
> 5. 修改屏蔽词中间件取不到数据问题
> 6. 修改使用Timer与Task开启线程crash掉的问题

###版本：6.1.6.0 (2014-2-28) Release
> 1. 增加Session停服恢复功能
> 2. 增加Server Console打印logo字符画信息
> 3. 修改Server Console回车无法退出问题
> 4. 修改强制停服写入Redis的数据丢失问题

###版本：6.1.5.8 (2014-2-19) Release
> 1. 增加通讯协议写入流支持可Protobuf序列化的对象
> 2. 增加Action通讯流可支持Gzip压缩
> 3. 修改语言包为空异常的问题
> 4. 修改Python脚本不能调用CSharp脚本问题
> 5. 修改Socket Push消息的调用方式
> 6. 修改生成Sql表结构语句BUG
> 7. 修改渠道登录中间件不能正常登录问题

###版本：6.1.5.6 (2014-1-26) Release
> 1. 增加exe版(console），web版本(IIS)的游戏服宿主程序
> 2. 增加Model支持脚本化，实现不停服更新
> 3. 增加Language支持脚本化
> 4. 修改Sns与Pay Center组件的数据库连接字符串
> 5. 修改输出Log异常信息重复问题
> 6. 修改消息队列写MSSQL数据库时SqlParamter被占用问题


###版本：6.1.5.5 (2014-1-10) Release
> 1. 增加对对象属性的原子操作方法
> 2. 修改全局缓存序列化时存储到Redis内存溢出问题
> 3. 修改Protobuf不能序列化私有成员的问题


###版本：6.1.5.3 (2013-12-31) Release
> 1. 增加游戏运行环境配置类
> 2. 增加Sync模型支持，简化部分数据通讯协议
> 3. 修改数据库为可选配置
> 4. 修改生成MySql语句含有关键词问题


###版本：6.0.5.2 (2013-12-5) Release
> 1. 增加C#脚本中能引用多个C#脚本文件的支持
> 2. 修正Web应用程序中使用C#脚本解析不到Bin目录的问题


###版本：6.0.5.1 (2013-12-4) Release
> 1. 修正缓存删除时不会更新到Redis的问题
> 2. 修正Model组合3个以上子类时Change事件未绑定的问题
> 3. 修正中间层MySql与MsSql数据库Sql语句分页问题


###版本：6.0.5.0 (2013-11-29) Release
> 1. 增加C#脚本支持
> 2. 增加Pay和Sns中间件对Mysql数据库支持
> 3. 精简布署步骤，取消Redis写入程序，将其移到游戏底层运行
> 4. 修正Mysql对中文可能会出现乱码的BUG


###版本：5.6.3.5 (2013-11-25) Release
> 1. 优化实体ChangeKey队列，减少写库IO（默认为5分钟写入一次数据库）
> 2. 优化Protobuf序列化启用自动GZip压缩，减少Redis内存消耗
> 3. 修正MySql操作命令的Bug

###版本：5.5.3.5 (2013-11-12) Release
> 1. 增加对Linux平台的支持
> 2. 去掉对MSMQ消息队列的依赖
> 3. 从这个版本开始，Scut游戏服务器引擎将Redis作为必备的存储，数据库是可选存储
> 4. 增加Redis过期的缓存自动加载功能
> 5. 日志数据库以年月划分表

###版本：5.3.3.3 (2013-10-26) Release
> 1. 支持Mysql数据库
> 2. 简化DLL数量
> 3. 简化配置
> 3. 简化GM命令，并分离配置


###版本：5.2.3.2 (2013-10-18) Release
> 1. 增加通过定义实体类生成数据库的表结构
> 2. 优化获取或设置实体类属性的反射方法
> 3. 增加静态注入AOP，简化实体类属性的写法



What's Scut Game Server Framework？
=================
Scut is a free, open source, stable game server framework, which support C#/Python script. Scut's design philosophy is to "Developing online game easier"，It includes a development framework and no-sql/database storage services, and many game system modules. Save a lot of game developers working hours，it allows the user to focus on business logic. Scut Game Server Framework also provide ScutSDK(eg. for Cocos2d-x) which make communications between server and client very easy.

Benefits Features
----------------
###Platform
<ul>
<li>Windows
<li>Linux
</ul>

###Data Persistence (DBs)
<ul>
<li>SQL: MySQL/MS SQL Server
<li>NoSQL: Redis
</ul>

###Protocols: http/socket
<ul>
<li>Transfer protocol is very lean and slim
<li>Scut wraps up the networking layer of each client platform
<li>Communicate cross-platform and cross-protocol
<li>Code generate automaticly: forget about de-/serialization
</ul>

###Server Dev Framework
> Language: C#/Python


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
Github: https://github.com/scutgame/scut<br />
QQ Group: 138266675<br />



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