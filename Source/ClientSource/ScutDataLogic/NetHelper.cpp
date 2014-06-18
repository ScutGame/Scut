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
#include "NetHelper.h"
#ifndef SCUT_WIN32
#include <ctype.h>
#endif
#include "md5.h"
#include "Int64.h"
namespace ScutDataLogic
{

	int CNetWriter::s_Counter		 = 1;
	int CNetWriter::s_CounterTemp = 1;
	std::string CNetWriter::s_Key;
	unsigned long long CNetWriter::s_userID = 0;
	std::string CNetWriter::s_strSessionID;
	std::string CNetWriter::s_strSt;
	std::string CNetWriter::s_md5Key = "44CAC8ED53714BF18D60C5C7B6296000";
	std::string CNetWriter::s_strUrl;
	std::string CNetWriter::s_strPostData;
	std::string CNetWriter::s_strUserData;
	CNetWriter* CNetWriter::instance = NULL;

	CNetWriter* CNetWriter::getInstance()
	{
		if (instance == NULL)
		{
			instance = new CNetWriter();
		}
		return instance;
	}
	CNetWriter::CNetWriter(void)
	{
		resetData();
	}

	CNetWriter::~CNetWriter(void)
	{
		instance = NULL;
	}
	void CNetWriter::resetData()
	{
		s_strPostData.clear();
		s_strUserData.clear();

		s_CounterTemp = s_Counter;
		char szBuf[256] = {0};
#ifdef SCUT_WIN32
		sprintf(szBuf, "MsgId=%d&Sid=%s&Uid=%I64d&St=%s", s_Counter, s_strSessionID.c_str(), s_userID, s_strSt.c_str());
#else
		sprintf(szBuf, "MsgId=%d&Sid=%s&Uid=%lld&St=%s", s_Counter, s_strSessionID.c_str(), s_userID, s_strSt.c_str());
#endif
	
		 ++s_Counter;
		s_strUserData += szBuf;
	}

	void CNetWriter::resetDataNoRefCount()
	{
		s_strPostData.clear();
		s_strUserData.clear();

		char szBuf[256] = {0};
#ifdef SCUT_WIN32
		sprintf(szBuf, "MsgId=%d&Sid=%s&Uid=%I64d&St=%s", s_CounterTemp, s_strSessionID.c_str(), s_userID, s_strSt.c_str());
#else
		sprintf(szBuf, "MsgId=%d&Sid=%s&Uid=%lld&St=%s", s_CounterTemp, s_strSessionID.c_str(), s_userID, s_strSt.c_str());
#endif
		s_strUserData += szBuf;
	}

	void CNetWriter::writeInt32(const char* szKey, int nValue)
	{
		char szBuf[256] = {0};
		sprintf(szBuf, "&%s=%d", szKey, nValue);
		s_strUserData += szBuf;

	}
	void CNetWriter::writeFloat(const char* szKey, float fvalue)
	{
		char szBuf[256] = {0};
		sprintf(szBuf, "&%s=%f", szKey, fvalue);
		s_strUserData += szBuf;

	}
	void CNetWriter::writeString(const char* szKey, const char* szValue)//
	{
		if (szValue == NULL)
		{
			return ;
		}
		char * pOut = new char[strlen(szValue) *3 + 1];
		memset(pOut, 0, sizeof(char) * (strlen(szValue) *3 + 1));
		url_encode(szValue, pOut, strlen(szValue) *3);
		char szTemp[256] = {0};
		sprintf(szTemp, "&%s=", szKey);
		s_strUserData	+= szTemp;
		s_strUserData	+= pOut;
		delete []pOut;
	}
	void CNetWriter::writeInt64(const char* szKey, unsigned long long nValue)
	{
		char szBuf[256] = {0};
#ifdef SCUT_WIN32
		sprintf(szBuf, "&%s=%I64d", szKey, nValue);
#else
		sprintf(szBuf, "&%s=%lld", szKey, nValue);
#endif
		s_strUserData += szBuf;

	}
	void CNetWriter::writeInt64(const char* szKey, CInt64& obj)
	{
		writeInt64(szKey, obj.getValue());
	}
	void CNetWriter::writeWord(const char* szKey, unsigned short sValue)
	{
		char szBuf[256] = {0};
#ifdef SCUT_WIN32
		sprintf(szBuf, "&%s=%d", szKey, sValue);
#else
		sprintf(szBuf, "&%s=%d", szKey, sValue);
#endif
		s_strUserData += szBuf;
	}
	void CNetWriter::writeBuf(const char* szKey, unsigned char* buf, int nSize)
	{
		assert(false);
	}
	void CNetWriter::setUrl(const char* szUrl)
	{
		s_strUrl = szUrl;
	}
	std::string& CNetWriter::generatePostData()
	{
		s_strPostData = s_strUrl;
		s_strPostData += "?d=";

		//md5
		//s_strUserData += "&pv2=1";
		std::string strtemp = s_strUserData;
		
		strtemp += s_md5Key;
		md5 alg;
		alg.Update((unsigned char*)strtemp.c_str(), strtemp.size());
		alg.Finalize();
		char * pMd5 = PrintMD5(alg.Digest());
		std::string strSign;
		if (pMd5)
		{
			strSign = pMd5;
			free(pMd5);
		}

		std::string str = s_strUserData + "&sign=";
		str += strSign.c_str();

 		char * pOut = new char[str.size() *3 + 1];
 		memset(pOut, 0, sizeof(char) * (str.size() *3 + 1));
 		url_encode((unsigned char*)str.c_str(), str.size(), pOut, str.size() *3);
 		s_strPostData += pOut;
 		delete []pOut;

		return s_strPostData;
	}

