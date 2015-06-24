#include "cocos2d.h"
#include "ScutExt.h"
#include "CCEGLView.h"
#include "AppDelegate.h"
#include "CCLuaEngine.h"
#include "SimpleAudioEngine.h"
#include "Lua_extensions_CCB.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS || CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID || CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
#include "Lua_web_socket.h"
#endif

static void initLuaGlobalVariables(const std::string& entry);

using namespace CocosDenshion;

USING_NS_CC;

AppDelegate::AppDelegate()
{
}

AppDelegate::~AppDelegate()
{
    SimpleAudioEngine::end();
}

bool AppDelegate::applicationDidFinishLaunching()
{
    // initialize director
    CCDirector *pDirector = CCDirector::sharedDirector();
    pDirector->setOpenGLView(CCEGLView::sharedOpenGLView());

    // turn on display FPS
    pDirector->setDisplayStats(true);

    // set FPS. the default value is 1.0/60 if you don't call this
    pDirector->setAnimationInterval(1.0 / 60);

    // register lua engine
    CCLuaEngine* pEngine = CCLuaEngine::defaultEngine();
    CCScriptEngineManager::sharedManager()->setScriptEngine(pEngine);

    CCLuaStack *pStack = pEngine->getLuaStack();
    lua_State *tolua_s = pStack->getLuaState();
    tolua_extensions_ccb_open(tolua_s);
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS || CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID || CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
    pStack = pEngine->getLuaStack();
    tolua_s = pStack->getLuaState();
    tolua_web_socket_open(tolua_s);
#endif
    
#if (CC_TARGET_PLATFORM == CC_PLATFORM_BLACKBERRY)
    CCFileUtils::sharedFileUtils()->addSearchPath("script");
#endif

	pStack->loadChunksFromZip("res/framework_precompiled.zip");
	std::string lua_entry = "scripts/main.lua";
	initLuaGlobalVariables(lua_entry);
	std::string path = CCFileUtils::sharedFileUtils()->fullPathForFilename(lua_entry.c_str());
	pEngine->executeScriptFile(path.c_str());

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

void initLuaGlobalVariables(const std::string& entry)
{
	//GLOBAL_ROOT_DIR
	CCLuaEngine* pEngine = CCLuaEngine::defaultEngine();
	CCLuaStack* pStack = pEngine->getLuaStack();
	CCFileUtils* pFileUtils = CCFileUtils::sharedFileUtils();
	using namespace std;
	string path = pFileUtils->fullPathForFilename(entry.c_str());
	// replace "\" with "/", normalize the path
	int pos = string::npos;
	while ((pos = path.find_first_of("\\")) != string::npos)
	{
		path.replace(pos, 1, "/");
	}

	string script_dir = path.substr(0, path.find_last_of("/"));
	string root_dir = script_dir.substr(0, script_dir.find_last_of("/"));
	CCLOG("RootDir: %s\nScriptDir: %s \n",root_dir.c_str(), script_dir.c_str());

	std::string env = "GLOBAL_ROOT_DIR=\""; env.append(root_dir); env.append("\"");
	pEngine->executeString(env.c_str());

	env = "__LUA_STARTUP_FILE__=\"";env.append(path);env.append("\"");
	pEngine->executeString(env.c_str());

	pStack->addSearchPath(script_dir.c_str());
	pFileUtils->addSearchPath(root_dir.c_str());
	pFileUtils->addSearchPath(script_dir.c_str());

    ScutExt::Init(root_dir+"/");
}