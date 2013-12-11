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
#ifndef _DEFINES_H_
#define _DEFINES_H_

#ifndef SCUT_WIN32

#if defined(BOOL) 
#undef BOOL
#endif
//	typedef signed char BOOL;

	typedef unsigned int UINT;
	typedef void* HANDLE;
#if defined(DWORD)  //ANDROID NDK下面会有重定义
#undef DWORD
#endif
	typedef unsigned long DWORD;
	typedef unsigned short      WORD;
	typedef unsigned char       BYTE;
	typedef DWORD   COLORREF;
	typedef DWORD   *LPCOLORREF;
	typedef void * LPVOID;
	typedef float               FLOAT;
	typedef int                 INT;
	typedef unsigned int        UINT;
	typedef unsigned int        *PUINT;

	typedef long long INT64;

	typedef char CHAR;
	typedef short SHORT;
	typedef long LONG;

	#define CP_ACP 0
#ifndef MAX_PATH
	#define MAX_PATH          260
#endif

	#ifndef IN
	#define IN
	#endif

	#ifndef OUT
	#define OUT
	#endif

	//#define strnicmp strncasecmp

	#define FALSE 0
	#define TRUE  1

	#ifdef SCUT_UPHONE
		#ifndef WIN32
			typedef struct tagSIZE
			{
				LONG        cx;
				LONG        cy;
			} SIZE, *PSIZE, *LPSIZE;

			#define TEXT(quote) quote   // r_winnt

			#define RGB(r,g,b)          ((COLORREF)(((BYTE)(r)|((WORD)((BYTE)(g))<<8))|(((DWORD)(BYTE)(b))<<16)))
		#endif	
	#endif

	#ifdef _UNICODE
		#ifndef LPCTSTR
		typedef const wchar_t *LPCTSTR;
		#endif	

		#ifndef LPTSTR
		typedef wchar_t* LPTSTR;
		#endif

		#ifndef TCHAR
				typedef wchar_t TCHAR;
		#endif
	#else
		#ifndef LPCTSTR
		typedef const char *LPCTSTR;
		#endif	

		#ifndef LPTSTR
		typedef char* LPTSTR;
		#endif

		#ifndef TCHAR
				typedef char TCHAR;
		#endif
	#endif



	//#ifndef LPCSTR
	//typedef const char *LPCSTR;
	//#endif

	////typedef wchar_t TCHAR;
	//typedef const wchar_t *LPCWSTR;
	//typedef unsigned char _TXCHAR;

	#define SS_ANSI

	#ifdef DEBUG
	# define TRACE printf
	#else
	# define TRACE(str)
	#endif

#else
	#pragma warning (disable:4786) // needed by StdString.h
#define WIN32_LEAN_AND_MEAN
#include <windows.h>
#endif

//#include "StdString.h"

#include "ScutString.h" 

#define nvl(str1, str2) (str1 != NULL ? str1 : str2)

#define SAFE_FREE(mem) if (mem != NULL) { free(mem); mem = NULL; }

#ifdef SCUT_IPHONE

#endif

#endif
