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

#include "FileHelper.h"
#include "ZipUtils.h"
#include "Trace.h"
#include "ScutUtility.h"
#include "ScutExt.h"
#include "CCImage.h"
#include <vector>

#if defined(SCUT_IPHONE) || defined(SCUT_ANDROID) || defined(SCUT_MAC)
#include <dlfcn.h>
#include <sys/stat.h>
#include<sys/types.h>
#include <dirent.h>
#include<unistd.h>
#include<errno.h>
#endif
#include<string>
#include"LuaString.h"
#ifdef SCUT_WIN32
#include <WinBase.h>
#endif

#ifndef FILE_SEP
#ifdef SCUT_WIN32
#define FILE_SEP '\\'
#else
#define FILE_SEP '/'
#endif

#endif
#include "LuaHost.h"
#include "CCDirector.h"
#include "PathUtility.h"

using namespace ScutSystem;


std::string getPath(const char* szPath, bool bOnly2X)
{
	if(szPath) 
		return ScutDataLogic::CFileHelper::getPath(szPath, bOnly2X);
	else
		return NULL;
}

extern "C" int   (Scutlua_pcall) (lua_State *L, int nargs, int nresults);

namespace ScutDataLogic
{
	int CFileHelper::s_width = 960;
	int CFileHelper::s_height = 640;

	std::string CFileHelper::s_strResource			= "resource";
	std::string CFileHelper::s_strResource480_800	= "resource480_800";
	std::string CFileHelper::s_strResource1280_800	= "resource1280_800";
#ifdef SCUT_WIN32
	std::string CFileHelper::s_strConfigResource    = "";
#endif
	std::string CFileHelper::s_strAndroidSDPath = "";
	std::string CFileHelper::s_strAndroidPackagePath = "";
	std::string CFileHelper::s_strRelativePath = "";
	std::string CFileHelper::s_strIPhoneBundleID = "";

	void string_replace(std::string& strBig, const std::string & strsrc, const std::string &strdst)
	{
		std::string::size_type pos = 0;
		while( (pos = strBig.find(strsrc, pos)) != std::string::npos)

		{
			strBig.replace(pos, strsrc.length(), strdst);
			pos += strdst.length();
		}
	}

	std::string CFileHelper::getPath(const char* szPath, bool bOnly2X)
	{
		if (szPath == NULL)
		{
			ScutLog("getPath Input parameter == Null");
			return std::string();
		}
		if (szPath[0] == '/') {
			return std::string(szPath);
		}

#ifdef SCUT_ANDROID
		if (strstr(szPath, "resource/")) {
			return std::string(szPath);
		}
#endif
		std::string strDirPath = "";
		strDirPath += getResourceDir(szPath);
		
		// prevent getPath(getPath(imageName));  "icon/head.png" will become "resource/resource/head.png"
		std::string strTempPath(szPath);
		
		if (strDirPath.length() > 0 && strTempPath.find(strDirPath) == 0) 
		{
			strDirPath = "";
		}

		if (strDirPath.size())
		{
			strDirPath += FILE_SEP;

			float fScale = cocos2d::Director::getInstance()->getContentScaleFactor();
			if (fScale > 1 || !(s_width == 480 && s_height == 320 || s_height == 320&& s_width == 480 || s_width
					< 480 && s_height <480))
			{
				bool bTmx = false;
				const char* pPos = strstr(szPath, ".jpg");
				if (pPos == NULL)	//yubo image
				{
					pPos = strstr(szPath, ".ndj");
				}
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".png");
				}
				/*
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".pnx");
				}
				*/
				if (pPos == NULL)	//yubo image
				{
					pPos = strstr(szPath, ".ndp");
				}
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".tmx");
					bTmx = true;
				}
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".pvr");
					bTmx = true;
				}
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".ccz");
					bTmx = true;
				}
				if (pPos == NULL)
				{
					pPos = strstr(szPath, ".plist");
					bTmx = true;
				}
				if(pPos == NULL)
				{
					pPos = strstr(szPath, ".dat");
				}
				if (pPos != NULL)
				{
					char szTmp[256] = {0};
					char szFileName[256] = {0};
					memcpy(szFileName, szPath, pPos - szPath);
					if (bTmx && ((s_width == 1024 && s_height == 768) || (s_height == 1024&& s_width == 768)))
					{
						if (bOnly2X)
						{
							sprintf(szTmp, "%s@2x%s", szFileName, pPos);
						}
						else
							sprintf(szTmp, "%s@2x-ipad%s", szFileName, pPos);
					    }
#if (defined SCUT_MAC || defined SCUT_WIN32)
					else if (bTmx && ((s_width == 960 && s_height == 640) || (s_height == 960 && s_width == 640)))
					{
						if (bOnly2X)
						{
							sprintf(szTmp, "%s@2x%s", szFileName, pPos);
						}
						else
							sprintf(szTmp, "%s@2x-ipad%s", szFileName, pPos);
					}
#endif
					else
					{
						sprintf(szTmp, "%s@2x%s", szFileName, pPos);
					}
					
					strDirPath += szTmp;
				}
				else
				{
					strDirPath += szPath;
				}
			}
			else
			{
				strDirPath += szPath;
			}
			
		}
		else
		{
			strDirPath = szPath;
		}
