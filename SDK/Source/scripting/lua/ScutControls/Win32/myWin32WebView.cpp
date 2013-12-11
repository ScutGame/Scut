#include "Win32WebView.h"
#include "cocos2d.h"
#include "myWin32WebView.h"

namespace NdCxControl
{
	void *Win32WebView(const char *pszUrl, cocos2d::CCRect rcScreenFrame, const char *pszTitle, const char *pszNormal, const char *pszPushDown)
	{
 		CWin32WebView *pWebView = new CWin32WebView();

		if(pWebView && pWebView->init(pszUrl, rcScreenFrame, pszTitle, pszNormal, pszPushDown))
		{
			return pWebView;
		}
		else
		{
			if (pWebView)
			{
				delete pWebView;
			}
			return NULL;
		}
	}

	void CloseWin32WebView(void *pWebView)
	{
		if (pWebView)
		{
			CWin32WebView *pView = (CWin32WebView*)pWebView;
			pView->close();
		}
	}

	void SwitchWin32WebViewUrl(void *pWebView, const char *pszUrl)
	{
		if (pWebView)
		{
			CWin32WebView *pView = (CWin32WebView*)pWebView;
			pView->switchUrl(pszUrl);
		}
	}
}