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
#include <map>
#include <string>
#include "LuaIni.h"

#define LANPATH_DIR "lan"

namespace ScutUtility
{
	/**
	* @brief 多语言支持
	* @remarks 通过加载不同ini文件达成多语言的支持
	*/
	class CScutLuaLan
	{
	private:
		CScutLuaLan(){}
		~CScutLuaLan(){}

	private:
		typedef struct LAN_INFO{
			std::string strName;
			ScutDataLogic::CLuaIni* pIni;
		}LAN_INFO;

	public:
		static CScutLuaLan* instance();
		bool Add(const char* pszLan, const char* pszPath);
		bool Switch(const char* pszLan);
		void Remove(const char* pszLan);
		const char* Get(const char *group, const char *key);
		void RemoveAll();

	private:
		void add_node(const char* pszLan, const char* pszPath);
		void release_ini(const char* pszLan);
		bool load_ini(const char* pszLan);
	private:
		typedef std::map<std::string, LAN_INFO*> LANINFO_MAP;
		typedef std::pair<std::string, LAN_INFO*> LANINFO_PAIR;

		static CScutLuaLan* m_instance;
		LANINFO_MAP m_mapLanInfo;
		std::string m_curLan;
	};

	class CYBTest
	{
	public:
		CYBTest(){}
		~CYBTest(){}

	public:
		static std::string test(const char* lan);
	};
}