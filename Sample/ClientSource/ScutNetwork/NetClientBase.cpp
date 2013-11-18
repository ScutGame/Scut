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
#include "HttpClient.h"
#include <curl/curl.h>
//#include <curl/types.h>
#include "INetStatusNotify.h"
#include <vector>
#include "ZipUtils.h"
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
using namespace std;
using namespace ScutDataLogic;


class CurlHandlePool
{
public:
	static CurlHandlePool* Instance()
	{
		static CurlHandlePool g_Instance;
		return &g_Instance;
	}
	~CurlHandlePool()
	{
		for (unsigned int i = 0; i < m_IdleList.size(); i++)
		{
			curl_easy_cleanup(m_IdleList[i]);
		}
		m_IdleList.clear();
	}
	CURL* GetHandle()
	{
		CURL* ret = curl_easy_init();
		return ret;
	}
	void FreeHandle(CURL* handle)
	{
		//m_IdleList.push_back(handle);
		if (handle)
		{
			curl_easy_cleanup(handle);
		}		
	}
private:
	vector<CURL*> m_IdleList;
};

/*******************************************************************
* Function  : HttpClient constructor
* Parameters: -
* Returns   : -
* Purpose   : Initialize the object
*******************************************************************/
CNetClientBase::CNetClientBase()
{
	curl_handle     = NULL;
	host[0]	        = '\0';
	m_sgThreadID = 0;
	m_bIsBusy = false;
	m_bUseProgressReport = false;
	m_bAsyncProcessing = false;
	m_nTimeOut = 30;		//默认30s超时
	m_pNetNotify = NULL;
	m_strLuaHandle = "";

	Initialize();
}

/*******************************************************************
* Function  : HttpClient desctructor
* Parameters: -
* Returns   : -
* Purpose   : Object cleanup
*******************************************************************/
CNetClientBase::~CNetClientBase()
{
	m_sAsyncInfo.Reset();
	//Reset();
	if (curl_handle != NULL && !m_bIsBusy)
	{
		//CurlHandlePool::Instance()->FreeHandle(curl_handle);
		//curl_easy_cleanup(curl_handle); // cleanup curl stuff
		//curl_handle = NULL;
	}
}

/*******************************************************************
* Function  : AddHeader()
* Parameters: name & value - The HTTP header to add to the request
* Returns   : TRUE - Header added
*             FALSE - Header not added
* Purpose   : To add a header for the HTTP GET/POST request
*******************************************************************/
bool CNetClientBase::AddHeader(const char *name, const char *value)
{
	return TRUE;
}

/*******************************************************************
* Function  : Reset()
* Parameters: -
* Returns   : -
* Purpose   : To partially cleanup the object so that it can be
*             reused for more requests. Note that the proxy settings
*             are not reset.
*******************************************************************/
void CNetClientBase::Reset()
{
	m_bAsyncProcessing = false;
	
	host[0]	= '\0';

    if (curl_handle == NULL)
        Initialize();
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
CScutString CNetClientBase::GetHost()
{
	return this->host;
}


/*******************************************************************
* Function  : FullReset()
* Parameters: -
* Returns   : -
* Purpose   : To fully cleanup the object so that it can be
*             reused for more requests. Note that all CURL
*			  properties are reset, including the proxy settings.
*******************************************************************/
void CNetClientBase::FullReset()
{
 	Reset();
}

void CNetClientBase::Initialize()
{
	host[0]			= '\0';

	/* init the curl session */
	//curl_handle = curl_easy_init();
	curl_handle = CurlHandlePool::Instance()->GetHandle();
}

bool ScutNetwork::CNetClientBase::IsBusy()
{
	return m_bIsBusy;
}

void ScutNetwork::AsyncInfo::Reset()
{
	Response = NULL;
	PostData = NULL;
	PostDataSize = 0;
	FormFlag = false;
	Sender = NULL;
	Status = aisNone;
	Mode = amGet;
	ProtocalType = 0;
	pScene = NULL;
	Data1 = 0;
	Data2 = 0;
}

bool ScutNetwork::CNetClientBase::GetUseProgressReport()
{
	return m_bUseProgressReport;
}

void ScutNetwork::CNetClientBase::SetUseProgressReport( bool bReport )
{
	m_bUseProgressReport = bReport;
}

//int ScutNetwork::CNetClientBase::ProgressReportProc( void* ptr, double dlTotal, double dlNow, double ulTotal, double ulNow )
//{
//	if (ptr)
//	{
//		return ((CNetClientBase*)ptr)->DoProgressReport(dlTotal, dlNow, ulTotal, ulNow);
//	}
//	return -1;	
//}

int ScutNetwork::CNetClientBase::DoProgressReport( double dlTotal, double dlNow, double ulTotal, double ulNow )
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

AsyncInfo* ScutNetwork::CNetClientBase::GetAsyncInfo()
{
	return &m_sAsyncInfo;
}

int ScutNetwork::CNetClientBase::GetTimeOut()
{
	return m_nTimeOut;
}

void ScutNetwork::CNetClientBase::SetTimeOut( int nTimeOut )
{
	if (nTimeOut != m_nTimeOut && nTimeOut >= 0)
	{
		m_nTimeOut = nTimeOut;
	}
}

void ScutNetwork::CNetClientBase::SetNetStautsNotify( INetStatusNotify* pNetNotify )
{
	m_pNetNotify = pNetNotify;
}

CURL* ScutNetwork::CNetClientBase::GetCurlHandle()
{
	return curl_handle;
}

INetStatusNotify* ScutNetwork::CNetClientBase::GetNetStautsNotify()
{
	return m_pNetNotify;
}

void ScutNetwork::CNetClientBase::RegisterCustomLuaHandle(const char* handle)
{
	m_strLuaHandle = handle;
}

bool ScutNetwork::CNetClientBase::isLuaHandleExist()
{
	return m_strLuaHandle.size() > 0;
}