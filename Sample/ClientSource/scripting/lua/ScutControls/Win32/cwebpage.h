/*
 * This include file is meant to be included with any C/C++ source you
 * write which uses the cwebpage DLL.
 */

#ifndef __CWEBPAGE_H_INCLUDED
#define __CWEBPAGE_H_INCLUDED

#ifdef UNICODE
#define _UNICODE
#endif
#include <windows.h>
#include <exdisp.h>		/* Defines of stuff like IWebBrowser2. This is an include file with Visual C 6 and above */
#include <mshtml.h>		/* Defines of stuff like IHTMLDocument2. This is an include file with Visual C 6 and above */
#include <mshtmhst.h>	/* Defines of stuff like IDocHostUIHandler. This is an include file with Visual C 6 and above */
#include <crtdbg.h>		// for _ASSERT()

#ifdef __cplusplus
extern "C" {
#endif

long DisplayHTMLPage(HWND hwnd, LPTSTR webPageName);
void ResizeBrowser(HWND hwnd, DWORD width, DWORD height);
long EmbedBrowserObject(HWND hwnd);
void UnEmbedBrowserObject(HWND hwnd);


#ifdef __cplusplus
}
#endif

#endif /* __CWEBPAGE_H_INCLUDED */
