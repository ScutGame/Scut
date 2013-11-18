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
#ifndef _HTTP_CLIENT_RESPONSE_H_
#define _HTTP_CLIENT_RESPONSE_H_

//#include "Defines.h"
#include "Stream.h"

using namespace ScutSystem;

namespace ScutNetwork
{
	class CHttpClientResponse  
	{
	public:
		CHttpClientResponse();
		virtual ~CHttpClientResponse();

		char * GetBodyPtr();
		unsigned int GetBodyLength();
		bool DataContains(const char *searchStr);
		bool ContentTypeContains(char *searchStr);
		void Reset();		
		void SetContentType(char *contentType);		

		char* GetTargetFile();
		void SetTargetFile(const char* pszFileName);

		ScutSystem::CStream* GetTarget();
		//注意：为了方便使用，这边传入的流指针将会被管理（根据需要进行释放）
		void SetTarget(CStream* pTarget);

		//是否使用断点续传
		bool GetUseDataResume();
		void SetUseDataResume(bool bUse = false);

		const char* GetRequestUrl();
		void SetRequestUrl(const char* pszUrl);

		const char* GetLastResponseUrl();
		void SetLastResponseUrl(const char* pszUrl);

		int GetTargetRawSize();

		int GetStatusCode();
		void SetStatusCode(int nCode);

		void SetSendData(const char* data, unsigned int dataLen);
		const char* GetSendData();
		unsigned int GetSendDataLength();

	protected:
		void SetData(const char *response, unsigned int responseLen);
	private:
		unsigned int m_nDataSize;
		char* m_pData;
		char m_cContentType[255];
		char m_cTargetFile[255];
		char* m_pszRequestUrl;
		char* m_pszLastResponseUrl;
		CStream* m_pTarget;
		bool m_bUseDataResume;
		int m_nRawTargetSize;
		int m_nStatusCode;

		char* m_pSendData;
		unsigned int m_nSendDataSize;
	};
}


#endif

