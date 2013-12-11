#include "Win32WebView.h"
#include <Windows.h>
#include "cwebpage.h"
#include "FileHelper.h"
#include "./win32/LoadBitmapPNG.h"
#include "cocos2d.h"
using namespace cocos2d;
enum { ID_LABEL = 1, ID_BUTTON };
#define WINDOW_CLASS_CONTAINER TEXT("WINCLASS_CONTAINER")
#define WINDOW_CLASS_WEBVIEW TEXT("WINCLASS_WEBVIEW")

namespace NdCxControl
{
unsigned char WindowCount = 0;

extern wchar_t* CharToWChar(const char* pszStr, unsigned int nCodePage);
extern char* WCharToChar(const wchar_t* pwszStr, unsigned int nCodePage);
bool isFileExists(const char* szFilePath)
{
	DWORD dwFileAttr = GetFileAttributesA(szFilePath);
	if (INVALID_FILE_ATTRIBUTES == dwFileAttr
		|| (dwFileAttr&FILE_ATTRIBUTE_DIRECTORY))	{
			return false;
	}	
	return true;
}

static LRESULT CALLBACK _WindowProcContainer(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{
	switch(uMsg)
	{
	case WM_COMMAND:
		switch(wParam) 
		{
		case ID_BUTTON: 
			DestroyWindow(hwnd);
			UnregisterClass(WINDOW_CLASS_CONTAINER, GetModuleHandle(NULL));
			UnregisterClass(WINDOW_CLASS_WEBVIEW, GetModuleHandle(NULL));
		}
		break;
	case WM_CTLCOLORSTATIC:
		{
			HDC hdcStatic = (HDC)wParam;
			SetBkMode(hdcStatic, TRANSPARENT);
			SetTextColor(hdcStatic, RGB(255,255,255));
			
			return (LONG)CreateSolidBrush(RGB(86, 49, 10));
		}

	}
	return(DefWindowProc(hwnd, uMsg, wParam, lParam));
}

static LRESULT CALLBACK _WindowProcWebView(HWND hwnd, UINT uMsg, WPARAM wParam, LPARAM lParam)
{

	switch (uMsg)
	{
	case WM_SIZE:
		{
			// Resize the browser object to fit the window
			ResizeBrowser(hwnd, LOWORD(lParam), HIWORD(lParam));
			return(0);
		}

	case WM_CREATE:
		{
			// Embed the browser object into our host window. We need do this only
			// once. Note that the browser object will start calling some of our
			// IOleInPlaceFrame and IOleClientSite functions as soon as we start
			// calling browser object functions in EmbedBrowserObject().
			if (EmbedBrowserObject(hwnd)) return(-1);

			// Another window created with an embedded browser object
			++WindowCount;

			// Success
			return(0);
		}

	case WM_DESTROY:
		{
			// Detach the browser object from this window, and free resources.
			UnEmbedBrowserObject(hwnd);
			OleUninitialize();
			// One less window
			--WindowCount;

			return(TRUE);
		}
	}

	return(DefWindowProc(hwnd, uMsg, wParam, lParam));

}

CWin32WebView::CWin32WebView(void)
:m_pszUrl(NULL)
,m_hWebView(NULL)
,m_hContainer(NULL)
{
}

CWin32WebView::~CWin32WebView(void)
{
}

HBITMAP CWin32WebView::bitmapWithPath(const char *path, int &width, int &height)
{
	HBITMAP hbitmap = NULL;
	
	//mark es2.0
	//if (!isFileExists(path) && cocos2d::CCImage::getIsScaleEnabled())
	//{
	//	string strTemp(path);
	//	if (strTemp.rfind("@2x") == std::string::npos)
	//	{
	//		int t = strTemp.rfind(".");
	//		if (t != std::string::npos)
	//		{
	//			strTemp.insert(t, "@2x");
	//			
	//			strTemp = NdDataLogic::CFileHelper::getPath(strTemp.c_str());
	//			if (isFileExists(strTemp.c_str()))
	//			{ 
	//				hbitmap = cocos2d::LoadBitmapPNG(strTemp.c_str(), width, height);
	//			}
	//		}
	//	}
	//}
		
	return hbitmap;
}

bool CWin32WebView::init(const char *pszUrl, 
						 cocos2d::CCRect screenFrame, 
						 const char *pszTitle, 
						 const char *pszNormalImg,
						 const char *pszPushImg)
{
	bool bRet = false;
	while (1)
	{
		if (OleInitialize(NULL) != S_OK)
		{
			break;
		}

		HINSTANCE hInstance = GetModuleHandle( NULL );
		WNDCLASS  wc;		// Windows Class Structure
		HWND hParent = CCDirector::sharedDirector()->getOpenGLView()->getHWnd();
		float scale = CC_CONTENT_SCALE_FACTOR();
		screenFrame.origin.x *= scale;
		screenFrame.origin.y *= scale;
		screenFrame.size.width *= scale;
		screenFrame.size.height *= scale;

		// Redraw On Size, And Own DC For Window.
		wc.style          = CS_HREDRAW|CS_HREDRAW;  
		wc.lpfnWndProc    = _WindowProcContainer;							// WndProc Handles Messages
		wc.cbClsExtra     = 0;                              // No Extra Window Data
		wc.cbWndExtra     = 0;								// No Extra Window Data
		wc.hInstance      = hInstance;						// Set The Instance
		wc.hIcon          = LoadIcon( NULL, IDI_WINLOGO );	// Load The Default Icon
		wc.hCursor        = LoadCursor( NULL, IDC_ARROW );	// Load The Arrow Pointer
		wc.hbrBackground  = NULL;                           // No Background Required For GL
		wc.lpszMenuName   = NULL;                           // We Don't Want A Menu
		wc.lpszClassName  = WINDOW_CLASS_CONTAINER;  
		wc.hbrBackground  = (HBRUSH)CreateSolidBrush(RGB(86, 49, 10));



		if(!RegisterClass(&wc))
		{
			break;
		}

		m_hContainer = CreateWindowEx(
			WS_EX_APPWINDOW | WS_EX_WINDOWEDGE,	// Extended Style For The Window
			WINDOW_CLASS_CONTAINER,									// Class Name
			NULL,												// Window Title
			WS_CHILD,	// Defined Window Style
			(int)screenFrame.origin.x, (int)screenFrame.origin.y,								                // Window Position
			(int)screenFrame.size.width,                                                  // Window Width
			(int)screenFrame.size.height,                                                  // Window Height
			hParent,												// No Parent Window
			(HMENU)ID_BUTTON,												// No Menu
			hInstance,											// Instance
			NULL );

		if(m_hContainer == NULL)
		{		
			break;
		}

		screenFrame.origin.y = 0;
		screenFrame.origin.x = 0;
		// web view
		if (pszTitle && strlen(pszTitle) > 0)
		{
			int width, height;
			HBITMAP hNormal = bitmapWithPath(pszNormalImg, width, height);

			screenFrame.origin.y += height;
			screenFrame.size.height -= height;

			HWND hButton = CreateWindowA("Button",
				"Hello",
				BS_DEFPUSHBUTTON | WS_CHILD | WS_VISIBLE | BS_BITMAP | BS_FLAT ,
				screenFrame.size.width - width,0,                                                    
				width, height,                                                  
				m_hContainer,
				(HMENU)ID_BUTTON,
				hInstance,
				NULL);
			LRESULT lr = SendMessage(hButton,BM_SETIMAGE,IMAGE_BITMAP,(LPARAM)hNormal );

			int nFontHeight = 18 * scale;
			wchar_t* pszTemp = CharToWChar(pszTitle, CP_UTF8);
			HWND hStatic = CreateWindow(TEXT("Static"),
				LPCWSTR(pszTemp),
				WS_CHILD | WS_VISIBLE | SS_CENTER,
				(screenFrame.size.width - 200)/2, (height - nFontHeight)/2,
				200,nFontHeight,
				m_hContainer,
				0,
				hInstance,
				0);
			delete []pszTemp;

			if (NULL == hStatic)
			{
				break;
			}

			HFONT hFont= CreateFontA (nFontHeight, 0, 0, 0, 
				FW_DONTCARE, 
				FALSE, 
				FALSE, 
				FALSE, 
				ANSI_CHARSET, 
				OUT_DEFAULT_PRECIS, 
				CLIP_DEFAULT_PRECIS, 
				DEFAULT_QUALITY, 
				DEFAULT_PITCH | FF_SWISS,
				"Arial");
			if (hFont)
			{
				SendMessage (hStatic, WM_SETFONT, WPARAM (hFont), TRUE);
			}
		}

		wc.style          = CS_HREDRAW|CS_HREDRAW;  
		wc.lpfnWndProc    = _WindowProcWebView;							// WndProc Handles Messages
		wc.cbClsExtra     = 0;                              // No Extra Window Data
		wc.cbWndExtra     = 0;								// No Extra Window Data
		wc.hInstance      = hInstance;						// Set The Instance
		wc.hIcon          = LoadIcon( NULL, IDI_WINLOGO );	// Load The Default Icon
		wc.hCursor        = LoadCursor( NULL, IDC_ARROW );	// Load The Arrow Pointer
		wc.hbrBackground  = NULL;                           // No Background Required For GL
		wc.lpszMenuName   = NULL;                           // We Don't Want A Menu
		wc.lpszClassName  = WINDOW_CLASS_WEBVIEW;  

		if(!RegisterClass(&wc))
		{
			break;
		}

		m_hWebView = CreateWindowEx(
			WS_EX_APPWINDOW | WS_EX_WINDOWEDGE,	// Extended Style For The Window
			WINDOW_CLASS_WEBVIEW,									// Class Name
			NULL,												// Window Title
			WS_CHILD,	// Defined Window Style
			(int)screenFrame.origin.x, (int)screenFrame.origin.y,								                // Window Position
			(int)screenFrame.size.width,                                                  // Window Width
			(int)screenFrame.size.height,                                                  // Window Height
			m_hContainer,												// No Parent Window
			NULL,												// No Menu
			hInstance,											// Instance
			NULL );

		if(m_hWebView == NULL)
		{		
			break;
		}

		wchar_t* pszTemp = CharToWChar(pszUrl, CP_UTF8);
		DisplayHTMLPage(m_hWebView, pszTemp);
		delete []pszTemp;

		ShowWindow(m_hWebView, SW_SHOW);
		UpdateWindow(m_hWebView);

		ShowWindow(m_hContainer, SW_SHOW);
		UpdateWindow(m_hContainer);

		bRet = true;
		break;
	}
	if (!bRet)
	{
		close();
		OleUninitialize();
	}
	return bRet;
}

void CWin32WebView::close(void)
{
	if (m_hContainer)
	{
		DestroyWindow(m_hContainer);
	}
	UnregisterClass(WINDOW_CLASS_CONTAINER, GetModuleHandle(NULL));
	UnregisterClass(WINDOW_CLASS_WEBVIEW, GetModuleHandle(NULL));
}

void CWin32WebView::switchUrl(const char *pszUrl)
{
	wchar_t* pszTemp = CharToWChar(pszUrl, CP_UTF8);
	DisplayHTMLPage(m_hWebView, pszTemp);
	delete []pszTemp;
}
}