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
#include "ScutUtils.h"
#include <string>
//#include "cocos2d.h"

#ifdef SCUT_WIN32
#include <winsock2.h> //该头文件定义了Socket编程的功能
#include <stdio.h>    //该头文件声明了输入输出流函数
#include <stdlib.h>   //该头文件定义了一些通用函数
#include <httpext.h>   //该头文件支持HTTP请求
#include <windef.h>   //该头文件定义了windows的所有数据基本型态
#include <Nb30.h>   //该头文件声明了netbios的所有的函数
#pragma comment(lib,"ws2_32.lib")    //连接ws2_32.lib库.只要程序中用到Winsock API 函数，都要用到 Ws2_32.lib
#pragma comment(lib,"netapi32.lib")   //连接Netapi32.lib库，MAC获取中用到了NetApi32.DLL的功能
#endif

#ifdef SCUT_ANDROID
#include "android/ScutUtilityJni.h"
//#include "Cocos2dJni.h"
#include <stdio.h>
#include <sys/types.h> 
#include <sys/param.h> 
#include <sys/ioctl.h> 
#include <sys/socket.h> 
#include <net/if.h> 
#include <netinet/in.h> 
#include <net/if_arp.h> 
#include <arpa/inet.h>

#define  LOG_TAG    "ScutUtils"
#include <android/log.h>
#define  LOGE(...)  __android_log_print(ANDROID_LOG_ERROR,LOG_TAG,__VA_ARGS__)
#endif

#ifdef SCUT_IPHONE
#include "iphone/IphoneInterface.h"
#include "IPAddress.h"
#endif

#ifdef SCUT_MAC
#include "IPAddress.h"

#include <stdio.h>

#include <CoreFoundation/CoreFoundation.h>

#include <IOKit/IOKitLib.h>
#include <IOKit/network/IOEthernetInterface.h>
#include <IOKit/network/IONetworkInterface.h>
#include <IOKit/network/IOEthernetController.h>
#endif

namespace ScutUtility
{
	EPlatformType ScutUtility::ScutUtils::GetPlatformType()
	{
		EPlatformType type = ptWin32;
#ifdef SCUT_IPHONE
		type = ptiPhone;
#ifdef SCUT_IPHONE_APPSTORE
		type = ptiPhone_AppStore;
#endif
		std::string strType = getDeviceModel();
		if (strType.compare("iPod") == 0)
		{
			type = ptiPod;
		}
		else if (strType.compare("iPad") == 0)
		{
			type = ptiPad;
		}
#elif SCUT_MAC
		type = ptMac;
#elif SCUT_ANDROID
		type = ptANDROID;	
#else
		
#endif
		return type;
	}
	
	static std::string s_Imsi = "";
	static std::string s_Imei = "";
	
	const char* ScutUtility::ScutUtils::getImsi()
	{
		if (s_Imsi.size() != 0)
		{
			return s_Imsi.c_str();
		}
#ifdef SCUT_ANDROID
		char* p = getAndroidImsi();
		if (p)
		{
			s_Imsi = p;
			free(p);
		}
#endif
		
#ifdef SCUT_IPHONE
		s_Imsi = getIphoneImsi();
#endif
		
		return s_Imsi.c_str();
	}
	
	const char* ScutUtility::ScutUtils::getImei()
	{
		if (s_Imei.size() > 0)
		{
			return s_Imei.c_str();
		}
#ifdef SCUT_ANDROID
		char* p = getAndroidImei();
		if (p)
		{
			s_Imei = p;
			free(p);
		}
#endif
		
#ifdef SCUT_IPHONE
		s_Imei = getIphoneImei();
#endif
		
#ifdef SCUT_MAC
		s_Imei = getMacAddress();
#endif
		
		return s_Imei.c_str();
	}
	
	
	int ScutUtils::scheduleLocalNotification( const char* pszSoundName, const char* pszAlertBody, const char* pszAlertAction, const char* pszLaunchImage, 
										   double timeIntervalSince1970, int repeatInterval, bool hasAction, const char* pszAlertTitle, bool hasVibration, int iconResId)
	{
#ifdef SCUT_IPHONE
		return scheduleIosLocalNotification(pszSoundName, pszAlertBody, pszAlertAction, pszLaunchImage, timeIntervalSince1970, repeatInterval, hasAction);
#elif SCUT_ANDROID
		LOGE("enter ScutUtils::scheduleLocalNotification");
		unsigned char action = hasVibration ? 1 : 0;
		std::string strSoundName = pszSoundName == NULL ? "" : pszSoundName;
		scheduleAndroidLocalNotification(strSoundName, iconResId, pszAlertTitle == NULL? "" : pszAlertTitle, pszAlertBody == NULL ? "" : pszAlertBody, timeIntervalSince1970 * 1000, action, repeatInterval * 1000);
		LOGE("end ScutUtils::scheduleLocalNotification");
		return 1;
#endif
		
		return 0;
	}
	
