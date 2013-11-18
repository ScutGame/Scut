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



#include "LuaIni.h"
#include <string>
#include "Trace.h"
#include "FileHelper.h"
#include "Utility.h"
namespace ScutDataLogic
{
	CLuaIni::CLuaIni(void)
	{
		m_pIni = new CIniFile();
	}

	CLuaIni::~CLuaIni(void)
	{
		if (m_pIni)
		{
			delete m_pIni;
			m_pIni = NULL;
		}
	}

	std::string CLuaIni::initSavePath(const char*szFileName)
	{
		return CFileHelper::getWritablePath(szFileName);
	}
	bool CLuaIni::Load(const char *filename)
	{
		if (filename == NULL)
		{
			return false;
		}
		if (m_pIni->Load(initSavePath(filename).c_str()) != 0)
		{
			ScutLog("Load Ini file Error %s\r\n", initSavePath(filename).c_str());
			return false;
		}		
		return true;
	}
	bool CLuaIni::APLoad(const char *filename)
	{
		if (filename == NULL)
		{
			return false;
		}
		if (m_pIni->Load(filename) != 0)
		{
			ScutLog("Load Ini file Error %s\r\n", filename);
			return false;
		}		
		return true;
	}
	bool CLuaIni::Save(const char *filename)
	{
		if (filename == NULL)
		{
			return false;
		}
		if (m_pIni->Save(initSavePath(filename).c_str()) != 0)
		{
			ScutLog("Save Ini file Error %s", initSavePath(filename).c_str());
			return false;
		}

		return true;
	}

	const char *CLuaIni::Get(const char *key, const char *value, const char *def)
	{
		return m_pIni->Get(key, value, def);
	}
	int CLuaIni::GetInt(const char *key, const char *value, const char *def)
	{
		return m_pIni->GetInt(key, value, def);
	}

	void CLuaIni::Set(const char *key, const char *value, const char *set)
	{
		m_pIni->Set(key, value, set);
	}
	void CLuaIni::SetInt(const char *key, const char *value, int nValue)
	{
		m_pIni->SetInt(key, value, nValue);
	}
}
