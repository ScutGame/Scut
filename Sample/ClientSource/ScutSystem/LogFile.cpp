#include "stdafx.h"
#ifdef WIN32
#else // #ifdef WIN32
#include <string.h>
#include <unistd.h>
#endif

#include "LogFile.h"
#include <time.h>

CLogFile::CLogFile(const CStdString &filename)
{
	m_strLogFile = filename;
	fp = fopen(m_strLogFile, "a");

	if(fp == NULL)
	{
		fp = fopen(m_strLogFile, "w");
	}
#ifndef WIN32
	if (fp == NULL)
	{
		fp = stderr;
	}
#endif
	fseek(fp, 0, SEEK_END);

	m_nFileSize = ftell(fp);
#ifndef WIN32
	/* i have to do all these shit things here becuase of that
	 * blasted g++ screws my happiness if i declare lock as a
	 * global variable in this file itself.it conflicts with
	 * iostream lock ! :-( */
	pthread_mutex_init(&lock,NULL);
#else
	InitializeCriticalSection(&lock);
#endif

}

CLogFile::~CLogFile()
{
	fclose(fp);
#ifdef WIN32
	DeleteCriticalSection(&lock);
#endif
}

void CLogFile::Write(const LOGTYPE logtype, const char *format, ...)
{
#ifdef WIN32
	EnterCriticalSection(&lock);
#else
	pthread_mutex_lock(&lock);
	time_t timeVal=0;
#endif

	CStdString formatStr;
	CStdString str;
	char strTime[80];

	if (fp == NULL)
		goto write_exit; // Log file has been disabled

	va_list list;
	va_start(list, format);

#ifdef WIN32
	char strDate[80];
	formatStr.Format("[%s %s] ", _strdate(strDate), _strtime(strTime));
	formatStr += format;
#else
	time(&timeVal);
	strcpy(strTime,ctime(&timeVal));
	if ( strTime[0] != '\0' )
	{
		strTime[strlen(strTime) -1 ] = '\0';
	}
	formatStr.Format("[ %s ] ",strTime);
	formatStr += format;
#endif
				
	str.FormatV(formatStr.c_str(), list);
	va_end(list);

	if(str.GetAt(str.length()-1) != '\n')
	{
		str += "\n";
	}
	else if(str.GetAt(str.length()-2) == '\r')
	{
		str.Delete(str.length()-2,2);
		str += "\n";
	}

	m_nFileSize += str.length();

	//if(logtype == LOG_BASIC ||
	//   (UINT)logtype <= settings.m_nLogLevel)
	//{
		fprintf(fp, "%s", str.c_str());
	//}

	fflush(fp);

	//if(settings.m_bLimitLog && m_nFileSize > (long)(settings.m_nLogSize * 1024))
	//{
	//	/* Trim the file */
	//	Trim();
	//}

write_exit:
#ifdef WIN32
	LeaveCriticalSection(&lock);
#else
	pthread_mutex_unlock(&lock);
#endif
}

void CLogFile::Trim()
{
	fclose(fp);
#ifdef WIN32
	_unlink(m_strLogFile);
	fp = fopen(m_strLogFile, "at");
#else
	unlink(m_strLogFile);
	fp = fopen(m_strLogFile, "a+");
#endif
	m_nFileSize = 0;
}

