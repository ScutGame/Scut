/*
** Lua binding: ScutScene
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:20:34.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutScene_open (lua_State* tolua_S);

#include "ScutScene.h"

/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutScene (lua_State* tolua_S)
{
 ScutScene* self = (ScutScene*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}
#endif


/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
#ifndef Mtolua_typeid
#define Mtolua_typeid(L,TI,T)
#endif
 tolua_usertype(tolua_S,"LUA_FUNCTION");
 Mtolua_typeid(tolua_S,typeid(LUA_FUNCTION), "LUA_FUNCTION");
 tolua_usertype(tolua_S,"ScutSystem::CStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CStream), "ScutSystem::CStream");
 tolua_usertype(tolua_S,"CCScene");
 Mtolua_typeid(tolua_S,typeid(CCScene), "CCScene");
 tolua_usertype(tolua_S,"ScutScene");
 Mtolua_typeid(tolua_S,typeid(ScutScene), "ScutScene");
}

/* method: new of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_new00
static int tolua_ScutScene_ScutScene_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutScene* tolua_ret = (ScutScene*)  Mtolua_new((ScutScene)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutScene");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'new'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_new00_local
static int tolua_ScutScene_ScutScene_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutScene* tolua_ret = (ScutScene*)  Mtolua_new((ScutScene)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutScene");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'new'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_delete00
static int tolua_ScutScene_ScutScene_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'delete'", NULL);
#endif
  Mtolua_delete(self);
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'delete'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: init of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_init00
static int tolua_ScutScene_ScutScene_init00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'init'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->init();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'init'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: node of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_node00
static int tolua_ScutScene_ScutScene_node00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutScene* tolua_ret = (ScutScene*)  ScutScene::node();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutScene");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'node'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerCallback of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerCallback00
static int tolua_ScutScene_ScutScene_registerCallback00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
  const char* pszCallback = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerCallback'", NULL);
#endif
  {
   self->registerCallback(pszCallback);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerCallback'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: execCallback of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_execCallback00
static int tolua_ScutScene_ScutScene_execCallback00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,4,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,5,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,6,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
  int nTag = ((int)  tolua_tonumber(tolua_S,2,0));
  int nNetState = ((int)  tolua_tonumber(tolua_S,3,0));
  ScutSystem::CStream* lpData = ((ScutSystem::CStream*)  tolua_tousertype(tolua_S,4,0));
  void* lpExternal = ((void*)  tolua_touserdata(tolua_S,5,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'execCallback'", NULL);
#endif
  {
   self->execCallback(nTag,nNetState,lpData,lpExternal);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'execCallback'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerOnExit of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerOnExit00
static int tolua_ScutScene_ScutScene_registerOnExit00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"LUA_FUNCTION",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
  LUA_FUNCTION pszFunc = *((LUA_FUNCTION*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerOnExit'", NULL);
#endif
  {
   self->registerOnExit(pszFunc);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerOnExit'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerOnEnter of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerOnEnter00
static int tolua_ScutScene_ScutScene_registerOnEnter00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutScene",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"LUA_FUNCTION",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutScene* self = (ScutScene*)  tolua_tousertype(tolua_S,1,0);
  LUA_FUNCTION pszFunc = *((LUA_FUNCTION*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerOnEnter'", NULL);
#endif
  {
   self->registerOnEnter(pszFunc);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerOnEnter'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerNetErrorFunc of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerNetErrorFunc00
static int tolua_ScutScene_ScutScene_registerNetErrorFunc00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszCallback = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutScene::registerNetErrorFunc(pszCallback);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerNetErrorFunc'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerNetCommonDataFunc of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerNetCommonDataFunc00
static int tolua_ScutScene_ScutScene_registerNetCommonDataFunc00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszCallback = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutScene::registerNetCommonDataFunc(pszCallback);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerNetCommonDataFunc'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerNetDecodeEnd of class  ScutScene */
#ifndef TOLUA_DISABLE_tolua_ScutScene_ScutScene_registerNetDecodeEnd00
static int tolua_ScutScene_ScutScene_registerNetDecodeEnd00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutScene",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszCallback = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutScene::registerNetDecodeEnd(pszCallback);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerNetDecodeEnd'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_ScutScene_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  #ifdef __cplusplus
  tolua_cclass(tolua_S,"ScutScene","ScutScene","CCScene",tolua_collect_ScutScene);
  #else
  tolua_cclass(tolua_S,"ScutScene","ScutScene","CCScene",NULL);
  #endif
  tolua_beginmodule(tolua_S,"ScutScene");
   tolua_function(tolua_S,"new",tolua_ScutScene_ScutScene_new00);
   tolua_function(tolua_S,"new_local",tolua_ScutScene_ScutScene_new00_local);
   tolua_function(tolua_S,".call",tolua_ScutScene_ScutScene_new00_local);
   tolua_function(tolua_S,"delete",tolua_ScutScene_ScutScene_delete00);
   tolua_function(tolua_S,"init",tolua_ScutScene_ScutScene_init00);
   tolua_function(tolua_S,"node",tolua_ScutScene_ScutScene_node00);
   tolua_function(tolua_S,"registerCallback",tolua_ScutScene_ScutScene_registerCallback00);
   tolua_function(tolua_S,"execCallback",tolua_ScutScene_ScutScene_execCallback00);
   tolua_function(tolua_S,"registerOnExit",tolua_ScutScene_ScutScene_registerOnExit00);
   tolua_function(tolua_S,"registerOnEnter",tolua_ScutScene_ScutScene_registerOnEnter00);
   tolua_function(tolua_S,"registerNetErrorFunc",tolua_ScutScene_ScutScene_registerNetErrorFunc00);
   tolua_function(tolua_S,"registerNetCommonDataFunc",tolua_ScutScene_ScutScene_registerNetCommonDataFunc00);
   tolua_function(tolua_S,"registerNetDecodeEnd",tolua_ScutScene_ScutScene_registerNetDecodeEnd00);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutScene (lua_State* tolua_S) {
 return tolua_ScutScene_open(tolua_S);
};
#endif

