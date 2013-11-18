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

#define MAXINTERFACES 16 

struct HttpDataInfo 
{
	// misc global
	bool			m_bAutoStartWithwindows;
	bool            m_bSawDisclaimer;
	bool            m_bAlwaysOnTop;
	bool            m_bHideTrayIcon;
	bool			m_bQuietMode;
	CScutString		m_UserAgent;
	CScutString       m_sDomain;

	// proxy
	CScutString		m_strHttpProxyHost;
	UINT			m_nHttpProxyPort;
	CScutString		m_strHttpsProxyHost;
	UINT			m_nHttpsProxyPort;
	bool			m_bUseProxy;
	bool            m_bProxyNTLM;
	bool			m_bProxyAuth;
	bool            m_bProxyAuthNTLM;
	CScutString		m_strProxyAuthPassword;
	CScutString		m_strProxyAuthUsername;

	// Activity Log
	bool			m_bCreateLog;
	bool			m_bLimitLog;
	UINT			m_nLogLevel;
	UINT			m_nLogSize;
};


typedef struct MemoryStruct_t
{
	char *memory;
	size_t size;
	CHttpSession *session;
	vector<CScutString> headers;
} MemoryStruct;

HttpDataInfo settings;
//extern LogFile logFile;

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
		//if (m_IdleList.size() > 0)
		//{
		//	CURL* ret = m_IdleList[0];
		//	m_IdleList.erase(m_IdleList.begin());
		//	return ret;
		//}
		//else
		//{
		//	CURL* ret = curl_easy_init();
		//	return ret;
		//}
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

bool IsGzipOrDeflateData(MemoryStruct& ms)
{
	for (unsigned int i = 0; i < ms.headers.size(); i++)
	{
		CScutString temp = ms.headers[i];
		if (temp.find("Content-Encoding") != -1)
		{
			if (temp.find("gzip") != -1 || temp.find("deflate") != -1)
			{
				return true;
			}
		}
	}
	return false;
}

void GZipUnZipStream(CStream* pStm)
{
	if (pStm->GetSize() > 0)
	{
		pStm->SetPosition(0);
		char* pszBuffer = new char[pStm->GetSize()];
		pStm->ReadBuffer(pszBuffer, pStm->GetSize());
		char* pszOut;
		int nLen = ZipUtils::ccInflateMemory((unsigned char*)pszBuffer, pStm->GetSize(), (unsigned char**)&pszOut);
		if (nLen > 0)
		{
			pStm->SetSize(0);
			pStm->WriteBuffer(pszOut, nLen);
			delete []pszOut;
		}		
		delete []pszBuffer;
	}	
}

//写响应数据
static size_t WriteMemoryCallback(void *ptr, size_t size, size_t nmemb, void *data)
{
	register int realsize = size * nmemb;

	CHttpClientResponse* pResponse = (CHttpClientResponse*)data;
	if (pResponse->GetTarget())
	{
		CStream* pStm = pResponse->GetTarget();
		if (!pStm->WriteBuffer((char*)ptr, realsize))
		{
			return 0;
		}
	}
	return realsize;
}

static size_t HttpHeaderData(void *ptr, size_t size, size_t nmemb, void *stream)
{
	register int realsize = size * nmemb;
	MemoryStruct *mem = (MemoryStruct *)stream;

	if(CScutString::NoCaseCmp((const char *)(ptr), "set-cookie:", 11) == 0)
	{
		mem->session->AddCookie(static_cast<char *>(ptr));
	}
	CScutString strTemp((const char*)ptr, size * nmemb - 1);
	if (strTemp != "" && strTemp.find("HTTP") == std::string::npos)
	{
		mem->headers.push_back(strTemp);
	}
	return realsize;
}

/*******************************************************************
* Function  : HttpClient constructor
* Parameters: -
* Returns   : -
* Purpose   : Initialize the object
*******************************************************************/
CHttpClient::CHttpClient(CHttpSession *s)
{
	curl_handle     = NULL;
    pResponsePage   = NULL;
    headers         = NULL;
	host[0]	        = '\0';
	httpProxyHost[0] = '\0';
	httpsProxyHost[0] = '\0';
    session			= s;
    referer         = "";
	m_sgThreadID = 0;
	m_bIsBusy = false;
	m_bUseProgressReport = false;
	m_bAsyncProcessing = false;
	m_nTimeOut = 15;		//默认15s超时
	m_pNetNotify = NULL;

	Initialize();
}

