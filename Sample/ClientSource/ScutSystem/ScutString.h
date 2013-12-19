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
#ifndef _SCUT_STRING_H_
#define _SCUT_STRING_H_

#include <stdarg.h>
#include <string>
//#include "Defines.h"
#include <assert.h>
#include <algorithm>

namespace ScutSystem
{
	#define MAX_FMT_TRIES		5	 
	#define FMT_BLOCK_SIZE		2048 //格式化块大小
	#define BUFSIZE_1ST			256
	#define BUFSIZE_2ND			512
	#define STD_BUF_SIZE		1024

	//扩展字符串,ANSI版本
	class CScutString: public std::string
	{
		template<class Type>
		inline const Type& SSMIN(const Type& arg1, const Type& arg2)
		{
			return arg2 < arg1 ? arg2 : arg1;
		}
		template<class Type>
		inline const Type& SSMAX(const Type& arg1, const Type& arg2)
		{
			return arg2 > arg1 ? arg2 : arg1;
		}
	public:
		CScutString()                             : std::string( ){};
		CScutString(std::string str)              : std::string(str){};
		CScutString(const CScutString& stringSrc)   : std::string( stringSrc.c_str() ){};
		CScutString(const char* psz )    : std::string( psz ){};
		CScutString(char ch, int nRepeat = 1 )   : std::string( ch, nRepeat ){};
		CScutString(const char* lpch, int nLength )   : std::string( lpch, nLength ){};

		void ReplaceStr( const char *crefPattern, const char *crefReplaceBy );

		CScutString& TrimRight()
		{
			this->erase(this->find_last_not_of(" ") + 1);
			return *this;
		}

		CScutString& TrimLeft()
		{
			this->erase(0, this->find_first_not_of(" "));
			return *this;
		}

		CScutString& Trim()
		{
			return TrimLeft().TrimRight();
		}

		bool IsEmpty() const
		{
			return this->empty();
		}

		int GetLength() const
		{
			return static_cast<int>(this->length());
		}

		char GetAt(int nIdx) const
		{
			return this->at(static_cast<size_type>(nIdx));
		}

		CScutString Left(int nCount) const
		{
			nCount = nCount > (int)this->size() ? this->size() : nCount;
			nCount = nCount > 0 ? nCount : 0;	
			return this->substr(0, nCount); 
		}

		int Insert(int nIdx, char ch)
		{
			if ( static_cast<size_type>(nIdx) > this->size()-1 )
				this->append(1, ch);
			else
				this->insert(static_cast<size_type>(nIdx), 1, ch);

			return GetLength();
		}
		int Insert(int nIdx, const char* sz)
		{
			if ( static_cast<size_type>(nIdx) >= this->size() )
				this->append(sz, static_cast<size_type>(strlen(sz)));
			else
				this->insert(static_cast<size_type>(nIdx), sz);

			return GetLength();
		}

		int CompareNoCase(const char* lpszValue)
		{
			return CScutString::NoCaseCmp(this->c_str(), lpszValue);
		}

		int Compare(const char* szThat) const
		{
			return this->compare(szThat);	
		}

		operator const char*() const
		{
			return this->c_str();
		}

		char& operator[](int nIdx)
		{
			return static_cast<std::string*>(this)->operator[](static_cast<size_type>(nIdx));
		}

		const char& operator[](int nIdx) const
		{
			return static_cast<const std::string*>(this)->operator[](static_cast<size_type>(nIdx));
		}

#if !defined(SCUT_IPHONE) && !defined(SCUT_MAC)
		char& operator[](unsigned int nIdx)
		{
			return static_cast<std::string*>(this)->operator[](static_cast<size_type>(nIdx));
		}

		const char& operator[](unsigned int nIdx) const
		{
			return static_cast<const std::string*>(this)->operator[](static_cast<size_type>(nIdx));
		}
#endif

		CScutString Mid(int nFirst) const
		{
			return Mid(nFirst, this->GetLength()-nFirst);
		}

		CScutString Mid(int nFirst, int nCount) const
		{
			if ( nFirst < 0 )
				nFirst = 0;
			if ( nCount < 0 )
				nCount = 0;

			int nSize = static_cast<int>(this->size());

			if ( nFirst + nCount > nSize )
				nCount = nSize - nFirst;

			if ( nFirst > nSize )
				return CScutString();
			
			assert(nFirst >= 0);
			assert(nFirst + nCount <= nSize);

			return this->substr(static_cast<size_type>(nFirst),
				static_cast<size_type>(nCount));
		}

		void Empty()
		{
			this->erase();
		}

		int Find(char ch) const
		{
			size_type nIdx	= this->find_first_of(ch);
			return static_cast<int>(std::string::npos == nIdx  ? -1 : nIdx);
		}

		int Find(const char* szSub) const
		{
			size_type nIdx	= this->find(szSub);
			return static_cast<int>(std::string::npos == nIdx ? -1 : nIdx);
		}

		int Find(char ch, int nStart) const
		{	

			size_type nIdx	= this->find_first_of(ch, static_cast<size_type>(nStart));
			return static_cast<int>(std::string::npos == nIdx ? -1 : nIdx);
		}

		int Find(const char* szSub, int nStart) const
		{
			size_type nIdx	= this->find(szSub, static_cast<size_type>(nStart));
			return static_cast<int>(std::string::npos == nIdx ? -1 : nIdx);
		}

		int FindOneOf(const char* szCharSet) const
		{
			size_type nIdx = this->find_first_of(szCharSet);
			return static_cast<int>(std::string::npos == nIdx ? -1 : nIdx);
		}

