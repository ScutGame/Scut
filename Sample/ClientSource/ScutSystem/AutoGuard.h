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
#ifndef _AUTOGURARD_H
#define _AUTOGURARD_H

#ifdef WIN32
#include <windows.h>
#else
#include <pthread.h>
#endif

namespace ScutSystem
{
	//线程互斥体
	class CThreadMutex
	{
	public:
		CThreadMutex()
		{
		#ifdef WIN32
			InitializeCriticalSection(&m_CritSec);
		#else
			pthread_mutexattr_t attr;
			pthread_mutexattr_init(&attr);
			pthread_mutexattr_settype(&attr, PTHREAD_MUTEX_RECURSIVE);
			pthread_mutex_init(&m_ThreadMutex, &attr);
			pthread_mutexattr_destroy(&attr);
		#endif			
		}

		~CThreadMutex()
		{
		#ifdef WIN32
			DeleteCriticalSection(&m_CritSec);
		#else
			pthread_mutex_destroy( &m_ThreadMutex );
			#endif			
		}

		//锁定
		inline void Lock()
		{
		#ifdef WIN32
			EnterCriticalSection(&m_CritSec);
		#else
			pthread_mutex_lock( &m_ThreadMutex );
		#endif			
		}

		//解锁
		inline void Unlock()
		{
			#ifdef WIN32
			LeaveCriticalSection(&m_CritSec);
			#else
			pthread_mutex_unlock( &m_ThreadMutex );
			#endif			
		}

	private:
#ifdef WIN32
		_RTL_CRITICAL_SECTION m_CritSec;
#else
		pthread_mutex_t m_ThreadMutex;
#endif		
	};  

	//自动保护锁，使用的时候直接在栈中创建即可
	class CAutoGuard
	{
	public:
		CAutoGuard(CThreadMutex& mutex);
		~CAutoGuard(void);
	private:
		CThreadMutex& m_ThreadMutex;
	};

	//自动保护锁使用宏
	#define AUTO_GUARD(mtx ) \
	CAutoGuard g_xx0010xx(mtx) 
}

#endif