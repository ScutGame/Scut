/*
** Lua binding: SceneGenerator
** Generated automatically by tolua++-1.0.92 on 08/11/11 14:35:22.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_SceneGenerator_open (lua_State* tolua_S);

#include "SceneGenerator.h"

/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
#ifndef Mtolua_typeid
#define Mtolua_typeid(L,TI,T)
#endif
 tolua_usertype(tolua_S,"cocos2d::CCNode");
 Mtolua_typeid(tolua_S,typeid(cocos2d::CCNode), "cocos2d::CCNode");
 tolua_usertype(tolua_S,"CCNode");
 Mtolua_typeid(tolua_S,typeid(CCNode), "CCNode");
 tolua_usertype(tolua_S,"NdCxControl::NdScene");
 Mtolua_typeid(tolua_S,typeid(NdCxControl::NdScene), "NdCxControl::NdScene");
 tolua_usertype(tolua_S,"NdCxControl::CSceneGenerator");
 Mtolua_typeid(tolua_S,typeid(NdCxControl::CSceneGenerator), "NdCxControl::CSceneGenerator");
}

/* method: Instance of class  NdCxControl::CSceneGenerator */
#ifndef TOLUA_DISABLE_tolua_SceneGenerator_NdCxControl_CSceneGenerator_Instance00
static int tolua_SceneGenerator_NdCxControl_CSceneGenerator_Instance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"NdCxControl::CSceneGenerator",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   NdCxControl::CSceneGenerator* tolua_ret = (NdCxControl::CSceneGenerator*)  NdCxControl::CSceneGenerator::Instance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"NdCxControl::CSceneGenerator");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Instance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AcquireScene of class  NdCxControl::CSceneGenerator */
#ifndef TOLUA_DISABLE_tolua_SceneGenerator_NdCxControl_CSceneGenerator_AcquireScene00
static int tolua_SceneGenerator_NdCxControl_CSceneGenerator_AcquireScene00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"NdCxControl::CSceneGenerator",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  NdCxControl::CSceneGenerator* self = (NdCxControl::CSceneGenerator*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszSceneName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AcquireScene'", NULL);
#endif
  {
   NdCxControl::NdScene* tolua_ret = (NdCxControl::NdScene*)  self->AcquireScene(lpszSceneName);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"NdCxControl::NdScene");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AcquireScene'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetChildByName of class  NdCxControl::CSceneGenerator */
#ifndef TOLUA_DISABLE_tolua_SceneGenerator_NdCxControl_CSceneGenerator_GetChildByName00
static int tolua_SceneGenerator_NdCxControl_CSceneGenerator_GetChildByName00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"NdCxControl::CSceneGenerator",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCNode",0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  NdCxControl::CSceneGenerator* self = (NdCxControl::CSceneGenerator*)  tolua_tousertype(tolua_S,1,0);
  CCNode* pParent = ((CCNode*)  tolua_tousertype(tolua_S,2,0));
  const char* lpszChildName = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetChildByName'", NULL);
#endif
  {
   cocos2d::CCNode* tolua_ret = (cocos2d::CCNode*)  self->GetChildByName(pParent,lpszChildName);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"cocos2d::CCNode");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetChildByName'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_SceneGenerator_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_module(tolua_S,"NdCxControl",0);
  tolua_beginmodule(tolua_S,"NdCxControl");
   tolua_cclass(tolua_S,"CSceneGenerator","NdCxControl::CSceneGenerator","",NULL);
   tolua_beginmodule(tolua_S,"CSceneGenerator");
    tolua_function(tolua_S,"Instance",tolua_SceneGenerator_NdCxControl_CSceneGenerator_Instance00);
    tolua_function(tolua_S,"AcquireScene",tolua_SceneGenerator_NdCxControl_CSceneGenerator_AcquireScene00);
    tolua_function(tolua_S,"GetChildByName",tolua_SceneGenerator_NdCxControl_CSceneGenerator_GetChildByName00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_SceneGenerator (lua_State* tolua_S) {
 return tolua_SceneGenerator_open(tolua_S);
};
#endif