		void SetAt(int nIndex, char ch)
		{
			assert(this->size() > static_cast<size_type>(nIndex));
			this->at(static_cast<size_type>(nIndex)) = ch;
		}

		void MakeLower()
		{
		
		}

		void MakeReverse()
		{
			std::reverse(this->begin(), this->end());
		}

		void MakeUpper()
		{ 

		}


		int Replace(char chOld, char chNew)
		{
			int nReplaced	= 0;

			for ( std::string::iterator iter=this->begin(); iter != this->end(); iter++ )
			{
				if ( *iter == chOld )
				{
					*iter = chNew;
					nReplaced++;
				}
			}

			return nReplaced;
		}

		int Replace(const char* szOld, const char* szNew)
		{
			int nReplaced		= 0;
			size_type nIdx			= 0;
			size_type nOldLen		= strlen(szOld);

			if ( 0 != nOldLen )
			{
				static const char ch	= char(0);
				size_type nNewLen		= strlen(szNew);
				const char* szRealNew	= szNew == 0 ? &ch : szNew;

				while ( (nIdx=this->find(szOld, nIdx)) != std::string::npos )
				{
					this->replace(this->begin()+nIdx, this->begin()+nIdx+nOldLen,
						szRealNew);

					nReplaced++;
					nIdx += nNewLen;
				}
			}

			return nReplaced;
		}


		//CScutString& operator=(const CScutString& str)
		//{ 
		//	ssasn(*this, str); 
		//	return *this;
		//}

		//CScutString& operator=(const std::string& str)
		//{
		//	ssasn(*this, str);
		//	return *this;
		//}

		void Format(const char* szFmt, ...)
		{
			va_list argList;
			va_start(argList, szFmt);
			FormatV(szFmt, argList);
			va_end(argList);
		}

		void FormatV(const char* szFormat, va_list argList)
		{
//#ifdef SS_ANSI

			int nLen	= strlen(szFormat) + STD_BUF_SIZE;
			vsnprintf(GetBuf(nLen), nLen-1, szFormat, argList);
			RelBuf();

//#else
//
//			CT* pBuf			= NULL;
//			int nChars			= 1;
//			int nUsed			= 0;
//			size_type nActual	= 0;
//			int nTry			= 0;
//
//			do	
//			{
//				// Grow more than linearly (e.g. 512, 1536, 3072, etc)
//
//				nChars			+= ((nTry+1) * FMT_BLOCK_SIZE);
//				pBuf			= reinterpret_cast<CT*>(_alloca(sizeof(CT)*nChars));
//				nUsed			= ssnprintf(pBuf, nChars-1, szFormat, argList);
//
//				// Ensure proper NULL termination.
//
//				nActual			= nUsed == -1 ? nChars-1 : SSMIN(nUsed, nChars-1);
//				pBuf[nActual+1]= '\0';
//
//
//			} while ( nUsed < 0 && nTry++ < MAX_FMT_TRIES );
//
//			// assign whatever we managed to format
//
//			this->assign(pBuf, nActual);
//
//#endif
		}

		void AppendFormat(const char* szFmt, ...)
		{
			va_list argList;
			va_start(argList, szFmt);
			AppendFormatV(szFmt, argList);
			va_end(argList);
		}

		void AppendFormatV(const char* szFmt, va_list argList)
		{
			char szBuf[STD_BUF_SIZE];
			int nLen = vsnprintf(szBuf, STD_BUF_SIZE-1, szFmt, argList);
			if ( 0 < nLen )
				this->append(szBuf, nLen);
		}
	public:
		static int NoCaseCmp(const char* sz1, const char* sz2, int count = -1)
		{
#ifdef SCUT_UPHONE
	#ifndef WIN32
			if (count == -1)
			{
				return strcasecmp(sz1, sz2);
			}
			else
				return strncasecmp(sz1, sz2, count);
	#else
			if (count == -1)
			{
				return stricmp(sz1, sz2);
			}
			else
				return strnicmp(sz1, sz2, count);
	#endif		
#elif SCUT_WIN32
			if (count == -1)
			{
				return _stricmp(sz1, sz2);
			}
			else
				return _strnicmp(sz1, sz2, count);
#else	//Linux,Unix
			if (count == -1)
			{
				return strcmp(sz1, sz2);
			}
			else
				return strncmp(sz1, sz2, count);
#endif
		}
	private:
		char* GetBuf(int nMinLen = -1)
		{
			if ( static_cast<int>(this->size()) < nMinLen )
				this->resize(static_cast<size_type>(nMinLen));

			return this->empty() ? const_cast<char*>(this->data()) : &(this->at(0));
		}
		void RelBuf(int nNewLen=-1)
		{
			this->resize(static_cast<size_type>(nNewLen > -1 ? nNewLen :
				strlen(this->c_str())));
		}
		inline void	ssasn(std::string& sDst, const std::string& sSrc)
		{
			if ( sDst.c_str() != sSrc.c_str() )
			{
				sDst.erase();
				sDst.assign(sSrc);
			}
		}
	};


#ifdef WIN32
#define STSCANF _stscanf
#define TCSCHR  _tcschr
#define TCSINC  _tcsinc
#define TCSNULL NULL
#else
	/* now for some windows data types ! */
#define STSCANF sscanf
#define TCSCHR  strchr
#define TCSINC(ptr)  ptr+1
#define TCSNULL '\0'
#define vsscanf ourvsscanf
	/* phew ! finally done ! */
#endif
}


#endif 

