#import "IphoneInterface.h"
#import "EAGLView.h"
#import <Foundation/Foundation.h>
#include <stdio.h>
#include <sys/sysctl.h>

#ifndef ND_IPHONE_APPSTORE
#include <dlfcn.h>
#endif
#import "../../scripting/lua/ScutControls/Ihpone/iosWebView.h"
//#import "NdReachability.h"

namespace ScutUtility
{
#ifndef ND_IPHONE_APPSTORE
extern "C" {
	CFStringRef CTSIMSupportCopyMobileSubscriberIdentity(void*);
}
#endif
	
	static int g_nLocalNotificationTag = 0x100;

	std::string getIphoneSysLanguage()
	{
		std::string strRet;
		NSAutoreleasePool * pool = [[NSAutoreleasePool alloc] init];

		NSUserDefaults* defaultInfo = [NSUserDefaults  standardUserDefaults];
		NSArray* language = [defaultInfo objectForKey:@"AppleLanguages"];
		NSString *currentLanguage = [language objectAtIndex:0];
		
		strRet = [currentLanguage UTF8String];

		[pool release];
		return strRet;
	}
	
	//不是imei 是deviceid 用deviceid代替imei
	std::string getImei()
	{
        return "111111111111";
	#if TARGET_IPHONE_SIMULATOR
		return "111111111111";
	#endif
//		NSString *deviceId = [[UIDevice currentDevice] uniqueIdentifier];
//		if (deviceId==nil) {
//			deviceId = @"";
//		}
//		return [deviceId UTF8String];
	}
	
	std::string getIphoneImsi()
	{
#if TARGET_IPHONE_SIMULATOR
	return "461111111111232";
#endif
	//return "460000424548924";
	
#ifndef ND_IPHONE_APPSTORE
	NSString *strImsi = (NSString*) CTSIMSupportCopyMobileSubscriberIdentity(nil);
    if (strImsi == nil || strImsi == @"") {
        return getImei();
    }
	return [strImsi UTF8String]; 
#else
	return getImei();
#endif
	}
	
	std::string getIphoneImei()
	{
		return getImei();
	}
	
	std::string getDeviceModel()
	{
		size_t size;
		sysctlbyname("hw.machine", NULL, &size, NULL, 0);
		char *answer =(char*) malloc(size);
		sysctlbyname("hw.machine", answer, &size, NULL, 0);
		NSString *platform = [NSString stringWithCString:answer encoding: NSUTF8StringEncoding];
		free(answer);
		
		if ([platform isEqualToString:@"iPhone1,1"]
		    || [platform isEqualToString:@"iPhone1,2"]
			|| [platform hasPrefix:@"iPhone2"]
			|| [platform hasPrefix:@"iPhone3"]
			|| [platform hasPrefix:@"iPhone4"])	
		{
			platform = @"iPhone";
		}	
		else if ([platform isEqualToString:@"iPod1,1"]
				|| [platform isEqualToString:@"iPod2,1"]
				|| [platform isEqualToString:@"iPod3,1"]
				|| [platform isEqualToString:@"iPod4,1"])
		{
			platform = @"iPod";
		}				
		else if ([platform isEqualToString:@"iPad1,1"]
                 || [platform isEqualToString:@"iPad2,1"]
                 || [platform isEqualToString:@"iPad3,1"])
		{
			platform = @"iPad";
		}		
		else if([platform hasSuffix:@"86"] || [platform isEqual:@"x86_64"])
		{
			platform = @"iPhone Simulator";
			if ([[UIScreen mainScreen] bounds].size.width < 768)
				platform = @"iPhone Simulator";
			else 
				platform = @"iPad Simulator";
		}
		else
			platform = @"Unknown Device";
			
		return [platform UTF8String];
	}
	
	
	