#if defined(SCUT_IPHONE) || defined(SCUT_MAC)
		if (s_strIPhoneBundleID.size() == 0)
		{
			s_strIPhoneBundleID = ScutDataLogic::getBundleID();
		}

		std::string strRelativePath = s_strIPhoneBundleID;
		strRelativePath += FILE_SEP;
		strRelativePath += strDirPath;
		std::string strFilePath = ScutDataLogic::getDocumentFilePathByFileName(strRelativePath.c_str());
		struct stat st;
		if (true && stat(strFilePath.c_str(), &st) != 0)//cocos2d::CCImage::getIsScaleEnabled()
		{
			if (strFilePath.rfind("@2x") == std::string::npos)
			{
				strTempPath = strFilePath;
				int t = strTempPath.rfind(".");
				if (t != std::string::npos)
				{
					strTempPath.insert(t, "@2x");
					if (stat(strTempPath.c_str(), &st) == 0)
					{
						return strFilePath;
					}
				}
			}
			
//			if (strDirPath.rfind("@2x") == std::string::npos)
//			{
//				int t = strDirPath.rfind(".");
//				if (t != std::string::npos)
//				{
//					strDirPath.insert(t, "@2x");
//				}
//			} 
		}
		
		if (stat(strFilePath.c_str(), &st) == 0) 
		{
			return strFilePath;
		}
		else
		{
            if (strFilePath.rfind(".png") != std::string::npos)
            {
                strTempPath = strFilePath;
                string_replace(strTempPath, ".png", ".pnx");
                if (stat(strTempPath.c_str(), &st) == 0)
                {
                    return strTempPath;
                }
            }

			std::string retPath = std::string(appFullPathFromRelativePath(strDirPath.c_str()));
			if (retPath == strDirPath)
			{
				strTempPath = strDirPath;
				string_replace(strTempPath, ".png", ".pnx");
				retPath = std::string(appFullPathFromRelativePath(strTempPath.c_str()));
				if (retPath == strTempPath)
				{
					return strDirPath;
				}
				else
					return retPath;
			}
            
            return retPath;
		}
#endif

#ifdef SCUT_ANDROID
		if (s_strAndroidSDPath.size())
		{
			std::string strFilePath = s_strAndroidSDPath + strDirPath;
			struct stat st;

			if (true && stat(strFilePath.c_str(), &st) != 0)
			{
				if (strFilePath.rfind("@2x") == std::string::npos)
				{
					strTempPath = strFilePath;
					int t = strTempPath.rfind(".");
					if (t != std::string::npos)
					{
						strTempPath.insert(t, "@2x");
						if (stat(strTempPath.c_str(), &st) == 0)
						{
							return strFilePath;
						}
					}
				}
			}

			if (stat(strFilePath.c_str(), &st) == 0) 
			{
				return strFilePath;
			}
			else
			{
				if (strFilePath.rfind(".png") != std::string::npos)
				{
					strTempPath = strFilePath;
					string_replace(strTempPath, ".png", ".pnx");
					if (stat(strTempPath.c_str(), &st) == 0)
					{
						return strTempPath;
					}
				}
			}
		}

		return std::string(strDirPath);
#endif

#ifdef SCUT_WIN32
		std::string ret= ScutExt::getResRootDir();
		ret += strDirPath;

		struct stat st; 
		if (ret.rfind(".png") != std::string::npos && stat(ret.c_str(), &st) != 0)
		{
			strTempPath = ret;
			string_replace(strTempPath, ".png", ".pnx");
			if (stat(strTempPath.c_str(), &st) == 0)
			{
				return strTempPath;
			}
		}
		return ret;
#endif	
	}

	void CFileHelper::setWinSize(int nWidth, int nHeight)
	{
		if (nWidth > nHeight)
		{
			s_width  = nWidth;
			s_height = nHeight;
		}
		else
		{
			s_width  = nHeight;
			s_height = nWidth;
		}
	}


	std::string CFileHelper::getResourceDir(const char* pszFileName)
	{
		std::string strRet;
		const char* pPos = strstr(pszFileName, ".jpg");
		/*if (pPos == NULL)	//yubo image
		{
			pPos = strstr(pszFileName, ".ndj");
		}*/
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".png");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".pnx");
		}
		/*
		if (pPos == NULL)	//yubo image
		{
			pPos = strstr(pszFileName, ".ndp");
		}*/
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".tmx");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".spr");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".mp3");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".ogg");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".plist");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".pvr");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".ccz");
		}
		if (pPos == NULL)
		{
			pPos = strstr(pszFileName, ".dat");
		}
		if (pPos == NULL)
		{
			return strRet;
		}

