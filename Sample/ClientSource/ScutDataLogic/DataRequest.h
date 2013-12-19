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


#pragma once
#ifndef _SCUT_DATAREQUEST_H_
#define _SCUT_DATAREQUEST_H_

#include <map>
#include <list>
#include "NetClientBase.h"
#include "HttpSession.h"
#include "HttpClientResponse.h"
#include "AutoGuard.h"
#include "INetStatusNotify.h"
#include "TcpClient.h"

using namespace std;
using namespace ScutSystem;
using namespace ScutNetwork;

namespace ScutDataLogic
{	
	class CDataHandler;

	//TODO:之后引入HttpClient池
	typedef struct RequestInfo 
	{
		CDataHandler* Handler;
		int HandlerTag;
		CNetClientBase* NetClient;
		CHttpClientResponse* HttpResponse;
		CHttpSession* HttpSession;
		DWORD StartTime;
		string Url;
		LPVOID LPData;
		int Status;

		//add by cd 
		void* pScene;
		RequestInfo();
		~RequestInfo();
	} *PRequestInfo;

	//网络错误
	enum ERequestError
	{
		reNetFailed,		//失败
		reNetTimeOut,		//超时
	};

	typedef void(*LUA_DATAHANDLE_CALLBACK)(void *pScene, int nTag, int nNet, CStream* lpData, LPVOID lpExternal);

	//数据请求类，完成数据的同步异步请求
	class CDataRequest: public INetStatusNotify
	{
	private:
		CDataRequest(void);
		virtual ~CDataRequest(void);
	public:
		static CDataRequest* Instance();
		
		///////////////////////////C++ 模式相关函数//////////////////////

		//同步执行请求
		bool ExecRequest(CDataHandler* pHandler, const char* lpszUrl);
		//异步执行请求
		DWORD AsyncExecRequest(CDataHandler* pHandler, int nTag, const char* lpszUrl, LPVOID lpData, void* pScene = NULL);
		//取消异步请求，暂不支持
		bool CancelAsyncRequest(DWORD dwRequestID);

		//同步执行请求
		bool ExecTcpRequest(CDataHandler* pHandler, const char* lpszUrl, const char* lpSendData, unsigned int nDateLen = 0);
		//异步执行请求
		DWORD AsyncExecTcpRequest(CDataHandler* pHandler, int nTag, const char* lpszUrl, LPVOID lpData, void* pScene, const char* lpSendData, unsigned int nDateLen = 0);

		///////////////////////////LUA 模式相关函数//////////////////////

		//同步执行请求
		bool ExecRequest(void* pScene);
		//添加数据的处理者 目前默认是ScutScene
		DWORD AsyncExecRequest(void* pScene, const char* szUrl, int nTag, void* lpData);

		//用socket方式请求数据，lpSendData为要发送的数据，nDateLen为要发送的数据长度
		DWORD AsyncExecTcpRequest(void* pScene, const char* szUrl, int nTag, void* lpData, const char* lpSendData, unsigned int nDateLen = 0);

		//注册数据处理回调函数
		void RegisterLUACallBack(LUA_DATAHANDLE_CALLBACK pCallBack);
		//取一笔LUA数据并进行处理
		void PeekLUAData();

		void SetUseKeepAliveMode(bool bKeepAlive);
		bool GetUseKeepAliveMode();

		virtual void OnNotify(PAsyncInfo pAi);
	private:
		void ProcAsyncInfo(PAsyncInfo pAi);
		void FreeRequestInfo(PRequestInfo pRi);
		//判断是否需要请求，主要用于控制用户不能频繁点击
		bool ShouldRequest(CDataHandler* pHandler, int nTag, const char* lpszUrl);

		//LUA
		void LuaHandleData(void* pScene, int nTag, int nNetRet, CStream* lpData, LPVOID lpExternal);
		bool LuaHandlePushData(CStream* lpData);
		void LuaHandleErrorData();
	public:
		bool LuaHandlePushDataWithInt(int p);
	private:
		void Initialize();
		void Finitialize();
	private:
		unsigned int m_sgAsyncThreadID;
		CThreadMutex m_ThreadMutex;
		map<CNetClientBase*, PRequestInfo>* m_pMapRequestInfos;		
		list<PRequestInfo> m_RequestInfos;
		list<CHttpClientResponse*> m_PushResponseInfos;
		list<int> m_ErrorInfos;
		CHttpClient* m_pOnlyOneClient; 
		CHttpSession* m_pOnlyOneSession;
		CTcpClient* m_pTcpClient;
		LUA_DATAHANDLE_CALLBACK m_pLuaDataHandleCallBack;
	};
}

#endif //_SCUT_DATAREQUEST_H_