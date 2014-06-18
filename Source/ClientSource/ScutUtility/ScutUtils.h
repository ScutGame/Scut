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
#pragma once
#include <string>

namespace ScutUtility
{
	enum EPlatformType
	{
		//Win32
		ptWin32,
		//Ipod
		ptiPod,
		//Ipad
		ptiPad,
		//破解版iPhone和iPad
		ptiPhone,
		//非破解版iPhone
		ptiPhone_AppStore,
		//ANDROID
		ptANDROID,
		// mac
		ptMac,
		// WP7
		ptwindowsPhone7,
		//未知
		ptUnknow,
	};

	//当前活跃网络类型
	enum EActiveNetworkType
	{
		antNone,
		antWIFI,
		ant2G,
		ant3G,
	};

	class ScutUtils
	{
	public:
		//获取平台类型
		static EPlatformType GetPlatformType();
		//获取IMSI
		static const char* getImsi();
		//获取IMEI
		static const char* getImei();

		/*
		//注册iphone本地通知
		//pszSoundName:声音名
		//pszAlertBody:警告信息，如果没有设置，Notification被激发时将不显示提醒信息
		//pszAlertAction:alertAction的内容将作为提醒中动作按钮上的文字，如果未设置的话，提醒信息中的动作按钮将显示为“View”相对文字形式
		//pszLaunchImage:alertLaunchImage是在用户点击提醒框中动作按钮（“View”）时，等待应用加载时显示的图片，这个将替代应用原本设置的加载图 片
		//timeIntervalSince1970:激发时间
		//repeatInterval:重复激发的间隔时间
		//hasAction:hasAction是一个控制是否在提醒框中显示动作按钮的布尔值
		*
		*		注册ANDROID本地通知
		*	pszSoundName：	声音文件路径（mp3文件）,如果不要声音则输入空字符串""
		*	pszAlertBody：	消息内容
		*	pszAlertAction：无定义，可填入NULL
		*	pszLaunchImage：无定义，可填入NULL
		*	timeIntervalSince1970：	显示在消息末尾的时间戳，如System.currentTimeMillis()
		*	hasAction：		无定义
		*	repeatInterval：重复激发的间隔时间，0表示不重复激发
		*	pszAlertTitle：	消息标题
		*	hasVibration：	是否振动
		*	iconResId：		显示在消息头的图标资源的id，如R.drawable.icon。如果不显示图标，则填入其他大于0的数字，如1
		*/
		static int scheduleLocalNotification(const char* pszSoundName, const char* pszAlertBody, const char* pszAlertAction, const char* pszLaunchImage, 
											double timeIntervalSince1970, int repeatInterval, bool hasAction, const char* pszAlertTitle = NULL, bool hasVibration = true, int iconResId = 1);
		//取消某个特定的通知
		static void cancelLocalNotification(int nNotificationID);
		//取消所有通知
		static void cancelLocalNotifications();

		//设置文本到剪贴板
		static void setTextToClipBoard(std::string content);
		//从剪贴板获取文本
		static const char* getTextFromClipBoard();

		//启动其他程序
		static void launchApp(std::string packageName, std::string data = "");

		//安装其他程序
		static void installPackage(std::string packageFilePath);

		//判断某个程序是否已安装
		static bool checkAppInstalled(std::string packageName, bool bForceUpdate);

		//获取本机内已安装的apps。格式为“包名_包名_……”
		static const char* getInstalledApps();

		//注册浏览器的回调
		static void registerWebviewCallback(std::string strFun);

		//获取Mac地址
		static const char* getMacAddress();
		//获取utf-8字符串中各个字符的字节长度
		//pszStr 源字符串
		//outLengthes 存放每个字符的字节长度数组
		//nSize 返回的字符数量
		//如果outLengthes为NULL，那么只返回字符数量
		//nSize不允许为NULL
		static bool GetUtf8StringLengthes(const char* pszStr, unsigned char** outLengthes, int* nSize);

		//返回到启动自身的app去
		static void GoBack();
		//获取启动时传过来的数据
		static const char* getOpenUrlData();

		//是否已越狱
		static bool isJailBroken();

		//获取当前app的包名(id)
		static const char* getCurrentAppId();

		//获取当前网络类型
		static EActiveNetworkType getActiveNetworkInfo();
	};

}