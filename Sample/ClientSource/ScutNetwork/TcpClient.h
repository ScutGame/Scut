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
#ifndef _SOCKET_CLIENT_H_
#define _SOCKET_CLIENT_H_

#include "curl.h"
//#include "types.h"
#include "easy.h"

//#include "Defines.h"
#include "HttpClientResponse.h"
#include "HttpSession.h"
#include "ScutString.h"	
//#include "ScutSocket.h"
#include "INetStatusNotify.h"
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
	class CTcpClient : public CNetClientBase
	{
	public:
		CTcpClient();
		virtual ~CTcpClient();		

		int TcpGet(const char* host, const char* port, CHttpClientResponse &resp);
		int TcpGet(const char* url, CHttpClientResponse &resp);

		///////////////////////////////异步//////////////////////////

		//由于异步处理，因此resp参数必须一直有效
		int AsyncTcpGet(const char* host, const char* port, CHttpClientResponse* resp);
		int AsyncTcpGet(const char* url, CHttpClientResponse* resp);

		virtual int AsyncNetGet(const char* url, CHttpClientResponse* resp);

		//重设状态
		virtual void FullReset();
		virtual void Reset();
		CScutString GetHost();	
		unsigned short GetPort();

		static int GetUrlHost(const char *url, char *host);		

		static int wait_on_socket(int sockfd, int for_recv, long timeout_ms);

		static void FreeCurlHandler(CURL* curl_handle);
	private:
#ifdef SCUT_WIN32
		static DWORD WINAPI AsyncThreadProc(LPVOID puCmd);
#else
		static void* AsyncThreadProc(void * puCmd);
#endif		
		static int ProgressReportProc(void* ptr, double dlTotal, double dlNow, double ulTotal, double ulNow);
	private:
		virtual void Initialize();
		virtual int DoProgressReport(double dlTotal, double dlNow, double ulTotal, double ulNow);

		int DoGetInternal(const char *url, CHttpClientResponse &resp);
		int DoPostInternal(const char *url, const void * postData, int nPostDataSize, CHttpClientResponse &resp, bool formflag = false);
	private:
		static unsigned short	port;

		int		m_nSocket;
	};

}

#endif

