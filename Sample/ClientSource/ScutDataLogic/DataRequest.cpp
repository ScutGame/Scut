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


#include "StdAfx.h"
#include "DataRequest.h"
#include "DataHandler.h"
#include "ScutUtility.h"
#include "FileHelper.h"
#include "LuaHost.h"
#include "NetStreamExport.h"
#include "AutoGuard.h"
#include "ScutUtility.h"
#include "NetHelper.h"
#include "TcpSceneManager.h"
#include "CCDirector.h"
using namespace ScutSystem;
using namespace ScutDataLogic;

const int MSG_DATAREQUEST_TERMINATED = 0x1080;

const int TICK_ACTIONLIMIT = 1000; //1秒

//#ifdef SCUT_WIN32
static bool g_bUseSequenceNetRequest = false;
//#else
//static bool g_bUseSequenceNetRequest = false;
//#endif

#define CS_LOGIC_LUAFILE "Logic.lua"

void l_error(lua_State *L, const char *fmt, ...) {
	va_list argp;
	va_start(argp, fmt);
	vfprintf(stderr, fmt, argp);
	va_end(argp);
	lua_close(L);
	exit(EXIT_FAILURE);
}

CDataRequest::CDataRequest(void)
{
	//请求列表
	m_pMapRequestInfos = new map<CNetClientBase*, PRequestInfo>();
	m_sgAsyncThreadID = 0;
	m_pLuaDataHandleCallBack = NULL;
	if (g_bUseSequenceNetRequest)
	{
		m_pOnlyOneSession = new CHttpSession();
		m_pOnlyOneClient = new CHttpClient(m_pOnlyOneSession);
	}
	CTcpSceneManager::getInstance()->setNotify(this);
	//初始化
	Initialize();
}

CDataRequest::~CDataRequest(void)
{
	Finitialize();
	//释放剩下的请求
	AUTO_GUARD(m_ThreadMutex);
	if (m_pMapRequestInfos)
	{
		map<CNetClientBase*, PRequestInfo>::iterator it = m_pMapRequestInfos->begin();
		for (; it != m_pMapRequestInfos->end(); it++)
		{
			FreeRequestInfo(it->second);
		}
		delete m_pMapRequestInfos;
	}
	if (m_pOnlyOneClient)
	{
		delete m_pOnlyOneClient;
	}
	if (m_pOnlyOneSession)
	{
		delete m_pOnlyOneSession;
	}
}

CDataRequest* ScutDataLogic::CDataRequest::Instance()
{
	static CDataRequest g_DataRequestInst;
	return &g_DataRequestInst;
}

bool ScutDataLogic::CDataRequest::ExecTcpRequest(CDataHandler* pHandler, const char* lpszUrl, const char* lpSendData, unsigned int nDataLen)
{
	if (pHandler)
	{
		if (m_pTcpClient == NULL)
		{
			m_pTcpClient = new CTcpClient();
		}

		if (nDataLen == 0)
		{
			nDataLen = strlen(lpSendData);
		}

		CHttpClientResponse resp;
		resp.SetTarget(new CMemoryStream());
		resp.SetSendData(lpSendData, nDataLen);
		if (!m_pTcpClient->IsBusy())
		{
			//直接执行同步请求
			if (m_pTcpClient->TcpGet(lpszUrl, resp) == 0) //成功
			{		
				//在此判断是否存在
				if (pHandler && pHandler->IsAlive())
				{
					return pHandler->HandleData(0, *resp.GetTarget(), NULL);
				}			
			}
		}
		else
		{
			CTcpClient* pTcpClient = new CTcpClient();

			if (pTcpClient)
			{
				//直接执行同步请求
				if (pTcpClient->TcpGet(lpszUrl, resp) == 0) //成功
				{		
					//在此判断是否存在
					if (pHandler && pHandler->IsAlive())
					{
						return pHandler->HandleData(0, *resp.GetTarget(), NULL);
					}			
				}
			}
		}

	}
	return false;
}

bool ScutDataLogic::CDataRequest::ExecRequest( CDataHandler* pHandler, const char* lpszUrl )
{
	if (pHandler)
	{
		CHttpSession hs;
		CHttpClient hc(&hs);
		CHttpClientResponse resp;
		resp.SetTarget(new CMemoryStream());
		//直接执行同步请求
		if (hc.HttpGet(lpszUrl, resp) == 0) //成功
		{		
			//在此判断是否存在
			if (pHandler && pHandler->IsAlive())
			{
				return pHandler->HandleData(0, *resp.GetTarget(), NULL);
			}			
		}
	}
	return false;
}

