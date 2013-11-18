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
#include "Stream.h"

#ifdef SCUT_UPHONE
#include "ssFile.h"
#include "TUString.h"
#elif defined(SCUT_IPHONE) || defined(SCUT_MAC)
#include <unistd.h>
//#else ifdef SS_ANDROID
#elif SCUT_WIN32
#include <io.h>
#elif SCUT_ANDROID
#include <string.h>
#include <fcntl.h>
#endif


namespace ScutSystem
{
	//缓存大小
	const int CS_MAXBUFSIZE = 0xC000; //48K
	//内存每次递增幅度
	const int CS_MEMORYDELTA = 0x2000;//8K

	CStream::CStream(void)
	{
		m_Buffer = NULL;
	}

	CStream::CStream( const CStream* pSource )
	{
		m_Buffer = NULL;
		this->CopyFrom(const_cast<CStream*>(pSource));
	}

	CStream::~CStream(void)
	{
		if (m_Buffer)
		{
			delete []m_Buffer;
			m_Buffer = NULL;
		}
	}

	int CStream::GetPosition()
	{
		return Seek(0, soCurrent);
	}

	void CStream::SetPosition( const int nPos )
	{
		Seek(nPos, soBegin);
	}

	int CStream::GetSize()
	{
		int nPos = Seek(0, soCurrent);
		int ret = Seek(0, soEnd);
		Seek(nPos, soBegin);
		return ret;
	}


	bool CStream::ReadBuffer( char* pszBuffer, int nSize )
	{
		if (nSize != 0 && this->Read(pszBuffer, nSize) != nSize)
		{
			return false;
		}
		return true;
	}

	bool CStream::WriteBuffer( const char* pszBuffer, int nSize )
	{
		if (nSize != 0 && this->Write(pszBuffer, nSize) != nSize)
		{
			return false;
		}
		return true;
	}

	int CStream::CopyFrom( CStream* pSource, int nSize )
	{
		if (!pSource || pSource->GetSize() == 0)
		{
			return 0;
		}
		if (nSize == 0) //表示要拷贝全部
		{
			pSource->SetPosition(0);
			nSize = pSource->GetSize();
		}
		int ret = nSize;
		char* pszBuffer = NULL;	//暂时的缓存指针
		int nBufSize = nSize;			//缓存大小
		if (nBufSize > CS_MAXBUFSIZE)
		{
			nBufSize = CS_MAXBUFSIZE;
		}
		pszBuffer = (char*)malloc(nBufSize);
		if (pszBuffer)
		{
			int nTempSize = 0;	//每次读的大小
			while (nSize != 0)
			{
				if (nSize > nBufSize)
				{
					nTempSize = nBufSize;
				}
				else
					nTempSize = nSize;
				pSource->ReadBuffer(pszBuffer, nTempSize);
				this->WriteBuffer(pszBuffer, nTempSize);
				nSize -= nTempSize;
			}
			free(pszBuffer);
			return ret;
		}
		return 0;
	}

	char* CStream::GetBuffer(int nSize)
	{
		if (m_Buffer)
		{
			delete []m_Buffer;
			m_Buffer = NULL;
		}
		m_Buffer = new char[nSize + 1];
		memset(m_Buffer, 0, nSize + 1);
		SetPosition(0);
		ReadBuffer(m_Buffer, nSize);
		return m_Buffer;
	}

	//void CStream::ReleaseBuffer(char* pBuffer)
	//{
	//	if (pBuffer)
	//	{
	//		delete []pBuffer;
	//	}
	//}

	CHandleStream::CHandleStream(): m_hHandle(NULL)
	{
	}

	void CHandleStream::SetSize( const int nSize )
	{
		Seek(nSize, soBegin);
#ifdef SCUT_UPHONE
		EOS_SetFileSize(m_hHandle, nSize);
#elif defined(SCUT_IPHONE) || defined(SCUT_MAC)
		ftruncate(m_hHandle, GetPosition());
#elif defined (SCUT_ANDROID)
		ftruncate(m_hHandle, nSize);
#else
		_chsize(m_hHandle, nSize);
#endif		
	}

