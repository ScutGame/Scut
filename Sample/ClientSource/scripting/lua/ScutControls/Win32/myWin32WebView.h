#ifndef __MY_WIN32_WEB_VIEW_H__
#define __MY_WIN32_WEB_VIEW_H__
#include "cocos2d.h"

namespace NdCxControl
{
	void *Win32WebView(const char *pszUrl, cocos2d::CCRect rcScreenFrame, const char *pszTitle, const char *pszNormal, const char *pszPushDown);

	void CloseWin32WebView(void *pWebView);

	void SwitchWin32WebViewUrl(void *pWebView, const char *pszUrl);
}
#endif