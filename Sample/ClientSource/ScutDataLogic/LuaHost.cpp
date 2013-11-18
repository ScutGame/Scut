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


#include "StdAfx.h"
#include "LuaHost.h"
#include "CCLuaEngine.h"
#include "CCScene.h"
#include "Cocos2dxLuaLoader.h"
#include "tolua_fix.h"
using namespace ScutDataLogic;
using namespace cocos2d;
LuaHost::LuaHost(void)
{
//  	m_pLuaState = luaL_newstate();
// 	toluafix_open(m_pLuaState);
//  	luaL_openlibs(m_pLuaState);
// 	AddLuaLoader(cocos2dx_lua_loader);

	m_pLuaState = CCLuaEngine::defaultEngine()->getLuaStack()->getLuaState();
}

LuaHost::~LuaHost(void)
{
	//lua_close(m_pLuaState);
}

LuaHost* ScutDataLogic::LuaHost::Instance()
{
 	static LuaHost g_LuaHost;
 	return &g_LuaHost;
}

lua_State* ScutDataLogic::LuaHost::GetLuaState()
{
	return m_pLuaState;
}

void ScutDataLogic::LuaHost::AddLuaLoader(lua_CFunction func)
{
	if (! func)
	{
		return;
	}
	// stack content after the invoking of the function
	// get loader table
	lua_getglobal(m_pLuaState, "package");                     // package
	lua_getfield(m_pLuaState, -1, "loaders");                  // package, loaders

	// insert loader into index 2
	lua_pushcfunction(m_pLuaState, func);                      // package, loaders, func
	for (int i = lua_objlen(m_pLuaState, -2) + 1; i > 2; --i)
	{
		lua_rawgeti(m_pLuaState, -2, i - 1);                   // package, loaders, func, function
		// we call lua_rawgeti, so the loader table now is at -3
		lua_rawseti(m_pLuaState, -3, i);                       // package, loaders, func
	}
	lua_rawseti(m_pLuaState, -2, 2);                           // package, loaders

	// set loaders into package
	lua_setfield(m_pLuaState, -2, "loaders");                  // package

	lua_pop(m_pLuaState, 1);
}

bool ScutDataLogic::LuaHost::execSpriteCallback(const std::string & strFunc, void* pSprite, int nAnimationIndex, int nFrameIndex, int nPlayFlag)
{
	if (!pushfunc(strFunc))
	{
		return false;
	}
	
	tolua_pushusertype(m_pLuaState,(void*)pSprite,"ScutAnimation::CCScutSprite");
	tolua_pushnumber(m_pLuaState, nAnimationIndex);
	tolua_pushnumber(m_pLuaState, nFrameIndex);
	tolua_pushnumber(m_pLuaState, nPlayFlag);

	int error = Scutlua_pcall(m_pLuaState,4,0);
	// Handle errors
	if ( error )
	{
		std::string msg = lua_tostring(m_pLuaState,-1);
		lua_pop(m_pLuaState,1);
		lua_settop( m_pLuaState, 0 );
		std::string msgerror = "(CCLuaScriptModule) Unable to execute scripted event Handler: "+strFunc +msg+"\n";
		ScutLog("%s  %d", msgerror.c_str(), __LINE__);
		return false;
	}
	// return it
	return true;
}

bool ScutDataLogic::LuaHost::execSceneCallback(const std::string& strFunc, int nTag, int nNetState, LPVOID lpExternal, void* pScene)
{

	if (!pushfunc(strFunc))
	{
		return false;
	}
	//tolua_pushnumber(m_pLuaState, nNetState);
	tolua_pushusertype(m_pLuaState,(void*)pScene,"CCScene");
	tolua_pushnumber(m_pLuaState, nTag);

	int error = Scutlua_pcall(m_pLuaState,2,0);
	// Handle errors
	if ( error )
	{
		std::string msg = lua_tostring(m_pLuaState,-1);
		lua_pop(m_pLuaState,1);
		lua_settop( m_pLuaState, 0 );
		std::string msgerror = "(CCLuaScriptModule) Unable to execute scripted event Handler: "+strFunc +msg+"\n";
		ScutLog("%s  %d", msgerror.c_str(), __LINE__);
		return false;
	}
	// return it
	return true;
}