/*******************************************************************
* Function  : HttpClient desctructor
* Parameters: -
* Returns   : -
* Purpose   : Object cleanup
*******************************************************************/
CHttpClient::~CHttpClient()
{
	m_sAsyncInfo.Reset();
	//Reset();
	if (curl_handle != NULL && !m_bIsBusy)
	{
		CurlHandlePool::Instance()->FreeHandle(curl_handle);
		//curl_easy_cleanup(curl_handle); // cleanup curl stuff
		curl_handle = NULL;
	}
}

/*******************************************************************
* Function  : SetHttpProxy()
* Parameters: proxyHost, proxyPort - HTTP Proxy
* Returns   : -
* Purpose   : To set the HTTP proxy values
*******************************************************************/
void CHttpClient::SetHttpProxy(const char *proxyHost, unsigned int proxyPort)
{
	strcpy(httpProxyHost, proxyHost);
	httpProxyPort = proxyPort;
}

/*******************************************************************
* Function  : UseHttpProxy()
* Parameters: bUseProxy - Is the proxy to be used?
* Returns   : -
* Purpose   : To inform HttpClient whether the HTTP proxy is to be
*             used for the HTTP GET/POST requests
*******************************************************************/
void CHttpClient::UseHttpProxy(bool bUseProxy)
{
	bUseHttpProxy = bUseProxy;
}

/*******************************************************************
* Function  : UseHttpsProxy()
* Parameters: bUseProxy - Is the proxy to be used?
* Returns   : -
* Purpose   : To inform HttpClient whether the HTTPS proxy is to be
*             used for the HTTPS GET/POST requests
*******************************************************************/
void CHttpClient::UseHttpsProxy(bool bUseProxy)
{
	bUseHttpsProxy = bUseProxy;
}

/*******************************************************************
* Function  : SetHttpsProxy()
* Parameters: proxyHost, proxyPort - HTTPS Proxy
* Returns   : -
* Purpose   : To set the HTTPS proxy values
*******************************************************************/
void CHttpClient::SetHttpsProxy(const char *proxyHost, unsigned int proxyPort)
{
	strcpy(httpsProxyHost, proxyHost);
	httpsProxyPort = proxyPort;
}

/*******************************************************************
* Function  : AddHeader()
* Parameters: name & value - The HTTP header to add to the request
* Returns   : TRUE - Header added
*             FALSE - Header not added
* Purpose   : To add a header for the HTTP GET/POST request
*******************************************************************/
bool CHttpClient::AddHeader(const char *name, const char *value)
{
	CScutString header;

	if(name == NULL || value == NULL)
		header = "\r\n";
	else
		header.Format("%s: %s", name, value);

    // If Cookie, replace existing line, if any
    int found = 0;
    if (headers != NULL && strcmp(name, "Cookie") == 0)
    {
        struct curl_slist *h = headers;
        for (; h; h = h->next)
        {
            if (strncmp(h->data, name, 6) == 0 && h->data[6] == ':')
            {
                SAFE_FREE(h->data);
                char *dup = DuplicateStr(header.c_str());
                h->data = dup;
                found++;
            }
        }
    }

    if (found == 0)
    	headers = curl_slist_append(headers, header.c_str());

	return TRUE;
}


bool CHttpClient::HttpPost(const char *url, const void * postData, int nPostDataSize, CHttpClientResponse &resp, bool formflag)
{
    bool retVal = 1;

    for(int i = 0; i < HTTP_MAX_RETRIES; i++)
    {
        if( (retVal = DoPostInternal(url, postData, nPostDataSize, resp, formflag)) == 0)
        {
            if (resp.GetBodyLength() == 0)
            {
                //logFile.Write(LOG_BASIC, "HTTP POST returned no data - trying again...");
            }
			else if (i > 0)
            {
				//logFile.Write(LOG_BASIC, "HTTP POST finally succeeded");
            }
            break;
        }
#ifdef SCUT_WIN32
		Sleep(1000);
#elif SCUT_ANDROID
		usleep(1000);
#else
		usleep(1000);
#endif		
    }

    if (resp.GetBodyLength() == 0)
    {
        //logFile.Write(LOG_BASIC, "HTTP POST returned no data - returning failure...");
        //retVal = 1;
    }

    return retVal;
}

