/*
** Lua binding: ScutDataLogic
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:18:27.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutDataLogic_open (lua_State* tolua_S);

#include"../FileHelper.h"
#include"../LuaIni.h"
#include"NetHelper.h"
#include"LuaString.h"
#include"Int64.h"
#include"../FileHelper.h"
#include"../LuaIni.h"
#include"../LuaString.h"
#include"DataRequest.h"
#include "NetStreamExport.h"

/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutDataLogic__CLuaIni (lua_State* tolua_S)
{
 ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CLuaString (lua_State* tolua_S)
{
 ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_DWORD (lua_State* tolua_S)
{
 DWORD* self = (DWORD*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CNetReader (lua_State* tolua_S)
{
 ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CInt64 (lua_State* tolua_S)
{
 ScutDataLogic::CInt64* self = (ScutDataLogic::CInt64*) tolua_tousertype(tolua_S,1,0);
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
 tolua_usertype(tolua_S,"ScutDataLogic::CInt64");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CInt64), "ScutDataLogic::CInt64");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetWriter");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CNetWriter), "ScutDataLogic::CNetWriter");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetReader");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CNetReader), "ScutDataLogic::CNetReader");
 tolua_usertype(tolua_S,"size_t");
 Mtolua_typeid(tolua_S,typeid(size_t), "size_t");
 tolua_usertype(tolua_S,"ScutDataLogic::CLuaIni");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CLuaIni), "ScutDataLogic::CLuaIni");
 tolua_usertype(tolua_S,"INetStatusNotify");
 Mtolua_typeid(tolua_S,typeid(INetStatusNotify), "INetStatusNotify");
 tolua_usertype(tolua_S,"ScutDataLogic::CDataRequest");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CDataRequest), "ScutDataLogic::CDataRequest");
 tolua_usertype(tolua_S,"ScutDataLogic::CLuaString");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CLuaString), "ScutDataLogic::CLuaString");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetStreamExport");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CNetStreamExport), "ScutDataLogic::CNetStreamExport");
 tolua_usertype(tolua_S,"DWORD");
 Mtolua_typeid(tolua_S,typeid(DWORD), "DWORD");
 tolua_usertype(tolua_S,"ScutDataLogic::CFileHelper");
 Mtolua_typeid(tolua_S,typeid(ScutDataLogic::CFileHelper), "ScutDataLogic::CFileHelper");
}

/* method: new of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string strValue = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  Mtolua_new((ScutDataLogic::CLuaString)(strValue));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
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

/* method: new_local of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00_local
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string strValue = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  Mtolua_new((ScutDataLogic::CLuaString)(strValue));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
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

/* method: delete of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
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

/* method: setString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
  const char* szValue = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setString'", NULL);
#endif
  {
   self->setString(szValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setString'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getCString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getCString'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->getCString();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getCString'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getSize of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getSize'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getSize();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  std::string strValue = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  Mtolua_new((ScutDataLogic::CLuaString)(strValue));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01_local
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01_local(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  std::string strValue = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  Mtolua_new((ScutDataLogic::CLuaString)(strValue));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete01
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'delete'", NULL);
#endif
  Mtolua_delete(self);
 }
 return 0;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: setString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString01
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
  const char* szValue = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setString'", NULL);
#endif
  {
   self->setString(szValue);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getCString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString01
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getCString'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->getCString();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getSize of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize01
static int tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getSize'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getSize();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeInt32 of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt3200
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt3200(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nValue = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeInt32'", NULL);
#endif
  {
   self->writeInt32(szKey,nValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeInt32'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeFloat of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeFloat00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeFloat00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  float fvalue = ((float)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeFloat'", NULL);
#endif
  {
   self->writeFloat(szKey,fvalue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeFloat'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeString of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeString00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeString00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* szValue = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeString'", NULL);
#endif
  {
   self->writeString(szKey,szValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeString'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeInt64 of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6400
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6400(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned long long nValue = ((unsigned long long)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeInt64'", NULL);
#endif
  {
   self->writeInt64(szKey,nValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeInt64'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeInt64 of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6401
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6401(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutDataLogic::CInt64* obj = ((ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeInt64'", NULL);
#endif
  {
   self->writeInt64(szKey,*obj);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6400(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeWord of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeWord00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeWord00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned short sValue = ((unsigned short)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeWord'", NULL);
#endif
  {
   self->writeWord(szKey,sValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeWord'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeBuf of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeBuf00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeBuf00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* szKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned char* buf = ((unsigned char*)  tolua_tostring(tolua_S,3,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'writeBuf'", NULL);
#endif
  {
   self->writeBuf(szKey,buf,nSize);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'writeBuf'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setUrl of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUrl00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUrl00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szUrl = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CNetWriter::setUrl(szUrl);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setUrl'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: url_encode of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,4,&tolua_err) || !tolua_isusertype(tolua_S,4,"size_t",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  const char* str = ((const char*)  tolua_tostring(tolua_S,2,0));
  char* dst = ((char*)  tolua_tostring(tolua_S,3,0));
  size_t dst_len = *((size_t*)  tolua_tousertype(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'url_encode'", NULL);
#endif
  {
   self->url_encode(str,dst,dst_len);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'url_encode'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: url_encode of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode01
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,5,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,6,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
  unsigned char* src = ((unsigned char*)  tolua_tostring(tolua_S,2,0));
  int src_len = ((int)  tolua_tonumber(tolua_S,3,0));
  char* dst = ((char*)  tolua_tostring(tolua_S,4,0));
  int dst_len = ((int)  tolua_tonumber(tolua_S,5,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'url_encode'", NULL);
#endif
  {
   int tolua_ret = (int)  self->url_encode(src,src_len,dst,dst_len);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInstance of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_getInstance00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_getInstance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CNetWriter* tolua_ret = (ScutDataLogic::CNetWriter*)  ScutDataLogic::CNetWriter::getInstance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CNetWriter");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getInstance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: generatePostData of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_generatePostData00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_generatePostData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetWriter* self = (ScutDataLogic::CNetWriter*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'generatePostData'", NULL);
#endif
  {
   std::string tolua_ret = (std::string)  self->generatePostData();
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'generatePostData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: resetData of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_resetData00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_resetData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CNetWriter::resetData();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'resetData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setSessionID of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setSessionID00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setSessionID00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszSessionID = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CNetWriter::setSessionID(pszSessionID);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setSessionID'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setUserID of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUserID00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUserID00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CInt64 value = *((ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutDataLogic::CNetWriter::setUserID(value);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setUserID'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setStime of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setStime00
static int tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setStime00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetWriter",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszTime = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CNetWriter::setStime(pszTime);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setStime'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_delete00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
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

/* method: getCInt64 of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getCInt6400
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getCInt6400(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getCInt64'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->getCInt64();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getCInt64'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getString of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getString00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getString00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutDataLogic::CLuaString",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
  ScutDataLogic::CLuaString* pOutString = ((ScutDataLogic::CLuaString*)  tolua_tousertype(tolua_S,2,0));
  int nLength = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getString'", NULL);
#endif
  {
   self->getString(pOutString,nLength);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getString'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getByte of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getByte00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getByte00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getByte'", NULL);
#endif
  {
   unsigned char tolua_ret = (unsigned char)  self->getByte();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getByte'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getWord of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getWord00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getWord00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getWord'", NULL);
#endif
  {
   unsigned short tolua_ret = (unsigned short)  self->getWord();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getWord'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInstance of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getInstance00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getInstance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CNetReader* tolua_ret = (ScutDataLogic::CNetReader*)  ScutDataLogic::CNetReader::getInstance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CNetReader");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getInstance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getResult of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getResult00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getResult00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getResult'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getResult();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getResult'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getRmId of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getRmId00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getRmId00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getRmId'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getRmId();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getRmId'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getActionID of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getActionID00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getActionID00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getActionID'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getActionID();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getActionID'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getErrMsg of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getErrMsg00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getErrMsg00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getErrMsg'", NULL);
#endif
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  self->getErrMsg();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getErrMsg'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getStrStime of class  ScutDataLogic::CNetReader */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetReader_getStrStime00
static int tolua_ScutDataLogic_ScutDataLogic_CNetReader_getStrStime00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetReader",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getStrStime'", NULL);
#endif
  {
   ScutDataLogic::CLuaString* tolua_ret = (ScutDataLogic::CLuaString*)  self->getStrStime();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaString");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getStrStime'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* function: getPath */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_getPath00
static int tolua_ScutDataLogic_getPath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isstring(tolua_S,1,0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szPath = ((const char*)  tolua_tostring(tolua_S,1,0));
  bool bOnly2X = ((bool)  tolua_toboolean(tolua_S,2,false));
  {
   std::string tolua_ret = (std::string)  getPath(szPath,bOnly2X);
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getPath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getPath of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getPath00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getPath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isboolean(tolua_S,3,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szPath = ((const char*)  tolua_tostring(tolua_S,2,0));
  bool bOnly2X = ((bool)  tolua_toboolean(tolua_S,3,false));
  {
   std::string tolua_ret = (std::string)  ScutDataLogic::CFileHelper::getPath(szPath,bOnly2X);
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getPath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setAndroidSDCardDirPath of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szPath = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CFileHelper::setAndroidSDCardDirPath(szPath);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setAndroidSDCardDirPath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getAndroidSDCardDirPath of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutDataLogic::CFileHelper::getAndroidSDCardDirPath();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getAndroidSDCardDirPath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setAndroidResourcePath of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidResourcePath00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidResourcePath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszPath = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CFileHelper::setAndroidResourcePath(pszPath);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setAndroidResourcePath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getFileData of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileData00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* pszMode = ((const char*)  tolua_tostring(tolua_S,3,0));
  unsigned long pSize = ((unsigned long)  tolua_tonumber(tolua_S,4,0));
  {
   unsigned char* tolua_ret = (unsigned char*)  ScutDataLogic::CFileHelper::getFileData(pszFileName,pszMode,&pSize);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
   tolua_pushnumber(tolua_S,(lua_Number)pSize);
  }
 }
 return 2;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getFileData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: freeFileData of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_freeFileData00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_freeFileData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  unsigned char* pFileDataPtr = ((unsigned char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutDataLogic::CFileHelper::freeFileData(pFileDataPtr);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'freeFileData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: executeScriptFile of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_executeScriptFile00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_executeScriptFile00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszFile = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   bool tolua_ret = (bool)  ScutDataLogic::CFileHelper::executeScriptFile(pszFile);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'executeScriptFile'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: encryptPwd of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_encryptPwd00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_encryptPwd00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pPwd = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* key = ((const char*)  tolua_tostring(tolua_S,3,0));
  {
   ScutDataLogic::CLuaString tolua_ret = (ScutDataLogic::CLuaString)  ScutDataLogic::CFileHelper::encryptPwd(pPwd,key);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CLuaString)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CLuaString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CLuaString));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CLuaString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'encryptPwd'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getFileState of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileState00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileState00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   int tolua_ret = (int)  ScutDataLogic::CFileHelper::getFileState(pszFileName);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getFileState'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: createDirs of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDirs00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDirs00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szDir = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   bool tolua_ret = (bool)  ScutDataLogic::CFileHelper::createDirs(szDir);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'createDirs'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: isDirExists of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isDirExists00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isDirExists00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* dir = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   bool tolua_ret = (bool)  ScutDataLogic::CFileHelper::isDirExists(dir);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'isDirExists'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: createDir of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDir00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDir00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* dir = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   bool tolua_ret = (bool)  ScutDataLogic::CFileHelper::createDir(dir);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'createDir'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: isFileExists of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isFileExists00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isFileExists00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szFilePath = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   bool tolua_ret = (bool)  ScutDataLogic::CFileHelper::isFileExists(szFilePath);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'isFileExists'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getWritablePath of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getWritablePath00
static int tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getWritablePath00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CFileHelper",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* szFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   std::string tolua_ret = (std::string)  ScutDataLogic::CFileHelper::getWritablePath(szFileName);
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getWritablePath'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CLuaIni* tolua_ret = (ScutDataLogic::CLuaIni*)  Mtolua_new((ScutDataLogic::CLuaIni)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaIni");
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

/* method: new_local of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00_local
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CLuaIni* tolua_ret = (ScutDataLogic::CLuaIni*)  Mtolua_new((ScutDataLogic::CLuaIni)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CLuaIni");
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

/* method: delete of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_delete00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
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

/* method: Load of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Load00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Load00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* filename = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Load'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->Load(filename);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Load'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: APLoad of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_APLoad00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_APLoad00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* filename = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'APLoad'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->APLoad(filename);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'APLoad'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Save of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Save00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Save00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* filename = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Save'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->Save(filename);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Save'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Get of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Get00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Get00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isstring(tolua_S,4,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* key = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
  const char* def = ((const char*)  tolua_tostring(tolua_S,4,"error"));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Get'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->Get(key,value,def);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Get'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetInt of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_GetInt00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_GetInt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isstring(tolua_S,4,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* key = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
  const char* def = ((const char*)  tolua_tostring(tolua_S,4,"0"));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetInt'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetInt(key,value,def);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetInt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Set of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Set00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Set00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* key = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
  const char* set = ((const char*)  tolua_tostring(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Set'", NULL);
#endif
  {
   self->Set(key,value,set);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Set'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetInt of class  ScutDataLogic::CLuaIni */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CLuaIni_SetInt00
static int tolua_ScutDataLogic_ScutDataLogic_CLuaIni_SetInt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CLuaIni",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*)  tolua_tousertype(tolua_S,1,0);
  const char* key = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
  int nValue = ((int)  tolua_tonumber(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetInt'", NULL);
#endif
  {
   self->SetInt(key,value,nValue);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetInt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Instance of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_Instance00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_Instance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CDataRequest* tolua_ret = (ScutDataLogic::CDataRequest*)  ScutDataLogic::CDataRequest::Instance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CDataRequest");
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

/* method: ExecRequest of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_ExecRequest00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_ExecRequest00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CDataRequest* self = (ScutDataLogic::CDataRequest*)  tolua_tousertype(tolua_S,1,0);
  void* pScene = ((void*)  tolua_touserdata(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ExecRequest'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->ExecRequest(pScene);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ExecRequest'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncExecRequest of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecRequest00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecRequest00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,5,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,6,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CDataRequest* self = (ScutDataLogic::CDataRequest*)  tolua_tousertype(tolua_S,1,0);
  void* pScene = ((void*)  tolua_touserdata(tolua_S,2,0));
  const char* szUrl = ((const char*)  tolua_tostring(tolua_S,3,0));
  int nTag = ((int)  tolua_tonumber(tolua_S,4,0));
  void* lpData = ((void*)  tolua_touserdata(tolua_S,5,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncExecRequest'", NULL);
#endif
  {
   DWORD tolua_ret = (DWORD)  self->AsyncExecRequest(pScene,szUrl,nTag,lpData);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((DWORD)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"DWORD");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(DWORD));
     tolua_pushusertype(tolua_S,tolua_obj,"DWORD");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncExecRequest'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncExecTcpRequest of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,5,0,&tolua_err) ||
     !tolua_isstring(tolua_S,6,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,7,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,8,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CDataRequest* self = (ScutDataLogic::CDataRequest*)  tolua_tousertype(tolua_S,1,0);
  void* pScene = ((void*)  tolua_touserdata(tolua_S,2,0));
  const char* szUrl = ((const char*)  tolua_tostring(tolua_S,3,0));
  int nTag = ((int)  tolua_tonumber(tolua_S,4,0));
  void* lpData = ((void*)  tolua_touserdata(tolua_S,5,0));
  const char* lpSendData = ((const char*)  tolua_tostring(tolua_S,6,0));
  unsigned int nDataLen = ((unsigned int)  tolua_tonumber(tolua_S,7,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncExecTcpRequest'", NULL);
#endif
  {
   DWORD tolua_ret = (DWORD)  self->AsyncExecTcpRequest(pScene,szUrl,nTag,lpData,lpSendData,nDataLen);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((DWORD)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"DWORD");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(DWORD));
     tolua_pushusertype(tolua_S,tolua_obj,"DWORD");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncExecTcpRequest'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: PeekLUAData of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_PeekLUAData00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_PeekLUAData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CDataRequest* self = (ScutDataLogic::CDataRequest*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'PeekLUAData'", NULL);
#endif
  {
   self->PeekLUAData();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'PeekLUAData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: LuaHandlePushDataWithInt of class  ScutDataLogic::CDataRequest */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00
static int tolua_ScutDataLogic_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CDataRequest",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CDataRequest* self = (ScutDataLogic::CDataRequest*)  tolua_tousertype(tolua_S,1,0);
  int p = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'LuaHandlePushDataWithInt'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->LuaHandlePushDataWithInt(p);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'LuaHandlePushDataWithInt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: recordBegin of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordBegin00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordBegin00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'recordBegin'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->recordBegin();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'recordBegin'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: recordEnd of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordEnd00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordEnd00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'recordEnd'", NULL);
#endif
  {
   self->recordEnd();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'recordEnd'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getBYTE of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getBYTE00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getBYTE00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getBYTE'", NULL);
#endif
  {
   unsigned char tolua_ret = (unsigned char)  self->getBYTE();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getBYTE'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getWORD of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getWORD00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getWORD00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getWORD'", NULL);
#endif
  {
   unsigned short tolua_ret = (unsigned short)  self->getWORD();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getWORD'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getDWORD of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDWORD00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDWORD00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getDWORD'", NULL);
#endif
  {
   unsigned int tolua_ret = (unsigned int)  self->getDWORD();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getDWORD'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInt of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getInt'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getInt();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getInt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getFloat of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getFloat00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getFloat00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getFloat'", NULL);
#endif
  {
   float tolua_ret = (float)  self->getFloat();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getFloat'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getDouble of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDouble00
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDouble00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getDouble'", NULL);
#endif
  {
   double tolua_ret = (double)  self->getDouble();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getDouble'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInt64 of class  ScutDataLogic::CNetStreamExport */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt6400
static int tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt6400(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CNetStreamExport",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CNetStreamExport* self = (ScutDataLogic::CNetStreamExport*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getInt64'", NULL);
#endif
  {
   unsigned long long tolua_ret = (unsigned long long)  self->getInt64();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getInt64'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
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

/* method: new_local of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new00_local
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
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

/* method: delete of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_delete00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CInt64* self = (ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
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

/* method: new of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  unsigned long long value = ((unsigned long long)  tolua_tonumber(tolua_S,2,0));
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)(value));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64_new00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new01_local
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new01_local(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  unsigned long long value = ((unsigned long long)  tolua_tonumber(tolua_S,2,0));
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)(value));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64_new00_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new02
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new02(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* other = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)(*other));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64_new01(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_new02_local
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_new02_local(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* other = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutDataLogic::CInt64* tolua_ret = (ScutDataLogic::CInt64*)  Mtolua_new((ScutDataLogic::CInt64)(*other));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64_new01_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator+ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__add00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__add00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  int value = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator+'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator+(value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.add'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator+ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__add01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__add01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator+'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator+(*value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64__add00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator- of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__sub00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__sub00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  int value = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator-'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator-(value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.sub'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator- of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__sub01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__sub01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator-'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator-(*value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64__sub00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator* of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__mul00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__mul00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  int value = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator*'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator*(value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.mul'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator* of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__mul01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__mul01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator*'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator*(*value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64__mul00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator/ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__div00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__div00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  int value = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator/'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator/(value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.div'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator/ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__div01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__div01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator/'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->operator/(*value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64__div00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator== of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__eq00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__eq00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator=='", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->operator==(*value);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.eq'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator<= of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__le00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__le00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator<='", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->operator<=(*value);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.le'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator< of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64__lt00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64__lt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'operator<'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->operator<(*value);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function '.lt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: equal of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_equal00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_equal00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'equal'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->equal(*value);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'equal'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: str of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_str00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_str00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutDataLogic::CInt64* self = (ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'str'", NULL);
#endif
  {
   std::string tolua_ret = (std::string)  self->str();
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'str'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: mod of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_mod00
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_mod00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  int value = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'mod'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->mod(value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'mod'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: mod of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_ScutDataLogic_ScutDataLogic_CInt64_mod01
static int tolua_ScutDataLogic_ScutDataLogic_CInt64_mod01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"const ScutDataLogic::CInt64",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ScutDataLogic::CInt64",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  const ScutDataLogic::CInt64* self = (const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,1,0);
  const ScutDataLogic::CInt64* value = ((const ScutDataLogic::CInt64*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'mod'", NULL);
#endif
  {
   ScutDataLogic::CInt64 tolua_ret = (ScutDataLogic::CInt64)  self->mod(*value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((ScutDataLogic::CInt64)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(ScutDataLogic::CInt64));
     tolua_pushusertype(tolua_S,tolua_obj,"ScutDataLogic::CInt64");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
tolua_lerror:
 return tolua_ScutDataLogic_ScutDataLogic_CInt64_mod00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_ScutDataLogic_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CLuaString","ScutDataLogic::CLuaString","",tolua_collect_ScutDataLogic__CLuaString);
   #else
   tolua_cclass(tolua_S,"CLuaString","ScutDataLogic::CLuaString","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CLuaString");
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete00);
    tolua_function(tolua_S,"setString",tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString00);
    tolua_function(tolua_S,"getCString",tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString00);
    tolua_function(tolua_S,"getSize",tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize00);
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CLuaString_new01_local);
    tolua_function(tolua_S,"delete",tolua_ScutDataLogic_ScutDataLogic_CLuaString_delete01);
    tolua_function(tolua_S,"setString",tolua_ScutDataLogic_ScutDataLogic_CLuaString_setString01);
    tolua_function(tolua_S,"getCString",tolua_ScutDataLogic_ScutDataLogic_CLuaString_getCString01);
    tolua_function(tolua_S,"getSize",tolua_ScutDataLogic_ScutDataLogic_CLuaString_getSize01);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CNetWriter","ScutDataLogic::CNetWriter","",NULL);
   tolua_beginmodule(tolua_S,"CNetWriter");
    tolua_function(tolua_S,"writeInt32",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt3200);
    tolua_function(tolua_S,"writeFloat",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeFloat00);
    tolua_function(tolua_S,"writeString",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeString00);
    tolua_function(tolua_S,"writeInt64",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6400);
    tolua_function(tolua_S,"writeInt64",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeInt6401);
    tolua_function(tolua_S,"writeWord",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeWord00);
    tolua_function(tolua_S,"writeBuf",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_writeBuf00);
    tolua_function(tolua_S,"setUrl",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUrl00);
    tolua_function(tolua_S,"url_encode",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode00);
    tolua_function(tolua_S,"url_encode",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_url_encode01);
    tolua_function(tolua_S,"getInstance",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_getInstance00);
    tolua_function(tolua_S,"generatePostData",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_generatePostData00);
    tolua_function(tolua_S,"resetData",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_resetData00);
    tolua_function(tolua_S,"setSessionID",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setSessionID00);
    tolua_function(tolua_S,"setUserID",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setUserID00);
    tolua_function(tolua_S,"setStime",tolua_ScutDataLogic_ScutDataLogic_CNetWriter_setStime00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CNetReader","ScutDataLogic::CNetReader","ScutDataLogic::CNetStreamExport",tolua_collect_ScutDataLogic__CNetReader);
   #else
   tolua_cclass(tolua_S,"CNetReader","ScutDataLogic::CNetReader","ScutDataLogic::CNetStreamExport",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CNetReader");
    tolua_function(tolua_S,"delete",tolua_ScutDataLogic_ScutDataLogic_CNetReader_delete00);
    tolua_function(tolua_S,"getCInt64",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getCInt6400);
    tolua_function(tolua_S,"getString",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getString00);
    tolua_function(tolua_S,"getByte",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getByte00);
    tolua_function(tolua_S,"getWord",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getWord00);
    tolua_function(tolua_S,"getInstance",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getInstance00);
    tolua_function(tolua_S,"getResult",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getResult00);
    tolua_function(tolua_S,"getRmId",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getRmId00);
    tolua_function(tolua_S,"getActionID",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getActionID00);
    tolua_function(tolua_S,"getErrMsg",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getErrMsg00);
    tolua_function(tolua_S,"getStrStime",tolua_ScutDataLogic_ScutDataLogic_CNetReader_getStrStime00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
  tolua_endmodule(tolua_S);
  tolua_function(tolua_S,"getPath",tolua_ScutDataLogic_getPath00);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CFileHelper","ScutDataLogic::CFileHelper","",NULL);
   tolua_beginmodule(tolua_S,"CFileHelper");
    tolua_function(tolua_S,"getPath",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getPath00);
    tolua_function(tolua_S,"setAndroidSDCardDirPath",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00);
    tolua_function(tolua_S,"getAndroidSDCardDirPath",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00);
    tolua_function(tolua_S,"setAndroidResourcePath",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_setAndroidResourcePath00);
    tolua_function(tolua_S,"getFileData",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileData00);
    tolua_function(tolua_S,"freeFileData",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_freeFileData00);
    tolua_function(tolua_S,"executeScriptFile",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_executeScriptFile00);
    tolua_function(tolua_S,"encryptPwd",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_encryptPwd00);
    tolua_function(tolua_S,"getFileState",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getFileState00);
    tolua_function(tolua_S,"createDirs",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDirs00);
    tolua_function(tolua_S,"isDirExists",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isDirExists00);
    tolua_function(tolua_S,"createDir",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_createDir00);
    tolua_function(tolua_S,"isFileExists",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_isFileExists00);
    tolua_function(tolua_S,"getWritablePath",tolua_ScutDataLogic_ScutDataLogic_CFileHelper_getWritablePath00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CLuaIni","ScutDataLogic::CLuaIni","",tolua_collect_ScutDataLogic__CLuaIni);
   #else
   tolua_cclass(tolua_S,"CLuaIni","ScutDataLogic::CLuaIni","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CLuaIni");
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_delete00);
    tolua_function(tolua_S,"Load",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Load00);
    tolua_function(tolua_S,"APLoad",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_APLoad00);
    tolua_function(tolua_S,"Save",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Save00);
    tolua_function(tolua_S,"Get",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Get00);
    tolua_function(tolua_S,"GetInt",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_GetInt00);
    tolua_function(tolua_S,"Set",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_Set00);
    tolua_function(tolua_S,"SetInt",tolua_ScutDataLogic_ScutDataLogic_CLuaIni_SetInt00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_constant(tolua_S,"reNetFailed",ScutDataLogic::reNetFailed);
   tolua_constant(tolua_S,"reNetTimeOut",ScutDataLogic::reNetTimeOut);
   tolua_cclass(tolua_S,"CDataRequest","ScutDataLogic::CDataRequest","INetStatusNotify",NULL);
   tolua_beginmodule(tolua_S,"CDataRequest");
    tolua_function(tolua_S,"Instance",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_Instance00);
    tolua_function(tolua_S,"ExecRequest",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_ExecRequest00);
    tolua_function(tolua_S,"AsyncExecRequest",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecRequest00);
    tolua_function(tolua_S,"AsyncExecTcpRequest",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00);
    tolua_function(tolua_S,"PeekLUAData",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_PeekLUAData00);
    tolua_function(tolua_S,"LuaHandlePushDataWithInt",tolua_ScutDataLogic_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CNetStreamExport","ScutDataLogic::CNetStreamExport","",NULL);
   tolua_beginmodule(tolua_S,"CNetStreamExport");
    tolua_function(tolua_S,"recordBegin",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordBegin00);
    tolua_function(tolua_S,"recordEnd",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_recordEnd00);
    tolua_function(tolua_S,"getBYTE",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getBYTE00);
    tolua_function(tolua_S,"getWORD",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getWORD00);
    tolua_function(tolua_S,"getDWORD",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDWORD00);
    tolua_function(tolua_S,"getInt",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt00);
    tolua_function(tolua_S,"getFloat",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getFloat00);
    tolua_function(tolua_S,"getDouble",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getDouble00);
    tolua_function(tolua_S,"getInt64",tolua_ScutDataLogic_ScutDataLogic_CNetStreamExport_getInt6400);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CInt64","ScutDataLogic::CInt64","",tolua_collect_ScutDataLogic__CInt64);
   #else
   tolua_cclass(tolua_S,"CInt64","ScutDataLogic::CInt64","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CInt64");
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CInt64_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CInt64_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CInt64_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutDataLogic_ScutDataLogic_CInt64_delete00);
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CInt64_new01);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CInt64_new01_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CInt64_new01_local);
    tolua_function(tolua_S,"new",tolua_ScutDataLogic_ScutDataLogic_CInt64_new02);
    tolua_function(tolua_S,"new_local",tolua_ScutDataLogic_ScutDataLogic_CInt64_new02_local);
    tolua_function(tolua_S,".call",tolua_ScutDataLogic_ScutDataLogic_CInt64_new02_local);
    tolua_function(tolua_S,".add",tolua_ScutDataLogic_ScutDataLogic_CInt64__add00);
    tolua_function(tolua_S,".add",tolua_ScutDataLogic_ScutDataLogic_CInt64__add01);
    tolua_function(tolua_S,".sub",tolua_ScutDataLogic_ScutDataLogic_CInt64__sub00);
    tolua_function(tolua_S,".sub",tolua_ScutDataLogic_ScutDataLogic_CInt64__sub01);
    tolua_function(tolua_S,".mul",tolua_ScutDataLogic_ScutDataLogic_CInt64__mul00);
    tolua_function(tolua_S,".mul",tolua_ScutDataLogic_ScutDataLogic_CInt64__mul01);
    tolua_function(tolua_S,".div",tolua_ScutDataLogic_ScutDataLogic_CInt64__div00);
    tolua_function(tolua_S,".div",tolua_ScutDataLogic_ScutDataLogic_CInt64__div01);
    tolua_function(tolua_S,".eq",tolua_ScutDataLogic_ScutDataLogic_CInt64__eq00);
    tolua_function(tolua_S,".le",tolua_ScutDataLogic_ScutDataLogic_CInt64__le00);
    tolua_function(tolua_S,".lt",tolua_ScutDataLogic_ScutDataLogic_CInt64__lt00);
    tolua_function(tolua_S,"equal",tolua_ScutDataLogic_ScutDataLogic_CInt64_equal00);
    tolua_function(tolua_S,"str",tolua_ScutDataLogic_ScutDataLogic_CInt64_str00);
    tolua_function(tolua_S,"mod",tolua_ScutDataLogic_ScutDataLogic_CInt64_mod00);
    tolua_function(tolua_S,"mod",tolua_ScutDataLogic_ScutDataLogic_CInt64_mod01);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutDataLogic (lua_State* tolua_S) {
 return tolua_ScutDataLogic_open(tolua_S);
};
#endif

