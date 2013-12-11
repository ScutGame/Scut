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
#include "stdafx.h"
#include <string.h>
#include "TcpClient.h"
#include <curl/curl.h>
//#include <curl/types.h>
#include "INetStatusNotify.h"
#include <vector>
#include "TcpSceneManager.h"
#include "ZipUtils.h"
#include "AutoGuard.h"
#include "Trace.h"
#include "LuaHelper.h"
#ifdef SCUT_ANDROID
#include <stdio.h> 
#include <sys/types.h> 
#include <sys/param.h> 
#include <sys/ioctl.h> 
#include <sys/socket.h> 
#include <net/if.h> 
#include <netinet/in.h> 
#include <net/if_arp.h> 
#include <arpa/inet.h>
#endif

#if defined(SCUT_IPHONE) || defined(SCUT_MAC)
#include <sys/sockio.h> 
#endif 

#ifdef SCUT_UPHONE
#include "ssAppMgr.h"
#include "ssTsd.h"
#include "ssMsgQueue.h"
#elif SCUT_ANDROID
#include <unistd.h>
#include <pthread.h>
#endif

using namespace ScutNetwork;
using namespace ScutDataLogic;
using namespace std;

static int	s_nSocket = 0;
CThreadMutex m_sSendThreadMutex;
class TcpCurlHandlePool
{
public:
	static TcpCurlHandlePool* Instance()
	{
		static TcpCurlHandlePool g_Instance;
		return &g_Instance;
	}
	~TcpCurlHandlePool()
	{
		for (unsigned int i = 0; i < m_IdleList.size(); i++)
		{
			curl_easy_cleanup(m_IdleList[i]);
		}
		m_IdleList.clear();
	}
	CURL* GetHandle()
	{
		if (ret == NULL)
		{
			ret = curl_easy_init();
		}
		return ret;
	}
	void FreeHandle(CURL* handle)
	{
		//m_IdleList.push_back(handle);
		if (handle)
		{
			curl_easy_cleanup(handle);
			if ((unsigned int)this->GetHandle() == (unsigned int)handle)
			{
				ret = NULL;
			}
		}		
	}
	TcpCurlHandlePool()
	{
		ret = NULL;
	}
private:
	vector<CURL*> m_IdleList;
	CURL* ret;
};



/*******************************************************************
* Function  : CSocketClient constructor
* Parameters: -
* Returns   : -
* Purpose   : Initialize the object
*******************************************************************/
CTcpClient::CTcpClient()
{
	curl_handle     = NULL;
	host[0]	        = '\0';
	m_sgThreadID = 0;
	m_bIsBusy = false;
	m_bUseProgressReport = false;
	m_bAsyncProcessing = false;
	m_nTimeOut = 30;		//默认30s超时
	m_pNetNotify = NULL;
	m_nSocket = 0;
}

/*******************************************************************
* Function  : CSocketClient desctructor
* Parameters: -
* Returns   : -
* Purpose   : Object cleanup
*******************************************************************/
CTcpClient::~CTcpClient()
{
	m_sAsyncInfo.Reset();
	//Reset();
	if (curl_handle != NULL && !m_bIsBusy)
	{
		//CurlHandlePool::Instance()->FreeHandle(curl_handle);
		//curl_handle = NULL;
	}
}

//for_recv:0-send; 1-recv
static int wait_on_socket(curl_socket_t sockfd, int for_recv, long timeout_ms)
{
	struct timeval tv;
	fd_set infd, outfd, errfd;
	int res;

	tv.tv_sec = timeout_ms / 1000;
	tv.tv_usec= (timeout_ms % 1000) * 1000;

	FD_ZERO(&infd);
	FD_ZERO(&outfd);
	FD_ZERO(&errfd);

	FD_SET(sockfd, &errfd); /* always check for error */ 

	if(for_recv)
	{
		FD_SET(sockfd, &infd);
	}
	else
	{
		FD_SET(sockfd, &outfd);
	}

	/* select() returns the number of signalled sockets or -1 */ 
	res = select(sockfd + 1, &infd, &outfd, &errfd, &tv);
	return res;
}


