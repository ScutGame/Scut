#include "main.h"
#include "AppDelegate.h"
#include "CCEGLView.h"
#include "FileHelper.h"

USING_NS_CC;

// uncomment below line, open debug console
#define USE_WIN32_CONSOLE
int g_nScreenWidth = 960;
int g_nScreenHeight = 640;

int APIENTRY _tWinMain(HINSTANCE hInstance,
                       HINSTANCE hPrevInstance,
                       LPTSTR    lpCmdLine,
                       int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

#ifdef USE_WIN32_CONSOLE
    AllocConsole();
    freopen("CONIN$", "r", stdin);
    freopen("CONOUT$", "w", stdout);
    freopen("CONOUT$", "w", stderr);
#endif

	int nRes = 0;
	char szFolderName[MAX_PATH] = {0};
	//int nScreenWidth = 960, nScreenHeight = 640;
	sscanf(lpCmdLine, "%d %d %s", &g_nScreenWidth, &g_nScreenHeight, szFolderName);
	//_tscanf
	if (strlen(szFolderName) > 0)
	{
		ScutDataLogic::CFileHelper::setResourceFolderName(szFolderName);
	}
    // create the application instance
    AppDelegate app;
    CCEGLView* eglView = CCEGLView::sharedOpenGLView();

	eglView->setViewName("MainApp");
	eglView->setFrameSize(g_nScreenWidth, g_nScreenHeight);
    int ret = CCApplication::sharedApplication()->run();

#ifdef USE_WIN32_CONSOLE
    FreeConsole();
#endif

    return ret;
}