int CHttpClient::DoPostInternal(const char *url, const void * postData, int nPostDataSize, CHttpClientResponse &resp, bool formflag)
{
	//m_bIsBusy = true;

	CURLcode res;
	MemoryStruct chunk;
	CScutString strProxy;
	CScutString strProxyAuth;
	
	chunk.memory = NULL; /* we expect realloc(NULL, size) to work */
	chunk.size   = 0;    /* no data at this point */
	chunk.session = session;
	chunk.headers.clear();

	/* init the curl session if necessary */
    if (curl_handle == NULL)
        Initialize();

	resp.SetRequestUrl(url);

    CScutString cookies = session->GetCookies(this);
	if (cookies != "")
    {
        //logFile.Write(LOG_COOKIE, "Setting curl cookie to: %s", LPCTSTR(cookies.c_str()));
        curl_easy_setopt(curl_handle, CURLOPT_COOKIE, LPCTSTR(cookies.c_str()));
    }
	
	/* specify URL to get */
	curl_easy_setopt(curl_handle, CURLOPT_URL, url);
	
	/* send all data to this function  */
	//curl_easy_setopt(curl_handle, CURLOPT_WRITEFUNCTION, WriteMemoryCallback);
	curl_easy_setopt(curl_handle, CURLOPT_FILE, static_cast<void *>(&resp));
	curl_easy_setopt(curl_handle, CURLOPT_HEADERFUNCTION, HttpHeaderData);
	curl_easy_setopt(curl_handle, CURLOPT_WRITEHEADER, static_cast<void *>(&chunk));
	curl_easy_setopt(curl_handle, CURLOPT_HTTPHEADER, headers);
	//Apache服务器需要设定此项
	//curl_easy_setopt(curl_handle, CURLOPT_IGNORE_CONTENT_LENGTH, TRUE);

	/* Set the operation to POST */
	if (formflag)
    {
	    curl_easy_setopt(curl_handle, CURLOPT_HTTPPOST, postData);
    }
	else
    {
		curl_easy_setopt(curl_handle, CURLOPT_POSTFIELDS, postData);
		if (nPostDataSize != -1)
		{
			curl_easy_setopt(curl_handle, CURLOPT_POSTFIELDSIZE, nPostDataSize);
		}
    }
	
    /* set proxy stuff */
    if (settings.m_bProxyNTLM)
        curl_easy_setopt(curl_handle, CURLOPT_HTTPAUTH, CURLAUTH_NTLM);
    if (settings.m_bProxyAuthNTLM)
        curl_easy_setopt(curl_handle, CURLOPT_PROXYAUTH, CURLAUTH_NTLM);

	//超时处理
	if (m_nTimeOut > 0)
	{
		curl_easy_setopt(curl_handle, CURLOPT_TIMEOUT, m_nTimeOut);
	}

	if(CScutString::NoCaseCmp(url, "https://", 8) == 0)
	{
		if(bUseHttpsProxy)
		{
			strProxy.Format("%s:%d", httpsProxyHost, httpsProxyPort);
			curl_easy_setopt(curl_handle, CURLOPT_PROXY, strProxy.c_str());
		}
	}
	else if(bUseHttpProxy)
	{
		strProxy.Format("%s:%d", httpProxyHost, httpProxyPort);
		curl_easy_setopt(curl_handle, CURLOPT_PROXY, strProxy.c_str());
	}

	if(settings.m_bProxyAuth == TRUE)
	{
		strProxyAuth.Format("%s:%s", settings.m_strProxyAuthUsername.c_str(), settings.m_strProxyAuthPassword.c_str());
		curl_easy_setopt(curl_handle, CURLOPT_PROXYUSERPWD, strProxyAuth.c_str());
	}

	/* get it! */
	res = curl_easy_perform(curl_handle);
	
	char *effectiveUrl = NULL;
	curl_easy_getinfo(curl_handle, CURLINFO_EFFECTIVE_URL, &effectiveUrl);
    referer = effectiveUrl;
    //logFile.Write(LOG_ADVANCED, "last URL visited = %s", effectiveUrl);
	GetUrlHost(effectiveUrl, host);

	resp.SetLastResponseUrl(effectiveUrl);

	/* cleanup curl stuff */
	/*curl_easy_cleanup(curl_handle);
    curl_handle = NULL;*/

	//resp.SetData(chunk.memory, chunk.size);

	//处理压缩数据
	if (IsGzipOrDeflateData(chunk) && resp.GetTarget())
	{
		GZipUnZipStream(resp.GetTarget());
	}

	free(chunk.memory);

	if(res == CURLE_PARTIAL_FILE)
    {
		//logFile.Write(LOG_BASIC, "WARNING: HTTP POST returned CURL error: %s (%d) - ignoring",curl_easy_strerror(res), res);
    }

    if(res != CURLE_OK)
    {
        //logFile.Write(LOG_BASIC, "HTTP POST failed. CURL error: %s (%d)",curl_easy_strerror(res), res);
    }

	//m_bIsBusy = false;

	return res;
}