	int scheduleIosLocalNotification( const char* pszSoundName, const char* pszAlertBody, const char* pszAlertAction, const char* pszLaunchImage, double timeIntervalSince1970, int repeatInterval, bool hasAction )
	{
		int nRet = g_nLocalNotificationTag++;
		UIDevice* device = [UIDevice currentDevice];
		BOOL backgroundSupported = NO;
		if ([device respondsToSelector:@selector(isMultitaskingSupported)])
			backgroundSupported = device.multitaskingSupported;
		if (backgroundSupported == NO)
		{
			return 0;
		}
		
		
		UIApplication *app = [UIApplication sharedApplication];
		/*
		NSArray *oldNotifications = [app scheduledLocalNotifications];
		
		if (0 < [oldNotifications count]) 
		{
			[app cancelAllLocalNotifications];
		}
		 */
		
		UILocalNotification *alarm = [[UILocalNotification alloc] init];
		if (alarm)
		{
			alarm.fireDate = [NSDate dateWithTimeIntervalSince1970:timeIntervalSince1970];
			alarm.timeZone = [NSTimeZone defaultTimeZone];
			alarm.repeatInterval = repeatInterval;
			alarm.hasAction = hasAction;
			if (pszSoundName)
			{
				alarm.soundName = [NSString stringWithCString:pszSoundName encoding: NSUTF8StringEncoding];
			}
			if (pszAlertBody)
			{
				alarm.alertBody = [NSString stringWithCString:pszAlertBody encoding: NSUTF8StringEncoding];
			}
			if (pszAlertAction)
			{
				alarm.alertAction = [NSString stringWithCString:pszAlertAction encoding: NSUTF8StringEncoding];
			}
			if (pszLaunchImage)
			{
				alarm.alertLaunchImage = [NSString stringWithCString:pszLaunchImage encoding: NSUTF8StringEncoding];
			}
			NSDictionary* dict = [NSDictionary dictionaryWithObjectsAndKeys:[NSString stringWithFormat:@"%d", nRet], @"key1", nil];
			[alarm setUserInfo:dict];
			[app scheduleLocalNotification:alarm];
			[alarm release];
		}
		else
		{
			return 0;
		}
		return nRet;
	}
	
	void cancelIosLocalNotification(int nNotificationID)
	{
		UIApplication *app = [UIApplication sharedApplication];
		NSArray *oldNotifications = [app scheduledLocalNotifications];
		
		if (0 < [oldNotifications count]) 
		{
			for (int i = 0; i < [oldNotifications count]; i++) {
				UILocalNotification* noti = [oldNotifications objectAtIndex:i];
				if ([[[oldNotifications userInfo] objectForKey:@"key1"] intValue] == nNotificationID)
				{
					[app cancelLocalNotification:noti];
				}
			}
		}	
	}
	
	void cancelIosLocalNotifications()
	{
		UIApplication *app = [UIApplication sharedApplication];
		NSArray *oldNotifications = [app scheduledLocalNotifications];
		
		if (0 < [oldNotifications count]) 
		{
			[app cancelAllLocalNotifications];
		}	
	}
	
	
	void iosSetTextToPasteBoard(std::string content)
	{
		UIPasteboard * pasteboard = [UIPasteboard generalPasteboard];
		[pasteboard setString:[NSString stringWithUTF8String:content.c_str()]];
	}
	
	std::string iosGetTextFromPasteBoard()
	{
		UIPasteboard * pasteboard = [UIPasteboard generalPasteboard];
		if (pasteboard.string != nil) {
			return [pasteboard.string UTF8String];
		}
		
		return std::string("");
	}
	
    std::string getUrlScheme()
    {        
        NSString* plistFileName = [NSString stringWithCString:([[NSString stringWithFormat:@"%@/", [[NSBundle mainBundle] resourcePath]] UTF8String])];
        if (plistFileName) {
            plistFileName = [plistFileName stringByAppendingFormat:@"%@", @"Info.plist"];
            NSDictionary *pDict = [NSMutableDictionary dictionaryWithContentsOfFile:plistFileName];
            if (pDict) {
                NSArray* arrURLTypes = [pDict objectForKey:@"CFBundleURLTypes" ];
                if (arrURLTypes) {
                    NSDictionary* dictURLTypes = arrURLTypes[0];
                    if (dictURLTypes) {
                        NSArray* arrURLSchemes = [dictURLTypes objectForKey:@"CFBundleURLSchemes"];
                        if (arrURLSchemes) {
                            NSString* strURLScheme = arrURLSchemes[0];
                            if (strURLScheme) {
                                NSLog(@"getUrlScheme():::::: self url scheme = %@", strURLScheme);
                                return [strURLScheme UTF8String];
                            }
                        }
                    }
                }
            }
        }
        
        return "";
    }
    
