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
#ifndef _HTTPDEBUG_H
#define _HTTPDEBUG_H

#include <sys/stat.h>
#include <stdlib.h>
#include <stdio.h>
#include <signal.h>
#include <string.h>
#include <errno.h>
#include <limits.h>
#include <stddef.h>
#include <stdarg.h>

#ifdef SCUT_WIN32
	#define WIN32_LEAN_AND_MEAN

	#include <windows.h>
	#include <winsvc.h>
	#define PATH_MAX MAX_PATH
	#define S_ISDIR(x) ((x) & _S_IFDIR)
	#define DIRSEP '\\'
	#define snprintf _snprintf
	#define vsnprintf _vsnprintf
	#define sleep(x) Sleep((x) * 1000)
	#define WINCDECL __cdecl
#else
	#include <sys/wait.h>
	#include   <sys/socket.h>
	#include   <netinet/in.h>
	#include   <arpa/inet.h>
	#include <unistd.h>
	#include <netdb.h>
	#include <dirent.h>
	#define DIRSEP '/'
	#define WINCDECL
#endif // _WIN32

#ifdef SCUT_ANDROID
	#include <ANDROID/log.h>
	#include <sys/stat.h>
	#define  LOG_TAG    "libScut"
	#define  LOGI(...)  __android_log_print(ANDROID_LOG_INFO,LOG_TAG,__VA_ARGS__)
	#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)
	#define  ScutLog LOGE
#else
	#ifdef _WIN32
		#ifdef __cplusplus
			extern "C" {
		#endif

		void __dbg_printf (const char * format,...);

		#ifdef __cplusplus
			}
		#endif
		#define  ScutLog __dbg_printf
	#else
		#define ScutLog printf
	#endif
#endif



#endif