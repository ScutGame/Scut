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
#include "FileHelper.h"
#include "ZipUtils.h"
#include "Trace.h"
#include "../ScutSystem/ScutUtility.h"
#include "CCImage.h"
#include <vector>

//#ifdef SCUT_ANDROID
#include "zip_support/unzip.h"
//#endif
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
	std::string CFileHelper::s_strAndroidSDPath;
	std::string CFileHelper::s_strAndroidPackagePath;
	std::string CFileHelper::s_strRelativePath;
	std::string CFileHelper::s_strIPhoneBundleID;

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
		std::string strDirPath;

		strDirPath += getResourceDir(szPath);
		
		// prevetnt getPath(getPath(imageName));  "icon/head.png" will become "resource/resource/head.png"
		std::string strTempPath(szPath);
		
		if (strDirPath.length() > 0 && strTempPath.find(strDirPath) == 0) 
		{
			strDirPath = "";
		}

		if (strDirPath.size())
		{
			strDirPath += FILE_SEP;

			float fScale = cocos2d::CCDirector::sharedDirector()->getContentScaleFactor();
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
		if (strchr(szPath, ':'))
		{
			std::string strRet = szPath;
			return strRet;
		}
		char full_path[_MAX_PATH + 1];
		::GetModuleFileNameA(NULL, full_path, _MAX_PATH + 1);

		std::string ret((char*)full_path);

		// remove xxx.exe
		ret =  ret.substr(0, ret.rfind("\\") + 1);
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
				std::string strFilenName = s_strRelativePath + pszFileName;
				pBuffer =  getFileDataFromZip(s_strAndroidPackagePath.c_str(), strFilenName.c_str(), pSize);
			}
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

#ifdef SCUT_ANDROID
	unsigned char* CFileHelper::getFileDataFromZip(const char* pszZipFilePath, const char* pszFileName, unsigned long * pSize)
	{
		unsigned char * pBuffer = NULL;
		unzFile pFile = NULL;
		*pSize = 0;

		do 
		{
			if(!pszZipFilePath || !pszFileName)
				break;
			if ((strlen(pszZipFilePath) == 0))
			{
				break;
			}
		
			pFile = unzOpen(pszZipFilePath);
			if(!pFile)break;

			int nRet = unzLocateFile(pFile, pszFileName, 1);
			if(UNZ_OK != nRet)break;
	
			char szFilePathA[260];
			unz_file_info FileInfo;
			nRet = unzGetCurrentFileInfo(pFile, &FileInfo, szFilePathA, sizeof(szFilePathA), NULL, 0, NULL, 0);
			if(UNZ_OK != nRet)break;


			nRet = unzOpenCurrentFile(pFile);
			if(UNZ_OK != nRet)break;


			pBuffer = new unsigned char[FileInfo.uncompressed_size];
			int nSize = 0;
			nSize = unzReadCurrentFile(pFile, pBuffer, FileInfo.uncompressed_size);
			//assert(nSize == 0 || nSize == FileInfo.uncompressed_size);

			*pSize = FileInfo.uncompressed_size;
			unzCloseCurrentFile(pFile);
		} while (0);

		if (pFile)
		{
			unzClose(pFile);
		}


		return pBuffer;
	}

