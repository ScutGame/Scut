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
#include <assert.h>
#include "Trace.h"
#include "ScutLuaLan.h"
#include "FileHelper.h"
#include "PathUtility.h"
#include "PlatformProcedure.h"
#include "LuaIni.h"
#ifndef FILE_SEP
#ifdef SCUT_WIN32
#define FILE_SEP '\\'
#else
#define FILE_SEP '/'
#endif
#endif

namespace ScutUtility
{
	CScutLuaLan* CScutLuaLan::m_instance = NULL;

	CScutLuaLan* ScutUtility::CScutLuaLan::instance()
	{
		if (NULL == m_instance)
		{
			m_instance = new CScutLuaLan();
		}

		return m_instance;
	}

	bool CScutLuaLan::Add(const char* pszLan, const char* pszPath)
	{
		std::string path = LANPATH_DIR;
		path += FILE_SEP;
		path += pszPath;
		path = ScutDataLogic::CFileHelper::getWritablePath(path.c_str());

		if (!ScutSystem::CPathUtility::IsFileExists(path.c_str()))
		{
			std::string dir = CPlatformProcedure::ProcessDir(LANPATH_DIR, pszPath);
			if (!dir.empty())
			{
				if (ScutSystem::CPathUtility::IsFileExists(dir.c_str()))
				{
					path = dir;
				}else
				{
					ScutLog("can not open file: %s", dir.c_str());
					return false;
				}
			}else
			{
				ScutLog("can not open file: %s", path.c_str());
				return false;
			}
		}

		LANINFO_MAP::iterator itfind = m_mapLanInfo.find(std::string(pszLan));
		bool bSame = false;
		if (itfind != m_mapLanInfo.end())
		{
			bSame = (itfind->second->strName == path);

			if (!bSame)
			{
				Remove(pszLan);
			}
		}

		if (!bSame)
		{
			add_node(pszLan, path.c_str());
		}


		return true;
	}

	void CScutLuaLan::Remove(const char* pszLan)
	{
		LANINFO_MAP::iterator it = m_mapLanInfo.find(std::string(pszLan));

		if (m_mapLanInfo.end() == it)
		{
			return;
		}

		if (it->second != NULL)
		{
			if (it->second->pIni != NULL)
			{
				delete it->second->pIni;
			}
			delete it->second;
		}

		m_mapLanInfo.erase(it);

		if (m_curLan.compare(pszLan) == 0)
		{
			m_curLan = "";
		}

		return;
	}

	void CScutLuaLan::RemoveAll()
	{
		for (LANINFO_MAP::iterator it=m_mapLanInfo.begin(); it!=m_mapLanInfo.end(); ++it)
		{
			if (it->second != NULL)
			{
				if (it->second->pIni != NULL)
				{
					delete it->second->pIni;
				}
				delete it->second;
			}
		}

		m_mapLanInfo.clear();
		m_curLan = "";
	}

	bool CScutLuaLan::Switch(const char* pszLan)
	{
		LANINFO_MAP::iterator cur = m_mapLanInfo.find(std::string(pszLan));		
		if (cur == m_mapLanInfo.end())
		{
			return false;
		}

		if (!m_curLan.empty())
		{
			if (m_curLan.compare(pszLan) == 0)
			{
				return true;
			}

			release_ini(m_curLan.c_str());
		}

		m_curLan = pszLan;

		return true;
	}

	const char* CScutLuaLan::Get(const char *group, const char *key)
	{
		if (m_curLan.empty())
		{
			ScutLog("current language is empty");
			return "";
		}

		LANINFO_MAP::iterator it = m_mapLanInfo.find(m_curLan);

		if (it == m_mapLanInfo.end())
		{
			ScutLog("cann't find language %s", m_curLan.c_str());
			return "";
		}

		assert(it->second != NULL);
		if (NULL == it->second)
		{
			return "";
		}

		if (NULL == it->second->pIni)
		{
			load_ini(m_curLan.c_str());
		}

		return it->second->pIni->Get(group, key, "");
	}

	void CScutLuaLan::add_node(const char* pszLan, const char* pszPath)
	{
		LAN_INFO* pInfo = new LAN_INFO();
		pInfo->strName = pszPath;
		pInfo->pIni = NULL;
		m_mapLanInfo.insert(LANINFO_PAIR(std::string(pszLan), pInfo));

		if (m_curLan.empty())
		{
			m_curLan = pszLan;
		}
	}

	void CScutLuaLan::release_ini(const char* pszLan)
	{
		LANINFO_MAP::iterator it = m_mapLanInfo.find(std::string(pszLan));

		if (it != m_mapLanInfo.end() && it->second!=NULL && it->second->pIni!=NULL)
		{
			delete it->second->pIni;
			it->second->pIni = NULL;
		}
	}

	bool CScutLuaLan::load_ini(const char* pszLan)
	{
		LANINFO_MAP::iterator it = m_mapLanInfo.find(std::string(pszLan));
		
		assert(it!=m_mapLanInfo.end() && it->second!=NULL);
		if (it==m_mapLanInfo.end() || it->second==NULL)
		{
			return false;
		}

		LAN_INFO* pinfo = it->second;
		if (pinfo->pIni == NULL)
		{
			pinfo->pIni = new ScutDataLogic::CLuaIni;
			
			return pinfo->pIni->APLoad(pinfo->strName.c_str());
		}

		return true;
	}


	std::string CYBTest::test(const char* lan)
	{
		CScutLuaLan::instance()->Add("chinese", "chinese.ini");
		CScutLuaLan::instance()->Add("english", "english.ini");
		CScutLuaLan::instance()->Switch(lan);
		return CScutLuaLan::instance()->Get("bmp", "test");
	}

}