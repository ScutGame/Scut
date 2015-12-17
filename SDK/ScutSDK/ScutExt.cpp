#include "ScutExt.h"
#include "cocos2d.h"
#include "CCLuaEngine.h"
#include "CCScriptSupport.h"
#include <string.h>

#ifdef __cplusplus
extern "C" {
#endif 
#include "lua.h"
#include "lauxlib.h"
#ifdef __cplusplus
};
#endif

USING_NS_CC;

extern int tolua_Scut_open (lua_State* tolua_S);
extern "C" int (Scutlua_pcall) (lua_State *L, int nargs, int nresults);

ScutExt* ScutExt::sInstance = NULL;
std::string ScutExt::sResRootDir = "";

int execute_lua_function(lua_State *L, int numArgs, bool removeResult)
{
	int functionIndex = -(numArgs + 1);
	if (!lua_isfunction(L, functionIndex))
	{
		CCLOG("value at stack [%d] is not function", functionIndex);
		lua_pop(L, numArgs + 1); // remove function and arguments
		return 0;
	}

	int traceback = 0;
	lua_getglobal(L, "__G__TRACKBACK__");                         /* L: ... func arg1 arg2 ... G */
	if (!lua_isfunction(L, -1))
	{
		lua_pop(L, 1);                                            /* L: ... func arg1 arg2 ... */
	}
	else
	{
		lua_insert(L, functionIndex - 1);                         /* L: ... G func arg1 arg2 ... */
		traceback = functionIndex - 1;
	}

	int error = 0;
	error = lua_pcall(L, numArgs, 1, traceback);                  /* L: ... [G] ret */
	if (error)
	{
		if (traceback == 0)
		{
			CCLOG("[LUA ERROR] %s", lua_tostring(L, - 1));        /* L: ... error */
			lua_pop(L, 1); // remove error message from stack
		}
		else                                                            /* L: ... G error */
		{
			lua_pop(L, 2); // remove __G__TRACKBACK__ and error message from stack
		}
		return 0;
	}

	int ret = 0;
	if (removeResult)
	{
		if (lua_isnumber(L, -1))
		{
			ret = lua_tointeger(L, -1);
		}
		else if (lua_isboolean(L, -1))
		{
			ret = lua_toboolean(L, -1);
		}
		// remove return value from stack
		lua_pop(L, 1);                                            /* L: ... [G] */
	}
	else
	{
		ret = 1;
	}

	if (traceback)
	{
		lua_remove(L, removeResult ? -1 : -2);
	}

	return ret;
}

int cc_lua_require(lua_State *L)
{
	lua_pushvalue(L, lua_upvalueindex(1));
	lua_pushvalue(L, lua_upvalueindex(2));
	return execute_lua_function(L, 1, false);
}

int lua_loadChunksFromZip( lua_State *L )
{
	return 1;
}

void ScutExt::Init(const std::string& resRootDir)
{
	CC_ASSERT(!sInstance && "can only initialize ScutExt once");

	sInstance = new ScutExt();

	LuaEngine* pEngine = LuaEngine::getInstance();
	LuaStack *pStack = pEngine->getLuaStack();
	lua_State *tolua_s = pStack->getLuaState();

	// register CCLuaLoadChunksFromZip
	lua_pushcfunction(tolua_s, lua_loadChunksFromZip);
	lua_setglobal(tolua_s, "CCLuaLoadChunksFromZip");

	tolua_Scut_open(tolua_s);
    sResRootDir = resRootDir;
}

ScutExt* ScutExt::getInstance()
{
	return sInstance;
}

void ScutExt::RegisterPauseHandler( const char* pszFuncName )
{
	if (pszFuncName)
	{
		m_strPauseHandler = pszFuncName;
	}
}

void ScutExt::RegisterResumeHandler( const char* pszFuncName )
{
	if (pszFuncName)
	{
		m_strResumeHandler = pszFuncName;
	}
}

void ScutExt::RegisterBackHandler( const char* pszFuncName)
{
	if (!pszFuncName)
	{
		return;
	}

	m_strBackHandler = pszFuncName;
}

bool ScutExt::ExcuteBackHandler()
{
	if (m_strBackHandler.size() > 0)
	{
		return ScriptEngineManager::getInstance()->getScriptEngine()->executeGlobalFunction(m_strBackHandler.c_str());
	}

	return false;
}

void ScutExt::registerSceneChangeCallback( SCENE_CHANGE_CALLBACK fun )
{
	m_pSceneChangeCallback = fun;
}

void ScutExt::RegisterErrorHandler( const char* pszFuncName )
{
	m_strErrorHandler = pszFuncName;
}

std::string& ScutExt::GetErrorHandler()
{
	return m_strErrorHandler;
}

void ScutExt::UnregisterErrorHandler()
{
	m_strErrorHandler = "";
}

void ScutExt::RegisterSocketPushHandler( const char* pszFuncName )
{
	m_strSocketPushHandler = pszFuncName;
}

