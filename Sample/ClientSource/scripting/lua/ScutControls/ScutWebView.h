#ifndef _SCUTWEBVIEW_H_
#define _SCUTWEBVIEW_H_
#include "CCPlatformMacros.h"
#include <string>
#include "ccTypes.h"
#include "CCDirector.h"
//#include "cocos2d.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
#include "Winuser.h"
#endif

#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
#include "./Android/AndroidWindow.h"
#endif
#include "../cocos2dx_support/CCLuaEngine.h"	

namespace ScutCxControl
{

	/**
	* @brief  WEB 页面控件
	* @remarks   
	* @see		
	*/
	class LUA_DLL ScutWebView
	{
	public:
		ScutWebView(void);
		virtual ~ScutWebView(void);
		//初始化编辑框控件

		/**
		* @brief  创建WEB 控件， 
		* @n<b>函数名称</b>					: init
		* @n@param  szUrl url地址
		* @param    CCRect 位置和大小,  仅IPHOEN 平台有效，ANDROID 平台无效
		* @param    char 标题显示,   仅IPHOEN 平台有效，ANDROID 平台无效
		* @param    char* 返回按据Normal图片 ,  仅IPHOEN 平台有效，ANDROID 平台无效
		* @param    char* 返回按据PushDown图片 ,  仅IPHOEN 平台有效，ANDROID 平台无效
		* @remarks  IPHOEN 、ANDROID 平台可用 。 
		* @see		
		*/
		bool init(char* szUrl, cocos2d::CCRect rcScreenFrame, const char* szTitle, const char* szNormal, const char* szPushDown);
		void close();
		void switchUrl(const char* szUrl);
	private:
		void* m_pInnerWebView;
	};
}
#endif