bool ScutDataLogic::CDataRequest::ExecRequest(void* pScene)
{
	const std::string strPostData = CNetWriter::getInstance()->generatePostData();
	if (strPostData.size())
	{
		CHttpSession hs;
		CHttpClient hc(&hs);
		CHttpClientResponse resp;
		resp.SetTarget(new CMemoryStream());
		//直接执行同步请求
		int nRet = hc.HttpGet(strPostData.c_str(), resp);
		//处理LUA数据
		CNetWriter::getInstance()->resetData();
		if (nRet == 0)
		{
			LuaHandleData(pScene, 0, aisSucceed, resp.GetTarget(), NULL);
		}
		else
		{
			LuaHandleData(pScene, 0, 0, resp.GetTarget(), NULL);
		}
	}
	return false;
}

DWORD ScutDataLogic::CDataRequest::AsyncExecRequest(CDataHandler* pHandler, int nTag, const char* lpszUrl,LPVOID lpData, void* pScene)
{
	//while(g_bUseSequenceNetRequest && m_pOnlyOneClient->IsBusy())
	//{
	//	Sleep(50);
	//}
	DWORD dwRet = 0;
	if (/*pHandler && ShouldRequest(pHandler, nTag, lpszUrl)*/1)
	{
		//加入请求列表
		AUTO_GUARD(m_ThreadMutex);
		PRequestInfo pRi = new RequestInfo();		
		if (g_bUseSequenceNetRequest && !m_pOnlyOneClient->IsBusy() && m_RequestInfos.size() == 0)
		{
			pRi->NetClient = m_pOnlyOneClient;
			pRi->NetClient->Reset();
			pRi->NetClient->AddHeader("Connection", "Keep-Alive");
			//pRi->HttpClient->AddHeader("keep-alive", "500");	
		}
		else
		{
			pRi->HttpSession = new CHttpSession();
			pRi->NetClient = new CHttpClient(pRi->HttpSession);
			pRi->NetClient->AddHeader("Connection", "Keep-Alive");
			//pRi->HttpClient->AddHeader("keep-alive", "500");			
		}		
		pRi->Handler = pHandler;
		pRi->HandlerTag = nTag;
		pRi->HttpResponse = new CHttpClientResponse();
		pRi->HttpResponse->SetTarget(new CMemoryStream());
		pRi->Url = lpszUrl;
		pRi->LPData = lpData;
		pRi->pScene = pScene;
		pRi->NetClient->SetNetStautsNotify(this);
		(*m_pMapRequestInfos)[pRi->NetClient] = pRi;		
		//执行异步网络请求
		if (pRi->NetClient->AsyncNetGet(pRi->Url.c_str(), pRi->HttpResponse) == 0)
		{
			dwRet = (DWORD)pRi->NetClient;
		}
	}
	return dwRet;
}

DWORD ScutDataLogic::CDataRequest::AsyncExecRequest(void* pScene, const char* szUrl, int nTag, void* lpData )
{
	DWORD nRet =  this->AsyncExecRequest(NULL, nTag, szUrl, lpData, pScene);
	
	return nRet;
}

