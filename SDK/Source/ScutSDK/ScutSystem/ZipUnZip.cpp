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
#include "ZipUnZip.h"
#include "unzip.h"
#include <vector>

using namespace std;

using namespace ScutSystem;

typedef struct tagDECOMPRESSPARAM{
	void* pDlg;
	void* appData;
	const char* lpszDecompressFile;
	const char* lpszDestDir;
	bool bResult;
}DECOMPRESSPARAM,*PDECOMPRESSPARAM;

typedef struct tagZipContent
{
	int index; 
	unsigned int dwAttributes;		
	std::string strFileName;

}ZipContent, *PZipContent, RarContent;

bool InputZipPassword(char* password)
{
	return FALSE;
}

bool Unzip(LPCTSTR lpszZip,LPCTSTR lpszDestDir,LPVOID lpParam, LPVOID appData = NULL, vector<string>* pvtFile = NULL,LPCTSTR lpszPreName = NULL)
{
	if (lpszZip == NULL)
	{
		return FALSE;
	}

	bool bUnzip = TRUE;
	char password[128]={0};
	bool bGetPassword = FALSE;
	do{
		if(bGetPassword)
			bGetPassword = FALSE;
		HZIP hz = OpenZip(lpszZip,password);
		if(hz == NULL)
			return FALSE;

		if(lpszDestDir != NULL)
		{
			SetUnzipBaseDir(hz,lpszDestDir);
		}

		ZIPENTRY ze; 
		GetZipItem(hz,-1,&ze);
		int numitems=ze.index;
		for (int zi=0; zi<numitems; zi++)
		{
			ZIPENTRY ze;
			GetZipItem(hz,zi,&ze);
			if( lpszPreName )
			{
				TCHAR szName[MAX_PATH]={0};
				printf(szName, "%s%s", lpszPreName, ze.name);
				strcpy(ze.name,szName);
			}
			if( pvtFile )
			{
				std::string strFile(ze.name);
				pvtFile->push_back(strFile);
			}
			ZRESULT zres = UnzipItem(hz, zi, ze.name);
			if(zres != ZR_OK && ze.comp_size)
			{
				ScutLog("UnZipFile Error %s", ze.name);
				if(zres == ZR_PASSWORD)
				{
					if(InputZipPassword(password)){
						bGetPassword = TRUE;
						break;
					}
				}
				bUnzip = FALSE;
				break;
			}
		}
		CloseZip(hz);
	}while(bGetPassword);
	return bUnzip;
}


static DWORD UnzipProc(void* lpParameter)
{
	PDECOMPRESSPARAM pDecompress = (PDECOMPRESSPARAM)lpParameter;
	if(pDecompress == NULL)
		return FALSE;

	if(Unzip(pDecompress->lpszDecompressFile,pDecompress->lpszDestDir,pDecompress->pDlg, pDecompress->appData))
		pDecompress->bResult = TRUE;
	else
		pDecompress->bResult = FALSE;

	return TRUE;
}

bool DecompressZipSync(const char* lpszZip, const char* lpszDestDir = NULL, const char* lpszDecFile = NULL, const char* lpszReName = NULL, void* appData = NULL)
{
	if(lpszZip == NULL)
	{
		return FALSE;
	}

	/* 解压其中一个文件 */
	if(lpszDecFile != NULL)
	{
		return FALSE;
		//return DecompressZipSyncByFileName(lpszZip, lpszDestDir, lpszDecFile, lpszReName);
	}

	/* 全部解压 */
	DECOMPRESSPARAM deParam = {NULL,appData,lpszZip,lpszDestDir,FALSE};
	UnzipProc(&deParam);

	return deParam.bResult;
}


////////////////////////////////CZipUnZip////////////////////////////////////////////

CZipUnZip::CZipUnZip(void)
{
}

CZipUnZip::~CZipUnZip(void)
{
}

bool ScutSystem::CZipUnZip::UnZipFile( const char* pszZipFileName, const char* pszDestDirName )
{
	return DecompressZipSync(pszZipFileName, pszDestDirName) == TRUE;
}