	static std::string s_MacAddress = "";
	
	extern "C" void InitAddresses();
	extern "C" void GetIPAddresses();
	extern "C" void GetHWAddresses();
	extern "C" char*  getMacAddressIOS(char* macAddress, char* ifName);
	
	const char* ScutUtils::getMacAddress()
	{
#ifdef SCUT_WIN32
		if (s_MacAddress.size() == 0)
		{
			NCB ncb;     //定义一个NCB(网络控制块)类型的结构体变量ncb
			typedef struct _ASTAT_     //自定义一个结构体_ASTAT_
			{
				ADAPTER_STATUS   adapt; 
				NAME_BUFFER   NameBuff   [30];     
			}ASTAT, *PASTAT;
			ASTAT Adapter;   
			
			typedef struct _LANA_ENUM     //自定义一个结构体_ASTAT_
			{
				UCHAR length; 
				UCHAR lana[MAX_LANA];     //存放网卡MAC地址 
			}LANA_ENUM;     
			LANA_ENUM lana_enum;   
			
			//   取得网卡信息列表     
			UCHAR uRetCode;     
			memset(&ncb, 0, sizeof(ncb));     //将已开辟内存空间ncb 的值均设为值 0
			memset(&lana_enum, 0, sizeof(lana_enum));     //清空一个结构类型的变量lana_enum，赋值为0
			//对结构体变量ncb赋值
			ncb.ncb_command = NCBENUM;     //统计系统中网卡的数量
			ncb.ncb_buffer = (unsigned char *)&lana_enum; //ncb_buffer成员指向由LANA_ENUM结构填充的缓冲区
			ncb.ncb_length = sizeof(LANA_ENUM);   
			//向网卡发送NCBENUM命令，以获取当前机器的网卡信息，如有多少个网卡，每个网卡的编号（MAC地址） 
			uRetCode = Netbios(&ncb); //调用netbois(ncb)获取网卡序列号    
			if(uRetCode != NRC_GOODRET)     
				return s_MacAddress.c_str();     
			
			//对每一个网卡，以其网卡编号为输入编号，获取其MAC地址   
			for(int lana=0; lana<lana_enum.length; lana++)     
			{
				ncb.ncb_command = NCBRESET;   //对网卡发送NCBRESET命令，进行初始化
				ncb.ncb_lana_num = lana_enum.lana[lana]; 
				uRetCode = Netbios(&ncb);   
				if(uRetCode == NRC_GOODRET) 
					break; 
			} 
			if(uRetCode != NRC_GOODRET)
				return s_MacAddress.c_str();     
			
			//   准备取得接口卡的状态块取得MAC地址
			memset(&ncb, 0, sizeof(ncb)); 
			ncb.ncb_command = NCBASTAT;    //对网卡发送NCBSTAT命令，获取网卡信息
			ncb.ncb_lana_num = lana_enum.lana[0];     //指定网卡号 
			strcpy((char*)ncb.ncb_callname, "*");   //远程系统名赋值为*
			ncb.ncb_buffer = (unsigned char *)&Adapter; //指定返回的信息存放的变量
			ncb.ncb_length = sizeof(Adapter);
			//接着发送NCBASTAT命令以获取网卡的信息
			uRetCode = Netbios(&ncb); 
			//   取得网卡的信息，并且如果网卡正常工作的话，返回标准的冒号分隔格式。   
			if(uRetCode != NRC_GOODRET)   
				return s_MacAddress.c_str(); 
			//把网卡MAC地址格式转化为常用的16进制形式,输出到字符串mac中 
			char mac[48] = {0};
			sprintf(mac,"%02X-%02X-%02X-%02X-%02X-%02X",     
					Adapter.adapt.adapter_address[0],     
					Adapter.adapt.adapter_address[1],     
					Adapter.adapt.adapter_address[2],     
					Adapter.adapt.adapter_address[3],     
					Adapter.adapt.adapter_address[4],     
					Adapter.adapt.adapter_address[5] 
					); 
			s_MacAddress = mac;
		}
#elif SCUT_ANDROID
		/*
		 if (s_MacAddress.size() == 0)
		 {
		 #define MAXINTERFACES 16 
		 register int fd, interface, retn = 0; 
		 struct ifreq buf[MAXINTERFACES]; 
		 struct arpreq arp; 
		 struct ifconf ifc; 
		 if ((fd = socket (AF_INET, SOCK_DGRAM, 0)) >= 0) 
		 { 
		 ifc.ifc_len = sizeof buf; 
		 ifc.ifc_buf = (caddr_t) buf; 
		 if (!ioctl (fd, SIOCGIFCONF, (char *) &ifc)) 
		 { 
		 //获取接口信息
		 interface = ifc.ifc_len / sizeof (struct ifreq); 
		 //根据接口信息循环获取设备IP和MAC地址
		 while (interface-- > 0) 
		 { 
		 //Mac地址
		 //if (!(ioctl (fd, SIOCGIFHWADDR, (char *)&buf[interface]))) 
		 { 
		 char mac[48] = {0};
		 sprintf(mac,"%02X:%02X:%02X:%02X:%02X:%02X",     
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[0],
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[1],
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[2],
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[3],
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[4],
		 (unsigned char)buf[interface].ifr_hwaddr.sa_data[5]);
		 s_MacAddress = mac;
		 printf("mac address : %s", s_MacAddress.c_str());
		 //break;
		 } 					
		 } //while
		 } 
		 //close(fd);
		 } 
		 }	
		 */
		//char* pszMac = getMacAddressJNI();
        char* pszMac = "";
		if (pszMac != NULL)
		{
			s_MacAddress = pszMac;
		}
		return s_MacAddress.c_str();
#elif SCUT_MAC
		kern_return_t	kernResult = KERN_SUCCESS;
		io_iterator_t	intfIterator;
		UInt8			MACAddress[kIOEthernetAddressSize];
		
		/** findEthernetInterfaces
		 */
		
		io_iterator_t *matchingServices = &intfIterator;
		CFMutableDictionaryRef	matchingDict;
		CFMutableDictionaryRef	propertyMatchDict;
		
		matchingDict = IOServiceMatching(kIOEthernetInterfaceClass);
        
		if (NULL == matchingDict) {
			printf("IOServiceMatching returned a NULL dictionary.\n");
		}
		else {
			propertyMatchDict = CFDictionaryCreateMutable(kCFAllocatorDefault, 0,
														  &kCFTypeDictionaryKeyCallBacks,
														  &kCFTypeDictionaryValueCallBacks);
			
			if (NULL == propertyMatchDict) {
				printf("CFDictionaryCreateMutable returned a NULL dictionary.\n");
			}
			else {
				CFDictionarySetValue(propertyMatchDict, CFSTR(kIOPrimaryInterface), kCFBooleanTrue); 
				
				CFDictionarySetValue(matchingDict, CFSTR(kIOPropertyMatchKey), propertyMatchDict);
				CFRelease(propertyMatchDict);
			}
		}
		
		kernResult = IOServiceGetMatchingServices(kIOMasterPortDefault, matchingDict, matchingServices);    
		if (KERN_SUCCESS != kernResult) {
			printf("IOServiceGetMatchingServices returned 0x%08x\n", kernResult);
		} else {
			/** GetMACAddress
			 */
			
			io_object_t		intfService;
			io_object_t		controllerService;
			UInt8 bufferSize = sizeof(MACAddress);
			
			if (bufferSize >= kIOEthernetAddressSize) {
				
				bzero(MACAddress, bufferSize);
				
				while ((intfService = IOIteratorNext(intfIterator)))
				{
					CFTypeRef	MACAddressAsCFData;        
					
					kernResult = IORegistryEntryGetParentEntry(intfService,
															   kIOServicePlane,
															   &controllerService);
					
					if (KERN_SUCCESS != kernResult) {
						printf("IORegistryEntryGetParentEntry returned 0x%08x\n", kernResult);
					}
					else {
						// Retrieve the MAC address property from the I/O Registry in the form of a CFData
						MACAddressAsCFData = IORegistryEntryCreateCFProperty(controllerService,
																			 CFSTR(kIOMACAddress),
																			 kCFAllocatorDefault,
																			 0);
						if (MACAddressAsCFData) {
							//CFShow(MACAddressAsCFData); // for display purposes only; output goes to stderr
							
							// Get the raw bytes of the MAC address from the CFData
				 			CFDataGetBytes((CFDataRef)MACAddressAsCFData, CFRangeMake(0, kIOEthernetAddressSize), MACAddress);
							CFRelease(MACAddressAsCFData);
						}
						
						// Done with the parent Ethernet controller object so we release it.
						(void) IOObjectRelease(controllerService);
					}
					
					// Done with the Ethernet interface object so we release it.
					(void) IOObjectRelease(intfService);
				}
			}
			
			if (KERN_SUCCESS != kernResult) {
				printf("GetMACAddress returned 0x%08x\n", kernResult);
			}
			else {
				char mac[48] = {0};
				sprintf(mac,"%02X-%02X-%02X-%02X-%02X-%02X",     
						MACAddress[0],     
						MACAddress[1],     
						MACAddress[2],     
						MACAddress[3],     
						MACAddress[4],     
						MACAddress[5] 
						); 
				s_MacAddress = mac;
			}
		}
		
		
#else //iPhone,ANDROID
		if (s_MacAddress.size() == 0)
		{
#if 0
			InitAddresses();
			GetIPAddresses();
			GetHWAddresses();
			for (int i = 0; i < MAXADDRS; i++)
			{
				static unsigned long localHost = 0x7F000001;//127.0.0.1
				static unsigned long localHost2 = 0x100007F;//127.0.0.1 iphone
				unsigned long theAddr;
				theAddr = ip_addrs[i];
				if (theAddr == 0)
				{
					break;
				}
				if (theAddr == localHost || theAddr == localHost2)
				{
					continue;
				}
				s_MacAddress = hw_addrs[i];
				break;
			}
#else
			char *dressString= (char*)malloc(18);
			getMacAddressIOS(dressString,"en0");
			s_MacAddress = dressString;
			free(dressString);
#endif
		}
#endif
		return s_MacAddress.c_str();
	}
	
