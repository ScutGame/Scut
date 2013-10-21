#include "ScutScene.h"
#include "SceneManager.h"
#include "NetHelper.h"
#include "LuaHost.h"
#include "HttpClient.h"
#include "NetHelper.h"
#include "CCLuaEngine.h"
using namespace ScutDataLogic;
namespace ScutCxControl
{
#define  eNetSuccess 10000
	std::string ScutScene::s_strNetErrorFunc;
	std::string ScutScene::s_strNetCommonFunc;
	std::string ScutScene::s_strNetDecodeend;
	ScutScene::ScutScene(void)
		: m_nEnter(0)
		, m_nExit(0)
	{
		CSceneManager::getInstance()->push(this);
	}

	ScutScene::~ScutScene(void)
	{
		CSceneManager::getInstance()->earse(this);
	}

	bool ScutScene::init()
	{
		return cocos2d::CCScene::init();
	}

	ScutScene* ScutScene::node(void)
	{
		ScutScene *pRet = new ScutScene(); 
		if (pRet && pRet->init()) 
		{ 
			pRet->autorelease(); 
			return pRet; 
		} 
		else 
		{ 
			delete pRet; 
			pRet = NULL; 
			return NULL; 
		} 
	}

	void ScutScene::onExit()
	{
		/*if (m_nExit)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFuncN(m_nExit, this);
		}*/
		CCScene::onExit();
	}

	void ScutScene::onEnter()
	{
		CCScene::onEnter();
		/*if (m_nEnter)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeFuncN(m_nEnter, this);
		}*/
	}

	void ScutScene::registerOnExit(int nFunc)
	{
		m_nExit = nFunc;
		LUALOG("[LUA] Add ScutScene script OnExit handler: %d", m_nExit);
	}

	void ScutScene::registerOnEnter(int nFunc)
	{
		m_nEnter = nFunc;
		LUALOG("[LUA] Add ScutScene script OnEnter handler: %d", m_nEnter);
	}

	void ScutScene::registerCallback(const char* pszCallback)
	{
		if (pszCallback)
		{
			m_strCallback = pszCallback;
		}
		else
		{
			cocos2d::CCLog("registerCallback error");
		}
	}

	void ScutScene::execCallback( int nTag, int nNetState, ScutSystem::CStream* lpData, void* lpExternal)
	{
 		if (m_strCallback.size() == 0)
 		{
 			return ;
 		}
		if (ScutNetwork::aisSucceed == nNetState)
		{
			assert(((ScutSystem::CMemoryStream*)lpData)->GetSize() > 0);
			bool bValue = ScutDataLogic::CNetReader::getInstance()->pushNetStream((char*)((ScutSystem::CMemoryStream*)lpData)->GetMemory(), ((ScutSystem::CMemoryStream*)lpData)->GetSize());
			if (!bValue)
			{
				return ;
			}
			bool bProcessNext = true;
			if (s_strNetCommonFunc.size() /*&& ScutDataLogic::CNetReader::getInstance()->getResult() == eNetSuccess*/)
			{
				//处理公共的事件和通用的Data块.
				
				ScutDataLogic::LuaHost::Instance()->execFunc(s_strNetCommonFunc,(void*)this, bProcessNext);
			}
			if (bProcessNext)
			{
				ScutDataLogic::LuaHost::Instance()->execSceneCallback(m_strCallback, nTag, nNetState, lpExternal, (void*)this);
			}
			
			if (s_strNetDecodeend.size())
			{
				ScutDataLogic::LuaHost::Instance()->execFunc(s_strNetDecodeend, (void*)this, nTag);
			}			
		}
		else 
		{
			if (s_strNetErrorFunc.size())
			{
				ScutDataLogic::LuaHost::Instance()->execFunc(s_strNetErrorFunc, (void*)this, nTag);
			}
		}
	}

	void ScutScene::registerNetErrorFunc( const char* pszCallback )
	{
		s_strNetErrorFunc = pszCallback;		
	}

	void ScutScene::registerNetCommonDataFunc( const char* pszCallback )
	{
		s_strNetCommonFunc = pszCallback;
	}

	void ScutScene::registerNetDecodeEnd( const char* pszCallback )
	{
		s_strNetDecodeend = pszCallback;
	}
}
