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



#include "PackageUnzipHandler.h"
#include "FileHelper.h"
#include "iniEx.h"
#include "CCFileUtils.h"
#include "Trace.h"

CPackageUnzipHandler* CPackageUnzipHandler::m_pInstance = NULL;

CPackageUnzipHandler::CPackageUnzipHandler()
{
	
}

CPackageUnzipHandler::~CPackageUnzipHandler()
{
	m_pInstance = NULL;
}

CPackageUnzipHandler* CPackageUnzipHandler::sharedPackageUnzipHandler()
{
	if (m_pInstance == NULL)
	{
		m_pInstance = new CPackageUnzipHandler();
	}

	return m_pInstance;
}

bool CPackageUnzipHandler::unzipFlag()
{
	ScutLog("CPackageUnzipHandler::unzipFlag begin");
	if (!getUnzipFlag())
	{
		ScutLog("CPackageUnzipHandler::unzipFlag unZipByFolder begin");
		//ScutLog("CFileHelper::unzipFlag szZipFile=%s  pszOutPutDir=%s %d errorMsg=%s", ScutDataLogic::CFileHelper::getANDROIDResourcePath(), ScutDataLogic::CFileHelper::getANDROIDSDCardDirPath(), __LINE__,strerror(errno));
		bool bUnzip = ScutDataLogic::CFileHelper::unZipByFolder(ScutDataLogic::CFileHelper::getAndroidResourcePath(), "assets", "lua", ScutDataLogic::CFileHelper::getAndroidSDCardDirPath());
		ScutLog("CPackageUnzipHandler::unzipFlag unZipByFolder end");
		setunzipFlag(bUnzip);
	}
	else
	{
		ScutLog("getUnzipFlag() == false");
	}

	ScutLog("CPackageUnzipHandler::unzipFlag end");

	return true;
}

void CPackageUnzipHandler::setunzipFlag(bool bUnzip)
{
	if (bUnzip)
	{
		ScutLog("CPackageUnzipHandler::setunzipFlag bUnzip == true");
	}
	writeToConfig(bUnzip);
}

const char* CPackageUnzipHandler::getUpdateConfigPath()
{
	if (m_strConfigPath.empty())
	{
		m_strConfigPath = cocos2d::CCFileUtils::sharedFileUtils()->getWritablePath();///data/data/apkname/
		if (!ScutDataLogic::CFileHelper::isDirExists(m_strConfigPath.c_str()))
		{
			ScutDataLogic::CFileHelper::createDirs(m_strConfigPath.c_str());
		}

		m_strConfigPath += UNZIP_FILENAME;


		FILE* pFile = fopen(m_strConfigPath.c_str(), "rb");
		if (!pFile)
		{
			pFile = fopen(m_strConfigPath.c_str(), "wb");
		}

		if (pFile)
		{
			fclose(pFile);
		}
	}
	
	ScutLog("CPackageUnzipHandler::getUpdateConfigPath  m_strConfigPath : %s", m_strConfigPath.c_str());

	return m_strConfigPath.c_str();
}

bool CPackageUnzipHandler::writeToConfig(bool bUnzip)
{
	const char* pszPathName = getUpdateConfigPath();
	if (!pszPathName)
	{
		return false;
	}

	int nFlag = bUnzip ? 1 : 0;

	ScutDataLogic::CIniFile* pSDIni = new ScutDataLogic::CIniFile();
	pSDIni->Load(pszPathName);
	pSDIni->SetInt("configflag", "configflag", nFlag);
	pSDIni->Save(pszPathName);

	delete pSDIni;

	return true;
}

bool CPackageUnzipHandler::getUnzipFlag()
{
	const char* pszPathName = getUpdateConfigPath();
	if (!pszPathName)
	{
		return false;
	}

	ScutDataLogic::CIniFile* pSDIni = new ScutDataLogic::CIniFile();
	pSDIni->Load(pszPathName);
	int nConfigflag = pSDIni->GetInt("configflag", "configflag", "-1");
	if (nConfigflag == -1)
	{
		pSDIni->SetInt("configflag", "configflag", 0);
		nConfigflag = pSDIni->GetInt("configflag", "configflag", 0);
		pSDIni->Save(pszPathName);
	}
	delete pSDIni;


	return nConfigflag == 1 ? true : false;
}