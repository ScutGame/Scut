#ifndef IPHONEINTERFACE_H
#define IPHONEINTERFACE_H
#include <string>
namespace ScutUtility
{
	std::string getIphoneSysLanguage();
	std::string getIphoneImsi();
	std::string getIphoneImei();
	std::string getDeviceModel();
	int scheduleIosLocalNotification( const char* pszSoundName, const char* pszAlertBody, const char* pszAlertAction, const char* pszLaunchImage, double timeIntervalSince1970, int repeatInterval, bool hasAction );
	void cancelIosLocalNotification(int nNotificationID);
	void cancelIosLocalNotifications();	
	void iosSetTextToPasteBoard(std::string content);
	std::string iosGetTextFromPasteBoard();
    
    void iosLaunchApp(std::string urlScheme, std::string data = "");
    int iosInstallPackage(std::string ipaFilePath);
    bool iosCheckAppInstalled(std::string packageName, bool bForceUpdate);
    void iosRegisterWebviewCallback(std::string strFun);
    void excWebviewCallback(std::string strFun, int code, std::string strParam);
    bool iosIsJailBroken();
    std::string iosGetInstalledApps();
    std::string iosGetCurrentAppId();
    
    void iosGoBack();
    std::string iosGetOpenUrlData();
    
    int iosGetActiveNetwork();
    
    //iosApp调用
    void iosSetGobackUrlScheme(std::string strUrlScheme, std::string strData);
}

#endif//IPHONEINTERFACE_H