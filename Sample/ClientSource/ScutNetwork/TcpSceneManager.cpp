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
#include "TcpSceneManager.h"
#include "AutoGuard.h"
#include "ZipUtils.h"
#include <vector>
#include "TcpClient.h"

using namespace ScutDataLogic;
using namespace std;

namespace ScutNetwork
{
	#define MAXINTERFACES 16 
	#define RECV_BUF_SIZE 1024 * 1024 //1M
	unsigned short	CTcpClient::port = 9527;
	CThreadMutex s_RecvThreadMutex;
	static bool s_ThreadExit = false;
	unsigned int m_sLisnterThreadID;

	CTcpSceneManager* CTcpSceneManager::instance = NULL;

	CTcpSceneManager::CTcpSceneManager(void)
	{
		curl_handle = NULL;
		m_pNetNotify = NULL;
	}

	CTcpSceneManager::~CTcpSceneManager(void)
	{
		s_ThreadExit = true;
		curl_handle = NULL;
		m_pNetNotify = NULL;
		instance = NULL;
	}

	CTcpSceneManager* CTcpSceneManager::getInstance()
	{
		if (NULL == instance)
		{
			instance = new CTcpSceneManager();
		}
		return instance;
	}
	

	void CTcpSceneManager::setNotify( INetStatusNotify* pNetNotify )
	{
		m_pNetNotify = pNetNotify;
	}

	void CTcpSceneManager::startListening()
	{
		s_ThreadExit = false;
		//启动监听线程
#ifdef SCUT_WIN32
		CreateThread(NULL, 0, (LPTHREAD_START_ROUTINE)AsyncListenerThreadProc, this, 0, (LPDWORD)(&m_sLisnterThreadID));
#else
		pthread_create((pthread_t*)&m_sLisnterThreadID, NULL, AsyncListenerThreadProc, this);
#endif
	}

	void CTcpSceneManager::setUrlHandle( CURL* handle )
	{
		curl_handle = handle;
	}

	vector<CHttpClientResponse*> GZipUnZipTcpStream(unsigned char* pszData, unsigned int nLen)
	{
		vector<CHttpClientResponse*> ret;
		int nPackSize = 0;		
		int nDelta = 0;//目前读取的长度
		while (nDelta + 4 < nLen)
		{
			//读取头长度
			unsigned char chPackSizes[4] = {0};
			memcpy(chPackSizes, pszData, 4);
			pszData += 4;
			nDelta += 4;
			//获取头长度
			nPackSize = *((int*)&chPackSizes[0]);
			if (nLen - nDelta < nPackSize)
			{
				break;;
			}
			unsigned char* pszBuffer = new unsigned char[nPackSize];
			memset(pszBuffer, 0, nPackSize);
			memcpy(pszBuffer, pszData, nPackSize);
			pszData += nPackSize;
			nDelta += nPackSize;
			int nLen = 0;
			unsigned char* pszOut = NULL;
			if (pszBuffer[0] == 0x1f && pszBuffer[1] == 0x8b && pszBuffer[2] == 0x08 && pszBuffer[3] == 0x00)
			{				
				unsigned char* pszOut;
				int nLen = ZipUtils::ccInflateMemory((unsigned char*)pszBuffer, nPackSize, (unsigned char**)&pszOut);
				if (nLen > 0)
				{
					CHttpClientResponse* pResp = new CHttpClientResponse();
					CMemoryStream* pMem = new CMemoryStream();
					pResp->SetTarget(pMem);
					pMem->WriteBuffer((char*)pszOut, nLen);
					ret.push_back(pResp);
					delete []pszOut;
				}					
			}
			else
			{
				CHttpClientResponse* pResp = new CHttpClientResponse();
				CMemoryStream* pMem = new CMemoryStream();
				pResp->SetTarget(pMem);
				pMem->WriteBuffer((char*)pszBuffer, nPackSize);
				ret.push_back(pResp);
			}	
			delete []pszBuffer;
		}
		return ret;
	}

