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
#ifndef _NET_CLIENT_BASE_H_
#define _NET_CLIENT_BASE_H_

#include "curl.h"
//#include "types.h"
#include "easy.h"

//#include "Defines.h"
#include "HttpClientResponse.h"
#include "HttpSession.h"
#include "ScutString.h"	

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

	enum EAsyncInfoStatus
	{
		aisNone,
		aisProgress,
		aisSucceed,	
		aisTimeOut,
		aisFailed,
	};

	enum EAsyncMode
	{
		amGet,
		amPost,
	};

	enum ENetType
	{
		ntNone,
		ntWIFI,
		ntCMWAP,
		ntCMNET,
	};


	//class CTcpClient;
	//class CHttpClient;
	class CNetClientBase;
	struct AsyncInfo
	{
		CNetClientBase* Sender;			//发起请求的HttpClient
		CHttpClientResponse* Response;	//响应对象		
		CScutString Url;						//URL
		const void* PostData;			//提交的数据
		int PostDataSize;				//提交的数据大小
		bool FormFlag;					//是否是表单提交
		EAsyncInfoStatus Status;		//网络请求异步状态
		EAsyncMode Mode;				//异步请求模式
		int ProtocalType;               //0:HTTP， 1:TCP
		void* pScene;
		int Data1;						//数据1
		int Data2;						//数据2
		AsyncInfo()
		{
			Reset();
		}
		~AsyncInfo()
		{
		}
		void Reset();
	};
	typedef AsyncInfo* PAsyncInfo;

	class CNetClientBase
	{
	public:
		CNetClientBase();
		virtual ~CNetClientBase();		

		virtual int AsyncNetGet(const char* url, CHttpClientResponse* resp) = 0;
	
		//异步情况下，需要设定网络状态通知接口
		virtual void SetNetStautsNotify(INetStatusNotify* pNetNotify);
		virtual INetStatusNotify* GetNetStautsNotify();
		//头信息与代理服务器设置
		virtual bool AddHeader(const char *name, const char *value);

		//获取和设定超时时间，以秒为单位，0或INFINTE表示无超时时间
		int GetTimeOut();
		void SetTimeOut(int nTimeOut);

		//是否使用进度通知
		virtual bool GetUseProgressReport();
		virtual void SetUseProgressReport(bool bReport);		

		//重设状态
		virtual void FullReset();
		virtual void Reset();
		CScutString	GetHost();	

		//是否处于繁忙状态，如果是那么不可再调用Get或Post
		//iPhone平台也可通过此判断异步请求是否已经完成
		virtual bool IsBusy();
		
		AsyncInfo* GetAsyncInfo();

		CURL* GetCurlHandle();

		//注册lua网络回调
		virtual void RegisterCustomLuaHandle(const char* szHandle);
	protected:
		virtual void Initialize();
		virtual int DoProgressReport(double dlTotal, double dlNow, double ulTotal, double ulNow);
		virtual bool isLuaHandleExist();
	
	protected:
		CURL			*curl_handle;
		char			host[256];
		unsigned int	m_sgThreadID;
		AsyncInfo		m_sAsyncInfo;

		bool			m_bIsBusy;
		bool			m_bUseProgressReport;
		bool			m_bAsyncProcessing;

		int				m_nTimeOut;

		INetStatusNotify*	m_pNetNotify;	//网络状态通知接口

		std::string		m_strLuaHandle;
	};

}

#endif

