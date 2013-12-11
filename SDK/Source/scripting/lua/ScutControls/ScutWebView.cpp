#include "ScutWebView.h"
#include "CCPlatformMacros.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
#endif

#ifdef Scut_ANDROID
#include "android/AndroidJni.h"
#elif defined Scut_WIN32
#include "win32/myWin32WebView.h"
#endif


namespace ScutCxControl
{
	bool ScutWebView::init(char* szUrl, cocos2d::CCRect rcScreenFrame, const char* szTitle, const char* szNormal, const char* szPushDown)
	{

#ifdef Scut_ANDROID
		std::string title = szTitle;
		if (szTitle == NULL)
		{
			title="";
		}
		std::string url = szUrl;
		if (szUrl == NULL)
		{
			url="";
		}
		return m_pInnerWebView = (void*)AndroidWebView(url, title, rcScreenFrame.origin.x, rcScreenFrame.origin.y,rcScreenFrame.size.width, rcScreenFrame.size.height);
#endif

#ifdef Scut_IPHONE
		void* IOSWebView(char* szUrl, cocos2d::CCRect rcScreenFrame, const char* szTitle, const char* szNormal, const char* szPushDown );
		m_pInnerWebView = IOSWebView(szUrl, rcScreenFrame, szTitle, szNormal, szPushDown );
		return m_pInnerWebView;
#elif defined(Scut_MAC)
		void* MACWebView(char* szUrl, cocos2d::CCRect rcScreenFrame, const char* szTitle, const char* szNormal, const char* szPushDown );
		m_pInnerWebView = MACWebView(szUrl, rcScreenFrame, szTitle, szNormal, szPushDown );
		return m_pInnerWebView;
#elif defined(Scut_WIN32)
		m_pInnerWebView = Win32WebView(szUrl, rcScreenFrame, szTitle, szNormal, szPushDown);
		return m_pInnerWebView;
#endif
		return false;

	}

	ScutWebView::ScutWebView( void )
	{
		m_pInnerWebView = 0;
	}

	ScutWebView::~ScutWebView( void )
	{

	}

	void ScutWebView::close()
	{
		if (m_pInnerWebView)
		{
#ifdef Scut_ANDROID
			CloseAndroidWebView();
#elif Scut_IPHONE
			void CloseIOSWebView(void* pWebView);
			CloseIOSWebView(m_pInnerWebView);
#elif Scut_MAC
			void CloseMACWebView(void* pWebView);
			CloseMACWebView(m_pInnerWebView);
#elif Scut_WIN32
			CloseWin32WebView(m_pInnerWebView);
#endif
		}
	}

	void ScutWebView::switchUrl( const char* szUrl )
	{
		if (m_pInnerWebView)
		{
#ifdef Scut_ANDROID
			SwitchAndroidWebViewUrl(szUrl);
#elif Scut_IPHONE
			void SwitchIOSWebViewUrl(void* pWebView, const char* szUrl);
			SwitchIOSWebViewUrl(m_pInnerWebView, szUrl);
#elif Scut_MAC
			void SwitchMACWebViewUrl(void* pWebView, const char* szUrl);
			SwitchMACWebViewUrl(m_pInnerWebView, szUrl);
#elif Scut_WIN32
			SwitchWin32WebViewUrl(m_pInnerWebView, szUrl);
#endif
		}
	}

}