bool ScutDataLogic::LuaHost::pushfunc(const std::string & strFunc)
{
	size_t found = strFunc.find('.');
	if (found == std::string::npos)
	{
		found = strFunc.find(':');
	}
	if (found == std::string::npos)
	{
		lua_getglobal(m_pLuaState, strFunc.c_str());
	}
	else
	{
		std::string strTable = strFunc.substr(0, found);
		std::string strFuncName  = strFunc.substr(found + 1);

		lua_getglobal(m_pLuaState, strTable.c_str());                        // Get the table instance
		int r_obj = luaL_ref(m_pLuaState, LUA_REGISTRYINDEX);

		lua_rawgeti(m_pLuaState, LUA_REGISTRYINDEX, r_obj);   // Get the object instance
		lua_getfield(m_pLuaState, -1, strFuncName.c_str());    // Get the function
		int r_func = luaL_ref(m_pLuaState, LUA_REGISTRYINDEX);
		lua_pop(m_pLuaState, 1);
		lua_rawgeti(m_pLuaState, LUA_REGISTRYINDEX, r_func);
	}

	// is it a function
	if ( !lua_isfunction(m_pLuaState,-1) )
	{
		lua_settop( m_pLuaState, 0 );
		std::string msg = "(CCLuaScriptModule) Unable to execute scripted event Handler: "+strFunc +" name does not represent a Lua function"+"\n";
		ScutLog("%s  %d", msg.c_str(), __LINE__);
		return false;
	}
	return true;
}
//执行无返回值的Lua函数
bool ScutDataLogic::LuaHost::execFunc(const std::string & strFunc, void* pScene, int nTag, void* lpData)
{
	if (strFunc.size() == 0)
	{
		return false;
	}
	if (!pushfunc(strFunc))
	{
		return false;
	}
	int error = 0;

	tolua_pushusertype(m_pLuaState,(void*)pScene,"CCScene");
	tolua_pushnumber(m_pLuaState, nTag);
	
	if (lpData)
	{
		int nTemp = (int)(int*)lpData;
		tolua_pushnumber(m_pLuaState, nTemp);
		error = Scutlua_pcall(m_pLuaState,3,0);
	}
	else
		error = Scutlua_pcall(m_pLuaState,2,0);

	// Handle errors
	if ( error )
	{
		std::string msg = lua_tostring(m_pLuaState,-1);
		lua_pop(m_pLuaState,1);
		lua_settop( m_pLuaState, 0 );
		std::string msgerror = " execute scripted event Handler: "+strFunc + msg +"\n";
		ScutLog("%s  %d", msgerror.c_str(), __LINE__);
		return false;
	}
	
	// return it
	return true;
}

//处理一个返回值
bool ScutDataLogic::LuaHost::execFunc( const std::string & strFunc, void* pScene, bool & bRet)
{
	if (strFunc.size() == 0)
	{
		return false;
	}
	if (!pushfunc(strFunc))
	{
		return false;
	}

	tolua_pushusertype(m_pLuaState,(void*)pScene,"CCScene");
	int error = Scutlua_pcall(m_pLuaState,1,1);
	// Handle errors
	if ( error )
	{
		std::string msg = lua_tostring(m_pLuaState,-1);
		lua_pop(m_pLuaState,1);
		lua_settop( m_pLuaState, 0 );
		std::string msgerror = " execute scripted event Handler: "+strFunc + msg +"\n";
		ScutLog("%s  %d", msgerror.c_str(), __LINE__);
		return false;
	}
	else
	{
		bRet = (bool)lua_toboolean(m_pLuaState, -1);
	}
	// return it
	return true;

}