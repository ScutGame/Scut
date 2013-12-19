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
#include "ScutLocale.h"
#include <assert.h>

#ifdef SCUT_ANDROID
#include "android/ScutUtilityJni.h"
#endif
#ifdef SCUT_IPHONE
#include "iphone/IphoneInterface.h"
#endif

namespace ScutUtility
{
	static std::string s_strSysLanguage = "";
	static std::string s_Imsi = "";
	static std::string s_Imei = "";

	CLocale::CLocale(void)
	{
	}

	CLocale::~CLocale(void)
	{
	}
	void CLocale::setLanguage(const char* pszLanguage)
	{
		//根据不同平台传进来的参数做一层转换
		
		if (pszLanguage)
		{
#ifdef SCUT_ANDROID
			if (strcmp(pszLanguage, "CN") == 0)
			{
				s_strSysLanguage = "zh_CN";
			}
			else if (strcmp(pszLanguage, "TW") == 0)
			{
				s_strSysLanguage = "zh_TW";
			}
			else if(strcmp(pszLanguage, "JP")== 0)
			{
				s_strSysLanguage = "ja_JP";
			}
			else
			{
				s_strSysLanguage = "en_US";
			}
#endif

#ifdef SCUT_IPHONE
		if (strcmp(pszLanguage, "ja") == 0)
		{
			s_strSysLanguage = "ja_JP";
		}
		else if (strcmp(pszLanguage, "zh-Hans") == 0)
		{
			//简体中文
			s_strSysLanguage = "zh_CN";
		}
		else if (strcmp(pszLanguage, "zh-Hant") == 0)
		{
			//繁体
			s_strSysLanguage = "zh_TW";
		}
		else
		{
			s_strSysLanguage = "en_US";
		}

#endif
#ifdef SCUT_WIN32
		s_strSysLanguage = pszLanguage;
#endif
		}
		else
		{
			assert(false);
		}
	}

	const char* CLocale::getLanguage()
	{
		if (s_strSysLanguage.size() == 0)
		{
#ifdef SCUT_IPHONE
			setLanguage(ScutUtility::getIphoneSysLanguage().c_str());
#endif

#ifdef SCUT_ANDROID
			char * p = getLanguageJNI();
			if (p)
			{
				setLanguage(p);
				free(p);
			}
#endif
#ifdef SCUT_WIN32
			if (s_strSysLanguage.size() == 0)
			{
				s_strSysLanguage = "zh_CN";
			}
#endif
		}

		return s_strSysLanguage.c_str();
	}

	bool CLocale::isSDCardExist()
	{
#ifdef SCUT_ANDROID
		return getSDCardStateJNI();
#else
		return false;
#endif
	}

	const char* CLocale::getImsi()
	{
		if (s_Imsi.size() != 0)
		{
			return s_Imsi.c_str();
		}
#ifdef SCUT_ANDROID
		char* p = getAndroidImsi();
		if (p)
		{
			s_Imsi = p;
			free(p);
		}
#endif

#ifdef SCUT_IPHONE
		s_Imsi = getIphoneImsi();
#endif

		return s_Imsi.c_str();
	}

	const char* CLocale::getImei()
	{
		if (s_Imei.size() != 0)
		{
			return s_Imei.c_str();
		}
#ifdef SCUT_ANDROID
		char* p = getAndroidImei();
		if (p)
		{
			s_Imei = p;
			free(p);
		}
#endif

#ifdef SCUT_IPHONE
		s_Imei = getIphoneImei();
#endif
		
		return s_Imei.c_str();
	}

	void CLocale::setImsi(const char* pszImsi)
	{
		if (pszImsi == NULL)
		{
			return;
		}
#ifdef SCUT_WIN32
		s_Imsi = pszImsi;
#endif
	}
}

