/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

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
****************************************************************************/


#pragma once
#ifndef _SCUT_IPLUGININFO_H_
#define _SCUT_IPLUGININFO_H_

#include <string>

using namespace std;

namespace ScutDataLogic
{
	enum EVisitType
	{
		vtOnce,				//一次调用
		vtEveryFrame,		//每帧调用
	};

	class IPluginInfo
	{
	public:
		IPluginInfo(void);
		virtual ~IPluginInfo(void);
	public:
		//返回插件名称
		virtual string GetPluginName() = 0;
		//返回插件描述
		virtual string GetPluginDescription() = 0;
		//插件返回类别
		virtual EVisitType GetVisitType() = 0;

		virtual void Visit(){};
	};
}

#endif
