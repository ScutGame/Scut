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
#include "PathUtility.h"
#include <time.h>


#ifdef WIN32
#include <io.h>
#else
#include <unistd.h>
#endif
#include "Defines.h"
using namespace ScutSystem;

#if defined(WIN32) || defined(__BORLANDC__) || defined(__MINGW32__)
#include <direct.h>
#define lumkdir(t) (_mkdir(t))
#else
#include <sys/stat.h>
#define lumkdir(t) (mkdir(t, 0755))
#endif

CPathUtility::CPathUtility(void)
{
}

CPathUtility::~CPathUtility(void)
{
}

bool ScutSystem::CPathUtility::IsFileExists( const char* pszFileName )
{
	if( pszFileName==NULL )
		return FALSE;
	char szFile[MAX_PATH]={0};
	int len = (int)strlen(pszFileName);
	if( pszFileName[len-1] == '\\' || pszFileName[len-1] == '/')
	{
		strcpy(szFile, pszFileName);
		strcat(szFile, "*");
	}
	else
	{
		strcpy(szFile, pszFileName);
	}
#ifdef WIN32
	return _access(szFile, 0) == 0;
#else
	return access(szFile, 0) == 0;
#endif	
}

void ScutSystem::CPathUtility::ForceDirectory( const char* pszRootDir, const char* pszDir )
{
	if (pszRootDir!=0)
	{ 
		char rd[MAX_PATH]; 
		strncpy(rd, pszRootDir, MAX_PATH); 
		size_t len= strlen(rd);
		if (len > 0 && (rd[len-1]=='/' || rd[len-1]=='\\')) 
			rd[len-1] = 0;
		//判断根目录是否存在，如果不存在则创建
		if (!IsFileExists(rd)) 
			lumkdir(rd);
	}
	if (*pszDir == 0) 
		return;
	const char *lastslash = pszDir, *c = lastslash;
	while (*c!=0) 
	{
		if (*c=='/' || *c=='\\') 
			lastslash = c; 
		c++;
	}
	const char *name = lastslash;
	if (lastslash != pszDir)
	{ 
		char tmp[MAX_PATH]; 
		memcpy(tmp, pszDir, sizeof(TCHAR) * (lastslash - pszDir));
		tmp[lastslash - pszDir] = 0;
		ForceDirectory(pszRootDir, tmp);
		name++;
	}
	char cd[MAX_PATH]; 
	*cd = 0; 
	if (pszRootDir != 0) 
		strncpy(cd, pszRootDir, MAX_PATH); 
	cd[MAX_PATH - 1] = 0;
	size_t len = strlen(cd); 
	strncpy(cd + len, pszDir, MAX_PATH - len); 
	cd[MAX_PATH-1] = 0;
	if (!IsFileExists(cd)) 
		lumkdir(cd);
//#ifndef WIN32
//	if (!IsFileExists(cd)) 
//		lumkdir(cd);
//#else
//	if (!IsFileExists(cd))
//	{ 
//		::CreateDirectory(cd, 0);
//	}
//#endif
}

bool ScutSystem::CPathUtility::DeleteFile( const char* pszFileName )
{
	if (pszFileName)
	{
		return remove(pszFileName) == 0;
	}
	return false;
}

bool ScutSystem::CPathUtility::RenameFile( const char* pszOldFileName, const char* pszNewFileName )
{
	if (pszOldFileName && pszNewFileName)
	{
		return rename(pszOldFileName, pszNewFileName) == 0;
	}
	return false;
}
