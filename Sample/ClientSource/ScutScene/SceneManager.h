#ifndef SCENEMANAGER_H
#define SCENEMANAGER_H
//用于管理 ScutScene
#include <map>

#include "Stream.h"
#include "ScutScene.h"

namespace ScutCxControl
{
	
	//场景管理类，ScutScene类对象在生成时将自动加入到CSceneManager里，无需手动加入
	class CSceneManager
	{
	public:
		~CSceneManager(void);
		static CSceneManager* getInstance();
		void push(ScutScene* pScene);
		void earse(ScutScene* pScene);
		bool find(ScutScene* pScene);
	private:
		static CSceneManager* instance;
		CSceneManager(void);
		std::map<ScutScene*, int> m_mapScutScene;
	};
	
	//处理
	void netDataDispatch(void *pScene, int nTag, int nNet, ScutSystem::CStream* lpData, void* lpExternal);

}


#endif//SCENEMANAGER_H