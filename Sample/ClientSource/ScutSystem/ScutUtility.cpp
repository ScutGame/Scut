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
#include "ScutUtility.h"
#include "WjcDes.h"
#include "StdDES2.h"


// for MIN MAX and sys/time.h on win32 platform
#ifdef SCUT_WIN32
#include <windows.h>
#endif

#ifdef SCUT_ANDROID
#include <time.h>
#elif defined(SCUT_IPHONE) || defined(SCUT_MAC)
#include <sys/time.h>
#endif // SCUT_ANDROID

#include<string>
using namespace std;

//using namespace ScutSystem;
namespace ScutSystem
{
CScutUtility::CScutUtility(void)
{
}

CScutUtility::~CScutUtility(void)
{
}

static std::string Transcode(unsigned char* Str, int len)
{
	const char Hexcode[]={'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
	std::string szResultStr;

	for(int i = 0;i < len; i++)
	{
		unsigned char c = (unsigned char)Str[i];
		if ((c >= 'a' && c <= 'z'))//(c >= 'A' && c <= 'Z')||(c >= '0' && c <= '9'))
		{
			szResultStr += c-'a'+ 'a';
		}
		else if(c >= 'A' && c <= 'Z')
		{
			szResultStr += c-'A'+ 'A';
		}
		else if(c >= '0' && c <= '9')
		{
			szResultStr += c-'0'+ '0';
		}
		else
		{
			szResultStr+= '%';
			szResultStr += "25";
			szResultStr+= Hexcode[c/16];
			szResultStr+= Hexcode[c%16];
		}
	}

	return szResultStr;
}

std::string Transcode2(const char* Str, int len)
{
	const char Hexcode[]={'0','1','2','3','4','5','6','7','8','9','A','B','C','D','E','F'};
	std::string szResultStr;

	for(int i = 0;i < len; i++)
	{
		unsigned char c = (unsigned char)Str[i];
		if ((c >= 'a' && c <= 'z') || (c >= 'A' && c <= 'Z')||(c >= '0' && c <= '9'))
		{
			szResultStr += c;
		}
		else
		{
			szResultStr+= '%';
			szResultStr+= Hexcode[c/16];
			szResultStr+= Hexcode[c%16];
		}
	}

	return szResultStr;
}

unsigned int ChartoUINT(const char c)
{
	if(c >= 'a' && c <= 'z')
		return c - 'a' +10;
	else if(c >= 'A' && c <= 'Z')
		return c - 'A' + 10;
	else if(c >= '0' && c <= '9')
		return c - '0';
	else
		return 0;
}

void DeTrandcode2(const char* Str, int len, char*& ret, int& nLen)
{
	ret = new char[len + 1];
	memset(ret, 0, len + 1);
	int nIndex = 0;
	for(int i = 0; i < len; i++)
	{
		char c = Str[i];
		if ((c >= 'a' && c <= 'z')||(c >= 'A' && c <= 'Z')||(c >= '0' && c <= '9'))
		{	
			ret[nIndex++] = c;
		}
		else if(Str[i]==0)
		{
			break;
		}
		else if(c == '.')
		{
			ret[nIndex++] = c;
		}
		else
		{
			char t = 0;
			t = ChartoUINT(Str[++i])*16;
			t += ChartoUINT(Str[++i]);
			ret[nIndex++] = t;
		}
	}
	nLen = nIndex;
}

void Run_PadPwd(const char *pPwd, char *outStr)
{
	if (NULL == pPwd && NULL == outStr)
	{
		return;
	}
	int length = strlen(pPwd);
	for(int i = 0, j = 0; j < length; i+=2, j++)
	{
		outStr[i] = pPwd[j];
	}
}

std::string Run_DePadPwd(const char *pPwd, int nLen)
{
	char szRet[1024] = {0};
	for (int i = 0, j = 0; i < nLen; i += 2, j++)
	{
		szRet[j] = pPwd[i];
	}
	return szRet;
}

std::string EncryptPwd(const char* pPwd, const char*key)
{
	Des_SetKey(key);

	int pos = 0;
	int length = strlen(pPwd);
	char* pPadStr = (char*)malloc(length*sizeof(char)*2+1);
	memset(pPadStr, 0, length*sizeof(char)*2 + 1);
	Run_PadPwd(pPwd, pPadStr);

	int padLen = (length*sizeof(char))*2 + 2;
	padLen = ((padLen + 7)/8)*8;
	char* dataTmp = (char*)malloc(sizeof(char)*(padLen + 2));
	memset(dataTmp, 0, sizeof(char)*(padLen + 2));
	memcpy(dataTmp, pPadStr, length*sizeof(char)*2 + 1);
	while(pos < padLen)
	{
		Des_Run(dataTmp + pos, dataTmp + pos, ENCRYPT);
		pos += 8;
	}

	std::string strEncode = Transcode2(dataTmp, sizeof(char)*(padLen));
	free(dataTmp);
	free(pPadStr);
	return strEncode;
}

std::string DecryptPwd(const char* pPwd, const char*key)
{
	Des_SetKey(key);
	char szRet[2048] = {0};
	char* pszRet = szRet;
	int nLen = 2048;
	DeTrandcode2(pPwd, strlen(pPwd), pszRet, nLen);
	nLen = ((nLen + 7) / 8) * 8;

	char *dataTmp = new char[nLen+2];
	memset(dataTmp,0,sizeof(char)*(nLen+2));
	memcpy(dataTmp,pszRet,sizeof(char)*nLen);
	int pos = 0;
	while(pos < nLen)
	{
		Des_Run(dataTmp+pos,dataTmp+pos,DECRYPT);
		pos += 8;
	}
	//char szRet2[1024] = {0};
	//memset(szRet2,0,sizeof(char)*(1024));
	//memcpy(szRet2,dataTmp,sizeof(char)*1024);

	std::string strRet = Run_DePadPwd(dataTmp, nLen);
	delete [] dataTmp;
	return strRet;
}

int CScutUtility::DesEncrypt( const char* lpszKey, const char* lpDataIn, std::string& strDataOut)
{
	if((lpszKey == NULL) || (lpDataIn == NULL))
	{
		return -1;
	}

	strDataOut = EncryptPwd(lpDataIn, lpszKey);
	return 0;
}

int CScutUtility::DesDecrypt( const char* lpszKey, const char* lpDataIn, std::string& strDataOut)
{
	if((lpszKey == NULL) || (lpDataIn == NULL))
	{
		return -1;
	}

	strDataOut = DecryptPwd(lpDataIn, lpszKey);
	return 0;
}

unsigned long CScutUtility::GetTickCount()
{
#if defined(SCUT_ANDROID) || defined(SCUT_IPHONE) || defined(SCUT_MAC)
    struct timeval now;
    gettimeofday(&now, 0);
    return now.tv_sec * 1000 + now.tv_usec / 1000;
#else
	return ::GetTickCount();
#endif // SCUT_ANDROID
}

std::string CScutUtility::GetNowTime()
{
#ifdef SCUT_WIN32
	SYSTEMTIME wtm;
	GetLocalTime(&wtm);
	char szTime[256];
	sprintf(szTime, "%4d-%2d-%2d %2d:%2d:%2d.%3d", wtm.wYear, wtm.wMonth, wtm.wDay, wtm.wHour, wtm.wMinute, wtm.wSecond, wtm.wMilliseconds);
	return szTime;
#elif defined(SCUT_ANDROID) || defined(SCUT_IPHONE)
	struct timeval tv;
	struct tm      tm;
	size_t         len = 28;
	char		   szTime[256] = {0};

	gettimeofday(&tv, NULL);
	localtime_r(&tv.tv_sec, &tm);
	strftime(szTime, len, "%Y-%m-%d %H:%M:%S", &tm);
	len = strlen(szTime);
	sprintf(szTime + len, ".%3d", (int)(tv.tv_usec / 1000));

	return szTime;
#endif
}

void CScutUtility::StdDesEncrypt( const char* lpszKey, const char* lpDataIn, std::string& strDataOut )
{
	StdDES2* des = new StdDES2();
	des->InitializeKey((char*)lpszKey, 0);
	des->EncryptAnyLength((char*)lpDataIn, strlen(lpDataIn), 0);
	char* sz = des->GetCiphertextAnyLength();
	strDataOut = Transcode2(sz, ((strlen(lpDataIn) + 7) / 8) * 8);
	delete des;
}

void CScutUtility::StdDesDecrypt( const char* lpszKey, const char* lpDataIn, std::string& strDataOut )
{
	StdDES2* des = new StdDES2();
	char szRet[2048] = {0};
	char* pszRet = szRet;
	int nLen = 2048;
	DeTrandcode2(lpDataIn, strlen(lpDataIn), pszRet, nLen);
	des->InitializeKey((char*)lpszKey, 0);
	des->DecryptAnyLength(pszRet, nLen, 0);
	strDataOut = des->GetPlaintextAnyLength();
	delete des;
}


//void Run_PadPwd(const char *pPwd, char *outStr)
//{
//	if (NULL == pPwd && NULL == outStr)
//	{
//		return;
//	}
//	int length = strlen(pPwd);
//	for(int i = 0, j = 0; j < length; i+=2, j++)
//	{
//		outStr[i] = pPwd[j];
//	}
//}
//
//string EncryptPwd(const char* pPwd, const char*key)
//{
//	DesPriv_SetKey(key);
//
//	int pos = 0;
//	int length = strlen(pPwd);
//	char* pPadStr = (char*)malloc(length*sizeof(char)*2+1);
//	memset(pPadStr, 0, length*sizeof(char)*2 + 1);
//	Run_PadPwd(pPwd, pPadStr);
//
//	int padLen = (length*sizeof(char))*2 + 2;
//	padLen = ((padLen + 7)/8)*8;
//	char* dataTmp = (char*)malloc(sizeof(char)*(padLen + 2));
//	memset(dataTmp, 0, sizeof(char)*(padLen + 2));
//	memcpy(dataTmp, pPadStr, length*sizeof(char)*2 + 1);
//	while(pos < padLen)
//	{
//		Des_Run(dataTmp + pos, dataTmp + pos, ENCRYPT);
//		pos += 8;
//	}
//
//	string strEncode = UrlEncode(dataTmp);
//	free(dataTmp);
//	free(pPadStr);
//	return strEncode;
//}
}