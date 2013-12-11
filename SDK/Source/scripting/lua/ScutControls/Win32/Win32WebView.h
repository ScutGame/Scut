#ifndef __WIN32_WEB_VIEW_H__
#define __WIN32_WEB_VIEW_H__
#include <windows.h>
#include "CCPlatformMacros.h"
#include <string>
#include "ccTypes.h"
#include "CCDirector.h"
#include "../cocos2dx_support/CCLuaEngine.h"
#include "Winuser.h"

namespace NdCxControl
{
class LUA_DLL CWin32WebView
{
public:
	CWin32WebView(void);
	~CWin32WebView(void);

	bool init(const char *pszUrl, cocos2d::CCRect screenFrame, const char *pszTitle, const char *pszNormalImg, const char *pszPushImg);
	void close(void);
	void switchUrl(const char *pszUrl);
protected:
	HBITMAP bitmapWithPath(const char *path, int &width, int &height);
private:
	const char *m_pszUrl;
	HWND	m_hWebView;
	HWND	m_hContainer;
};
} // end of namespace NdCxControl
#endif