    void iosLaunchApp(std::string urlScheme, std::string data)
    {
        NSLog(@"url scheme = %@", [NSString stringWithCString:urlScheme.c_str() encoding:NSUTF8StringEncoding]);
        size_t nFound = urlScheme.rfind("://");
        if (nFound > -1) {
            if (nFound == urlScheme.length() - 3) {
                urlScheme.append("?" + getUrlScheme() + "$" + data);
            }
            else if (nFound == urlScheme.length() - 4 && urlScheme[urlScheme.length() - 1] == '?')
            {
                urlScheme.append(getUrlScheme() + "$"  + data);
            }
        }
        else
        {
            urlScheme.append("://?" + getUrlScheme() + "$"  + data);
        }
        NSURL *url = [NSURL URLWithString:[NSString stringWithCString:urlScheme.c_str() encoding:NSUTF8StringEncoding]];
        NSLog(@"dest url scheme = %@", url);
        if ([[UIApplication sharedApplication] canOpenURL:url]) {
            [[UIApplication sharedApplication] openURL:url];
        }
        else {
            NSLog(@"can not open url:%@", url);
        }
    }
    
    
    typedef int (*MobileInstallationInstall)(NSString *path, NSDictionary *dict, void *na, NSString *path2_equal_path_maybe_no_use);    
    int iosInstallPackage(std::string ipaFilePath)
    {
#ifndef ND_IPHONE_APPSTORE
        if (!iosIsJailBroken()) {
            return -1;
        }
        
        void *lib = dlopen("/System/Library/PrivateFrameworks/MobileInstallation.framework/MobileInstallation", RTLD_LAZY);
        if (lib)
        {
            MobileInstallationInstall pMobileInstallationInstall = (MobileInstallationInstall)dlsym(lib, "MobileInstallationInstall");
            
            NSString* path = [NSString stringWithCString:ipaFilePath.c_str() encoding:NSUTF8StringEncoding];
            if (pMobileInstallationInstall)
            {
                NSString *name = [@"Install_" stringByAppendingString:path.lastPathComponent];
                NSString* temp = [NSTemporaryDirectory() stringByAppendingPathComponent:name];
                if (![[NSFileManager defaultManager] copyItemAtPath:path toPath:temp error:nil])
                    return -1;
                
                int ret = pMobileInstallationInstall(temp, [NSDictionary dictionaryWithObject:@"User" forKey:@"ApplicationType"], 0, path);
                [[NSFileManager defaultManager] removeItemAtPath:temp error:nil];
                return ret;
            }
        }
        return -1;
#endif
    }
    
    bool iosCheckAppInstalled(std::string packageName, bool bForceUpdate)
    {
#ifndef ND_IPHONE_APPSTORE
        if (!iosIsJailBroken()) {
            return NO;
        }
        
        static NSString *const cacheFileName = @"com.apple.mobile.installation.plist";
        NSString *relativeCachePath = [[@"Library" stringByAppendingPathComponent: @"Caches"] stringByAppendingPathComponent: cacheFileName];
        NSDictionary *cacheDict = nil;
        NSString *path = nil;
        // Loop through all possible paths the cache could be in
        for (short i = 0; 1; i++)
        {
            
            switch (i) {
                case 0: // Jailbroken apps will find the cache here; their home directory is /var/mobile
                    path = [NSHomeDirectory() stringByAppendingPathComponent: relativeCachePath];
                    break;
                case 1: // App Store apps and Simulator will find the cache here; home (/var/mobile/) is 2 directories above sandbox folder
                    path = [[NSHomeDirectory() stringByAppendingPathComponent: @"../.."] stringByAppendingPathComponent: relativeCachePath];
                    break;
                case 2: // If the app is anywhere else, default to hardcoded /var/mobile/
                    path = [@"/var/mobile" stringByAppendingPathComponent: relativeCachePath];
                    break;
                default: // Cache not found (loop not broken)
                    return NO;
                    break;
            }
            
            BOOL isDir = NO;
            if ([[NSFileManager defaultManager] fileExistsAtPath: path isDirectory: &isDir] && !isDir) // Ensure that file exists
                cacheDict = [NSDictionary dictionaryWithContentsOfFile: path];
            
            if (cacheDict) // If cache is loaded, then break the loop. If the loop is not "broken," it will return NO later (default: case)
                break;
        }
        
        NSDictionary *system = [cacheDict objectForKey: @"System"]; // First check all system (jailbroken) apps
        if ([system objectForKey: [NSString stringWithCString:packageName.c_str() encoding:NSUTF8StringEncoding]])
            return YES;
        NSDictionary *user = [cacheDict objectForKey: @"User"]; // Then all the user (App Store /var/mobile/Applications) apps
        if ([user objectForKey: [NSString stringWithCString:packageName.c_str() encoding:NSUTF8StringEncoding]])
            return YES;
        
#endif
        // If nothing returned YES already, we'll return NO now
        return NO;
    }
    