	void GZipUnZipTcpStream(CStream* pStm)
	{
		if (pStm->GetSize() > 0)
		{
			int nPackageSize = pStm->GetSize() - 4;

			pStm->SetPosition(4);
			char* pszBuffer = new char[nPackageSize];
			memset(pszBuffer, 0, nPackageSize);
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

	void sleep(int time)
	{
#ifdef SCUT_WIN32
		Sleep(time);
#elif SCUT_ANDROID
		usleep(time);
#else
		usleep(time);
#endif
	}

	bool CheckStmSize(unsigned char* pszStmData, int nCheckSize)
	{
		unsigned char chPackSizes[4] = {0};
		memcpy(chPackSizes, pszStmData, 4);
		int nPackSize = *((int*)&chPackSizes[0]);
		return nCheckSize >= nPackSize;
	}

#ifdef SCUT_WIN32
	DWORD WINAPI CTcpSceneManager::AsyncListenerThreadProc(LPVOID puCmd)
#else
	void* CTcpSceneManager::AsyncListenerThreadProc( void * puCmd )
#endif
	{	
		CTcpSceneManager* pTcpSceneManager = (CTcpSceneManager*)puCmd;

		unsigned char* temp = new unsigned char[RECV_BUF_SIZE];
		memset(temp, 0, RECV_BUF_SIZE);
		int nRecvLen = 0; //用来判断流的位置，目前不大改的情况下处理
		int nTotalLen = 0;
		for(;;){ 
			if (s_ThreadExit)
			{
				ScutNetwork::CTcpClient::FreeCurlHandler(pTcpSceneManager->curl_handle);
				break;
			}			

			int socket;
			CURLcode res = curl_easy_getinfo(pTcpSceneManager->curl_handle, CURLINFO_LASTSOCKET, &socket);

			if(res != CURLE_OK){				
				return NULL;
			}

			int nResult = CTcpClient::wait_on_socket(socket, 1, 10);
			if(nResult == 0){
				continue;
			}
			else if (nResult == -1)
			{
				if (pTcpSceneManager->m_pNetNotify)
				{
					AsyncInfo asyncInfo;
					asyncInfo.ProtocalType = 1;
					asyncInfo.Status = aisFailed;
					pTcpSceneManager->m_pNetNotify->OnNotify(&asyncInfo);
				}
				//通知业务层接收失败
				ScutNetwork::CTcpClient::FreeCurlHandler(pTcpSceneManager->curl_handle);
				delete pTcpSceneManager;
				instance = NULL;
				break;
			}
			
			//memset(temp, 0, RECV_BUF_SIZE);

			if (!pTcpSceneManager->curl_handle)
			{
				break;
			}
			size_t recv_len = 0;
			res = curl_easy_recv(pTcpSceneManager->curl_handle, temp + nRecvLen, RECV_BUF_SIZE - nRecvLen, &recv_len); 
			if ((res == CURLE_OK && recv_len != 0) || res == CURLE_OPERATION_TIMEDOUT)
			{
				nTotalLen += recv_len;
				nRecvLen += recv_len;
				if (!CheckStmSize((unsigned char*)temp, nTotalLen))
				{
					continue;
				}
				//通知				
				if (pTcpSceneManager->m_pNetNotify)
				{
					vector<CHttpClientResponse*> vecResps = GZipUnZipTcpStream((unsigned char*)temp, nTotalLen);
					for (unsigned int i = 0; i < vecResps.size(); i++)
					{
						AsyncInfo asyncInfo;
						asyncInfo.ProtocalType = 1;
						asyncInfo.Status = (res == CURLE_OK) ? aisSucceed : aisTimeOut;

						asyncInfo.Response = vecResps[i];
						int nMsgId = 0;
						ScutSystem::CMemoryStream* lpData = (ScutSystem::CMemoryStream*)asyncInfo.Response->GetTarget();
						if (lpData)
						{
							nMsgId = pTcpSceneManager->getRmID((char*)lpData->GetMemory(), lpData->GetSize());
							TcpData* pData = pTcpSceneManager->getSceneByTag(nMsgId);
							if (pData)
							{
								asyncInfo.Data1 = pData->Tag;
								asyncInfo.pScene = pData->pScene;
							}
						}
						pTcpSceneManager->m_pNetNotify->OnNotify(&asyncInfo);
						pTcpSceneManager->erase(nMsgId);
						asyncInfo.Response = NULL;
					}
				}
			}
			memset(temp, 0, RECV_BUF_SIZE);
			nTotalLen = 0;
			nRecvLen = 0;
		} 
		delete []temp;
		return NULL;
	}

	bool CTcpSceneManager::isListening()
	{
		return curl_handle != NULL;
	}

	void CTcpSceneManager::push( int nMsgId, int nTag, void* pScene )
	{
		AUTO_GUARD(s_RecvThreadMutex);
		map<int, TcpData*>::iterator it = m_mapScene.find(nTag);
		if (it == m_mapScene.end())
		{
			TcpData* pData = new TcpData();
			pData->pScene = pScene;
			pData->Tag = nTag;
			m_mapScene.insert(std::pair<int, TcpData*>(nMsgId, pData));	
		}
	}

	void CTcpSceneManager::erase( int nTag )
	{
		AUTO_GUARD(s_RecvThreadMutex);
		map<int, TcpData*>::iterator it = m_mapScene.find(nTag);
		if (it != m_mapScene.end())
		{
			delete it->second;
			m_mapScene.erase(it);
		}
	}

	TcpData* CTcpSceneManager::getSceneByTag( int nTag )
	{
		AUTO_GUARD(s_RecvThreadMutex);
		map<int, TcpData*>::iterator it = m_mapScene.find(nTag);
		if (it != m_mapScene.end())
		{
			return it->second;
		}

		return NULL;
	}

	int CTcpSceneManager::getRmID( char* pdataStream ,int wSize)
	{
		int nStart = 0;
		int nStreamSize = getNumberValue(pdataStream, wSize, nStart, 4);
		if(nStreamSize != wSize)
		{
			return -1;
		}

		int nResult = getNumberValue(pdataStream, wSize, nStart, 4);

		int nRmId = getNumberValue(pdataStream, wSize, nStart, 4);

		return nRmId;
	}

	int CTcpSceneManager::getNumberValue( char* pdataStream, int wSize, int& nStart, int nLength)
	{
		if(nStart + nLength > wSize )
		{
			return -1;	
		}

		int dw;
		memcpy(&dw, pdataStream + nStart, sizeof(int));
		nStart=nStart+4;

		return dw;
	}

	void CTcpSceneManager::release()
	{
		curl_handle = NULL;
		s_ThreadExit = true;
	}

}


		