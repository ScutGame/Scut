#ifndef ScutSCENE_H
#define ScutSCENE_H
#include "CCScene.h"
#include "Stream.h"
namespace ScutCxControl
{
	class ScutScene:public cocos2d::CCScene
	{
	public:
		ScutScene(void);
		virtual ~ScutScene(void);
		bool init();
		static ScutScene* node(void);
		//lua func (int nTag, void * pUserData, const char* pszNetData, int nNetDataSize);
		void registerCallback(const char* pszCallback);
		//执行各类场景的回调函数进行网络数据的分发，由netDataDispatch(void *pScene, int nTag, int nNet, ScutSystem::CStream* lpData, void* lpExternal)调用
		void execCallback( int nTag, int nNetState, ScutSystem::CStream* lpData, void* lpExternal);
		void onExit();
		void onEnter();
		//注册退出场景的LUA回调函数，回调函数原型：(*pszCallback)(ScutScene* pScutScene);
		void registerOnExit(int pszFunc);
		//注册进入场景的LUA回调函数，回调函数原型：(*pszCallback)(ScutScene* pScutScene);
		void registerOnEnter(int pszFunc);

		//注册网络错误处理的LUA回调函数，回调函数原型：(*pszCallback)(ScutScene* pScutScene, int tag);
		static void registerNetErrorFunc(const char* pszCallback);
		//注册网络数据公用数据头处理的LUA回调函数，回调函数原型：(*pszCallback)(ScutScene* pScutScene); 
		static void registerNetCommonDataFunc(const char* pszCallback);//处理每个协议公段的字段

		//注册网络数据处理结束的LUA的回调函数，回调函数原型：(*pszCallback)(ScutScene* pScutScene, int tag);
		static void registerNetDecodeEnd(const char* pszCallback);
	private:
		int m_nEnter;
		int m_nExit;
		std::string m_strCallback;
		static std::string s_strNetErrorFunc;
		static std::string s_strNetCommonFunc;
		static std::string s_strNetDecodeend;
	};



}

#include "tolua_fix.h"
using namespace ScutCxControl;
typedef int LUA_FUNCTION;

#endif//ScutSCENE_H