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



#ifndef LUAINI_H
#define LUAINI_H
#include "iniEx.h"
#include <string>
namespace ScutDataLogic
{
	class CLuaIni
	{
	public:
		CLuaIni(void);
		~CLuaIni(void);

		//相对于资源目录的 完整的路径由里面组合
		bool Load(const char *filename);
		//使用完整路径加载
		bool APLoad(const char *filename);
		bool Save(const char *filename);

		const char *Get(const char *key, const char *value, const char *def = "error");
		int GetInt(const char *key, const char *value, const char *def = "0");

		void Set(const char *key, const char *value, const char *set);
		void SetInt(const char *key, const char *value, int nValue);
		


		
	private:

		std::string initSavePath(const char*szFileName);
		CIniFile* m_pIni;

	};
}


#endif//LUAINI_H