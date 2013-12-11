#include "cocos2d.h"
#include "AppDelegate.h"
#include "SimpleAudioEngine.h"
#include "script_support/CCScriptSupport.h"
#include "CCLuaEngine.h"
#include "FileHelper.h"
#include "LuaHost.h"
#include "DataRequest.h"
#include "ScutSDK.h"

using namespace CocosDenshion;
using namespace ScutAnimation;
using namespace ScutDataLogic;

USING_NS_CC;


#ifdef WIN32
extern int g_nScreenWidth;
extern int g_nScreenHeight;
#endif

AppDelegate::AppDelegate()
{
    // fixed me
    //_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF|_CRTDBG_LEAK_CHECK_DF);
}

AppDelegate::~AppDelegate()
{
    // end simple audio engine here, or it may crashed on win32
    SimpleAudioEngine::sharedEngine()->end();
    //CCScriptEngineManager::purgeSharedManager();
}

bool AppDelegate::applicationDidFinishLaunching()
{
    // initialize director
    CCDirector *pDirector = CCDirector::sharedDirector();
    pDirector->setOpenGLView(CCEGLView::sharedOpenGLView());
    
    CCEGLView::sharedOpenGLView()->setDesignResolutionSize(g_nScreenWidth, g_nScreenHeight, kResolutionNoBorder);

    // turn on display FPS
    pDirector->setDisplayStats(true);

    // set FPS. the default value is 1.0/60 if you don't call this
    pDirector->setAnimationInterval(1.0 / 60);

    // register lua engine
    CCLuaEngine* pEngine = CCLuaEngine::defaultEngine();
    CCScriptEngineManager::sharedManager()->setScriptEngine(pEngine);
	lua_State * pLuaState = ScutDataLogic::LuaHost::Instance()->GetLuaState();
	char tempPath[MAX_PATH + 1];
	::GetModuleFileNameA(NULL,tempPath, _MAX_PATH + 1);
	string ret((char*)tempPath);
	ret = ret.substr(0,ret.rfind("\\") +  1);
	std::string path =ret +  "lua/mainapp.lua";
	pEngine->executeScriptFile(path.c_str());

	CCSize szWin = pDirector->getWinSize();
	CFileHelper::setWinSize(szWin.width, szWin.height);
    return true;
}

// This function will be called when the app is inactive. When comes a phone call,it's be invoked too
void AppDelegate::applicationDidEnterBackground()
{
    CCDirector::sharedDirector()->stopAnimation();
    SimpleAudioEngine::sharedEngine()->pauseBackgroundMusic();
}

// this function will be called when the app is active again
void AppDelegate::applicationWillEnterForeground()
{
    CCDirector::sharedDirector()->startAnimation();
    SimpleAudioEngine::sharedEngine()->resumeBackgroundMusic();
}