//异步执行请求
DWORD ScutDataLogic::CDataRequest::AsyncExecTcpRequest(CDataHandler* pHandler, int nTag, const char* lpszUrl, LPVOID lpData, void* pScene, const char* lpSendData, unsigned int nDataLen)
{
	DWORD dwRet = 0;
	if (/*pHandler && ShouldRequest(pHandler, nTag, lpszUrl)*/1)
	{		
		//加入请求列表
		AUTO_GUARD(m_ThreadMutex);

		if (lpSendData && nDataLen == 0)
		{
			nDataLen = strlen(lpSendData);
		}

		if (nDataLen != 0)
		{
			char* pSendData    = new char[nDataLen + 4];
			memset(pSendData, 0, nDataLen + 4);

			char header[4] = {0};

			char first = (nDataLen & 0x000000ff);
			header[0] = first;
			char second = (nDataLen & 0x0000ff00) >> 8;
			header[1] = second;
			char third = (nDataLen & 0x00ff0000) >> 16;
			header[2] = third;
			char forth = nDataLen >> 24;
			header[3] = forth;

			memcpy((char*)((intptr_t)pSendData), header, 4);
			memcpy(pSendData + 4, lpSendData, nDataLen);

			lpSendData = pSendData;
		}

		CTcpClient* pTcpClient = new CTcpClient();

		CHttpClientResponse* HttpResponse = new CHttpClientResponse();
		HttpResponse->SetTarget(new CMemoryStream());
		HttpResponse->SetSendData(lpSendData, nDataLen + 4);
		
		int nMsgid = CNetWriter::getTag();
		//执行异步网络请求
		//if (pTcpClient->AsyncNetGet(lpszUrl, HttpResponse) == 0)
		//{
		//	dwRet = (DWORD)0;
		//}
		int nResult = pTcpClient->TcpGet(lpszUrl, *HttpResponse);
		if (nResult == 0)
		{
			CTcpSceneManager::getInstance()->push(nMsgid, nTag, pScene);
			dwRet = (DWORD)0;
		}
		else
		{
			PRequestInfo pRi = new RequestInfo();
			pRi->pScene = pScene;
			pRi->HttpResponse = HttpResponse;
			pRi->Status = nResult;
			pRi->HandlerTag = nMsgid;
			m_RequestInfos.push_back(pRi);
		}
	}
	return dwRet;
}

DWORD ScutDataLogic::CDataRequest::AsyncExecTcpRequest(void* pScene, const char* szUrl, int nTag, void* lpData, const char* lpSendData, unsigned int nDateLen)
{	
	DWORD nRet =  this->AsyncExecTcpRequest(NULL, nTag, szUrl, lpData, pScene, lpSendData, nDateLen);
 
	return nRet;
}

void ScutDataLogic::CDataRequest::ProcAsyncInfo( PAsyncInfo pAi )
{
	if (pAi->ProtocalType == 0)
	{
		map<CNetClientBase*, PRequestInfo>::iterator it;
		it = m_pMapRequestInfos->find(pAi->Sender); 
		if (it != m_pMapRequestInfos->end())
		{
			PRequestInfo pRi = it->second;
			if (pRi->Handler)	//C++处理方式
			{
				switch (pAi->Status)
				{
				case aisSucceed:	
					pRi->Handler->HandleData(pRi->HandlerTag, *pAi->Response->GetTarget(), pRi->LPData);
					//从请求列表中移除
					FreeRequestInfo(pRi);					
					//LuaHandleData(pRi->HandlerTag, pAi->Status, *pAi->Response->GetTarget(), pRi->LPData);
					m_pMapRequestInfos->erase(it);
					break;
				case aisProgress:
					pRi->Handler->HandleProgress(pRi->HandlerTag, pAi->Data1, pRi->LPData);
					break;
				case aisFailed:
				case aisTimeOut:
					//从请求列表中移除
					pRi->Handler->HandleFailed(pRi->HandlerTag, pAi->Status == aisTimeOut ? reNetTimeOut : reNetFailed);
					FreeRequestInfo(pRi);
					//LuaHandleData(pRi->HandlerTag, pAi->Status, *pAi->Response->GetTarget(), pRi->LPData);
					m_pMapRequestInfos->erase(it);
					break;
				}
			}
			else	//LUA处理方式
			{
				//加入到请求列表中
				m_pMapRequestInfos->erase(it);
				pRi->Status = pAi->Status;
				m_RequestInfos.push_back(pRi);
			}
		}
	}
	else if (pAi->ProtocalType == 1)
	{
		PRequestInfo pRi = new RequestInfo();
		pRi->pScene = pAi->pScene;
		pRi->HttpResponse = pAi->Response;
		pRi->Status = pAi->Status;
		pRi->HandlerTag = pAi->Data1;
		m_RequestInfos.push_back(pRi);
	}
}

void ScutDataLogic::CDataRequest::FreeRequestInfo( PRequestInfo pRi )
{
	AUTO_GUARD(m_ThreadMutex);
	if (g_bUseSequenceNetRequest && pRi->NetClient == m_pOnlyOneClient)
	{
		pRi->NetClient = NULL;
	}

	if (pRi->NetClient == m_pTcpClient)
	{
		pRi->NetClient = NULL;
	}

	if (pRi)
	{
		delete pRi;
	}
}

