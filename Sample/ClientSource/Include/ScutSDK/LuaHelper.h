/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

http://www.scutgame.com

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
****************************************************************************/
#ifndef LuaHelper_H
#define LuaHelper_H


extern "C"
{
#include "lua.h"
#include "tolua++.h"
#include "lualib.h"
}

#include "NetClientBase.h"

namespace ScutNetwork
{	
	class LuaHelper
	{
	private:
		LuaHelper(void);
		virtual ~LuaHelper(void);
	public:
		static LuaHelper* Instance();
		void setLuaState( lua_State* pState );

		void AddLuaLoader(lua_CFunction func);
		bool execSpriteCallback(const std::string & strFunc, void* pSprite, int nAnimationIndex, int nFrameIndex, int nPlayFlag);
		bool execSceneCallback(const std::string& strFunc, int nTag, int nNetState, LPVOID lpExternal, void* pScene);
		//处理一个返回值
		//如果bCallback return
		bool execFunc(const std::string & strFunc,  void* pScene,bool & bCallback);
		//处理无返回值
		bool execFunc(const std::string & strFunc, AsyncInfo* pAi);
		bool pushfunc(const std::string & strFunc);
	private:
		
		lua_State* m_pLuaState;
	};
}
#endif//LuaHelper_H