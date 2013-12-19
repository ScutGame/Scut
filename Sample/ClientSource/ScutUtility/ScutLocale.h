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

#ifndef LOCALE_H
#define LOCALE_H
#include <string>
namespace ScutUtility
{
	class CLocale
	{
	private:
		CLocale(void);
		~CLocale(void);
	public:		
		static void setLanguage(const char* pszLanguage);
		//"zh_CN"  简体 
		//"zh_TW"  繁体
		//"ja_JP"  日语
		//"en_US"  英语 不是以上三种的都识别为英语 
		static const char* getLanguage();

		//dst 缓冲区需大于64字节
		//iPhone Appstore 需要包含SCUT_IPHONE_APPSTORE宏
		//iPhone 模拟器需要包含SCUT_IPHONE_SIMULATOR宏
		static const char* getImsi();
		static const char* getImei();
		//以下接口接适合于Win32测试用。
		static void setImsi(const char* pszImsi);
		static bool isSDCardExist();
	};
}

#endif//LOCALE_H