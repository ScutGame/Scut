/*
** Lua binding: ScutAnimation
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:17:09.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutAnimation_open (lua_State* tolua_S);

#include"../ScutAnimationManager.h"
#include"../ScutSprite.h"

/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
#ifndef Mtolua_typeid
#define Mtolua_typeid(L,TI,T)
#endif
 tolua_usertype(tolua_S,"ScutAnimation::CScutAniGroup");
 Mtolua_typeid(tolua_S,typeid(ScutAnimation::CScutAniGroup), "ScutAnimation::CScutAniGroup");
 tolua_usertype(tolua_S,"ScutAnimation::CScutAniData");
 Mtolua_typeid(tolua_S,typeid(ScutAnimation::CScutAniData), "ScutAnimation::CScutAniData");
 tolua_usertype(tolua_S,"ScutAnimation::CCScutSprite");
 Mtolua_typeid(tolua_S,typeid(ScutAnimation::CCScutSprite), "ScutAnimation::CCScutSprite");
 tolua_usertype(tolua_S,"CCNode");
 Mtolua_typeid(tolua_S,typeid(CCNode), "CCNode");
 tolua_usertype(tolua_S,"ScutAnimation::CScutAnimationManager");
 Mtolua_typeid(tolua_S,typeid(ScutAnimation::CScutAnimationManager), "ScutAnimation::CScutAnimationManager");
 tolua_usertype(tolua_S,"ScutAnimation::CScutFrame");
 Mtolua_typeid(tolua_S,typeid(ScutAnimation::CScutFrame), "ScutAnimation::CScutFrame");
}

/* method: node of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_node00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_node00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutAnimation::CScutAniGroup",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CScutAniGroup* aniGroup = ((ScutAnimation::CScutAniGroup*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutAnimation::CCScutSprite* tolua_ret = (ScutAnimation::CCScutSprite*)  ScutAnimation::CCScutSprite::node(aniGroup);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutAnimation::CCScutSprite");
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

/* method: CopyFromSprite of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_CopyFromSprite00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_CopyFromSprite00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CCScutSprite* src = ((ScutAnimation::CCScutSprite*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutAnimation::CCScutSprite* tolua_ret = (ScutAnimation::CCScutSprite*)  ScutAnimation::CCScutSprite::CopyFromSprite(src);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutAnimation::CCScutSprite");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'CopyFromSprite'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setCurAni of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_setCurAni00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_setCurAni00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CCScutSprite* self = (ScutAnimation::CCScutSprite*)  tolua_tousertype(tolua_S,1,0);
  int aniIndex = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setCurAni'", NULL);
#endif
  {
   self->setCurAni(aniIndex);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setCurAni'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerFrameCallback of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_registerFrameCallback00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_registerFrameCallback00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CCScutSprite* self = (ScutAnimation::CCScutSprite*)  tolua_tousertype(tolua_S,1,0);
  const char* pszCallback = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerFrameCallback'", NULL);
#endif
  {
   self->registerFrameCallback(pszCallback);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerFrameCallback'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: play of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_play00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_play00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CCScutSprite* self = (ScutAnimation::CCScutSprite*)  tolua_tousertype(tolua_S,1,0);
  bool bPlay = ((bool)  tolua_toboolean(tolua_S,2,true));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'play'", NULL);
#endif
  {
   self->play(bPlay);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'play'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: replace of class  ScutAnimation::CCScutSprite */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CCScutSprite_replace00
static int tolua_ScutAnimation_ScutAnimation_CCScutSprite_replace00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CCScutSprite",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CCScutSprite* self = (ScutAnimation::CCScutSprite*)  tolua_tousertype(tolua_S,1,0);
  int replaceIndex = ((int)  tolua_tonumber(tolua_S,2,0));
  const char* pszFile = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'replace'", NULL);