	void ScutUtils::cancelLocalNotification(int nNotificationID)
	{
#ifdef SCUT_IPHONE
		cancelIosLocalNotification(nNotificationID);
#elif SCUT_ANDROID
		cancelAndroidNotification(nNotificationID);
#endif
	}
	
	void ScutUtils::cancelLocalNotifications()
	{
#ifdef SCUT_IPHONE
		cancelIosLocalNotifications();
#elif SCUT_ANDROID
		cancelAndroidNotifications();
#endif
	}

	static std::string s_strTextFromClipBoard = "";
	void ScutUtils::setTextToClipBoard(std::string content)
	{
#ifdef SCUT_IPHONE
		iosSetTextToPasteBoard(content);
#elif SCUT_ANDROID
		androidCopyTextToClipBoard(content);
#endif
	}
	
	const char* ScutUtils::getTextFromClipBoard()
	{
#ifdef SCUT_IPHONE
		s_strTextFromClipBoard = iosGetTextFromPasteBoard();
#elif SCUT_ANDROID
		s_strTextFromClipBoard = androidGetTextFromClipBoard();
#endif
		return s_strTextFromClipBoard.c_str();
	}

	void ScutUtils::launchApp(std::string packageName, std::string data)
	{
#ifdef SCUT_IPHONE
		iosLaunchApp(packageName, data);
#elif SCUT_ANDROID
		androidLaunchApp(packageName);
#endif
	}

