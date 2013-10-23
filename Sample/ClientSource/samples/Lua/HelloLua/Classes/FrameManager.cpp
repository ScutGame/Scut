#include "DataRequest.h"
#include "FrameManager.h"
#include "CCScheduler.h"
//#include "UpdateEngine.h"
#include "CCDirector.h"
//using namespace NdUpdate;
using namespace ScutDataLogic;

CFrameManager::CFrameManager( void )
{
	cocos2d::CCDirector::sharedDirector()->getScheduler()->scheduleUpdateForTarget(this, 0, false);
}

CFrameManager::~CFrameManager( void )
{
	cocos2d::CCDirector::sharedDirector()->getScheduler()->unscheduleUpdateForTarget(this);
}

void CFrameManager::update( float dt )
{
	//执行网络数据处理
	CDataRequest::Instance()->PeekLUAData();
	//执行插件相关处理
	//CUpdateEngine::getInstance()->tick(dt);	
}

CFrameManager* CFrameManager::Instance()
{
	static CFrameManager g_Instance;
	return &g_Instance;
}

