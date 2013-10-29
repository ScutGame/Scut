#import "FileHelper.h"
#import <Foundation/Foundation.h>
#include<string>

using namespace std;
namespace ScutDataLogic
{
	string g_strModuleFilePath = "";
	string g_strTempReturnFilePath ="";
	const string& getResourcePath(const char *szSubPath)
	{
		
		if (g_strModuleFilePath.size() == 0) 
		{
					
			NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];


			g_strModuleFilePath = [[[NSBundle mainBundle] bundlePath] UTF8String];
			
			[pool release];
		}
		
		if (szSubPath != NULL) 
		{
			if(strlen(szSubPath) &&szSubPath[0] != '/')
			{
				g_strTempReturnFilePath = g_strModuleFilePath + '/' + szSubPath;
			}
			else
			{
				g_strTempReturnFilePath = g_strModuleFilePath + szSubPath;
				
			}
			//log("FilePath", "%s", g_strTempReturnFilePath.c_str());
			
			return g_strTempReturnFilePath;
		}
		else
		{
			return g_strModuleFilePath;
		}

	}
	
	string g_strDocumentDirPath = "";
	const string& getDocumentPath()
	{
		
		if (g_strDocumentDirPath.size())
		{
			return g_strDocumentDirPath;
		}
		else
		{
			NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
			NSArray* arrays= NSSearchPathForDirectoriesInDomains(NSDocumentDirectory, NSUserDomainMask, YES);
			NSString *paths = [arrays objectAtIndex:0];
	
			g_strDocumentDirPath = [paths UTF8String];
			[pool release];
		}
		
		return g_strDocumentDirPath;

	}
	
	string getDocumentFilePathByFileName(const char* szFilePath)//
	{
		//NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
		string strFilePath;
		if (szFilePath == NULL || strlen(szFilePath) == 0) {
			NSLog(@"Error szFilePath==null || strlen(szFilePath) == 0, in getDocumentFilePath");
			return strFilePath;
		}
		//log("DocumentPath", "%s", getDocumentPath().c_str());
		if (szFilePath[0] == '/')
		{
			strFilePath = getDocumentPath();
			strFilePath+= szFilePath;
		}
		else
		{
			strFilePath = getDocumentPath();
			strFilePath += '/';
			strFilePath += szFilePath;
		}
		//[pool release];
		return strFilePath;

	}
	const char* appFullPathFromRelativePath(const char* pszRelativePath)
	{

		 // NSAssert(pszRelativePath != nil, @"CCFileUtils: Invalid path");

		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
		 // do not convert an absolute path (starting with '/')
		NSString *relPath = [NSString stringWithUTF8String: pszRelativePath];
		 NSString *fullpath = nil;
	
		// only if it is not an absolute path
		if( ! [relPath isAbsolutePath] )
		{
			NSString *file = [relPath lastPathComponent];
			NSString *imageDirectory = [relPath stringByDeletingLastPathComponent];
			
			fullpath = [[NSBundle mainBundle] pathForResource:file
													   ofType:nil
												  inDirectory:imageDirectory];
		}

		if (fullpath == nil)
			fullpath = relPath;
		std::string strRet = [fullpath UTF8String];	
		[pool release];
		return strRet.c_str();

	}

	std::string getBundleID()
	{
		std::string strRet;
		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];
		NSString* identifier = [[NSBundle mainBundle] objectForInfoDictionaryKey:@"CFBundleIdentifier"];
		
		strRet = [identifier UTF8String];

		[pool release];
		return strRet;
	}
	
}