	void ScutUtils::installPackage(std::string packageFilePath)
	{
#ifdef SCUT_IPHONE
		iosInstallPackage(packageFilePath);
#elif SCUT_ANDROID
		androidInstallPackage(packageFilePath);
#endif
	}

	bool ScutUtils::checkAppInstalled(std::string packageName, bool bForceUpdate)
	{
#ifdef SCUT_IPHONE
		return iosCheckAppInstalled(packageName, bForceUpdate);
#elif SCUT_ANDROID
		return androidCheckAppInstalled(packageName, bForceUpdate);
#endif
		return false;
	}

	static std::string s_strInstalledApps = "";
	const char*  ScutUtils::getInstalledApps()
	{
#ifdef SCUT_IPHONE
		s_strInstalledApps = iosGetInstalledApps();
#elif SCUT_ANDROID
		s_strInstalledApps = androidGetInstalledApps();
#endif
		return s_strInstalledApps.c_str();
	}

	//注册浏览器的回调
	void ScutUtils::registerWebviewCallback(std::string strFun)
	{
#ifdef SCUT_IPHONE
		iosRegisterWebviewCallback(strFun);
#elif SCUT_ANDROID
		//androidRegisterWebviewCallback(strFun);
#endif
	}
	
	bool ScutUtils::GetUtf8StringLengthes( const char* pszStr, unsigned char** outLengthes, int* nSize )
	{
		if (!nSize || !pszStr)
		{
			return false;
		}
		unsigned int nLen = 0;
		char c = '\0';
		while ('\0' != (c = *pszStr))
		{
			// This char is ascii
			if (0 == (0x80 & c))
			{
				if (*outLengthes)
				{
					(*outLengthes)[nLen] = 1;
				}
				nLen++;
				pszStr++;
				continue;
			}
			else
			{
				// This is NOT a utf-8 header char
				if (0 == (0x40 & c))
				{
					pszStr++;
					continue;
				}
				
				// Parse the utf-8 header char to parse the char length
				unsigned char l = ((0xF0 & c) >> 4);
				switch (l)
				{
					case 0xF:// utf-8 char is 4 bytes
						if (*outLengthes)
						{
							(*outLengthes)[nLen] = 4;
						}
						pszStr += 4;
						break;
					case 0xE:// utf-8 char is 3 bytes
						if (*outLengthes)
						{
							(*outLengthes)[nLen] = 3;
						}
						pszStr += 3;
						break;
					case 0xC:// utf-8 char is 2 bytes
						if (*outLengthes)
						{
							(*outLengthes)[nLen] = 2;
						}
						pszStr += 2;
						break;
				}
				nLen++;
			}
		}
		*nSize = nLen;
		return true;
	}

