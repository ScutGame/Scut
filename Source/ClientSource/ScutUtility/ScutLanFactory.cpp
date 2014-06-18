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
#include <assert.h>
#include "ScutLanFactory.h"
#include "ScutLocale.h"

namespace ScutUtility
{
	CScutLanFactory* CScutLanFactory::m_instance = NULL;
	CScutLan* CScutLanFactory::m_lanInstance = NULL;

	CScutLan* CScutLanFactory::GetLanInstance()
	{
		if (NULL == m_instance)
		{
			m_instance = new CScutLanFactory();
		}

		if (NULL == m_lanInstance)
		{
			m_lanInstance = m_instance->Create(CLocale::getLanguage());
		}

		assert(NULL != m_lanInstance);

		return m_lanInstance;
	}

	CScutLan* CScutLanFactory::Create(const char* pszLan)
	{
		if (0 == strcmp(pszLan, "zh_CN"))		//简体中文
		{
			return new CScutLanCn(); 
		}else if (0 == strcmp(pszLan, "en_US"))	//英语
		{
			return new CScutLanUs(); 
		}else if (0 == strcmp(pszLan, "zh_TW"))	//繁体中文
		{
			return new CScutLanTw(); 
		}else if (0 == strcmp(pszLan, "ja_JP"))	//日语
		{
			return new CScutLanJp(); 
		}

		return new CScutLanUs();	//默认英语
	}

	CScutLanCn::CScutLanCn()
	{
		m_szOK = "确定";
		m_szCancel = "取消";
		m_szKnown = "知道了";
		m_szTimeOut = "请求超时，请确认您的网络可用";
		m_szFalseConnect = "网络连接失败，请确认您的可用";
		m_szUpdateError = "更新过程中，发生未知错误";
		m_szDownload = "下载";
		m_szExit = "退出";
		m_szSDNotExist = "SD卡不存在无法完成更新，请插入SD重试";
		m_szSDReadError = "读取SD卡数据失败，请插入SD卡重试";
		m_szResPackageLoadingTip = "正在下载资源包，是否切到后台下载？";
		m_szResPackageFinishTip = "资源包已经下载完成，是否立即退出游戏？";
	}

	CScutLanUs::CScutLanUs()
	{
		m_szOK = "OK";
		m_szCancel = "Cancel";
		m_szKnown = "OK";
		m_szTimeOut = "Request timeout, please make sure your network is available";
		m_szFalseConnect = "Network connection failed, please make sure your network is available";
		m_szUpdateError = "An unknown error occurred when updating";
		m_szDownload = "Download";
		m_szExit = "Exit";
		m_szSDNotExist = "SD card is not exist, please insert the SD card and try again";
		m_szSDReadError = "SD card read error, please insert the SD card and try again";
		m_szResPackageLoadingTip = "Downloading resource package, need to switch to download in background?";
		m_szResPackageFinishTip = "Has finished downloading the resource package, restart the game now?";
	}

	CScutLanTw::CScutLanTw()
	{
		m_szOK = "確定";
		m_szCancel = "取消";
		m_szKnown = "知道了";
		m_szTimeOut = "請求超時，請確認您的網絡可用";
		m_szFalseConnect = "網絡連接失敗，請確認您的網絡可用";
		m_szUpdateError = "更新過程中，發生未知錯誤";
		m_szDownload = "下載";
		m_szExit = "退出";
		m_szSDNotExist = "SD卡不存在無法完成更新，請插入SD卡重試";
		m_szSDReadError = "讀取SD卡數據失敗，請插入SD卡重試";
		m_szResPackageLoadingTip = "正在下載資源包，是否切到後臺下載？";
		m_szResPackageFinishTip = "資源包已經下載完成，是否立即退出遊戲？";
	}

	CScutLanJp::CScutLanJp()
	{
		m_szOK = "はい";
		m_szCancel = "いいえ";
		m_szKnown = "はい";
		m_szTimeOut = "Request timeout, please make sure your network is available";
		m_szFalseConnect = "Network connection failed, please make sure your network is available";
		m_szUpdateError = "An unknown error occurred when updating";
		m_szDownload = "Download";
		m_szExit = "Exit";
		m_szSDNotExist = "SD card is not exist, please insert the SD card and try again";
		m_szSDReadError = "SD card read error, please insert the SD card and try again";
		m_szResPackageLoadingTip = "Downloading resource package, need to switch to download in background?";
		m_szResPackageFinishTip = "Has finished downloading the resource package, restart the game now?";
	}
}

