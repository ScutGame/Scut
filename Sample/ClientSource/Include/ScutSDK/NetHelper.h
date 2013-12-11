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


#ifndef NETHELPER_H
#define NETHELPER_H
#include "Stream.h"
#include "NetStreamExport.h"
//由于解析网络流已经有对象解析  本类暂时用于打包请求数据流
namespace ScutDataLogic
{
	class CInt64;
	class CNetWriter
	{
	public:
		void writeInt32(const char* szKey, int nValue);
		void writeFloat(const char* szKey, float fvalue);
		void writeString(const char* szKey, const char* szValue);//
		void writeInt64(const char* szKey, unsigned long long nValue);
		void writeInt64(const char* szKey, CInt64& obj);
		void writeWord(const char* szKey, unsigned short sValue);
		void writeBuf(const char* szKey, unsigned char* buf, int nSize);
		static void setUrl(const char* szUrl);
		void url_encode(const char*str, char*dst, size_t dst_len);
		int url_encode(unsigned char* src, int src_len, char* dst, int dst_len);
		std::string& generatePostData();
		
		static CNetWriter* getInstance();
		static void resetData();
		static void resetDataNoRefCount();
		static void setSessionID(const char* pszSessionID);
		static void setUserID(CInt64 value);
		static void setStime(const char* pszTime);
		static int  getTag();
	private:
		static CNetWriter* instance;
		CNetWriter(void);
		~CNetWriter(void);
		static std::string s_Key;
		static unsigned long long s_userID;
		static std::string s_strSessionID;
		static std::string s_strSt;

		static std::string s_strUrl;
		static std::string s_strPostData;
		static std::string s_strUserData;
		static int s_Counter;
		static int s_CounterTemp;
		static std::string s_md5Key;


	};


	class CNetReader:public CNetStreamExport
	{
	public:
		~CNetReader();
		
		bool pushNetStream(char* pdataStream,int wSize);
		
		/*父类方法 

		bool recordBegin();
		void recordEnd()

		unsigned char	getBYTE(void);
		unsigned short	getWORD(void);
		unsigned int	getDWORD(void);
		int				getInt(void);
		float			getFloat(void);
		double			getDouble(void);
		unsigned long long getInt64();

		*/
		void getString(CLuaString* pOutString, int nLength);

		CInt64 getCInt64();

		unsigned char	getByte(void);
		unsigned short	getWord(void);

		static CNetReader* getInstance();

		int			getResult();
		int			getRmId();
		int			getActionID();
		CLuaString* getErrMsg();
		CLuaString* getStrStime();
	private:
		CNetReader();
		static			CNetReader* instance;
		int				m_nResult;
		int				m_nRmId;
		int				m_nActionID;
		CLuaString*		m_pStrErrMsg;
		CLuaString*		m_pStrStime;

	};

}

#endif//NETHELPER_H