/*******************************************************************
* Function  : DoGet()
* Parameters: url - The URL to which an HTTP GET is to be done
*             resp - The response object which will store the HTTP
*                    response received from the website
* Returns   : 0    - GET succeeded (could have any HTTP errors,
*                    including 404 errors)
*             1    - The GET failed most likely because a 
*                    connection coul not be established
* Purpose   : To do an HTTP GET with a max of HTTP_MAX_RETRIES retries
*******************************************************************/
int CHttpClient::HttpGet(const char *url, CHttpClientResponse &resp)
{
    int nRet = 1;

    for(int i = 0; i < HTTP_MAX_RETRIES; i++)
    {
        if ((nRet = DoGetInternal(url, resp)) == 0)
        {
            if (resp.GetBodyLength() == 0)
            {
                //logFile.Write(LOG_BASIC, "HTTP GET returned no data - trying again...");
            }
			else if (i > 0)
            {
				//logFile.Write(LOG_BASIC, "HTTP GET finally succeeded");
            }
            break;
        }
		FullReset();
#ifdef SCUT_WIN32
		Sleep(1000);
#elif SCUT_ANDROID
		usleep(1000);
#else
		usleep(1000);
#endif		
    }

    return nRet;
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
int CHttpClient::DoGetInternal(const char *url, CHttpClientResponse &resp)
{
	//m_bIsBusy = true;
	CURLcode res;
	MemoryStruct chunk;
	CScutString strProxy;
	CScutString strProxyAuth;
	
	chunk.memory = NULL; 
	chunk.size   = 0;  
	chunk.session = session;
	chunk.headers.clear();

    if (curl_handle == NULL)
        Initialize();

	resp.SetRequestUrl(url);

    CScutString cookies = session->GetCookies(this);
	if (cookies != "")
    {
        //logFile.Write(LOG_COOKIE, "Setting curl cookie to: %s", LPCTSTR(cookies.c_str()));
        curl_easy_setopt(curl_handle, CURLOPT_COOKIE, LPCTSTR(cookies.c_str()));
    }

	curl_easy_setopt(curl_handle, CURLOPT_URL, url);
	curl_easy_setopt(curl_handle, CURLOPT_FILE, static_cast<void *>(&resp));
	curl_easy_setopt(curl_handle, CURLOPT_WRITEHEADER, static_cast<void *>(&chunk));
	curl_easy_setopt(curl_handle, CURLOPT_HTTPHEADER, headers);
	//Ignore the Content-Length header. This is useful for Apache 1.x (and similar servers) which will report incorrect content length for files over 2 gigabytes. If this option is used, curl will not be able to accurately report progress, and will simply stop the download when the server ends the connection.
	//curl_easy_setopt(curl_handle, CURLOPT_IGNORE_CONTENT_LENGTH, TRUE);

    curl_easy_setopt(curl_handle, CURLOPT_HTTPGET, 1);

	//安全和代理服务器设置
    if (settings.m_bProxyNTLM)
        curl_easy_setopt(curl_handle, CURLOPT_HTTPAUTH, CURLAUTH_NTLM);
    if (settings.m_bProxyAuthNTLM)
        curl_easy_setopt(curl_handle, CURLOPT_PROXYAUTH, CURLAUTH_NTLM);

	//超时时间
	if (m_nTimeOut > 0)
	{
		curl_easy_setopt(curl_handle, CURLOPT_TIMEOUT, m_nTimeOut);
	}
	//断点续传
	if (resp.GetUseDataResume() && resp.GetTarget()->GetSize() > 0)
	{
		//curl_easy_setopt(curl_handle, CURLOPT_RESUME_FROM_LARGE, resp.GetTarget()->GetSize());
		CScutString strTemp;
		strTemp.Format("%d-", resp.GetTarget()->GetSize());
		curl_easy_setopt(curl_handle, CURLOPT_RANGE, strTemp.c_str());
	}

	if(CScutString::NoCaseCmp(url, "https://", 8) == 0)
	{
		if(bUseHttpsProxy)
		{
			strProxy.Format("%s:%d", httpsProxyHost, httpsProxyPort);
			curl_easy_setopt(curl_handle, CURLOPT_PROXY, strProxy.c_str());
		}
	}
	else if(bUseHttpProxy)
	{
		strProxy.Format("%s:%d", httpProxyHost, httpProxyPort);
		curl_easy_setopt(curl_handle, CURLOPT_PROXY, strProxy.c_str());
	}

	if(settings.m_bProxyAuth == TRUE)
	{
		strProxyAuth.Format("%s:%s", settings.m_strProxyAuthUsername.c_str(), settings.m_strProxyAuthPassword.c_str());
		curl_easy_setopt(curl_handle, CURLOPT_PROXYUSERPWD, strProxyAuth.c_str());
	}

	//进度设置，异步才启用进度显示功能
	if (m_bUseProgressReport && m_bAsyncProcessing)
	{
		curl_easy_setopt(curl_handle, CURLOPT_NOPROGRESS, 0);
		curl_easy_setopt(curl_handle, CURLOPT_PROGRESSFUNCTION, ProgressReportProc);
		curl_easy_setopt(curl_handle, CURLOPT_PROGRESSDATA, this);
	}	
	
	res = curl_easy_perform(curl_handle);
	
	char *effectiveUrl = NULL;
	curl_easy_getinfo(curl_handle, CURLINFO_EFFECTIVE_URL, &effectiveUrl);
    referer = effectiveUrl;
    //logFile.Write(LOG_ADVANCED, "last URL visited = %s", effectiveUrl);
	GetUrlHost(effectiveUrl, host);

	resp.SetLastResponseUrl(effectiveUrl);

	//获取状态码
	int nStatus = 0;
	curl_easy_getinfo(curl_handle, CURLINFO_HTTP_CODE, &nStatus);
	resp.SetStatusCode(nStatus);
	
	char *conttype = NULL;
	curl_easy_getinfo(curl_handle, CURLINFO_CONTENT_TYPE, &conttype);
	//logFile.Write(LOG_ADVANCED, "Content-Type = %s", nvl(conttype,"(null)"));
    	

	//resp.SetData(chunk.memory, chunk.size);
	resp.SetContentType(conttype);

	//处理压缩数据
	if (IsGzipOrDeflateData(chunk) && resp.GetTarget())
	{
		GZipUnZipStream(resp.GetTarget());
	}
/*
	if (res == CURLE_OK)
	{
		if (nStatus == 200)
		{
			//判断长度是否正确
			double dContentLength = 0;

			CURLcode code = curl_easy_getinfo(curl_handle, CURLINFO_CONTENT_LENGTH_DOWNLOAD , &dContentLength);
			if (code == CURLE_OK) //判断长度并解压
			{
				if (resp.GetTarget())
				{
					if (resp.GetTarget()->GetSize() == (int)dContentLength)
					{
						//处理压缩数据
						if (IsGzipOrDeflateData(chunk))
						{
							GZipUnZipStream(resp.GetTarget());
						}
					}
					else
						res = CURLE_RECV_ERROR;
				}

			}
			else //直接解压
			{
				//处理压缩数据
				if (IsGzipOrDeflateData(chunk) && resp.GetTarget())
				{
					GZipUnZipStream(resp.GetTarget());
				}
			}
		}
		else
			res = CURLE_RECV_ERROR;
	}
*/
	free(chunk.memory);

	if(res == CURLE_PARTIAL_FILE)
    {
		//logFile.Write(LOG_BASIC, "WARNING: HTTP GET returned CURL error: %s (%d) - ignoring",curl_easy_strerror(res), res);
    }

	if(res != CURLE_OK)
    {
		const char* strerror = curl_easy_strerror(res);
		if (strerror != NULL)
		{
			//m_pLogFile->Write(LOG_BASIC, strerror);
		}
        //logFile.Write(LOG_BASIC, "HTTP GET failed. CURL error: %s (%d)",curl_easy_strerror(res), res);
    }

	//m_bIsBusy = false;

	return res;
}

/*******************************************************************
* Function  : Reset()
* Parameters: -
* Returns   : -
* Purpose   : To partially cleanup the object so that it can be
*             reused for more requests. Note that the proxy settings
*             are not reset.
*******************************************************************/
void CHttpClient::Reset()
{
	SAFE_FREE(pResponsePage);
	responsePageLen = 0;
	m_bAsyncProcessing = false;
	
    if(headers != NULL)
   	    curl_slist_free_all(headers);
	headers = NULL;

	host[0]	= '\0';

    if (curl_handle == NULL)
        Initialize();

    if (referer != "")
        curl_easy_setopt(curl_handle, CURLOPT_REFERER, referer.c_str());
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
//CScutString CHttpClient::GetHost()
//{
//	return this->host;
//}

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
int CHttpClient::GetUrlHost(const char *url, char *host)
{
	int startPos;
	const char *ptr;

	// Check protocol
	if(strstr(url, "http://") == url)
	{
		startPos = 7;     // protocol field end
	}
	else if(strstr(url, "https://") == url)
	{
		startPos = 8;    // protocol field end
	}
	else
	{
		// Unsupported protocol
		return ERROR_UNSUPPORTED_PROTOCOL;
	}

	// Get web server name
	if((ptr = strchr(url + startPos, '/')) == NULL)
	{
		// Only host is specified without trailing "/"
		strcpy(host, url + startPos);
		return 0;
	}
	else
	{
		strncpy(host, url + startPos, ptr - (url + startPos));
		host[ptr - (url + startPos)] = '\0';
	}

	return 0;
}


/*******************************************************************
* Function  : FullReset()
* Parameters: -
* Returns   : -
* Purpose   : To fully cleanup the object so that it can be
*             reused for more requests. Note that all CURL
*			  properties are reset, including the proxy settings.
*******************************************************************/
void CHttpClient::FullReset()
{
	if (curl_handle != NULL)
    {
//		curl_easy_cleanup(curl_handle); // cleanup curl stuff
//        curl_handle = NULL;

    }
 	Reset();
	referer = "";
}

void CHttpClient::Initialize()
{
	bUseHttpProxy	= FALSE;
	bUseHttpsProxy  = FALSE;
	host[0]			= '\0';
	SAFE_FREE(pResponsePage);
	responsePageLen = 0;

	/* init the curl session */
	//curl_handle = curl_easy_init();
	curl_handle = CurlHandlePool::Instance()->GetHandle();
	
	curl_easy_setopt(curl_handle, CURLOPT_WRITEFUNCTION, WriteMemoryCallback);
	curl_easy_setopt(curl_handle, CURLOPT_HEADERFUNCTION, HttpHeaderData);
    curl_easy_setopt(curl_handle, CURLOPT_USERAGENT, settings.m_UserAgent.empty() ? "unknown" : settings.m_UserAgent.c_str());
	curl_easy_setopt(curl_handle, CURLOPT_FOLLOWLOCATION, 1);
    //curl_easy_setopt(curl_handle, CURLOPT_AUTOREFERER, 1);
	curl_easy_setopt(curl_handle, CURLOPT_MAXREDIRS, 10);

	/* set the SSL/TLS defaults */
	curl_easy_setopt(curl_handle, CURLOPT_SSLENGINE_DEFAULT, 1);
	curl_easy_setopt(curl_handle, CURLOPT_SSL_VERIFYPEER, 0);
    curl_easy_setopt(curl_handle, CURLOPT_SSL_VERIFYHOST, 0);
	curl_easy_setopt(curl_handle, CURLOPT_NOPROGRESS, 1);
	curl_easy_setopt(curl_handle, CURLOPT_NOSIGNAL, 1);
	curl_easy_setopt(curl_handle, CURLOPT_ENCODING, "gzip, deflate");
	//curl_easy_setopt(curl_handle, CURLOPT_ENCODING, "");

	if (session->cookieJar != "")
	{
		curl_easy_setopt(curl_handle, CURLOPT_COOKIEJAR,  session->cookieJar.c_str());
		curl_easy_setopt(curl_handle, CURLOPT_COOKIEFILE, session->cookieJar.c_str());
		//logFile.Write(LOG_BASIC, "CookieJar set to = %s", LPCTSTR(session->cookieJar));
	}

	if(settings.m_bUseProxy)
	{
		UseHttpProxy(TRUE);
		SetHttpProxy(settings.m_strHttpProxyHost, settings.m_nHttpProxyPort);
		if(!settings.m_strHttpsProxyHost.TrimRight().IsEmpty())
		{
			SetHttpsProxy(settings.m_strHttpsProxyHost, settings.m_nHttpsProxyPort);
			UseHttpsProxy(TRUE);
		}
	}
}

int ScutNetwork::CHttpClient::AsyncNetGet(const char* url, CHttpClientResponse* resp)
{
	return AsyncHttpGet(url, resp);
}

int ScutNetwork::CHttpClient::AsyncHttpGet(const char* url, CHttpClientResponse* resp)
{
	if (m_bIsBusy)
	{
		return -1;
	}
	m_sAsyncInfo.Reset();
	m_sAsyncInfo.Url = url;
	m_sAsyncInfo.Response = resp;
	m_sAsyncInfo.Sender = this;
	m_sAsyncInfo.Mode = amGet;
#ifdef SCUT_WIN32
	return (int)CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AsyncThreadProc, &m_sAsyncInfo, 0, (LPDWORD)(&m_sgThreadID));
#else
	return (int)pthread_create((pthread_t*)&m_sgThreadID, NULL, AsyncThreadProc, &m_sAsyncInfo);
#endif
}

int ScutNetwork::CHttpClient::AsyncHttpPost( const char* url, const void* postData, int nPostDataSize, CHttpClientResponse* resp, bool bFormFlag /*= false*/ )
{
	if (m_bIsBusy)
	{
		return -1;
	}
	m_sAsyncInfo.Reset();
	m_sAsyncInfo.Url = url;
	m_sAsyncInfo.Response = resp;
	m_sAsyncInfo.Sender = this;
	m_sAsyncInfo.PostData = postData;
	m_sAsyncInfo.PostDataSize = nPostDataSize;
	m_sAsyncInfo.FormFlag = bFormFlag;
	m_sAsyncInfo.Mode = amPost;
#ifdef SCUT_WIN32
	return (int)CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AsyncThreadProc, &m_sAsyncInfo, 0, (LPDWORD)(&m_sgThreadID));
#else
	return (int)pthread_create((pthread_t*)&m_sgThreadID, NULL, AsyncThreadProc, &m_sAsyncInfo);
#endif
}