#endif
  {
   self->replace(replaceIndex,pszFile);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'replace'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetInstance of class  ScutAnimation::CScutAnimationManager */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_GetInstance00
static int tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_GetInstance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutAnimation::CScutAnimationManager",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutAnimation::CScutAnimationManager& tolua_ret = (ScutAnimation::CScutAnimationManager&)  ScutAnimation::CScutAnimationManager::GetInstance();
    tolua_pushusertype(tolua_S,(void*)&tolua_ret,"ScutAnimation::CScutAnimationManager");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetInstance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: LoadSprite of class  ScutAnimation::CScutAnimationManager */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_LoadSprite00
static int tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_LoadSprite00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CScutAnimationManager",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CScutAnimationManager* self = (ScutAnimation::CScutAnimationManager*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszSprName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'LoadSprite'", NULL);
#endif
  {
   ScutAnimation::CCScutSprite* tolua_ret = (ScutAnimation::CCScutSprite*)  self->LoadSprite(lpszSprName);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutAnimation::CCScutSprite");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'LoadSprite'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: UnLoadSprite of class  ScutAnimation::CScutAnimationManager */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_UnLoadSprite00
static int tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_UnLoadSprite00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CScutAnimationManager",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CScutAnimationManager* self = (ScutAnimation::CScutAnimationManager*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszSprName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'UnLoadSprite'", NULL);
#endif
  {
   self->UnLoadSprite(lpszSprName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'UnLoadSprite'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ReleaseAllAniGroup of class  ScutAnimation::CScutAnimationManager */
#ifndef TOLUA_DISABLE_tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_ReleaseAllAniGroup00
static int tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_ReleaseAllAniGroup00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutAnimation::CScutAnimationManager",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutAnimation::CScutAnimationManager* self = (ScutAnimation::CScutAnimationManager*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ReleaseAllAniGroup'", NULL);
#endif
  {
   self->ReleaseAllAniGroup();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ReleaseAllAniGroup'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_ScutAnimation_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_module(tolua_S,"ScutAnimation",0);
  tolua_beginmodule(tolua_S,"ScutAnimation");
   tolua_cclass(tolua_S,"CScutAniGroup","ScutAnimation::CScutAniGroup","",NULL);
   tolua_beginmodule(tolua_S,"CScutAniGroup");
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CCScutSprite","ScutAnimation::CCScutSprite","CCNode",NULL);
   tolua_beginmodule(tolua_S,"CCScutSprite");
    tolua_function(tolua_S,"node",tolua_ScutAnimation_ScutAnimation_CCScutSprite_node00);
    tolua_function(tolua_S,"CopyFromSprite",tolua_ScutAnimation_ScutAnimation_CCScutSprite_CopyFromSprite00);
    tolua_function(tolua_S,"setCurAni",tolua_ScutAnimation_ScutAnimation_CCScutSprite_setCurAni00);
    tolua_function(tolua_S,"registerFrameCallback",tolua_ScutAnimation_ScutAnimation_CCScutSprite_registerFrameCallback00);
    tolua_function(tolua_S,"play",tolua_ScutAnimation_ScutAnimation_CCScutSprite_play00);
    tolua_function(tolua_S,"replace",tolua_ScutAnimation_ScutAnimation_CCScutSprite_replace00);
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CScutAnimationManager","ScutAnimation::CScutAnimationManager","",NULL);
   tolua_beginmodule(tolua_S,"CScutAnimationManager");
    tolua_function(tolua_S,"GetInstance",tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_GetInstance00);
    tolua_function(tolua_S,"LoadSprite",tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_LoadSprite00);
    tolua_function(tolua_S,"UnLoadSprite",tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_UnLoadSprite00);
    tolua_function(tolua_S,"ReleaseAllAniGroup",tolua_ScutAnimation_ScutAnimation_CScutAnimationManager_ReleaseAllAniGroup00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutAnimation",0);
  tolua_beginmodule(tolua_S,"ScutAnimation");
   tolua_constant(tolua_S,"ANIMATION_START",ScutAnimation::ANIMATION_START);
   tolua_constant(tolua_S,"ANIMATION_PLAYING",ScutAnimation::ANIMATION_PLAYING);
   tolua_constant(tolua_S,"ANIMATION_END",ScutAnimation::ANIMATION_END);
   tolua_constant(tolua_S,"REPLACE_START",ScutAnimation::REPLACE_START);
   tolua_constant(tolua_S,"REPLACE_WEAPON",ScutAnimation::REPLACE_WEAPON);
   tolua_constant(tolua_S,"REPLACE_COUNT",ScutAnimation::REPLACE_COUNT);
   tolua_cclass(tolua_S,"CScutFrame","ScutAnimation::CScutFrame","",NULL);
   tolua_beginmodule(tolua_S,"CScutFrame");
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CScutAniData","ScutAnimation::CScutAniData","",NULL);
   tolua_beginmodule(tolua_S,"CScutAniData");
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutAnimation (lua_State* tolua_S) {
 return tolua_ScutAnimation_open(tolua_S);
};
#endif