#endif


	bool CFileHelper::executeScriptFile(const char * pszFile)
	{

		if (pszFile == NULL || strlen(pszFile) == 0)
		{
			ScutLog("executeScriptFile error pszFile == null %s %d", __FILE__, __LINE__);
			return false;
		}

		unsigned long nSize;
		unsigned char* pBuffer;
		bool bRet = true; 
		pBuffer = (unsigned char*)getFileData(pszFile, "rb", &nSize);
		if (pBuffer)
		{
			int nTempSize = nSize;
			//int decryptResult = ScutEncrypt::DecryptData(pBuffer, nSize);
			unsigned char* pCodes = NULL;
			pCodes = pBuffer;
			/*
			if (decryptResult == 0)
			{
				pCodes = new unsigned char[nSize + 1];
				memcpy(pCodes, pBuffer, nSize);
				pCodes[nSize] = '\0';
				pBuffer -= nTempSize - nSize;
				delete []pBuffer;
			}
			else
			{
				pCodes = pBuffer;
			}
			*/
			unsigned char* pTempBuffer = pCodes;
			unsigned int outLength = 0;
			int err = 0;
			unsigned char* pszOut;
			err = unZipMemory(pCodes, nSize, &pszOut, &outLength);
			if (err == Z_OK)
			{
				pTempBuffer = pszOut;
				//pTempBuffer[outLength] = '\0';
				nSize = outLength;
			}
			else if (err == Z_DATA_ERROR)
			{
				ScutLog("ScutDataLogic::CFileHelper executeScriptFile unSzipMemory error ignore Z_DATA_ERROR\n");
			}
#ifdef SCUT_MAC
			else if (err == Z_BUF_ERROR)
			{
				ScutLog("ScutDataLogic::CFileHelper executeScriptFile unSzipMemory error ignore Z_BUF_ERROR\n");
			}
#endif
			else
			{
				ScutLog("ScutDataLogic::CFileHelper executeScriptFile unSzipMemory failed\n");
				return false;
			}

			int error =	luaL_loadbuffer(LuaHost::Instance()->GetLuaState(), (const char*)pTempBuffer, nSize, pszFile);
			
			error = Scutlua_pcall(LuaHost::Instance()->GetLuaState(),0,0);;
			// Handle errors
			if ( error )
			{
				bRet = false;
				std::string msg = lua_tostring(LuaHost::Instance()->GetLuaState(),-1);
				lua_pop(LuaHost::Instance()->GetLuaState(),1);
				lua_settop( LuaHost::Instance()->GetLuaState(), 0 );
				std::string msgerror = msg+"\n";
				ScutLog("%s  %d", msgerror.c_str(), __LINE__);

			}
			if (err == Z_OK)
			{
				delete []pszOut;
			}
			delete []pCodes;
		}
		else
		{
			ScutLog("executeScriptFile fileName:%s %s %d",pszFile, __FILE__, __LINE__);
			bRet = false;
		}
		return bRet;
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
		std::string strFilePath;
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

		char full_path[_MAX_PATH + 1];
		::GetModuleFileNameA(NULL, full_path, _MAX_PATH + 1);

		std::string ret((char*)full_path);

		// remove xxx.exe
		ret =  ret.substr(0, ret.rfind("\\") + 1);
		//ret += filename;
		strFilePath = ret + szFileName;
#endif
		if (CFileHelper::getFileState(strFilePath.c_str()) == -1)
		{
			CFileHelper::createDirs(strFilePath.c_str());
		}
		return strFilePath;

	}

	CLuaString CFileHelper::encryptPwd(const char* pPwd, const char*key)
	{
	#define  DES_KEY "j6=9=1ac"
		std::string strRet;
		ScutSystem::CScutUtility::DesEncrypt(DES_KEY, pPwd,strRet);
		return CLuaString(strRet);
	}


	//
	bool CFileHelper::unZip(const char* szZipFile, const char* pszOutPutDir)
	{
		ScutLog("CFileHelper::unZip begin");
		assert(szZipFile != NULL &&pszOutPutDir != NULL && strlen(pszOutPutDir) != 0);
		std::string strSaveDir = pszOutPutDir;
		if (strSaveDir.at(strSaveDir.size() -1) != FILE_SEP)
		{
			strSaveDir += FILE_SEP;
		}
		unzFile pFile = NULL;
		bool bRet = false;
		bool isDotNetZipPackage = true;

		do 
		{
			if(!szZipFile || strlen(szZipFile) == 0)
				break;

			unsigned char * pBuffer = NULL;

			pFile = unzOpen(szZipFile);
			if (!pFile)
			{
				break;
			}

			int err = unzGoToFirstFile(pFile);
			bool bHasErr = false;
			while (err == UNZ_OK)
			{
				char szFilePathA[256+1] = {0};
				unz_file_info FileInfo;
				err = unzGetCurrentFileInfo(pFile, &FileInfo, szFilePathA, sizeof(szFilePathA), NULL, 0, NULL, 0);
				if (err != UNZ_OK)
				{
					bHasErr = true;
					break;
				}

				bool bDir = (FileInfo.external_fa &0x40000000) != 0;
				int host = FileInfo.version >>8;
				if (host==0 || host==7 || host==11 || host==14)
				{ 
					bDir=  (FileInfo.external_fa&0x00000010)!=0;
				}
				
				std::string strSaveFilePath = strSaveDir + szFilePathA;

#ifdef SCUT_WIN32
				for (int i= 0; i < strSaveFilePath.size(); i++)
				{
					if (strSaveFilePath[i] == '/')
					{
						strSaveFilePath[i] = '\\';
					}
				}
#endif
				if (bDir)
				{
					createDirs(strSaveFilePath.c_str());
					isDotNetZipPackage = false;
				}
				else
				{
					err = unzOpenCurrentFile(pFile);
					if(UNZ_OK != err)
					{
						bHasErr = true;
						break;
					}

					pBuffer = new unsigned char[FileInfo.uncompressed_size];
					int nSize = 0;
					nSize = unzReadCurrentFile(pFile, pBuffer, FileInfo.uncompressed_size);
					unzCloseCurrentFile(pFile);

					if (isDotNetZipPackage)
					{
						std::string strSlash = strSaveFilePath;
						const char* pszSlash = strSaveFilePath.c_str();
						int i = strSaveFilePath.length() - 1;
						for (; i > 0; i--)
						{
							if (pszSlash[i] == '/' || pszSlash[i] == '\\')
							{
								break;
							}
						}
						if (i > 0 && i < strSaveFilePath.length())
						{
							strSlash = strSaveFilePath.substr(0, i);
						}
						CPathUtility::ForceDirectory(NULL, strSlash.c_str());
					}					
					bool bTempHasErr = bHasErr;
					FILE *hSaveFile = fopen(strSaveFilePath.c_str(), "wb");
					if (hSaveFile)
					{
						fwrite(pBuffer, nSize, 1, hSaveFile);
						fclose(hSaveFile);
					}
					else
					{

						ScutLog("unZip fopen error name=%s  %d errorMsg=%s", strSaveFilePath.c_str(), __LINE__,strerror(errno));
						if (strSaveFilePath.find(".mp3") != strSaveFilePath.npos)
						{
							bHasErr = bTempHasErr;
						}
						else
						{
							bHasErr = true;
						}
					}
					if (pBuffer)
					{
						delete []pBuffer;
						pBuffer = NULL;
					}

				}
			
				err = unzGoToNextFile(pFile);
			}
			if (!bHasErr)
			{
				bRet = true;
			}
			
		} while (0);

		if (pFile)
		{
			unzClose(pFile);
		}
		ScutLog("CFileHelper::unZip end");
		return bRet;
	}

	bool CFileHelper::unZipToMemory(const char* szZipFile, unsigned char **out, unsigned int *outLengh)
	{
		unzFile pFile = NULL;
		bool bRet = false;
		//bool isDotNetZipPackage = true;

		do 
		{
			if(!szZipFile || strlen(szZipFile) == 0)
				break;

			//unsigned char * pBuffer = NULL;

			pFile = unzOpen(szZipFile);
			if (!pFile)
			{
				break;
			}

			int err = unzGoToFirstFile(pFile);
			bool bHasErr = false;
			while (err == UNZ_OK)
			{
				char szFilePathA[256+1] = {0};
				unz_file_info FileInfo;
				err = unzGetCurrentFileInfo(pFile, &FileInfo, szFilePathA, sizeof(szFilePathA), NULL, 0, NULL, 0);
				if (err != UNZ_OK)
				{
					bHasErr = true;
					break;
				}

				bool bDir = (FileInfo.external_fa &0x40000000) != 0;
				int host = FileInfo.version >>8;
				if (host==0 || host==7 || host==11 || host==14)
				{ 
					bDir=  (FileInfo.external_fa&0x00000010)!=0;
				}

				{
					err = unzOpenCurrentFile(pFile);
					if(UNZ_OK != err)
					{
						bHasErr = true;
						break;
					}

					*out = new unsigned char[FileInfo.uncompressed_size + 1];
					memset(*out, 0, FileInfo.uncompressed_size + 1);
					int nSize = 0;
					nSize = unzReadCurrentFile(pFile, *out, FileInfo.uncompressed_size);
					unzCloseCurrentFile(pFile);
					
					*outLengh = nSize;
				}

				err = unzGoToNextFile(pFile);
			}
			if (!bHasErr)
			{
				bRet = true;
			}

		} while (0);

		if (pFile)
		{
			unzClose(pFile);
		}

		return bRet;
	}

	int CFileHelper::unZipMemory(unsigned char* in, unsigned int inLengh, unsigned char **out, unsigned int *outLength)
	{
		unsigned int bufferSize = 256 * 1024 * 6;
#ifdef SCUT_MAC
		bufferSize = 256 * 1024 * 50;
#endif
		uLongf size = bufferSize;
		*out = new unsigned char[bufferSize];
		memset(*out, 0, bufferSize);
		int err = Z_OK;
		err = uncompress(*out, &size, in, inLengh);

		if (err != Z_OK || *out == NULL) 
		{
			if (err == Z_MEM_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Out of memory while decompressing map data!\r\n");
			}
			else if (err == Z_VERSION_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Incompatible zlib version!\r\n");
			} 
			else if (err == Z_DATA_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Incorrect zlib compressed data!\r\n");
			}
			else
			{
				CCLOG("cocos2d: ZipUtils: Unknown error while decompressing map data!\r\n");
			}

			delete[] *out;
			*out = NULL;
			outLength = 0;

			return err;
		}

		*outLength = size;

		return err;
	}

	bool CFileHelper::zipMemory(unsigned char* in, unsigned int inLengh, unsigned char **out, unsigned int *outLength)
	{
		unsigned int bufferSize = 256 * 1024 * 4;
		*out = new unsigned char[bufferSize];
		memset(*out, 0, bufferSize);
		uLongf size = bufferSize;
		int err = Z_OK;
		err = compress(*out, &size, in, inLengh);

		if (err != Z_OK || *out == NULL) 
		{
			if (err == Z_MEM_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Out of memory while decompressing map data!\r\n");
			}
			else if (err == Z_VERSION_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Incompatible zlib version!\r\n");
			} 
			else if (err == Z_DATA_ERROR)
			{
				CCLOG("cocos2d: ZipUtils: Incorrect zlib compressed data!\r\n");
			}
			else
			{
				CCLOG("cocos2d: ZipUtils: Unknown error while decompressing map data!\r\n");
			}

			delete[] *out;
			*out = NULL;
			outLength = 0;

			return false;
		}
		
		*outLength = size;

		return true;
	
	}


	
	void CFileHelper::parseSubFoldersListFromZip(std::vector<std::string>& retVector, const char* szZipFile, const char* szAssetsName, const char* szRootFolderName)
	{
		assert(szZipFile != NULL);

		//std::vector<std::string> retVector;

		std::string strFolderName = szRootFolderName;

		//为了方便win32测试，都使用'/'
		if (strFolderName.find('/') == std::string::npos)
		{
			strFolderName += '/';
		}
		unzFile pFile = NULL;
		bool bRet = false;

		do 
		{
			if(!szZipFile || strlen(szZipFile) == 0)
				break;

			unsigned char * pBuffer = NULL;

			pFile = unzOpen(szZipFile);
			if (!pFile)
			{
				break;
			}
			int err = unzGoToFirstFile(pFile);
			bool bHasErr = false;
			while (err == UNZ_OK)
			{
				char szFilePathA[256+1] = {0};
				unz_file_info FileInfo;
				err = unzGetCurrentFileInfo(pFile, &FileInfo, szFilePathA, sizeof(szFilePathA), NULL, 0, NULL, 0);

				if (err != UNZ_OK)
				{
					bHasErr = true;
					break;
				}
				std::string strTempPath = szFilePathA;
				bool bDir = (FileInfo.external_fa &0x40000000) != 0;
				int host = FileInfo.version >>8;
				if (host==0 || host==7 || host==11 || host==14)
				{ 
					bDir=  (FileInfo.external_fa&0x00000010)!=0;
				}

				std::string strFilePathA = szFilePathA;
				if (strFilePathA.find(szAssetsName) != std::string::npos)
				{
					strFilePathA = strFilePathA.substr(strlen(szAssetsName) + 1, strFilePathA.length() - strlen(szAssetsName) - 1);
				
					if (szRootFolderName != NULL && strFilePathA.find(strFolderName) != std::string::npos)
					{
						size_t i = 0;
						for (; i < retVector.size(); i++)
						{
							if (strFilePathA.find(retVector[i] + '/') != std::string::npos)
							{
								break;
							}
						}
						if (i >= retVector.size())
						{
							int index = (int)strFilePathA.find('/', strFolderName.size() + 1);
							std::string strTempA(strFilePathA);
							if (index > 0)
							{
								strTempA = strFilePathA.substr(0, index);

								retVector.insert(retVector.end(), strTempA);
							}
						}
					}
				}
				/*else
				{
				}*/

				err = unzGoToNextFile(pFile);
				continue;
			}
			if (!bHasErr)
			{
				bRet = true;
			}

		} while (0);

		if (pFile)
		{
			ScutLog("unZipByFolder unzClose");
			unzClose(pFile);
		}
		if (bRet)
		{
			ScutLog("unZipByFolder success");
		}

		return ;
	}

	bool CFileHelper::unZipByFolder(const char* szZipFile, const char* szAssetsName, const char* szFolderName, const char* pszOutPutDir, const char* excludeExt)
	{
		assert(szZipFile != NULL &&pszOutPutDir != NULL && strlen(pszOutPutDir) != 0);

		std::string strSaveDir = pszOutPutDir;
		if (!isDirExists(strSaveDir.c_str()))
		{
			createDirs(strSaveDir.c_str());
		}
		std::string strFolderName = szFolderName;
		if (strFolderName.find(FILE_SEP) == std::string::npos)
		{
			strFolderName += FILE_SEP;
		}
		unzFile pFile = NULL;
		bool bRet = false;

		do 
		{
			if(!szZipFile || strlen(szZipFile) == 0)
				break;

			unsigned char * pBuffer = NULL;

			pFile = unzOpen(szZipFile);
			if (!pFile)
			{
				break;
			}
			int err = unzGoToFirstFile(pFile);
			bool bHasErr = false;
			while (err == UNZ_OK)
			{
				char szFilePathA[256+1] = {0};
				unz_file_info FileInfo;
				err = unzGetCurrentFileInfo(pFile, &FileInfo, szFilePathA, sizeof(szFilePathA), NULL, 0, NULL, 0);
				
				if (err != UNZ_OK)
				{
					bHasErr = true;
					break;
				}
				std::string strTempPath = szFilePathA;
				bool bDir = (FileInfo.external_fa &0x40000000) != 0;
				int host = FileInfo.version >>8;
				if (host==0 || host==7 || host==11 || host==14)
				{ 
					bDir=  (FileInfo.external_fa&0x00000010)!=0;
				}
				
				std::string strFilePathA = szFilePathA;
				if (strFilePathA.find(szAssetsName) != std::string::npos)
				{
					strFilePathA = strFilePathA.substr(strlen(szAssetsName) + 1, strFilePathA.length() - strlen(szAssetsName) - 1);
				}

				std::string strSaveFilePath = strSaveDir + strFilePathA;

				if (bDir)
				{
					bool bResult = createDirs(strSaveFilePath.c_str());
					CC_UNUSED_PARAM(bResult);
				}
				else
				{
					if (szFolderName != NULL && strSaveFilePath.find(strFolderName) == std::string::npos)
					{
						err = unzGoToNextFile(pFile);
						continue;
					}

					if (excludeExt != NULL && strSaveFilePath.find(excludeExt) != std::string::npos)
					{
						err = unzGoToNextFile(pFile);
						continue;
					}
				
					err = unzOpenCurrentFile(pFile);
					if(UNZ_OK != err)
					{
						bHasErr = true;
						break;
					}

					pBuffer = new unsigned char[FileInfo.uncompressed_size];
					int nSize = 0;
					nSize = unzReadCurrentFile(pFile, pBuffer, FileInfo.uncompressed_size);
					unzCloseCurrentFile(pFile);

					std::string tempPath;
					bool bFileName = (strSaveFilePath.find(".") != std::string::npos) ? true : false;
					if (bFileName)
					{
						int pos = strSaveFilePath.rfind(FILE_SEP);
						if (pos > -1)
						{
							tempPath.append(strSaveFilePath.substr(0, pos + 1));
							if (!isDirExists(tempPath.c_str()))
							{
								bool bResult = createDirs(tempPath.c_str());
								CC_UNUSED_PARAM(bResult);
							}
						}
					}
					FILE *hSaveFile = fopen(strSaveFilePath.c_str(), "wb");
					if (hSaveFile)
					{
						fwrite(pBuffer, nSize, 1, hSaveFile);
						fclose(hSaveFile);
					}
					else
					{
						ScutLog("unZip fopen error name=%s  %d errorMsg=%s", strSaveFilePath.c_str(), __LINE__,strerror(errno));
						bHasErr = true;
					}
					if (pBuffer)
					{
						delete []pBuffer;
						pBuffer = NULL;
					}

				}
			
				err = unzGoToNextFile(pFile);
			}
			if (!bHasErr)
			{
				bRet = true;
			}
			
		} while (0);

		if (pFile)
		{
			ScutLog("unZipByFolder unzClose");
			unzClose(pFile);
		}
		if (bRet)
		{
			ScutLog("unZipByFolder success");
		}
		return bRet;
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
		do 
		{
			struct stat buf;
			int n = stat(szFilePath, &buf);
			if ((0 != n)
				|| !(buf.st_mode&S_IFMT))	
			{
				bfind = false;
			}
		} while (0);
		if (!bfind)
		{
			std::string strFilenName = ScutDataLogic::CFileHelper::getAndroidRelativeDir();
			strFilenName += szFilePath;
			unsigned char * pBuffer = NULL;
			unzFile pFile = NULL;
			unsigned long pSize = 0;
			do 
			{
				pFile = unzOpen(ScutDataLogic::CFileHelper::getAndroidResourcePath());
				if(!pFile)break;

				int nRet = unzLocateFile(pFile, strFilenName.c_str(), 1);
				if(UNZ_OK != nRet)
				{
					bfind = false;
				}
				else
					bfind = true;
			} while (0);

			if (pFile)
			{
				unzClose(pFile);
			}
		}

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