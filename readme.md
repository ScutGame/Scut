# README

[![Build Status](http://scutgame.com/images/passing.png?branch=6.7.9.11)](http://scutgame.com/download/)


## What's Scut?

Scut is a free, open source, Stable game server framework, which support 
C#/Python/Lua script. Scut's design philosophy is to "Developing online game easier", 
It includes a development framework and no-sql/database storage services, 
and many game system modules. Save a lot of game developers working hours, 
it allows the user to focus on business logic. Scut Game Server Framework 
also provide ScutSDK(eg. for Cocos2d-x) which make communications between 
server and client very easy.


## Documentations

* Wiki: [https://github.com/ScutGame/Scut/wiki](https://github.com/ScutGame/Scut/wiki)
* Online API: [http://scutgame.com/doc/](http://scutgame.com/doc/)


## Resource

* HomePage: http://www.scutgame.com

* Github: https://github.com/scutgame/Scut.git

* Oschina: https://git.oschina.net/scutgame/Scut.git

* QQ Group: 138266675


## Git for developers

    $ git clone git://github.com/ScutGame/Scut.git
    or
    $ git clone https://git.oschina.net/scutgame/Scut.git


## Git for samples

If the Chinese downloaded using [oschina git](https://git.oschina.net/scutgame/Scut-samples),
else using [github](https://github.com/ScutGame/Scut-samples)

    $ git clone git://github.com/ScutGame/Scut-samples.git
    or
    $ git clone https://git.oschina.net/scutgame/Scut-samples.git


## Git for Cocos2d-x SDK Source

    $ git clone git://github.com/ScutGame/Client-source.git
    or
    $ git clone https://git.oschina.net/scutgame/Client-source.git

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


### Protocols: Http/ WebSocket / Socket

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

### Version: 6.7.9.11 (2015-12-17) Beta

* Add Redis & DB sync queue profile log.
* Fixed Sql command bug.
* Fixed socket send bug.



old version [more](http://scutgame.com/log).


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