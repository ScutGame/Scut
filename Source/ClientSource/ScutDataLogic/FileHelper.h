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




#ifndef FILEHELPER_H
#define FILEHELPER_H
#include <string>
#include <vector>

extern "C"
{
#include "lua.h"
#include "lauxlib.h"
#include "lauxlib.h"
}




std::string getPath(const char* szPath, bool bOnly2X = false);

namespace  ScutDataLogic
{
	class CLuaString;
	class  CFileHelper
	{
	public:
		static std::string getPath(const char* szPath, bool bOnly2X = false);
		//设置app 在ANDROID SD卡上的目录   
		static void setAndroidSDCardDirPath(const char *szPath);
		static const char* getAndroidSDCardDirPath();
		static void setAndroidResourcePath(const char* pszPath);//apk path
		static const char* getAndroidResourcePath(){return s_strAndroidPackagePath.c_str();}
		static void setAndroidRelativeDir(const char* pszPath);
		static const char* getAndroidRelativeDir(){return s_strRelativePath.c_str();}
#ifdef SCUT_ANDROID
		static unsigned char* getFileDataFromZip(const char* pszZipFilePath, const char* pszFileName, unsigned long * pSize);
#endif
		static unsigned char* getFileData(const char* pszFileName, const char* pszMode, unsigned long *pSize);
		static void freeFileData(unsigned char* pFileDataPtr);

		static bool executeScriptFile(const char * pszFile);

		//添加不同分辨率读取不同目录下的资源
		//iphone --->resource 
		//ANDROID --->480*320 resource   否则都读resource480_800
		static std::string  getResourceDir(const char* pszFileName);
#ifdef SCUT_WIN32
		static void setResourceFolderName(const char* pszResourceFolderName) { s_strConfigResource = pszResourceFolderName;}
#endif

		static void setWinSize(int nWidth, int nHeight);
		
		static int getFileState(const char* pszFileName);
		static bool createDirs(const char* szDir);
		static bool isDirExists(const char* dir);
		static bool createDir(const char* dir);
		static bool isFileExists(const char* szFilePath);

		static std::string getWritablePath(const char* szFileName);

		static CLuaString encryptPwd(const char* pPwd, const char*key);

		static bool unZip(const char* szZipFile, const char* pszOutPutDir);

		static bool unZipToMemory(const char* szZipFile, unsigned char **out, unsigned int *outLengh);

		static int unZipMemory(unsigned char* in, unsigned int inLengh, unsigned char **out, unsigned int *outLength);

		static bool zipMemory(unsigned char* in, unsigned int inLengh, unsigned char **out, unsigned int *outLength);

		static bool unZipByFolder(const char* szZipFile, const char* szAssetsName, const char* szFolderName, const char* pszOutPutDir, const char* excludeExt = NULL);	

		static void parseSubFoldersListFromZip(std::vector<std::string>& retVector, const char* szZipFile, const char* szAssetsName, const char* szRootFolderName);
	private:
		static int s_width;
		static int s_height;
#ifdef SCUT_WIN32
		static std::string s_strConfigResource;
#endif
		static std::string s_strResource;
		static std::string s_strResource480_800;
		static std::string s_strResource1280_800;
		static std::string s_strAndroidSDPath;
		static std::string s_strAndroidPackagePath;
		static std::string s_strRelativePath;
		static std::string s_strIPhoneBundleID;
	};


#if defined(SCUT_IPHONE) || defined(SCUT_MAC)

	extern "C" 
	{
		const std::string& getDocumentPath();
		std::string getDocumentFilePathByFileName(const char* szFilePath);

		//获取App 目录下的资源
		const char* appFullPathFromRelativePath(const char* pszRelativePath);

		std::string getBundleID();
	}

#endif
}
#endif//FILEHELPER_H