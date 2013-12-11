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
#ifndef TCPSCENEMANAGER_H
#define TCPSCENEMANAGER_H

//用于管理 ScutScene
#include <map>
#include "TcpClient.h"

using namespace std;
namespace ScutNetwork
{
	class TcpData
	{
	public:
		int Tag;
		void* pScene;

		TcpData()
		{
			Tag = 0;
			pScene = NULL;
		}

		~TcpData()
		{

		}
	};

	//场景管理类，ScutScene类对象在生成时将自动加入到CSceneManager里，无需手动加入
	class CTcpSceneManager
	{
	public:
		~CTcpSceneManager(void);
		static CTcpSceneManager* getInstance();

		void push( int nMsgId, int nTag, void* pScene );
		void erase( int nTag );
		TcpData* getSceneByTag( int nTag );
		void startListening();
		bool isListening();
		void setNotify(INetStatusNotify* pNetNotify);
		void setUrlHandle(CURL*   handle);
		void release();
	protected:
		int getRmID(char* pdataStream, int wSize);
		int getNumberValue(char* pdataStream, int wSize, int& nStart, int nLength);
#ifdef SCUT_WIN32
		static DWORD WINAPI AsyncListenerThreadProc(LPVOID puCmd);
#else
		static void* AsyncListenerThreadProc(void * puCmd);
#endif	
	protected:
		static CTcpSceneManager* instance;
		CTcpSceneManager(void);
		CURL*   curl_handle;
		INetStatusNotify* m_pNetNotify;
		map<int, TcpData*> m_mapScene;
	};
}


#endif//SCENEMANAGER_H