bool ScutDataLogic::CDataRequest::CancelAsyncRequest( DWORD dwRequestID )
{
	//TODO
	return false;
}

bool ScutDataLogic::CDataRequest::ShouldRequest( CDataHandler* pHandler, int nTag, const char* lpszUrl )
{
	//考虑到数量不是很多，从头到尾跑一次，之后可以通过map或hash来实现
	AUTO_GUARD(m_ThreadMutex);
	map<CNetClientBase*, PRequestInfo>::iterator it = m_pMapRequestInfos->begin();
	PRequestInfo pRi;
	for (; it != m_pMapRequestInfos->end(); it++)
	{
		pRi = it->second;
		if (pRi->Handler == pHandler && pRi->HandlerTag == nTag && pRi->Url.compare(lpszUrl) == 0 && (CScutUtility::GetTickCount() - pRi->StartTime) > TICK_ACTIONLIMIT )
		{
			return false;
		}
	}
	return true;
}

void ScutDataLogic::CDataRequest::Initialize()
{	 
}

void ScutDataLogic::CDataRequest::Finitialize()
{
}

void ScutDataLogic::CDataRequest::OnNotify( PAsyncInfo pAi )
{
	if (!pAi)
	{
		return;
	}
	AUTO_GUARD(m_ThreadMutex);
	if (pAi->ProtocalType == 1 && pAi->Data1 == 0)//通知
	{
		m_PushResponseInfos.push_back(pAi->Response);
	}
	else if (pAi->ProtocalType == 1 && pAi->Data1 == -1)//Socket Error
	{
		m_ErrorInfos.push_back(pAi->Data1);
	}
	else
	{
		ProcAsyncInfo(pAi);
	}
}

void ScutDataLogic::CDataRequest::LuaHandleData(void* pScene, int nTag, int nNetRet, CStream* lpData, LPVOID lpExternal )
{
	//ScutLog("LuaHandleData nTag:%d", nTag);
	if (m_pLuaDataHandleCallBack)
	{
		(*m_pLuaDataHandleCallBack)(pScene, nTag, nNetRet, lpData, lpExternal);
	}

	lua_State* pState = LuaHost::Instance()->GetLuaState();
	lua_getglobal(pState, "OnHandleData");
	CC_ASSERT( lua_isfunction(pState, -1) && "OnHandleData is not a function");

	lua_pushlightuserdata(pState, pScene);
	lua_pushnumber(pState, nTag);
	lua_pushnumber(pState, nNetRet);
	lua_pushnumber(pState, int(lpData));
	lua_pushnumber(pState, int(lpExternal));

	int nargs = 5;
	int traceback = 0;
	lua_getglobal(pState, "__G__TRACKBACK__");                         /* L: ... func arg1 arg2 ... G */
	if (!lua_isfunction(pState, -1))
	{
		lua_pop(pState, 1);                                            /* L: ... func arg1 arg2 ... */
	}
	else
	{
		traceback = -nargs-2;
		lua_insert(pState, traceback);                                 /* L: ... G func arg1 arg2 ... */
	}

	if (lua_pcall(pState, nargs, 0, traceback) != 0)
	{
		l_error(pState, "Call lua OnHandlerData failed: %s", lua_tostring(pState, -1));
	}

	lua_pop(pState, (traceback != 0) ? 2 : 1);
}

bool ScutDataLogic::CDataRequest::LuaHandlePushData( CStream* lpData )
{
	bool bRt = false;
	if (!lpData) return bRt;
	bool bPush = ScutDataLogic::CNetReader::getInstance()->pushNetStream((char*)((ScutSystem::CMemoryStream*)lpData)->GetMemory(), ((ScutSystem::CMemoryStream*)lpData)->GetSize());
	const char*  pushFunc = cocos2d::CCDirector::sharedDirector()->GetSocketPushHandler();
	if (pushFunc)
	{
		bRt = ScutDataLogic::LuaHost::Instance()->execFunc(pushFunc, NULL, bPush);
	}
	return bRt;
}

bool ScutDataLogic::CDataRequest::LuaHandlePushDataWithInt( int p )
{
	return LuaHandlePushData((CStream*)p);
}

