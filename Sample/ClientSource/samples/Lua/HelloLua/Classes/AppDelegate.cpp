#include "cocos2d.h"
#include "AppDelegate.h"
#include "FileHelper.h"
#include "SimpleAudioEngine.h"
#include "script_support/CCScriptSupport.h"
#include "CCLuaEngine.h"
#include "LuaHost.h"
#include "DataRequest.h"
USING_NS_CC;
using namespace CocosDenshion;
using namespace ScutDataLogic;


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

#ifdef WIN32
extern int g_nScreenWidth;
extern int g_nScreenHeight;
#endif


bool AppDelegate::applicationDidFinishLaunching()
{
    // initialize director
    CCDirector *pDirector = CCDirector::sharedDirector();
    pDirector->setOpenGLView(CCEGLView::sharedOpenGLView());
    
    CCEGLView::sharedOpenGLView()->setDesignResolutionSize(g_nScreenWidth, g_nScreenHeight, kResolutionShowAll);

    // turn on display FPS
    pDirector->setDisplayStats(true);

    // set FPS. the default value is 1.0/60 if you don't call this
    pDirector->setAnimationInterval(1.0 / 60);
	

    // register lua engine
    CCLuaEngine* pEngine = CCLuaEngine::defaultEngine();
    CCScriptEngineManager::sharedManager()->setScriptEngine(pEngine);
	
	lua_State * pLuaState = ScutDataLogic::LuaHost::Instance()->GetLuaState();

	//CDataRequest::Instance()->RegisterLUACallBack((LUA_DATAHANDLE_CALLBACK)&ScutCxControl::netDataDispatch);
	//CFrameManager::Instance(); 
	char tempPath[MAX_PATH + 1];
	::GetModuleFileNameA(NULL,tempPath, _MAX_PATH + 1);
	string ret((char*)tempPath);
	ret = ret.substr(0,ret.rfind("\\") +  1);

    //std::string path = CCFileUtils::sharedFileUtils()->fullPathForFilename("E:/ND/openSourceEngine/Scutgame/Release.win32/lua/mainapp.lua");
	std::string path =ret +  "lua/mainapp.lua";
	//CFileHelper::executeScriptFile(NdDataLogic::CFileHelper::getPath("lua/mainapp.lua").c_str());
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