	int CHandleStream::Read( char* pszBuffer, int nSize )
	{
#ifdef SCUT_UPHONE
		return EOS_Read(m_hHandle, pszBuffer, nSize);
#else
		return fread(pszBuffer, sizeof(char), nSize, (FILE*)m_hHandle);
#endif		
	}

	int CHandleStream::Write( const char* pszBuffer, int nSize )
	{
#ifdef SCUT_UPHONE
		return EOS_Write(m_hHandle, pszBuffer, nSize);
#else
		return fwrite(pszBuffer, sizeof(char), nSize, (FILE*)m_hHandle);
#endif		
	}

	int CHandleStream::Seek( int nOffset, EStreamOrigin origin )
	{
#ifdef SCUT_UPHONE
		return EOS_Seek(m_hHandle, nOffset, (int)origin);
#else
		if (fseek((FILE*)m_hHandle, nOffset, (int)origin) == 0)
		{
			return ftell((FILE*)m_hHandle);
		}
		return 0;
#endif
	}

	intptr_t CHandleStream::GetHandle()
	{
		return m_hHandle;
	}

	void CHandleStream::SetHandle( intptr_t hHandle )
	{
		m_hHandle = hHandle;
	}

	CFileStream::CFileStream()
	{
		
	}

	CFileStream::~CFileStream()
	{
		intptr_t hHandle = GetHandle();
		if (hHandle && hHandle != -1)
		{
			//Flush
#ifdef SCUT_UPHONE
			EOS_Close(hHandle);
#else
			fflush((FILE*)hHandle);
			fclose((FILE*)hHandle);
#endif			
		}
	}

	int CFileStream::Open( const char* lpszFileName, DWORD dwFlag, DWORD dwMode, char* chMode )
	{
#ifdef SCUT_UPHONE
		TUChar pszTemp[EOS_FILE_MAX_FNAME];
		TUString::StrGBToUnicode(pszTemp, (Char*)lpszFileName);
		intptr_t hHandle = EOS_Open(pszTemp, dwFlag, dwMode);
		if (hHandle != - 1)
		{
			SetHandle(hHandle);
			return 0;
		}
#else
		intptr_t hHandle = (intptr_t)fopen(lpszFileName, chMode);
		if (hHandle != 0)
		{
			SetHandle(hHandle);
			return 0;
		}
#endif	
		return -1;
	}

	int CBaseMemoryStream::Read( char* pszBuffer, int nSize )
	{
		if (m_nPosition >= 0 && nSize >= 0)
		{
			int ret = m_nSize - m_nPosition;
			if (ret > 0)
			{
				if (ret > nSize)
				{
					ret = nSize;
				}
				memcpy(pszBuffer, (char*)((intptr_t)m_pMemory + m_nPosition), ret);
				//CopyMemory(pszBuffer, (char*)((int)m_pMemory + m_nPosition), ret);
				m_nPosition += ret;
				return ret;
			}
		}
		return 0;
	}

	int CBaseMemoryStream::Seek( int nOffset, EStreamOrigin origin )
	{
		switch (origin)
		{
		case soBegin:
			m_nPosition = nOffset;
			break;
		case soCurrent:
			m_nPosition += nOffset;
			break;
		case soEnd:
			m_nPosition = m_nSize + nOffset;
			break;
		}
		return m_nPosition;
	}

	void CBaseMemoryStream::SaveTo( CStream* pDest )
	{
		if (m_nSize != 0)
		{
			pDest->WriteBuffer((char*)m_pMemory, m_nSize);
		}
	}