unsigned long ScutGetTickCount()
{
#ifdef SCUT_ANDROID
	struct timespec now;
	clock_gettime(CLOCK_MONOTONIC, &now);
	return now.tv_sec*1000000 + now.tv_nsec/1000;
#elif defined(SCUT_IPHONE) || defined(SCUT_MAC)
	return clock();
#else	
	return ::GetTickCount();
#endif // SCUT_ANDROID
}

void GZipUnZipSocketStream(CStream* pStm)
{
	if (pStm->GetSize() > 0)
	{
		int nPackageSize = pStm->GetSize() - 4;

		pStm->SetPosition(4);
		char* pszBuffer = new char[nPackageSize];
		pStm->ReadBuffer(pszBuffer, nPackageSize);
		char* pszOut;
		int nLen = ZipUtils::ccInflateMemory((unsigned char*)pszBuffer, nPackageSize, (unsigned char**)&pszOut);
		if (nLen > 0)
		{
			pStm->SetSize(0);
			pStm->WriteBuffer(pszOut, nLen);
			delete []pszOut;
		}		
		delete []pszBuffer;
	}	
}

/*******************************************************************
* Function  : DoGetInternal()
* Parameters: url - The URL to which an HTTP GET is to be done
*             resp - The response object which will store the HTTP
*                    response received from the website
* Returns   : TRUE - GET succeeded (could have any HTTP errors,
*                    including 404 errors)
*             FALSE - The GET failed most likely because a 
*                    connection coul not be established
* Purpose   : To do an HTTP GET
*******************************************************************/
int CTcpClient::DoGetInternal(const char *url, CHttpClientResponse &resp)
{
	AUTO_GUARD(m_sSendThreadMutex);

	CURLcode res;
	CScutString strProxy;
	CScutString strProxyAuth;
	size_t	ret_len;
	size_t	new_size;

	//与上次的连接不同
	if (s_nSocket == 0)
	{
		//Initialize();
		curl_handle = TcpCurlHandlePool::Instance()->GetHandle();
		curl_easy_setopt(curl_handle, CURLOPT_ENCODING, "gzip, deflate");

		resp.SetRequestUrl(url);

		GetUrlHost(url, host);

		curl_easy_setopt(curl_handle, CURLOPT_URL, host);
		curl_easy_setopt(curl_handle, CURLOPT_CONNECT_ONLY, 1L);

		//超时时间
		if (m_nTimeOut > 0)
		{
			curl_easy_setopt(curl_handle, CURLOPT_TIMEOUT, m_nTimeOut);
			curl_easy_setopt(curl_handle, CURLOPT_CONNECTTIMEOUT, m_nTimeOut);
		}

		//进度设置，异步才启用进度显示功能
		/*if (m_bUseProgressReport && m_bAsyncProcessing)
		{
			curl_easy_setopt(curl_handle, CURLOPT_NOPROGRESS, 0);
			curl_easy_setopt(curl_handle, CURLOPT_PROGRESSFUNCTION, ProgressReportProc);
			curl_easy_setopt(curl_handle, CURLOPT_PROGRESSDATA, this);
		}*/	

		res = curl_easy_perform(curl_handle);
		if(res != CURLE_OK){ 
			return ScutNetwork::aisFailed; 
		} 
	}
	else
	{
		curl_handle = TcpCurlHandlePool::Instance()->GetHandle();
	}

	res = curl_easy_getinfo(curl_handle, CURLINFO_LASTSOCKET, &s_nSocket); 

	if(res != CURLE_OK){ 
		return ScutNetwork::aisFailed; 
	} 

	unsigned int hasSendDataLength = 0;
	if (resp.GetSendDataLength() > 4)
	{
		do 
		{			
			res = curl_easy_send(curl_handle, resp.GetSendData() + hasSendDataLength, resp.GetSendDataLength() - hasSendDataLength, &ret_len); 
			//ScutLog("AsyncExecTcpRequest nTag:%d", 11);
			this->m_bIsBusy = false;
			if(res != CURLE_OK){ 
				ScutLog("CTcpClient::DoGetInternal curl_easy_send failed");
				CTcpSceneManager::getInstance()->release();
				FreeCurlHandler(curl_handle);
				return ScutNetwork::aisTimeOut; 
			}
			hasSendDataLength += ret_len;
		} while (ret_len < resp.GetSendDataLength());
	}	
	else
	{
		return ScutNetwork::aisFailed; 
	}
	new_size = 0; 

//#define MAXINTERFACES 16 
//#define RECV_BUF_SIZE 6144
//
//	char temp[RECV_BUF_SIZE] = {0};	
//	bool hasReceived = false;
//	unsigned long ticks = ScutGetTickCount();
//	for(;;){ 
//		size_t recv_len = 0;
//		memset(&temp[0], RECV_BUF_SIZE, sizeof(temp[0]));
//
//		res = curl_easy_recv(curl_handle, temp, RECV_BUF_SIZE, &recv_len); 
//
//		if (res == CURLE_AGAIN)
//		{
//			if (!hasReceived)
//			{
//				//由于curl库中的超时对socket支持不好，所以自己做了超时处理
//				if (m_nTimeOut != 0 && ScutGetTickCount() - ticks > ((unsigned int)m_nTimeOut) * 1000)
//				{
//					res = CURLE_OPERATION_TIMEDOUT;
//					break;
//				}
//				continue;
//			}
//			else
//			{
//				res = CURLE_OK;
//				break;
//			}
//		}
//		else if (res == CURLE_OK)
//		{
//			ticks = ScutGetTickCount();
//			hasReceived = true;
//
//			if (resp.GetTarget())
//			{
//				resp.GetTarget()->Seek(0, soend);
//				resp.GetTarget()->Write(temp, recv_len);
//				GZipUnZipSocketStream(resp.GetTarget());
//			}
//
//			if (recv_len == RECV_BUF_SIZE)
//			{
//				continue;
//			}
//		}
//		
//		break;
//	} 

	if (!CTcpSceneManager::getInstance()->isListening())
	{
		CTcpSceneManager::getInstance()->setUrlHandle(curl_handle);
		CTcpSceneManager::getInstance()->startListening();
	}
	
	return 0;
}