void ScutDataLogic::CDataRequest::LuaHandleErrorData()
{
	bool bValue = false;
	const char*  pushFunc = cocos2d::CCDirector::sharedDirector()->GetSocketErrorHandler();
	if (pushFunc)
	{
		ScutDataLogic::LuaHost::Instance()->execFunc(pushFunc, NULL, bValue);
	}
}

void ScutDataLogic::CDataRequest::RegisterLUACallBack(LUA_DATAHANDLE_CALLBACK pCallBack)
{
	m_pLuaDataHandleCallBack = pCallBack;
}
void ScutDataLogic::CDataRequest::PeekLUAData()
{
	list<PRequestInfo> tempRequestInfo;
	list<CHttpClientResponse*> tempPushResponseInfos;
	list<int> tempErrorInfos;
	if (true)
	{
		m_ThreadMutex.Lock();
		list<PRequestInfo>::iterator itBegin = m_RequestInfos.begin();
		while (itBegin != m_RequestInfos.end())
		{
			PRequestInfo pRi = *itBegin;
			tempRequestInfo.push_back(pRi);
			itBegin++;
		}
		m_RequestInfos.clear();

		list<CHttpClientResponse*>::iterator it = m_PushResponseInfos.begin();
		while (it != m_PushResponseInfos.end())
		{
			CHttpClientResponse* pResponse = *it;
			tempPushResponseInfos.push_back(pResponse);
			it++;
		}
		m_PushResponseInfos.clear();

		list<int>::iterator itError = m_ErrorInfos.begin();
		while (itError != m_ErrorInfos.end())
		{
			int error = *itError;
			tempErrorInfos.push_back(error);
			itError++;
		}
		m_ErrorInfos.clear();
		
		m_ThreadMutex.Unlock();
	}

	list<PRequestInfo>::iterator itBeginTemp = tempRequestInfo.begin();
	while (itBeginTemp != tempRequestInfo.end())
	{
		PRequestInfo pRi = *itBeginTemp;
		//处理状态值
		int nStatus = pRi->HttpResponse->GetStatusCode();
		if (nStatus == 200)
		{
			nStatus = pRi->Status;			
		}
		LuaHandleData(pRi->pScene, pRi->HandlerTag, nStatus, pRi->HttpResponse->GetTarget(), pRi->LPData);
		//Sleep(10 * 1000);
		itBeginTemp++;
		FreeRequestInfo(pRi);
	}
	tempRequestInfo.clear();

	list<CHttpClientResponse*>::iterator ittemp = tempPushResponseInfos.begin();
	while (ittemp != tempPushResponseInfos.end())
	{
		CHttpClientResponse* pResponse = *ittemp;
		if (pResponse)
		{
			//通知
			LuaHandlePushData(pResponse->GetTarget());
			delete pResponse;
		}
		ittemp++;
	}
	tempPushResponseInfos.clear();

	list<int>::iterator itErrorTemp = tempErrorInfos.begin();
	while (itErrorTemp != tempErrorInfos.end())
	{
		//接收错误通知
		LuaHandleErrorData();
		itErrorTemp++;
	}
	tempErrorInfos.clear();
}

void ScutDataLogic::CDataRequest::SetUseKeepAliveMode( bool bKeepAlive )
{
	g_bUseSequenceNetRequest = false;
	return;
	if (g_bUseSequenceNetRequest != bKeepAlive)
	{
		g_bUseSequenceNetRequest = bKeepAlive;
		if (!m_pOnlyOneClient)
		{
			m_pOnlyOneSession = new CHttpSession();
			m_pOnlyOneClient = new CHttpClient(m_pOnlyOneSession);
		}		
	}	
}

bool ScutDataLogic::CDataRequest::GetUseKeepAliveMode()
{
	return g_bUseSequenceNetRequest;
}

ScutDataLogic::RequestInfo::RequestInfo()
{
	Handler = NULL;
	HandlerTag = 0;
	NetClient = NULL;
	HttpResponse = NULL;
	HttpSession = NULL;
	StartTime = CScutUtility::GetTickCount();
	Url = "";
	LPData = NULL;
	Status = 0;
	pScene = NULL;
}

ScutDataLogic::RequestInfo::~RequestInfo()
{
	if (HttpResponse)
	{
		delete HttpResponse;
	}	
	if (NetClient)
	{
		delete NetClient;
	}			
	if (HttpSession)
	{
		delete HttpSession;
	}
}