#if defined(SCUT_IPHONE) || defined(SCUT_MAC)
		strRet = s_strResource;
#endif

#ifdef SCUT_ANDROID
		if (s_width == 480 && s_height == 320 || s_height == 320&& s_width == 480 || s_width
			 < 480 && s_height <480)
		{
			strRet = s_strResource;
		}
		else
		{
			//strRet = s_strResource480_800;
			strRet = s_strResource;
		}
#endif

#ifdef SCUT_WIN32
		if (s_strConfigResource.length() > 0)
		{
			strRet = s_strConfigResource;
		}
		else
		{
			if (s_width == 480 && s_height == 320 || s_height == 320&& s_width == 480 || s_width
				< 480 && s_height <480 || s_height == 1024 && s_width == 768 ||s_height == 768&& s_width == 1024
				|| s_height == 640 && s_width == 960 || s_height == 320 && s_width == 568)
			{
				strRet = s_strResource;
			}
			else if (s_width == 1280 && s_height == 720 || s_width == 1280 && s_height == 800 || 
				s_width == 720 && s_height == 1280 || s_width == 800 && s_height == 1280 )
			{
				strRet = s_strResource1280_800;
			}
			else//ANDROID 480*800
			{
				strRet = s_strResource480_800;
			}
		}
#endif
		return strRet;
	}

	void CFileHelper::setAndroidRelativeDir(const char* pszPath)
	{
		if (pszPath)
		{
			s_strRelativePath = pszPath;
			if (s_strRelativePath.at(s_strRelativePath.size() -1) != '/')
			{
				s_strRelativePath += '/';
			}
		}
	}
	void CFileHelper::setAndroidResourcePath(const char* pszPath)
	{
		if (pszPath)
		{
			s_strAndroidPackagePath = pszPath;
// 			if (s_strANDROIDPackagePath.at(s_strANDROIDPackagePath.size() -1) != '/')
// 			{
// 				s_strANDROIDPackagePath += '/';
// 			}
		}
	}

	void CFileHelper::setAndroidSDCardDirPath(const char *szPath)
	{
		if (szPath == NULL)
		{
			return ;
		}
		s_strAndroidSDPath = szPath;
		if (s_strAndroidSDPath.size())
		{
			if (s_strAndroidSDPath.at(s_strAndroidSDPath.size() -1) != '/')
			{
				s_strAndroidSDPath += '/';
			}
		}
	}

	const char* CFileHelper::getAndroidSDCardDirPath()
	{
		if (s_strAndroidSDPath.size())
		{
			return s_strAndroidSDPath.c_str();
		}
		return NULL;
	}

	unsigned char* CFileHelper::getFileData(const char* pszFileName, const char* pszMode, unsigned long *pSize)
	{
		unsigned char * pBuffer = NULL;
		*pSize = 0;
#ifdef SCUT_ANDROID
			do 
			{
				// read the file from hardware
				FILE *fp = fopen(pszFileName, pszMode);
				if(!fp)break;

				fseek(fp,0,SEEK_END);
				*pSize = ftell(fp);
				fseek(fp,0,SEEK_SET);
				pBuffer = new unsigned char[*pSize];
				*pSize = fread(pBuffer,sizeof(unsigned char), *pSize,fp);
				fclose(fp);
			} while (0);
			if (pBuffer == NULL)
			{
				ScutLog("getFileData Error fileName=%s", pszFileName);
			}

#else
		do 
		{
			// read the file from hardware
			FILE *fp = fopen(pszFileName, pszMode);
			if(!fp)break;

			fseek(fp,0,SEEK_END);
			*pSize = ftell(fp);
			fseek(fp,0,SEEK_SET);
			pBuffer = new unsigned char[*pSize];
			*pSize = fread(pBuffer,sizeof(unsigned char), *pSize,fp);
			fclose(fp);
		} while (0);
#endif
		
		return pBuffer;
	}

	int CFileHelper::getFileState(const char*szfilePath)
	{
		int nInfo = 0;
#ifdef SCUT_WIN32
		WIN32_FILE_ATTRIBUTE_DATA info;
		if (GetFileAttributesExA(szfilePath, GetFileExInfoStandard, &info) != 0)
		{
			nInfo =  info.dwFileAttributes & FILE_ATTRIBUTE_DIRECTORY;
		}
		else
		{
			nInfo = -1;
		}

#else
		struct stat st;
		if (stat(szfilePath, &st) == 0) {
			nInfo = S_ISDIR(st.st_mode);
		}
		else
		{
			nInfo = -1;
		}

#endif
		return nInfo;
	}


	bool CFileHelper::createDirs(const char* szDir)
	{
		if (szDir == NULL)
		{
			ScutLog("createDirs Error %s %d", szDir, __LINE__);
			return false;
		}

		const char *pBegin = szDir;
		const char *pend = szDir;
		do 	{
			char szBuf[MAX_PATH];
			pend = strchr(pend,FILE_SEP);
			if (NULL != pend)	
		 {
			 ++pend;
			 int nSize = pend-pBegin;
			 memcpy(szBuf,pBegin,nSize);
			 szBuf[nSize] = '\0';
			 std::string dir;
			 dir = szBuf;
			 if (!isDirExists(dir.c_str()))	
			 {
				 if(!createDir(dir.c_str()))	
				 {
					 ScutLog("createDirs Error %s %d", szDir, __LINE__);
					 break;
				 }
			 }
		 }
		} while (NULL != pend);

		return true;
	}

	bool CFileHelper::isDirExists(const char* dir)
	{
#ifdef SCUT_WIN32
		//TCHAR dirpath[MAX_PATH];
		//MultiByteToWideChar(936,0,dir,-1,dirpath,sizeof(dirpath));
		DWORD dwFileAttr = GetFileAttributes(dir);
		if (INVALID_FILE_ATTRIBUTES == dwFileAttr
			|| !(dwFileAttr&FILE_ATTRIBUTE_DIRECTORY))	{
				return false;
		}		
#else
		struct stat buf;
		int n = stat(dir, &buf);
		if ((0 != n)
			|| !(buf.st_mode&S_IFDIR))	{
				return false;
		}		

#endif
		return true;
	}
	

	bool CFileHelper::createDir(const char* dir)	
	{
		bool bRes = true;
#ifdef SCUT_WIN32
		//TCHAR dirpath[MAX_PATH];
		//MultiByteToWideChar(936,0,dir,-1,dirpath,sizeof(dirpath));
		BOOL b = CreateDirectory(dir,NULL);
		if (!b)	{
			bRes = false;
		}		
#else
		int n = mkdir(dir, 0777);
		if (0 != n)	{
			bRes = false;
		}		

#endif
		return bRes;
	}


	std::string CFileHelper::getWritablePath(const char* szFileName)
	{
		std::string strFilePath = "";
		if (szFileName == NULL)
		{
			return strFilePath;
		}

#ifdef SCUT_ANDROID
		const char* pszSDCard = CFileHelper::getAndroidSDCardDirPath();
		if (pszSDCard)
		{
			strFilePath = pszSDCard;
		}
		
		strFilePath += szFileName;
#endif

#if defined(SCUT_IPHONE) || defined(SCUT_MAC)
		std::string strRel = ScutDataLogic::getBundleID();
		strRel += FILE_SEP;
		strRel += szFileName;
		strFilePath = ScutDataLogic::getDocumentFilePathByFileName(strRel.c_str());
#endif

#ifdef SCUT_WIN32
		strFilePath = ScutExt::getResRootDir();
		strFilePath += szFileName;
#endif
		if (CFileHelper::getFileState(strFilePath.c_str()) == -1)
		{
			CFileHelper::createDirs(strFilePath.c_str());
		}
		return strFilePath;
	}

	CLuaString CFileHelper::encryptPwd(const char* pPwd, const char*key)
	{
		#define  DES_KEY "n7=7=7dk"
		std::string strRet;
		ScutSystem::CScutUtility::DesEncrypt(DES_KEY, pPwd,strRet);
		return CLuaString(strRet);
	}

	void CFileHelper::freeFileData( unsigned char* pFileDataPtr )
	{
		if (pFileDataPtr)
		{
			delete []pFileDataPtr;
		}
	}

	bool CFileHelper::isFileExists(const char* szFilePath)
	{
#if CC_TARGET_PLATFORM == CC_PLATFORM_WIN32
		DWORD dwFileAttr = GetFileAttributesA(szFilePath);
		if (INVALID_FILE_ATTRIBUTES == dwFileAttr
			|| (dwFileAttr&FILE_ATTRIBUTE_DIRECTORY))	{
				return false;
		}		
#elif CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID
		bool bfind = true;
		return bfind;
#else
		struct stat buf;
		int n = stat(szFilePath, &buf);
		if ((0 != n)
			|| !(buf.st_mode&S_IFMT))	{
				return false;
		}		

#endif
		return true;
	}
}