/*******************************************************************
* Function  : Reset()
* Parameters: -
* Returns   : -
* Purpose   : To partially cleanup the object so that it can be
*             reused for more requests. Note that the proxy settings
*             are not reset.
*******************************************************************/
void CTcpClient::Reset()
{
	m_bAsyncProcessing = false;
	host[0]	= '\0';

	if (curl_handle == NULL)
	{
		//Initialize();
		//curl_handle = TcpCurlHandlePool::Instance()->GetHandle();
		//curl_easy_setopt(curl_handle, CURLOPT_ENCODING, "gzip, deflate");
	}
}

/*******************************************************************
* Function  : GetHost()
* Parameters: host - The object to which the host value is to be
*                    copied
* Returns   : The host value via a parameter
* Purpose   : To retrieve the host value. This is required because
*             HTTP redirects could have resulted in the host to 
*             change to a new one. All future requests will have to
*             be given to the new host only.
*******************************************************************/
CScutString CTcpClient::GetHost()
{
	return this->host;
}

unsigned short CTcpClient::GetPort()
{
	return this->port;
}


/*******************************************************************
* Function  : SplitUrl()
* Parameters: url - The URL to be split into its components
*             host - The web server host name
* Returns   : 0 - Successful split
*             ERROR_UNSUPPORTED_PROTOCOL - Unknown protocol
* Purpose   : To split the URL to its basic components so that
*             we know which website to connect to and what to fetch
*             from there.
*******************************************************************/
int CTcpClient::GetUrlHost(const char *url, char *host)
{
	int startPos = 0;
	const char *ptr;

	// Check protocol
	if (strstr(url, "http") == NULL)
	{
		//if ((ptr = strchr(url + startPos, ':')) != NULL)
		//{
		//	strncpy(host, url, ptr - url);
		//	host[ptr - url] = '\0';

		//	port = (unsigned short)atoi(++ptr);

		//	return 0;
		//}

		if ((ptr = strchr(url + startPos, ':')) != NULL)
		{
			strcpy(host, url);
			host[strlen(url)] = '\0';

			port = (unsigned short)atoi(++ptr);

			return 0;
		}
	}

	// Unsupported protocol
	return ERROR_UNSUPPORTED_PROTOCOL;
}


