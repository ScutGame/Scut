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
//#include <iostream>
#include <string.h>
#include "HttpClientResponse.h"
#ifdef SCUT_UPHONE
#include "ssFile.h"
#endif

using namespace ScutNetwork;


/*******************************************************************
* Function  : HttpClientResponse constructor
* Parameters: -
* Returns   : -
* Purpose   : Initialize the object
*******************************************************************/
CHttpClientResponse::CHttpClientResponse()
{
	m_pSendData = NULL;
	m_nSendDataSize = 0;
	m_pData = NULL;	
	m_pTarget= NULL;
	m_nDataSize = 0;
	m_bUseDataResume = false;
	m_nStatusCode = 200;
	memset(m_cTargetFile, 0, sizeof(m_cTargetFile));
	memset(m_cContentType, 0, sizeof(m_cContentType));
	m_pszRequestUrl = NULL;
	m_pszLastResponseUrl = NULL;
    Reset();
}

/*******************************************************************
* Function  : HttpClientResponse destructor
* Parameters: -
* Returns   : -
* Purpose   : Object cleanup
*******************************************************************/
CHttpClientResponse::~CHttpClientResponse()
{
	if (m_pTarget)
	{
		delete m_pTarget;
		m_pTarget = NULL;
	}	
	if(m_pData != NULL)
	{
		delete [] m_pData;
	}
	if(m_pSendData != NULL)
	{
		delete [] m_pSendData;
	}
	if (m_pszRequestUrl)
	{
		free(m_pszRequestUrl);
	}
	if (m_pszLastResponseUrl)
	{
		free(m_pszLastResponseUrl);
	}
}

/*******************************************************************
* Function  : SetData()
* Parameters: response - The response data retrieved from the socket
*             responseLen - The length of the response data
* Returns   : -
* Purpose   : To set the response object with the data
*******************************************************************/
void CHttpClientResponse::SetData(const char *response, unsigned int responseLen)
{
	if(m_pData != NULL) {
		delete [] m_pData;
		m_pData=NULL;
	}

    m_nDataSize = responseLen;
	if(responseLen > 0)
	{
		m_pData    = new char[m_nDataSize + 1];
		memcpy(m_pData, response, m_nDataSize);
		m_pData[m_nDataSize]=0;
	}		
}


/*******************************************************************
* Function  : SetContentType()
* Parameters: contenttype - The content-type data
*             contenttypeLen - The length of the content-type data
* Returns   : -
* Purpose   : To set the response object with the data
*******************************************************************/
/* Edit SH. Keep track of the content type of responses */
void CHttpClientResponse::SetContentType(char *contentType)
{
	if (contentType!=NULL)
		strncpy(m_cContentType, contentType, sizeof(m_cContentType));
}

/*******************************************************************
* Function  : Reset()
* Parameters: -
* Returns   : -
* Purpose   : To partially cleanup the object so that it can be
*             reused for more requests.
*******************************************************************/
void CHttpClientResponse::Reset()
{
	//memset(m_cTargetFile, 0, sizeof(m_cTargetFile));
	memset(m_cContentType, 0, sizeof(m_cContentType));
	if (m_pszRequestUrl)
	{
		free(m_pszRequestUrl);
		m_pszRequestUrl = NULL;
	}
	if (m_pszLastResponseUrl)
	{
		free(m_pszLastResponseUrl);
		m_pszLastResponseUrl = NULL;
	}
	if(m_pData != NULL)
	{
		delete [] m_pData;
	}
	if (m_pSendData != NULL)
	{
		delete[] m_pSendData;
	}
	m_pSendData = NULL;
	m_nSendDataSize = 0;
	m_pData		= NULL;
    m_nDataSize     = 0;
	m_bUseDataResume = false;
}

/*******************************************************************
* Function  : DataContains()
* Parameters: searchStr - The string to search for in the response
*                         data
* Returns   : TRUE - The string is present in the data
*             FALSE - The string is not present in the data
* Purpose   : To determine if the specified string is present
*             in the response data
*******************************************************************/
bool CHttpClientResponse::DataContains(const char *searchStr)
{
	if(m_pData == NULL)
	{
		return FALSE;
	}

	if(strstr(m_pData, searchStr) == NULL)
	{
		return FALSE;
	}

	return TRUE;
}

/*******************************************************************
* Function  : ContentTypeContains()
* Parameters: searchStr - The string to search for in the ContentType
* Returns   : TRUE - The string is present in the cdata
*             FALSE - The string is not present in the cdata
* Purpose   : To determine if the specified string is present
*             in the content type
*******************************************************************/
bool CHttpClientResponse::ContentTypeContains(char *searchStr)
{
	if(m_cContentType == NULL)
		return FALSE;

	if(strstr(m_cContentType, searchStr) == NULL)
	{
		return FALSE;
	}

	return TRUE;
}

