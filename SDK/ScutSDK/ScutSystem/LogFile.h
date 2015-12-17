#ifndef _LOGFILE_H_
#define _LOGFILE_H_

#ifdef WIN32
#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000
#else
#include <iostream>
#include <pthread.h> /* for the locks */
#endif

#include "Defines.h"

enum LOGTYPE
{ 
	LOG_BASIC, 
	LOG_ADVANCED, 
	LOG_COOKIE 
};

class CLogFile  
{
public:	
	CLogFile(const CStdString &filename);
	virtual ~CLogFile();

	void Write(const LOGTYPE logtype, const char *format, ...);
protected:
	void Trim();
	CStdString  m_strLogFile;
	long		m_nFileSize;
	FILE		*fp;
#ifdef WIN32
	CRITICAL_SECTION lock;
#else
	pthread_mutex_t lock;
#endif
};

#endif