/*******************************************************************
* Function  : FullReset()
* Parameters: -
* Returns   : -
* Purpose   : To fully cleanup the object so that it can be
*             reused for more requests. Note that all CURL
*			  properties are reset, including the proxy settings.
*******************************************************************/
void CTcpClient::FullReset()
{
 	Reset();
}

void CTcpClient::Initialize()
{
	host[0]			= '\0';

	// init the curl session 
	//curl_handle = curl_easy_init();
	curl_handle = TcpCurlHandlePool::Instance()->GetHandle();
}

int ScutNetwork::CTcpClient::TcpGet(const char* host, const char* port, CHttpClientResponse &resp)
{
	return TcpGet((CScutString(host) + CScutString(port)).c_str(), resp);
}

int ScutNetwork::CTcpClient::TcpGet(const char* url, CHttpClientResponse &resp)
{
	int nRet = 1;

	for(int i = 0; i < HTTP_MAX_RETRIES; i++)
	{
		if ((nRet = DoGetInternal(url, resp)) == 0)
		{
		}
		//FullReset();
/*#ifdef SCUT_WIN32
		Sleep(1000);
#elif SCUT_ANDROID
		usleep(1000);
#else
		usleep(1000);
#endif	*/	
	}

	return nRet;
}

int ScutNetwork::CTcpClient::AsyncNetGet(const char* url, CHttpClientResponse* resp)
{
	return AsyncTcpGet(url, resp);
}

int ScutNetwork::CTcpClient::AsyncTcpGet(const char* url, CHttpClientResponse* resp)
{
	if (m_bIsBusy)
	{
		return -1;
	}
	m_sAsyncInfo.Reset();
	m_sAsyncInfo.Url = url;
	m_sAsyncInfo.Response = resp;
	m_sAsyncInfo.Sender = this;

#ifdef SCUT_WIN32
	return (int)CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AsyncThreadProc, &m_sAsyncInfo, 0, (LPDWORD)(&m_sgThreadID));
#else
	return (int)pthread_create((pthread_t*)&m_sgThreadID, NULL, AsyncThreadProc, &m_sAsyncInfo);
#endif
}

int ScutNetwork::CTcpClient::AsyncTcpGet(const char* host, const char* port, CHttpClientResponse* resp)
{
	CScutString url = CScutString(host) + ":" + CScutString(port);

	return AsyncTcpGet(url.c_str(), resp);
}