	void ScutUtils::GoBack()
	{
#ifdef SCUT_IPHONE
		iosGoBack();
#elif SCUT_ANDROID
#endif
	}

	const char* ScutUtils::getOpenUrlData()
	{
#ifdef SCUT_IPHONE
		return iosGetOpenUrlData().c_str();
#elif SCUT_ANDROID
#endif
		return "";
	}

	bool ScutUtils::isJailBroken()
	{
#ifdef SCUT_IPHONE
		return iosIsJailBroken();
#elif SCUT_ANDROID
#endif
		return false;
	}

	static std::string s_strCurrentAppId = "";
	const char* ScutUtils::getCurrentAppId()
	{
#ifdef SCUT_IPHONE
		s_strCurrentAppId = iosGetCurrentAppId();
#elif SCUT_ANDROID
		s_strCurrentAppId = androidGetCurrentAppId();
#endif
		return s_strCurrentAppId.c_str();
	}

	EActiveNetworkType ScutUtils::getActiveNetworkInfo()	
	{
		EActiveNetworkType eType = antWIFI;
#ifdef SCUT_IPHONE
		eType = (EActiveNetworkType)iosGetActiveNetwork();
#elif SCUT_ANDROID
		eType = (EActiveNetworkType)androidGetActiveNetwork();
#endif
		return eType;
	}
}
