Scut开源游戏服务器框架
=================


简介
----------------
游戏服务器框架是使用C#语言开发，特别适用于手机网络游戏，支持使用Python脚本进行游戏开发；可以支持多种数据库：MS SQL Server、Mysql（目前只支持MS Sql Server）；支持HTTP/Socket协议同时接入（交叉接入）；采用数据库建模，使用导出模板直接形成数据实体类对象，无须构造数据库读写库SQL语句，极大简化数据库设计和编码工作；业务逻辑层使用Python脚本开发，实现业务不停服更新，并且降低对开发人员的开发难度；同时提供了丰富的类库和API接口。

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


资源
----------------
博客园地址：http://www.cnblogs.com/scut/

技术交流QQ群：138266675


更新日志
----------------
###版本：5.2.3.2 (2013-10-18) 不稳定
> 1. 增加通过定义实体类生成数据库的表结构；
> 2. 优化获取或设置实体类属性的反射方法；
> 3. 增加静态注入AOP，简化实体类属性的写法；


###版本：5.1.3.1 (2013-10-14)
> 1.增加表结构检测,增加Remote通道
> 2.分发器dll引用调整
> 3.合并分发器功能


###版本：5.1.3.0 (2013-9-27)
> 1. 优化Socket
> 2. 内部传送不解包
> 3. 增加请求超时响应
> 4. 增加Socket分发器
> 5. Wcf通道改为Socket通道


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