#ifdef SCUT_WIN32
DWORD WINAPI ScutNetwork::CTcpClient::AsyncThreadProc(LPVOID puCmd)
#else
void* ScutNetwork::CTcpClient::AsyncThreadProc( void * puCmd )
#endif
{	
	int nRet = -1;
	if (puCmd)
	{
		PAsyncInfo pAi = (PAsyncInfo)puCmd;
		if (!pAi->Sender) {
			return NULL;
		}
		CTcpClient *pSender = (CTcpClient*)pAi->Sender;
		if (!pSender)
		{
			return NULL;
		}
		pSender->m_bAsyncProcessing = true;
		pSender->m_bIsBusy = true;

		nRet = pSender->TcpGet(pAi->Url, *(pAi->Response));
		pSender->m_bAsyncProcessing = false;
		pSender->m_bIsBusy = false;
		delete pSender;
		//switch ((CURLcode)nRet)
		//{
		//case CURLE_OK:
		//	pAi->Status = aisSucceed;
		//	break;
		//case CURLE_OPERATION_TIMEDOUT:
		//	pAi->Status = aisTimeOut;
		//	break;
		//default:
		//	pAi->Status = aisFailed;
		//	break;
		//}
		//pSender->m_bAsyncProcessing = false;
		//pSender->m_bIsBusy = false;
		////通知				
		//if (pSender->m_pNetNotify)
		//{
		//	pSender->m_pNetNotify->OnNotify(pAi);
		//}				
	}	
	return NULL;
}

int ScutNetwork::CTcpClient::ProgressReportProc( void* ptr, double dlTotal, double dlNow, double ulTotal, double ulNow )
{
	if (ptr)
	{
		return ((CTcpClient*)ptr)->DoProgressReport(dlTotal, dlNow, ulTotal, ulNow);
	}
	return -1;	
}

int ScutNetwork::CTcpClient::DoProgressReport( double dlTotal, double dlNow, double ulTotal, double ulNow )
{
	if (m_bUseProgressReport && m_bAsyncProcessing)
	{
		//通知线程或窗体
#ifdef SCUT_UPHONE
		if(!SS_GTID_IS_INVALID(m_sAsyncInfo.NotifyThread) || m_sAsyncInfo.NotifyhWnd != 0)
		{
			m_sAsyncInfo.Status = aisProgress;
			m_sAsyncInfo.Data1 = (dlTotal == 0) ? 0 : (int)(dlNow / dlTotal * 10000);
			int nRet = App_PostMessage(&m_sAsyncInfo.NotifyThread, MSG_HTTPCLIENT_ASYNCINFO, &m_sAsyncInfo, sizeof(AsyncInfo), m_sAsyncInfo.NotifyhWnd);
			if (nRet <= 0)
			{
				return -1;
			}
		}
#else
		m_sAsyncInfo.Status = aisProgress;
		int nRawSize = m_sAsyncInfo.Response->GetTargetRawSize();
		m_sAsyncInfo.Data1 = (dlTotal == 0) ? 0 : (int)((dlNow + nRawSize) / (dlTotal + nRawSize) * 10000);

		if (isLuaHandleExist())
		{
			LuaHelper::Instance()->execFunc(m_strLuaHandle, &m_sAsyncInfo);
		}
		else
		{
			if (m_pNetNotify)
			{
				m_pNetNotify->OnNotify(&m_sAsyncInfo);
			}
		}
#endif
	}	
	return 0;
}

int ScutNetwork::CTcpClient::wait_on_socket(int sockfd, int for_recv, long timeout_ms)
{
	struct timeval tv;
	fd_set infd, outfd, errfd;
	int res;

	tv.tv_sec = timeout_ms / 1000;
	tv.tv_usec= (timeout_ms % 1000) * 1000;

	FD_ZERO(&infd);
	FD_ZERO(&outfd);
	FD_ZERO(&errfd);

	FD_SET(sockfd, &errfd); /* always check for error */

	if(for_recv){
		FD_SET(sockfd, &infd);
	}else{
		FD_SET(sockfd, &outfd);
	}

	/* select() returns the number of signalled sockets or -1 */
	res = select(sockfd + 1, &infd, &outfd, &errfd, &tv);
	if (res > 0)
	{
		if(FD_ISSET(sockfd, &errfd))
			res = -1;
	}
	return res;
}

void ScutNetwork::CTcpClient::FreeCurlHandler(CURL* curl_handle)
{
	AUTO_GUARD(m_sSendThreadMutex);
	s_nSocket = 0;
	if (curl_handle)
	{
		TcpCurlHandlePool::Instance()->FreeHandle(curl_handle);
	}
}