#ifdef SCUT_WIN32
DWORD WINAPI ScutNetwork::CHttpClient::AsyncThreadProc(LPVOID puCmd)
#else
void* ScutNetwork::CHttpClient::AsyncThreadProc( void * puCmd )
#endif
{	
	int nRet = -1;
	if (puCmd)
	{
		PAsyncInfo pAi = (PAsyncInfo)puCmd;
		if (!pAi->Sender) {
			return NULL;
		}
		CHttpClient* pSender = (CHttpClient*)pAi->Sender;
		if (!pSender)
		{
			return NULL;
		}
		pSender->m_bAsyncProcessing = true;
		pSender->m_bIsBusy = true;

		if (pAi->Mode == amGet)
		{
			nRet = pSender->HttpGet(pAi->Url, *(pAi->Response));
		}
		else
			nRet = pSender->HttpPost(pAi->Url, pAi->PostData, pAi->PostDataSize, *(pAi->Response), pAi->FormFlag);
		switch ((CURLcode)nRet)
		{
		case CURLE_OK:
			pAi->Status = aisSucceed;
			break;
		case CURLE_OPERATION_TIMEDOUT:
			pAi->Status = aisTimeOut;
			break;
		default:
			pAi->Status = aisFailed;
			break;
		}
		pSender->m_bAsyncProcessing = false;
		pSender->m_bIsBusy = false;

		//通知	
		if (pSender->isLuaHandleExist())
		{
			LuaHelper::Instance()->execFunc(pSender->m_strLuaHandle, (AsyncInfo*)pAi);
		}
		else
		{

			if (pSender->m_pNetNotify)
			{
				pSender->m_pNetNotify->OnNotify(pAi);
			}	
		}			
	}	
	return NULL;
}

