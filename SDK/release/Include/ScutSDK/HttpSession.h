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
#ifndef _HTTP_SESSION_H_
#define _HTTP_SESSION_H_

#ifdef SCUT_WIN32
//#include <winsock.h>
#else
#include "Defines.h"
#endif
#include "ScutString.h"	

using namespace ScutSystem;


namespace ScutNetwork
{
	class CHttpClient;
	class CHttpClientResponse;

	class CHttpSession
	{
	public:
		void DeleteCookies();
		CScutString cookieJar;
		void Initialize(const char *username);
		CScutString GetCookies(CHttpClient *req);
		void AddCookie(const char *value);

		CHttpSession();
		virtual ~CHttpSession();

	protected:
		bool bSessionMgmt;

		typedef struct cookie_t
		{
			char name[1024];
			char value[4096];
			struct cookie_t *next;
		} COOKIE;

		COOKIE *cookieHead;
	};
}


#endif