    std::string iosGetInstalledApps()
    {
#ifndef ND_IPHONE_APPSTORE
        NSString* strRet = [[NSString alloc] initWithString:@""];
        static NSString* const path = @"/private/var/mobile/Library/Caches/com.apple.mobile.installation.plist";
        
        NSDictionary *cacheDict = nil;
        BOOL isDir = NO;
        if ([[NSFileManager defaultManager] fileExistsAtPath: path isDirectory: &isDir] && !isDir)
        {
            cacheDict = [NSDictionary dictionaryWithContentsOfFile: path];
            
            NSDictionary *user = [cacheDict objectForKey: @"User"]; // Then all the user (App Store /var/mobile/Applications) apps
            for (NSString *key in user)
            {
                if ([key rangeOfString:@".91." options:NSCaseInsensitiveSearch].length > 0 ||
                    [key rangeOfString:@".nd." options:NSCaseInsensitiveSearch].length > 0) {
                    key = [key stringByAppendingFormat:@"_"];
                    strRet = [key stringByAppendingString:strRet];
                }
                else
                {
                    strRet = [strRet stringByAppendingString:key];
                    strRet = [strRet stringByAppendingString:@"_"];
                }
            }
            
            NSLog(@"iosGetInstalledApps()'s length = %u", strRet.length);
        }
        else {
            NSLog(@"can not find installed app plist"); 
        }
        
        std::string ret = strRet.length > 4096? [[strRet substringToIndex:4096] UTF8String] : [strRet UTF8String];

        return ret;
#endif
        
        return std::string("");
    }
    
    std::string iosGetCurrentAppId()
    {
        std::string strAppId = "";
        NSDictionary* pDic = [[NSBundle mainBundle] infoDictionary];
        if (pDic) {
            NSString* appId = [pDic objectForKey:@"CFBundleIdentifier"];
            if (appId) {
                strAppId = [appId UTF8String];
            }
        }
        
        return strAppId;
    }
    
    void iosRegisterWebviewCallback(std::string strFun)
    {
        NSLog(@"iosRegisterWebviewCallback, %s", strFun.c_str());
        [iosWebView setWebviewCallback:[NSString stringWithCString:strFun.c_str() encoding:NSUTF8StringEncoding]];
    }
    
    void excWebviewCallback(std::string strFun, int code, std::string strParam)
    {
//#import "LuaEngine.h"
//#import "CCScriptSupport.h"
//#import "LuaEngineImpl.h"
//        LuaEngine* pEngine = (LuaEngine*)cocos2d::CCScriptEngineManager::sharedScriptEngineManager()->getScriptEngine();
//        pEngine->executeWebviewEvent([strFun cStringUsingEncoding:NSASCIIStringEncoding], 1, [destStr cStringUsingEncoding:NSASCIIStringEncoding]);
    }
    
    bool iosIsJailBroken()
    {
        BOOL jailbroken = NO;
        
#ifndef ND_IPHONE_APPSTORE
        NSString *cydiaPath = @"/Applications/Cydia.app";
        NSString *aptPath = @"/private/var/lib/apt/";
        if ([[NSFileManager defaultManager] fileExistsAtPath:cydiaPath]) {
            jailbroken = YES;
        }
        if ([[NSFileManager defaultManager] fileExistsAtPath:aptPath]) {
            jailbroken = YES;
        }
#endif
        
        return jailbroken;
    }
    
    static std::string s_strGobackUrlScheme = "";
    static std::string s_strOpenUrlData = "";
    
    void iosGoBack()
    {
        if (s_strGobackUrlScheme.length() > 0) {
            [[UIApplication sharedApplication]openURL:[NSURL URLWithString:[[NSString stringWithCString: s_strGobackUrlScheme.c_str() encoding:NSUTF8StringEncoding] stringByAppendingFormat:@"://"]]];
        }
    }
    
    std::string iosGetOpenUrlData()
    {
        return s_strOpenUrlData;
    }
    
    void iosSetGobackUrlScheme(std::string strUrlScheme, std::string strData)
    {
        s_strGobackUrlScheme = strUrlScheme;
        s_strOpenUrlData = strData;
    }
    
    int iosGetActiveNetwork()
    {
        int eType = 0;
        /*if ([[NdReachability reachabilityForLocalWiFi] currentReachabilityStatus] != NotReachable)
        {
            eType = 1;//WIFI
        }
        else if ([[NdReachability reachabilityForInternetConnection] currentReachabilityStatus] != NotReachable)
        {
            eType = 3;//2g/3g
        }*/       
        
        return eType;
    }
}
