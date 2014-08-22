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


## Git for developers

    $ git clone git://github.com/ScutGame/Scut.git
    or
    $ git clone git://git.oschina.net/scutgame/Scut.git


## Git for samples

If the Chinese downloaded using [oschina Git](https://git.oschina.net/scutgame/Scut-samples),
else using [Github](https://github.com/ScutGame/Scut-samples)

    $ git clone git://github.com/ScutGame/Scut-samples.git
    or
    $ git clone git://git.oschina.net/scutgame/Scut-samples.git


## Update Log

### Version: 6.7.8.7 (2014-8-15) Stable

* Add server to server for Http/Socket communication support
* Add support for the one-way mode, not the output response
* Add ulong/uint/ushort of communication parameter types
* Add script encryption of release, use "ScutSecurity.dll" library
* Add use "EventNotifier" of class asynchronous and timeout processing events
* Add the name of the database table using date format
* Add database field supported BLOB types
* Add entity property supported ulong/ushort/uint types
* Modify reconstruction of Session and User class
* Modify entity definition multiple primary from the Redis loading data problem

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