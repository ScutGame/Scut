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
#ifndef _HTTP_CLIENT_H_
#define _HTTP_CLIENT_H_

#include "curl.h"
//#include "types.h"
#include "easy.h"

//#include "Defines.h"
#include "HttpClientResponse.h"
#include "HttpSession.h"
#include "ScutString.h"	
#include "NetClientBase.h"

#ifdef SCUT_UPHONE
#include "ssGlobal.h"
#elif defined(SCUT_IPHONE) || defined(SCUT_MAC)
#include <iostream>
#include <pthread.h> 
#endif

using namespace ScutSystem;

#define ERROR_UNSUPPORTED_PROTOCOL -1
#define ERROR_SOCK_CREATION_FAILED -2
#define ERROR_CONNECT_FAILED       -3
#define HTTP_MAX_RETRIES            1

namespace ScutNetwork
{
	class INetStatusNotify;

	class CHttpClient : public CNetClientBase
	{
	public:
		CHttpClient(CHttpSession *s);
		virtual ~CHttpClient();		
	
		//同步Get与Post
		int HttpGet(const char *url, CHttpClientResponse &resp);		
		bool HttpPost(const char *url, const void * postData, int nPostDataSize, CHttpClientResponse &resp, bool formflag=false);

		///////////////////////////////异步Get与Post//////////////////////////

		//由于异步处理，因此resp参数必须一直有效
		int AsyncHttpGet(const char* url, CHttpClientResponse* resp);
		int AsyncHttpPost(const char* url, const void* postData, int nPostDataSize, CHttpClientResponse* resp, bool bFormFlag = false);
		
		virtual int AsyncNetGet(const char* url, CHttpClientResponse* resp);

		////异步情况下，需要设定网络状态通知接口
		//virtual void SetNetStautsNotify(INetStatusNotify* pNetNotify);

		ENetType GetNetType();

		//头信息与代理服务器设置
		virtual bool AddHeader(const char *name, const char *value);

		void UseHttpProxy(bool bUseProxy);
		void SetHttpProxy(const char *proxyHost, unsigned int proxyPort);
		void UseHttpsProxy(bool bUseProxy);
		void SetHttpsProxy(const char *proxyHost, unsigned int proxyPort);

		//重设状态
		virtual void FullReset();
		virtual void Reset();
		//CScutString	GetHost();	

		static int GetUrlHost(const char *url, char *host);		
	private:
#ifdef SCUT_WIN32
		static DWORD WINAPI AsyncThreadProc(LPVOID puCmd);
#else
		static void* AsyncThreadProc(void * puCmd);
#endif		
		static int ProgressReportProc(void* ptr, double dlTotal, double dlNow, double ulTotal, double ulNow);
	private:
		virtual void Initialize();
		int DoGetInternal(const char *url, CHttpClientResponse &resp);
		int DoPostInternal(const char *url, const void * postData, int nPostDataSize, CHttpClientResponse &resp, bool formflag = false);
		virtual int DoProgressReport(double dlTotal, double dlNow, double ulTotal, double ulNow);

		char* DuplicateStr(const char* lpszValue);
	private:
		bool			bUseHttpProxy;
		char			httpProxyHost[256];
		unsigned int	httpProxyPort;

		bool			bUseHttpsProxy;
		char			httpsProxyHost[256];
		unsigned int	httpsProxyPort;

		char			*pResponsePage;
		unsigned int	responsePageLen;
		struct curl_slist *headers;

		CHttpSession	*session;
		CScutString		referer;
	};

}

#endif

