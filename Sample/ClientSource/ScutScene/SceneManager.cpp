#include "SceneManager.h"
#include "CCDirector.h"

using namespace cocos2d;

namespace ScutCxControl
{
	CSceneManager* CSceneManager::instance = NULL;

	void SceneChangeCallBack(void* pSender, int nType)
	{
		if (pSender && nType == 2)
		{
			//CSceneManager::getInstance()->earse((ScutScene*)pSender);
		}		
	}

	CSceneManager::CSceneManager(void)
	{
		CCDirector::sharedDirector()->registerSceneChangeCallback(SceneChangeCallBack);
	}

	CSceneManager::~CSceneManager(void)
	{
		instance = NULL;
		m_mapScutScene.clear();
	}

	CSceneManager* CSceneManager::getInstance()
	{
		if (NULL == instance)
		{
			instance = new CSceneManager();
		}
		return instance;
	}
	
	void CSceneManager::push(ScutScene* pScene)
	{
		m_mapScutScene.insert(std::pair<ScutScene*, int>(pScene, 0));	
	}

	void CSceneManager::earse(ScutScene* pScene)
	{
		std::map<ScutScene*, int>::iterator it= m_mapScutScene.find(pScene);
		if (it != m_mapScutScene.end())
		{
			m_mapScutScene.erase(it);
		}
	}
	bool CSceneManager::find(ScutScene* pScene)
	{
		bool bRet = false;
		std::map<ScutScene*, int>::iterator it= m_mapScutScene.find(pScene);
		if (it != m_mapScutScene.end())
		{
			bRet = true;
		}
		return bRet;
	}

	void netDataDispatch(void *pScene, int nTag, int nNet, ScutSystem::CStream* lpData, void* lpExternal)
	{
		if (pScene == NULL)
		{
			return ;
		}
		bool bfind = CSceneManager::getInstance()->find((ScutScene*)pScene);
		if (bfind)
		{
			((ScutScene*)pScene)->execCallback(nTag, nNet, lpData, lpExternal);
		}
	}
}


		