int ScutNetwork::CHttpClient::ProgressReportProc( void* ptr, double dlTotal, double dlNow, double ulTotal, double ulNow )
{
	if (ptr)
	{
		return ((CHttpClient*)ptr)->DoProgressReport(dlTotal, dlNow, ulTotal, ulNow);
	}
	return -1;	
}

int ScutNetwork::CHttpClient::DoProgressReport( double dlTotal, double dlNow, double ulTotal, double ulNow )
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
		m_sAsyncInfo.Data2 = (dlTotal == 0) ? 0 : (dlTotal + nRawSize);

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

char* ScutNetwork::CHttpClient::DuplicateStr( const char* lpszValue )
{
#ifdef SCUT_WIN32
	return _strdup(lpszValue);
#else
	return strdup(lpszValue);
#endif
}

ScutNetwork::ENetType ScutNetwork::CHttpClient::GetNetType()
{
#ifdef SCUT_ANDROID
	register int fd, intrface, retn = 0; 
	struct ifreq buf[MAXINTERFACES]; 
	struct arpreq arp; 
	struct ifconf ifc; 
	if ((fd = socket (AF_INET, SOCK_DGRAM, 0)) >= 0) 
	{ 
		ifc.ifc_len = sizeof buf; 
		ifc.ifc_buf = (caddr_t) buf; 
		if (!ioctl (fd, SIOCGIFCONF, (char *) &ifc)) 
		{ 
			//获取接口信息
			intrface = ifc.ifc_len / sizeof (struct ifreq); 
			//printf("interface num is intrface=%d\n\n\n",intrface); 
			//根据接口信息循环获取设备IP和MAC地址
			while (intrface-- > 0) 
			{ 
				//获取设备名称
				//printf ("net device %s\n", buf[intrface].ifr_name); 

				//判断网卡类型 
				//if (!(ioctl (fd, SIOCGIFFLAGS, (char *) &buf[intrface]))) 
				//{ 
				//	if (buf[intrface].ifr_flags & IFF_PROMISC) 
				//	{ 
				//		puts ("the interface is PROMISC"); 
				//		retn++; 
				//	} 
				//} 
				//else 
				//{ 
				//	char str[256]; 
				//	sprintf (str, "cpm: ioctl device %s", buf[intrface].ifr_name); 
				//	perror (str); 
				//} 
				//判断网卡状态 
				if (buf[intrface].ifr_flags & IFF_UP) 
				{ 
					puts("the interface status is UP"); 
				} 
				else 
				{ 
					puts("the interface status is DOWN"); 
				} 
				//获取当前网卡的IP地址 
				if (!(ioctl (fd, SIOCGIFADDR, (char *)&buf[intrface]))) 
				{ 
					puts ("IP address is:"); 
					puts(inet_ntoa(((struct sockaddr_in*)(&buf[intrface].ifr_addr))->sin_addr)); 
					puts(""); 
					//puts (buf[intrface].ifr_addr.sa_data); 
				} 
				else 
				{ 
					char str[256]; 
					sprintf (str, "cpm: ioctl device %s", buf[intrface].ifr_name); 
					perror (str); 
				} 
			} //while
		} 
		else 
			perror ("cpm: ioctl"); 
	} 
	else 
		perror ("cpm: socket"); 
	close (fd); 
	return ntNone;
#endif
	return ntNone;
}