	void CNetWriter::url_encode(const char *src, char *dst, size_t dst_len) {
		static const char *dont_escape = "._-$,;~()";
		static const char *hex = "0123456789abcdef";
		const char *end = dst + dst_len;

		for (; *src != '\0' && dst < end; src++, dst++) {
			if (isalnum(*(const unsigned char *) src) ||
				strchr(dont_escape, * (const unsigned char *) src) != NULL) {
					*dst = *src;
			} else if (dst + 2 < end) {
				dst[0] = '%';
				dst[1] = hex[(* (const unsigned char *) src) >> 4];
				dst[2] = hex[(* (const unsigned char *) src) & 0xf];
				dst += 2;
			}
		}

		*dst = '\0';
	}

	int CNetWriter::url_encode(unsigned char* src, int src_len, char* dst, int dst_len)
	{
		static const char *dont_escape = "._-$,;~()";
		static const char *hex = "0123456789abcdef";
		const char *end = dst + dst_len;
		int Scutstlen = 0;
		for (int i = 0; i < src_len && dst < end; src++, i++, dst++, Scutstlen++) {
			if (isalnum(*(const unsigned char *) src) ||
				strchr(dont_escape, * (const unsigned char *) src) != NULL) {
					*dst = *src;
			} else if (dst + 2 < end) {
				dst[0] = '%';
				dst[1] = hex[(* (const unsigned char *) src) >> 4];
				dst[2] = hex[(* (const unsigned char *) src) & 0xf];
				dst += 2;
				Scutstlen+= 2;
			}
		}

		*dst = '\0';

		return Scutstlen;
	}


	void CNetWriter::setSessionID(const char* pszSessionID)
	{
		if (pszSessionID != NULL)
		{
			s_strSessionID = pszSessionID;
			//resetData();
			resetDataNoRefCount();
		}
	}
	void CNetWriter::setUserID(CInt64 value)
	{
		s_userID = value.getValue();
		//resetData();
		resetDataNoRefCount();
	}

	void CNetWriter::setStime(const char* pszTime)
	{
		if (pszTime != NULL)
		{
			s_strSt = pszTime;
			//resetData();
			resetDataNoRefCount();
		}
	}

	int CNetWriter::getTag()
	{
		return s_CounterTemp;
	}
	/*******************************************CNetReader**********************************/
	CNetReader* CNetReader::instance = NULL;
	CNetReader* CNetReader::getInstance()
	{
		if (NULL == instance)
		{
			instance = new CNetReader();
		}
		return instance;
	}

	CNetReader::CNetReader()
	{
		m_nResult	= 0;
		m_nRmId		= 0;
		m_nActionID	= 0;
		m_pStrStime = new CLuaString();
		m_pStrErrMsg= new CLuaString();
	}
	CNetReader::~CNetReader()
	{
		if (m_pStrStime)
		{
			delete m_pStrStime;
			m_pStrStime = NULL;
		}
		if (m_pStrErrMsg)
		{
			delete m_pStrErrMsg;
			m_pStrErrMsg = NULL;
		}
		instance = NULL;
	}
	void CNetReader::getString(CLuaString* pOutString, int nLength)
	{
		if (nLength <= 0)
		{
			return;
		}
		pOutString->setString(CNetStreamExport::getString(nLength));
		CNetStreamExport::freeStringBuffer();

	}

	bool CNetReader::pushNetStream(char* pdataStream,int wSize)
	{
		if (!CNetStreamExport::pushNetStream(pdataStream, wSize))
		{
			ScutLog("pushNetStream return false");
			return false;
		}		
		
		
		m_nResult = getInt();

		m_nRmId = getInt();

		int nErrorMsgSize = getInt();
		wchar_t* pszTemp = NULL;
		if (nErrorMsgSize)
		{
			getString(m_pStrErrMsg, nErrorMsgSize);
		}
		else
		{
			m_pStrErrMsg->setString("");
		}

		m_nActionID= getInt();

		//ScutLog("CNetReader::pushNetStream actionid is: %d m_pStrErrMsg is: %s", m_nActionID, m_pStrErrMsg->getCString());

		int nStSize = getInt();
		if (nStSize)
		{
			getString(m_pStrStime, nStSize);
			CNetWriter::setStime(m_pStrStime->getCString());
		}

		return true;
		//userData
	}

	//getMethod
	int			CNetReader::getResult()
	{		
		return m_nResult;
	}
	int			CNetReader::getRmId()
	{
		return m_nRmId;
	}
	int			CNetReader::getActionID()
	{
		return m_nActionID;
	}
	CLuaString* CNetReader::getErrMsg()
	{
		return m_pStrErrMsg;
	}
	CLuaString* CNetReader::getStrStime()
	{
		return m_pStrStime;
	}

	CInt64 CNetReader::getCInt64()
	{
		return CInt64(getInt64());
	}
	unsigned char	CNetReader::getByte(void)
	{
		return getBYTE();
	}

	unsigned short	CNetReader::getWord(void)
	{
		return getWORD();
	}

}