/*******************************************************************
* Function  : GetBodyPtr()
* Parameters: -
* Returns   : Pointer to the HTTP body section
* Purpose   : To return a pointer to the HTTP body section so 
*             that it can be used for string manipulation, regular
*             expression search, etc. It is recommended that the
*             contents not be modified.
*******************************************************************/
char * CHttpClientResponse::GetBodyPtr()
{
	return m_pData;
}

/*******************************************************************
* Function  : GetBodyLength()
* Parameters: -
* Returns   : Size of the HTTP body section
* Purpose   : To return a size of the HTTP body section so 
*             that it can be used with the body pointer
*******************************************************************/
unsigned int CHttpClientResponse::GetBodyLength() 
{
	return m_nDataSize;
}

char* ScutNetwork::CHttpClientResponse::GetTargetFile()
{
	return &m_cTargetFile[0];
}

void ScutNetwork::CHttpClientResponse::SetTargetFile( const char* pszFileName )
{
	if (pszFileName != NULL && strcmp(pszFileName, m_cTargetFile) != 0)
	{
		SetTarget(NULL);
	}
	
	memset(m_cTargetFile, 0, sizeof(m_cTargetFile));
	memcpy(m_cTargetFile, pszFileName, strlen(pszFileName));
}

ScutSystem::CStream* ScutNetwork::CHttpClientResponse::GetTarget()
{
	if (!m_pTarget && m_cTargetFile[0] != 0)
	{
		CFileStream* fs = new CFileStream();
#ifdef SCUT_UPHONE
		if (fs->Open(m_cTargetFile, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
#else
		char chMode[4] = {0};
		if (m_bUseDataResume) //使用断点续传
		{
			strcpy(chMode, "ab+");
		}
		else
			strcpy(chMode, "wb+");		
		if (fs->Open(m_cTargetFile, 0, 0, chMode) == 0)
#endif		
		{
			m_pTarget = fs;			
			m_nRawTargetSize = m_pTarget->GetSize();
		}
		else
		{
			delete fs;
		}
	}	
	return m_pTarget;
}

void ScutNetwork::CHttpClientResponse::SetTarget( CStream* pTarget )
{
	if (m_pTarget)
	{
		delete m_pTarget;
	}	
	m_pTarget = pTarget;
	if (m_pTarget)
		m_nRawTargetSize = m_pTarget->GetSize();
}

const char* ScutNetwork::CHttpClientResponse::GetRequestUrl()
{
	return m_pszRequestUrl;
}

void ScutNetwork::CHttpClientResponse::SetRequestUrl( const char* pszUrl )
{
	if (pszUrl)
	{
		if (m_pszRequestUrl)
		{
			free(m_pszRequestUrl);
		}		
		m_pszRequestUrl = strdup(pszUrl);
	}
}

const char* ScutNetwork::CHttpClientResponse::GetLastResponseUrl()
{
	return m_pszLastResponseUrl;
}

void ScutNetwork::CHttpClientResponse::SetLastResponseUrl( const char* pszUrl )
{
	if (pszUrl)
	{
		if (m_pszLastResponseUrl)
		{
			free(m_pszLastResponseUrl);
		}
		m_pszLastResponseUrl = strdup(pszUrl);
	}
}

bool ScutNetwork::CHttpClientResponse::GetUseDataResume()
{
	return m_bUseDataResume;
}

void ScutNetwork::CHttpClientResponse::SetUseDataResume( bool bUse /*= false*/ )
{
	m_bUseDataResume = bUse;
}

int ScutNetwork::CHttpClientResponse::GetTargetRawSize()
{
	return m_nRawTargetSize;
}

int ScutNetwork::CHttpClientResponse::GetStatusCode()
{
	return m_nStatusCode;
}

void ScutNetwork::CHttpClientResponse::SetStatusCode( int nCode )
{
	m_nStatusCode = nCode;
}


void ScutNetwork::CHttpClientResponse::SetSendData(const char* data, unsigned int dataLen)
{
	if(m_pSendData != NULL) {
		delete [] m_pSendData;
		m_pSendData=NULL;
	}

	m_nSendDataSize = dataLen;
	if(dataLen > 0 && data)
	{
		m_pSendData    = new char[m_nSendDataSize + 1];
		memcpy(m_pSendData, data, m_nSendDataSize);
		m_pSendData[m_nSendDataSize]=0;
	}	
}

const char* ScutNetwork::CHttpClientResponse::GetSendData()
{
	return m_pSendData;
}

unsigned int ScutNetwork::CHttpClientResponse::GetSendDataLength()
{
	return m_nSendDataSize;
}