const char* ScutExt::GetSocketPushHandler()
{
	if (m_strSocketPushHandler.length() > 0)
	{
		return m_strSocketPushHandler.c_str();
	}

	return NULL;
}

void ScutExt::RegisterSocketErrorHandler( const char* pszFuncName )
{
	m_strSocketErrorHandler = pszFuncName;
}

const char* ScutExt::GetSocketErrorHandler()
{
	if (m_strSocketErrorHandler.length() > 0)
	{
		return m_strSocketErrorHandler.c_str();
	}

	return NULL;
}

void ScutExt::PauseDirector()
{
	CCDirector::getInstance()->pause();
	if (m_strPauseHandler.size() > 0)
	{
		ScriptEngineManager::getInstance()->getScriptEngine()->executeGlobalFunction(m_strPauseHandler.c_str());
	}
}

void ScutExt::ResumeDirector()
{
	CCDirector::getInstance()->resume();
	if (m_strResumeHandler.size() > 0)
	{
		ScriptEngineManager::getInstance()->getScriptEngine()->executeGlobalFunction(m_strResumeHandler.c_str());
	}
}

void ScutExt::EndDirector()
{
	CCDirector::getInstance()->end();
}

int nderror_handler(lua_State* L)
{
	lua_Debug debug_info;
	int level = 0;
	std::string err;
	char tmp[10];

	if(lua_gettop(L) > 0 && lua_isstring(L, -1))
	{
		err += lua_tostring(L, -1);
	}

	while(lua_getstack(L, level++, &debug_info))
	{
		lua_getinfo(L, "l", &debug_info);
		lua_getinfo(L, "n", &debug_info);
		lua_getinfo(L, "S", &debug_info);

		err += '\n';
		err += '[';
		err += debug_info.what;
		err += "][";
		err += debug_info.namewhat;
		err += "][";
		//err += _itoa(debug_info.currentline, tmp, 10);
		err += "][";
		if(debug_info.name) err += debug_info.name;
		err += "]@[";
		err += debug_info.source;
		err += ']';
	}

	std::string& err_handler = ScutExt::getInstance()->GetErrorHandler();
	if(err_handler.size() > 0)
	{
		ScutExt::getInstance()->executeLogEvent(err_handler, err);
	}

	return 1;
}

extern "C" int Scutlua_pcall(lua_State *L, int nargs, int nresults)
{
	lua_pushcfunction(L, nderror_handler);
	lua_insert(L, -2-nargs);

	return lua_pcall(L, nargs, nresults, -2-nargs);
}

bool ScutExt::pushfunc(const std::string & strFunc)
{
	LuaEngine* pEngine = LuaEngine::getInstance();
	LuaStack *pStack = pEngine->getLuaStack();
	lua_State *tolua_s = pStack->getLuaState();

	size_t found = strFunc.find('.');
	if (found == std::string::npos)
	{
		found = strFunc.find(':');
	}
	if (found == std::string::npos)
	{
		lua_getglobal(tolua_s, strFunc.c_str());
	}
	else
	{
		std::string strTable = strFunc.substr(0, found);
		std::string strFuncName  = strFunc.substr(found + 1);

		lua_getglobal(tolua_s, strTable.c_str());                        // Get the table instance
		int r_obj = luaL_ref(tolua_s, LUA_REGISTRYINDEX);

		lua_rawgeti(tolua_s, LUA_REGISTRYINDEX, r_obj);   // Get the object instance
		lua_getfield(tolua_s, -1, strFuncName.c_str());    // Get the function
		int r_func = luaL_ref(tolua_s, LUA_REGISTRYINDEX);
		lua_pop(tolua_s, 1);
		lua_rawgeti(tolua_s, LUA_REGISTRYINDEX, r_func);
	}

	// is it a function
	if ( !lua_isfunction(tolua_s,-1) )
	{
		lua_settop( tolua_s, 0 );
		std::string msg = "(CCLuaScriptModule): "+strFunc +" name does not represent a Lua function"+"\n";
		CCLOG("%s  %d", msg.c_str(), __LINE__);
		return false;
	}
	return true;
}

void ScutExt::executeLogEvent( std::string& func, std::string& errlog )
{
	LuaEngine* pEngine = LuaEngine::getInstance();
	LuaStack *pStack = pEngine->getLuaStack();
	lua_State *tolua_s = pStack->getLuaState();

	if(!pushfunc(func))
	{
		return;
	}

	lua_pushstring(tolua_s, errlog.c_str());

	if (lua_pcall(tolua_s, 1, 0, 0) != 0)
	{
		CCLOG("[LUA ERROR] %s", lua_tostring(tolua_s, - 1));
		lua_pop(tolua_s, 1); // clean error message
		return;
	}

	return;
}

const std::string& ScutExt::getResRootDir()
{
    return sResRootDir;
}