	void CBaseMemoryStream::SaveTo( const char* lpszFileName )
	{
		CFileStream* fs = new CFileStream();
#ifdef SCUT_UPHONE
		if (fs->Open(lpszFileName, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
		{
			this->SaveTo(fs);
			delete fs;
		}
#else
		if (fs->Open(lpszFileName, 0, 0, "wb+") == 0)
		{
			this->SaveTo(fs);
			delete fs;
		}
#endif
	}

	void* CBaseMemoryStream::GetMemory()
	{
		return m_pMemory;
	}

	void CBaseMemoryStream::SetPointer( void* pNewPtr, int nSize )
	{
		m_pMemory = pNewPtr;
		m_nSize = nSize;
	}

	CBaseMemoryStream::CBaseMemoryStream(): m_pMemory(NULL), m_nSize(0), m_nPosition(0)
	{	
	}

	CMemoryStream::~CMemoryStream()
	{
		this->Clear();
	}

	void CMemoryStream::Clear()
	{
		SetCapacity(0);
		m_nSize = 0;
		m_nPosition = 0;
	}

	void CMemoryStream::LoadFrom( CStream* pSource )
	{
		pSource->SetPosition(0);
		int nSize = pSource->GetSize();
		this->SetSize(nSize);
		if (nSize != 0)
		{
			pSource->ReadBuffer((char*)m_pMemory, nSize);
		}
	}

	void CMemoryStream::LoadFrom( const char* lpszFileName )
	{
		CFileStream* pStream = new CFileStream();
#ifdef SCUT_UPHONE
		if (pStream->Open(lpszFileName, PO_CREAT | PO_RDWR | PO_BINARY, PS_IREAD | PS_IWRITE) == 0)
		{
			this->LoadFrom(pStream);
			delete pStream;
		}
#else
		if (pStream->Open(lpszFileName, 0, 0, "rb") == 0)
		{
			this->LoadFrom(pStream);
			delete pStream;
		}
#endif
	}

	void CMemoryStream::SetSize( const int nSize )
	{
		int nOldPos = m_nPosition;
		this->SetCapacity(nSize);
		m_nSize = nSize;
		if (nOldPos > nSize)
		{
			Seek(0, soEnd);
		}
	}

	int CMemoryStream::Write( const char* pszBuffer, int nSize )
	{
		if (m_nPosition >= 0 && nSize >= 0)
		{
			int nPos = m_nPosition + nSize;
			if (nPos > 0)
			{
				if (nPos > m_nSize)
				{
					if (nPos > m_nCapacity)
					{
						SetCapacity(nPos);
					}
					m_nSize = nPos;
				}
				memcpy((char*)((intptr_t)m_pMemory + m_nPosition), pszBuffer, nSize);
				//CopyMemory((char*)((int)m_pMemory + m_nPosition), pszBuffer, nSize);
				m_nPosition = nPos;
				return nSize;
			}
		}
		return 0;
	}

	void* CMemoryStream::Realloc( int& nNewCapacity )
	{
		if (nNewCapacity > 0 && nNewCapacity != m_nSize)
		{
			nNewCapacity = (nNewCapacity + (CS_MEMORYDELTA - 1)) ^ (CS_MEMORYDELTA - 1);
		}
		void* ret = this->GetMemory();
		if (nNewCapacity != m_nCapacity)
		{
			if (nNewCapacity == 0)
			{
				free(ret);
				ret = NULL;
			}
			else
			{
				if (m_nCapacity == 0)
				{
					ret = malloc(nNewCapacity);
				}
				else
					ret = realloc(ret, nNewCapacity);
			}
		}
		return ret;
	}

	void CMemoryStream::SetCapacity( int nNewCapacity )
	{
		SetPointer(Realloc(nNewCapacity), m_nSize);
		m_nCapacity = nNewCapacity;
	}

	int CMemoryStream::GetCapacity()
	{
		return m_nCapacity;
	}

	CMemoryStream::CMemoryStream(): m_nCapacity(0)
	{

	}
}
