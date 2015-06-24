/*
** Lua binding: Scut
** Generated automatically by tolua++-1.0.92 on 12/18/13 22:27:25.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_Scut_open (lua_State* tolua_S);

#include "ScutExt.h"
#include <string>
#include "ScutUtility/ScutLocale.h"
#include "ScutUtility/ScutUtils.h"
#include "ScutUtility/ScutLuaLan.h"
#include "ScutUtility/ScutLanFactory.h"
#include "ScutSystem/md5.h"
#include "ScutSystem/ScutUtility.h"
#include "ScutSystem/Stream.h"
#include "ScutNetwork/NetClientBase.h"
#include "ScutNetwork/HttpClient.h"
#include "ScutNetwork/TcpClient.h"
#include "ScutNetwork/HttpClientResponse.h"
#include "ScutNetwork/HttpSession.h"
#include "ScutDataLogic/FileHelper.h"
#include "ScutDataLogic/LuaIni.h"
#include "ScutDataLogic/NetHelper.h"
#include "ScutDataLogic/LuaString.h"
#include "ScutDataLogic/Int64.h"
//#include"../FileHelper.h"
//#include"../LuaIni.h"
//#include"../LuaString.h"
#include"DataRequest.h"
#include "NetStreamExport.h"
#include "ScutSystem/Defines.h"

/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutNetwork__AsyncInfo (lua_State* tolua_S)
{
 ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CMemoryStream (lua_State* tolua_S)
{
 ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpClientResponse (lua_State* tolua_S)
{
 ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_DWORD (lua_State* tolua_S)
{
 DWORD* self = (DWORD*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpClient (lua_State* tolua_S)
{
 ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CFileStream (lua_State* tolua_S)
{
 ScutSystem::CFileStream* self = (ScutSystem::CFileStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CStream (lua_State* tolua_S)
{
 ScutSystem::CStream* self = (ScutSystem::CStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CTcpClient (lua_State* tolua_S)
{
 ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CInt64 (lua_State* tolua_S)
{
 ScutDataLogic::CInt64* self = (ScutDataLogic::CInt64*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_BOOL (lua_State* tolua_S)
{
 BOOL* self = (BOOL*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CNetReader (lua_State* tolua_S)
{
 ScutDataLogic::CNetReader* self = (ScutDataLogic::CNetReader*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CBaseMemoryStream (lua_State* tolua_S)
{
 ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CLuaIni (lua_State* tolua_S)
{
 ScutDataLogic::CLuaIni* self = (ScutDataLogic::CLuaIni*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_intptr_t (lua_State* tolua_S)
{
 intptr_t* self = (intptr_t*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_CScutString (lua_State* tolua_S)
{
 CScutString* self = (CScutString*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutDataLogic__CLuaString (lua_State* tolua_S)
{
 ScutDataLogic::CLuaString* self = (ScutDataLogic::CLuaString*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CHandleStream (lua_State* tolua_S)
{
 ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpSession (lua_State* tolua_S)
{
 ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_md5 (lua_State* tolua_S)
{
 md5* self = (md5*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}
#endif


/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
 tolua_usertype(tolua_S,"ScutSystem::CMemoryStream");
 tolua_usertype(tolua_S,"ScutExt");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetStreamExport");
 tolua_usertype(tolua_S,"ScutUtility::CScutLan");
 tolua_usertype(tolua_S,"ScutDataLogic::CDataRequest");
 tolua_usertype(tolua_S,"ScutSystem::CFileStream");
 tolua_usertype(tolua_S,"ScutSystem::CStream");
 tolua_usertype(tolua_S,"BOOL");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetWriter");
 tolua_usertype(tolua_S,"ScutDataLogic::CFileHelper");
 tolua_usertype(tolua_S,"size_t");
 tolua_usertype(tolua_S,"ScutNetwork::INetStatusNotify");
 tolua_usertype(tolua_S,"ScutDataLogic::CLuaIni");
 tolua_usertype(tolua_S,"intptr_t");
 tolua_usertype(tolua_S,"md5");
 tolua_usertype(tolua_S,"ScutDataLogic::CLuaString");
 tolua_usertype(tolua_S,"ScutUtility::ScutUtils");
 tolua_usertype(tolua_S,"ScutNetwork::AsyncInfo");
 tolua_usertype(tolua_S,"ScutNetwork::CNetClientBase");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpClientResponse");
 tolua_usertype(tolua_S,"ScutUtility::CScutLuaLan");
 tolua_usertype(tolua_S,"DWORD");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpClient");
 tolua_usertype(tolua_S,"ScutUtility::CLocale");
 tolua_usertype(tolua_S,"ScutSystem::CScutUtility");
 tolua_usertype(tolua_S,"ScutDataLogic::CInt64");
 tolua_usertype(tolua_S,"ScutUtility::CScutLanFactory");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpSession");
 tolua_usertype(tolua_S,"ScutSystem::CBaseMemoryStream");
 tolua_usertype(tolua_S,"INetStatusNotify");
 tolua_usertype(tolua_S,"CScutString");
 tolua_usertype(tolua_S,"ScutSystem::CHandleStream");
 tolua_usertype(tolua_S,"CURL");
 tolua_usertype(tolua_S,"ScutNetwork::CTcpClient");
 tolua_usertype(tolua_S,"ScutDataLogic::CNetReader");
}

/* method: new of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_new00
static int tolua_Scut_ScutDataLogic_CLuaString_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_new00_local
static int tolua_Scut_ScutDataLogic_CLuaString_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_delete00
static int tolua_Scut_ScutDataLogic_CLuaString_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_setString00
static int tolua_Scut_ScutDataLogic_CLuaString_setString00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_getCString00
static int tolua_Scut_ScutDataLogic_CLuaString_getCString00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_getSize00
static int tolua_Scut_ScutDataLogic_CLuaString_getSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_new01
static int tolua_Scut_ScutDataLogic_CLuaString_new01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_new00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_new01_local
static int tolua_Scut_ScutDataLogic_CLuaString_new01_local(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_new00_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_delete01
static int tolua_Scut_ScutDataLogic_CLuaString_delete01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_delete00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: setString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_setString01
static int tolua_Scut_ScutDataLogic_CLuaString_setString01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_setString00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getCString of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_getCString01
static int tolua_Scut_ScutDataLogic_CLuaString_getCString01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_getCString00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getSize of class  ScutDataLogic::CLuaString */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaString_getSize01
static int tolua_Scut_ScutDataLogic_CLuaString_getSize01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CLuaString_getSize00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeInt32 of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeInt3200
static int tolua_Scut_ScutDataLogic_CNetWriter_writeInt3200(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeFloat00
static int tolua_Scut_ScutDataLogic_CNetWriter_writeFloat00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeString00
static int tolua_Scut_ScutDataLogic_CNetWriter_writeString00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeInt6400
static int tolua_Scut_ScutDataLogic_CNetWriter_writeInt6400(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeInt6401
static int tolua_Scut_ScutDataLogic_CNetWriter_writeInt6401(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CNetWriter_writeInt6400(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: writeWord of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeWord00
static int tolua_Scut_ScutDataLogic_CNetWriter_writeWord00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_writeBuf00
static int tolua_Scut_ScutDataLogic_CNetWriter_writeBuf00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_setUrl00
static int tolua_Scut_ScutDataLogic_CNetWriter_setUrl00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_url_encode00
static int tolua_Scut_ScutDataLogic_CNetWriter_url_encode00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_url_encode01
static int tolua_Scut_ScutDataLogic_CNetWriter_url_encode01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CNetWriter_url_encode00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInstance of class  ScutDataLogic::CNetWriter */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_getInstance00
static int tolua_Scut_ScutDataLogic_CNetWriter_getInstance00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_generatePostData00
static int tolua_Scut_ScutDataLogic_CNetWriter_generatePostData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_resetData00
static int tolua_Scut_ScutDataLogic_CNetWriter_resetData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_setSessionID00
static int tolua_Scut_ScutDataLogic_CNetWriter_setSessionID00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_setUserID00
static int tolua_Scut_ScutDataLogic_CNetWriter_setUserID00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetWriter_setStime00
static int tolua_Scut_ScutDataLogic_CNetWriter_setStime00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_delete00
static int tolua_Scut_ScutDataLogic_CNetReader_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getCInt6400
static int tolua_Scut_ScutDataLogic_CNetReader_getCInt6400(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getString00
static int tolua_Scut_ScutDataLogic_CNetReader_getString00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getByte00
static int tolua_Scut_ScutDataLogic_CNetReader_getByte00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getWord00
static int tolua_Scut_ScutDataLogic_CNetReader_getWord00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getInstance00
static int tolua_Scut_ScutDataLogic_CNetReader_getInstance00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getResult00
static int tolua_Scut_ScutDataLogic_CNetReader_getResult00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getRmId00
static int tolua_Scut_ScutDataLogic_CNetReader_getRmId00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getActionID00
static int tolua_Scut_ScutDataLogic_CNetReader_getActionID00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getErrMsg00
static int tolua_Scut_ScutDataLogic_CNetReader_getErrMsg00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetReader_getStrStime00
static int tolua_Scut_ScutDataLogic_CNetReader_getStrStime00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_getPath00
static int tolua_Scut_getPath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_getPath00
static int tolua_Scut_ScutDataLogic_CFileHelper_getPath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00
static int tolua_Scut_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00
static int tolua_Scut_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_setAndroidResourcePath00
static int tolua_Scut_ScutDataLogic_CFileHelper_setAndroidResourcePath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_getFileData00
static int tolua_Scut_ScutDataLogic_CFileHelper_getFileData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_freeFileData00
static int tolua_Scut_ScutDataLogic_CFileHelper_freeFileData00(lua_State* tolua_S)
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

/* method: encryptPwd of class  ScutDataLogic::CFileHelper */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_encryptPwd00
static int tolua_Scut_ScutDataLogic_CFileHelper_encryptPwd00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_getFileState00
static int tolua_Scut_ScutDataLogic_CFileHelper_getFileState00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_createDirs00
static int tolua_Scut_ScutDataLogic_CFileHelper_createDirs00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_isDirExists00
static int tolua_Scut_ScutDataLogic_CFileHelper_isDirExists00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_createDir00
static int tolua_Scut_ScutDataLogic_CFileHelper_createDir00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_isFileExists00
static int tolua_Scut_ScutDataLogic_CFileHelper_isFileExists00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CFileHelper_getWritablePath00
static int tolua_Scut_ScutDataLogic_CFileHelper_getWritablePath00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_new00
static int tolua_Scut_ScutDataLogic_CLuaIni_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_new00_local
static int tolua_Scut_ScutDataLogic_CLuaIni_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_delete00
static int tolua_Scut_ScutDataLogic_CLuaIni_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_Load00
static int tolua_Scut_ScutDataLogic_CLuaIni_Load00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_APLoad00
static int tolua_Scut_ScutDataLogic_CLuaIni_APLoad00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_Save00
static int tolua_Scut_ScutDataLogic_CLuaIni_Save00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_Get00
static int tolua_Scut_ScutDataLogic_CLuaIni_Get00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_GetInt00
static int tolua_Scut_ScutDataLogic_CLuaIni_GetInt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_Set00
static int tolua_Scut_ScutDataLogic_CLuaIni_Set00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CLuaIni_SetInt00
static int tolua_Scut_ScutDataLogic_CLuaIni_SetInt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_Instance00
static int tolua_Scut_ScutDataLogic_CDataRequest_Instance00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_ExecRequest00
static int tolua_Scut_ScutDataLogic_CDataRequest_ExecRequest00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecRequest00
static int tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecRequest00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00
static int tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_PeekLUAData00
static int tolua_Scut_ScutDataLogic_CDataRequest_PeekLUAData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00
static int tolua_Scut_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_recordBegin00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_recordBegin00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_recordEnd00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_recordEnd00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getBYTE00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getBYTE00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getWORD00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getWORD00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getDWORD00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getDWORD00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getInt00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getInt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getFloat00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getFloat00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getDouble00
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getDouble00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CNetStreamExport_getInt6400
static int tolua_Scut_ScutDataLogic_CNetStreamExport_getInt6400(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new00
static int tolua_Scut_ScutDataLogic_CInt64_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new00_local
static int tolua_Scut_ScutDataLogic_CInt64_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_delete00
static int tolua_Scut_ScutDataLogic_CInt64_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new01
static int tolua_Scut_ScutDataLogic_CInt64_new01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64_new00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new01_local
static int tolua_Scut_ScutDataLogic_CInt64_new01_local(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64_new00_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new02
static int tolua_Scut_ScutDataLogic_CInt64_new02(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64_new01(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: new_local of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_new02_local
static int tolua_Scut_ScutDataLogic_CInt64_new02_local(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64_new01_local(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator+ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__add00
static int tolua_Scut_ScutDataLogic_CInt64__add00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__add01
static int tolua_Scut_ScutDataLogic_CInt64__add01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64__add00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator- of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__sub00
static int tolua_Scut_ScutDataLogic_CInt64__sub00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__sub01
static int tolua_Scut_ScutDataLogic_CInt64__sub01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64__sub00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator* of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__mul00
static int tolua_Scut_ScutDataLogic_CInt64__mul00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__mul01
static int tolua_Scut_ScutDataLogic_CInt64__mul01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64__mul00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator/ of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__div00
static int tolua_Scut_ScutDataLogic_CInt64__div00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__div01
static int tolua_Scut_ScutDataLogic_CInt64__div01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64__div00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: operator== of class  ScutDataLogic::CInt64 */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__eq00
static int tolua_Scut_ScutDataLogic_CInt64__eq00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__le00
static int tolua_Scut_ScutDataLogic_CInt64__le00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64__lt00
static int tolua_Scut_ScutDataLogic_CInt64__lt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_equal00
static int tolua_Scut_ScutDataLogic_CInt64_equal00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_str00
static int tolua_Scut_ScutDataLogic_CInt64_str00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_mod00
static int tolua_Scut_ScutDataLogic_CInt64_mod00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_Scut_ScutDataLogic_CInt64_mod01
static int tolua_Scut_ScutDataLogic_CInt64_mod01(lua_State* tolua_S)
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
 return tolua_Scut_ScutDataLogic_CInt64_mod00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Sender of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Sender_ptr
static int tolua_get_ScutNetwork__AsyncInfo_Sender_ptr(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Sender'",NULL);
#endif
   tolua_pushusertype(tolua_S,(void*)self->Sender,"ScutNetwork::CNetClientBase");
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Sender of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Sender_ptr
static int tolua_set_ScutNetwork__AsyncInfo_Sender_ptr(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Sender'",NULL);
  if (!tolua_isusertype(tolua_S,2,"ScutNetwork::CNetClientBase",0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Sender = ((ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Response of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Response_ptr
static int tolua_get_ScutNetwork__AsyncInfo_Response_ptr(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Response'",NULL);
#endif
   tolua_pushusertype(tolua_S,(void*)self->Response,"ScutNetwork::CHttpClientResponse");
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Response of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Response_ptr
static int tolua_set_ScutNetwork__AsyncInfo_Response_ptr(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Response'",NULL);
  if (!tolua_isusertype(tolua_S,2,"ScutNetwork::CHttpClientResponse",0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Response = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Url of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Url
static int tolua_get_ScutNetwork__AsyncInfo_Url(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Url'",NULL);
#endif
   tolua_pushusertype(tolua_S,(void*)&self->Url,"CScutString");
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Url of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Url
static int tolua_set_ScutNetwork__AsyncInfo_Url(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Url'",NULL);
  if ((tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"CScutString",0,&tolua_err)))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Url = *((CScutString*)  tolua_tousertype(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: PostData of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_PostData
static int tolua_get_ScutNetwork__AsyncInfo_PostData(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'PostData'",NULL);
#endif
  tolua_pushuserdata(tolua_S,(void*)self->PostData);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: PostDataSize of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_PostDataSize
static int tolua_get_ScutNetwork__AsyncInfo_PostDataSize(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'PostDataSize'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->PostDataSize);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: PostDataSize of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_PostDataSize
static int tolua_set_ScutNetwork__AsyncInfo_PostDataSize(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'PostDataSize'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->PostDataSize = ((int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: FormFlag of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_FormFlag
static int tolua_get_ScutNetwork__AsyncInfo_FormFlag(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'FormFlag'",NULL);
#endif
  tolua_pushboolean(tolua_S,(bool)self->FormFlag);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: FormFlag of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_FormFlag
static int tolua_set_ScutNetwork__AsyncInfo_FormFlag(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'FormFlag'",NULL);
  if (!tolua_isboolean(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->FormFlag = ((bool)  tolua_toboolean(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Status of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Status
static int tolua_get_ScutNetwork__AsyncInfo_Status(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Status'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->Status);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Status of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Status
static int tolua_set_ScutNetwork__AsyncInfo_Status(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Status'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Status = ((ScutNetwork::EAsyncInfoStatus) (int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Mode of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Mode
static int tolua_get_ScutNetwork__AsyncInfo_Mode(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Mode'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->Mode);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Mode of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Mode
static int tolua_set_ScutNetwork__AsyncInfo_Mode(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Mode'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Mode = ((ScutNetwork::EAsyncMode) (int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: ProtocalType of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_ProtocalType
static int tolua_get_ScutNetwork__AsyncInfo_ProtocalType(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'ProtocalType'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->ProtocalType);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: ProtocalType of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_ProtocalType
static int tolua_set_ScutNetwork__AsyncInfo_ProtocalType(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'ProtocalType'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->ProtocalType = ((int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: pScene of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_pScene
static int tolua_get_ScutNetwork__AsyncInfo_pScene(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'pScene'",NULL);
#endif
  tolua_pushuserdata(tolua_S,(void*)self->pScene);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: pScene of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_pScene
static int tolua_set_ScutNetwork__AsyncInfo_pScene(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'pScene'",NULL);
  if (!tolua_isuserdata(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->pScene = ((void*)  tolua_touserdata(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Data1 of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Data1
static int tolua_get_ScutNetwork__AsyncInfo_Data1(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Data1'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->Data1);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Data1 of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Data1
static int tolua_set_ScutNetwork__AsyncInfo_Data1(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Data1'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Data1 = ((int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: Data2 of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__AsyncInfo_Data2
static int tolua_get_ScutNetwork__AsyncInfo_Data2(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Data2'",NULL);
#endif
  tolua_pushnumber(tolua_S,(lua_Number)self->Data2);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: Data2 of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__AsyncInfo_Data2
static int tolua_set_ScutNetwork__AsyncInfo_Data2(lua_State* tolua_S)
{
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'Data2'",NULL);
  if (!tolua_isnumber(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->Data2 = ((int)  tolua_tonumber(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_AsyncInfo_new00
static int tolua_Scut_ScutNetwork_AsyncInfo_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::AsyncInfo",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::AsyncInfo* tolua_ret = (ScutNetwork::AsyncInfo*)  Mtolua_new((ScutNetwork::AsyncInfo)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::AsyncInfo");
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

/* method: new_local of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_AsyncInfo_new00_local
static int tolua_Scut_ScutNetwork_AsyncInfo_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::AsyncInfo",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::AsyncInfo* tolua_ret = (ScutNetwork::AsyncInfo*)  Mtolua_new((ScutNetwork::AsyncInfo)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::AsyncInfo");
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

/* method: delete of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_AsyncInfo_delete00
static int tolua_Scut_ScutNetwork_AsyncInfo_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::AsyncInfo",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
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

/* method: Reset of class  ScutNetwork::AsyncInfo */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_AsyncInfo_Reset00
static int tolua_Scut_ScutNetwork_AsyncInfo_Reset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::AsyncInfo",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Reset'", NULL);
#endif
  {
   self->Reset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Reset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncNetGet of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_AsyncNetGet00
static int tolua_Scut_ScutNetwork_CNetClientBase_AsyncNetGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncNetGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncNetGet(url,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncNetGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetNetStautsNotify of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_SetNetStautsNotify00
static int tolua_Scut_ScutNetwork_CNetClientBase_SetNetStautsNotify00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutNetwork::INetStatusNotify",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  ScutNetwork::INetStatusNotify* pNetNotify = ((ScutNetwork::INetStatusNotify*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetNetStautsNotify'", NULL);
#endif
  {
   self->SetNetStautsNotify(pNetNotify);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetNetStautsNotify'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetNetStautsNotify of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetNetStautsNotify00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetNetStautsNotify00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetNetStautsNotify'", NULL);
#endif
  {
   ScutNetwork::INetStatusNotify* tolua_ret = (ScutNetwork::INetStatusNotify*)  self->GetNetStautsNotify();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::INetStatusNotify");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetNetStautsNotify'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AddHeader of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_AddHeader00
static int tolua_Scut_ScutNetwork_CNetClientBase_AddHeader00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  const char* name = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AddHeader'", NULL);
#endif
  {
   BOOL tolua_ret = (BOOL)  self->AddHeader(name,value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((BOOL)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(BOOL));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AddHeader'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetTimeOut of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetTimeOut00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetTimeOut00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetTimeOut'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetTimeOut();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetTimeOut'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetTimeOut of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_SetTimeOut00
static int tolua_Scut_ScutNetwork_CNetClientBase_SetTimeOut00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  int nTimeOut = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetTimeOut'", NULL);
#endif
  {
   self->SetTimeOut(nTimeOut);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetTimeOut'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetUseProgressReport of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetUseProgressReport00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetUseProgressReport00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetUseProgressReport'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->GetUseProgressReport();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetUseProgressReport'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetUseProgressReport of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_SetUseProgressReport00
static int tolua_Scut_ScutNetwork_CNetClientBase_SetUseProgressReport00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  bool bReport = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetUseProgressReport'", NULL);
#endif
  {
   self->SetUseProgressReport(bReport);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetUseProgressReport'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: FullReset of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_FullReset00
static int tolua_Scut_ScutNetwork_CNetClientBase_FullReset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'FullReset'", NULL);
#endif
  {
   self->FullReset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'FullReset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Reset of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_Reset00
static int tolua_Scut_ScutNetwork_CNetClientBase_Reset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Reset'", NULL);
#endif
  {
   self->Reset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Reset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetHost of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetHost00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetHost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetHost'", NULL);
#endif
  {
   CScutString tolua_ret = (CScutString)  self->GetHost();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((CScutString)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(CScutString));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetHost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: IsBusy of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_IsBusy00
static int tolua_Scut_ScutNetwork_CNetClientBase_IsBusy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'IsBusy'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->IsBusy();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'IsBusy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetAsyncInfo of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetAsyncInfo00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetAsyncInfo00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetAsyncInfo'", NULL);
#endif
  {
   ScutNetwork::AsyncInfo* tolua_ret = (ScutNetwork::AsyncInfo*)  self->GetAsyncInfo();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::AsyncInfo");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetAsyncInfo'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetCurlHandle of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_GetCurlHandle00
static int tolua_Scut_ScutNetwork_CNetClientBase_GetCurlHandle00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetCurlHandle'", NULL);
#endif
  {
   CURL* tolua_ret = (CURL*)  self->GetCurlHandle();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"CURL");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetCurlHandle'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterCustomLuaHandle of class  ScutNetwork::CNetClientBase */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00
static int tolua_Scut_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CNetClientBase",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CNetClientBase* self = (ScutNetwork::CNetClientBase*)  tolua_tousertype(tolua_S,1,0);
  const char* szHandle = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterCustomLuaHandle'", NULL);
#endif
  {
   self->RegisterCustomLuaHandle(szHandle);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterCustomLuaHandle'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_new00
static int tolua_Scut_ScutNetwork_CHttpClient_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* s = ((ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutNetwork::CHttpClient* tolua_ret = (ScutNetwork::CHttpClient*)  Mtolua_new((ScutNetwork::CHttpClient)(s));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpClient");
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

/* method: new_local of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_new00_local
static int tolua_Scut_ScutNetwork_CHttpClient_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* s = ((ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutNetwork::CHttpClient* tolua_ret = (ScutNetwork::CHttpClient*)  Mtolua_new((ScutNetwork::CHttpClient)(s));
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpClient");
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

/* method: delete of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_delete00
static int tolua_Scut_ScutNetwork_CHttpClient_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
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

/* method: HttpGet of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_HttpGet00
static int tolua_Scut_ScutNetwork_CHttpClient_HttpGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'HttpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->HttpGet(url,*resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'HttpGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: HttpPost of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_HttpPost00
static int tolua_Scut_ScutNetwork_CHttpClient_HttpPost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,5,&tolua_err) || !tolua_isusertype(tolua_S,5,"ScutNetwork::CHttpClientResponse",0,&tolua_err)) ||
     !tolua_isboolean(tolua_S,6,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,7,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  const void* postData = ((const void*)  tolua_touserdata(tolua_S,3,0));
  int nPostDataSize = ((int)  tolua_tonumber(tolua_S,4,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,5,0));
  bool formflag = ((bool)  tolua_toboolean(tolua_S,6,false));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'HttpPost'", NULL);
#endif
  {
   BOOL tolua_ret = (BOOL)  self->HttpPost(url,postData,nPostDataSize,*resp,formflag);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((BOOL)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(BOOL));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'HttpPost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncHttpGet of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpGet00
static int tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncHttpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncHttpGet(url,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncHttpGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncHttpPost of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpPost00
static int tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpPost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isuserdata(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,5,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,6,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,7,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  const void* postData = ((const void*)  tolua_touserdata(tolua_S,3,0));
  int nPostDataSize = ((int)  tolua_tonumber(tolua_S,4,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,5,0));
  bool bFormFlag = ((bool)  tolua_toboolean(tolua_S,6,false));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncHttpPost'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncHttpPost(url,postData,nPostDataSize,resp,bFormFlag);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncHttpPost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncNetGet of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_AsyncNetGet00
static int tolua_Scut_ScutNetwork_CHttpClient_AsyncNetGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncNetGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncNetGet(url,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncNetGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetNetType of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_GetNetType00
static int tolua_Scut_ScutNetwork_CHttpClient_GetNetType00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetNetType'", NULL);
#endif
  {
   ScutNetwork::ENetType tolua_ret = (ScutNetwork::ENetType)  self->GetNetType();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetNetType'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AddHeader of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_AddHeader00
static int tolua_Scut_ScutNetwork_CHttpClient_AddHeader00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* name = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* value = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AddHeader'", NULL);
#endif
  {
   BOOL tolua_ret = (BOOL)  self->AddHeader(name,value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((BOOL)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(BOOL));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AddHeader'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: UseHttpProxy of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_UseHttpProxy00
static int tolua_Scut_ScutNetwork_CHttpClient_UseHttpProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"BOOL",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  BOOL bUseProxy = *((BOOL*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'UseHttpProxy'", NULL);
#endif
  {
   self->UseHttpProxy(bUseProxy);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'UseHttpProxy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetHttpProxy of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_SetHttpProxy00
static int tolua_Scut_ScutNetwork_CHttpClient_SetHttpProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* proxyHost = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned int proxyPort = ((unsigned int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetHttpProxy'", NULL);
#endif
  {
   self->SetHttpProxy(proxyHost,proxyPort);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetHttpProxy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: UseHttpsProxy of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_UseHttpsProxy00
static int tolua_Scut_ScutNetwork_CHttpClient_UseHttpsProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"BOOL",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  BOOL bUseProxy = *((BOOL*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'UseHttpsProxy'", NULL);
#endif
  {
   self->UseHttpsProxy(bUseProxy);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'UseHttpsProxy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetHttpsProxy of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_SetHttpsProxy00
static int tolua_Scut_ScutNetwork_CHttpClient_SetHttpsProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* proxyHost = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned int proxyPort = ((unsigned int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetHttpsProxy'", NULL);
#endif
  {
   self->SetHttpsProxy(proxyHost,proxyPort);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetHttpsProxy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: FullReset of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_FullReset00
static int tolua_Scut_ScutNetwork_CHttpClient_FullReset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'FullReset'", NULL);
#endif
  {
   self->FullReset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'FullReset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Reset of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_Reset00
static int tolua_Scut_ScutNetwork_CHttpClient_Reset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Reset'", NULL);
#endif
  {
   self->Reset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Reset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetUrlHost of class  ScutNetwork::CHttpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClient_GetUrlHost00
static int tolua_Scut_ScutNetwork_CHttpClient_GetUrlHost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  char* host = ((char*)  tolua_tostring(tolua_S,3,0));
  {
   int tolua_ret = (int)  ScutNetwork::CHttpClient::GetUrlHost(url,host);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetUrlHost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_new00
static int tolua_Scut_ScutNetwork_CTcpClient_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CTcpClient* tolua_ret = (ScutNetwork::CTcpClient*)  Mtolua_new((ScutNetwork::CTcpClient)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CTcpClient");
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

/* method: new_local of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_new00_local
static int tolua_Scut_ScutNetwork_CTcpClient_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CTcpClient* tolua_ret = (ScutNetwork::CTcpClient*)  Mtolua_new((ScutNetwork::CTcpClient)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CTcpClient");
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

/* method: delete of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_delete00
static int tolua_Scut_ScutNetwork_CTcpClient_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
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

/* method: TcpGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_TcpGet00
static int tolua_Scut_ScutNetwork_CTcpClient_TcpGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,4,&tolua_err) || !tolua_isusertype(tolua_S,4,"ScutNetwork::CHttpClientResponse",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* host = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* port = ((const char*)  tolua_tostring(tolua_S,3,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'TcpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->TcpGet(host,port,*resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'TcpGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: TcpGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_TcpGet01
static int tolua_Scut_ScutNetwork_CTcpClient_TcpGet01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'TcpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->TcpGet(url,*resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
tolua_lerror:
 return tolua_Scut_ScutNetwork_CTcpClient_TcpGet00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncTcpGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet00
static int tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,4,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* host = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* port = ((const char*)  tolua_tostring(tolua_S,3,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncTcpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncTcpGet(host,port,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncTcpGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncTcpGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet01
static int tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncTcpGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncTcpGet(url,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
tolua_lerror:
 return tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncNetGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_AsyncNetGet00
static int tolua_Scut_ScutNetwork_CTcpClient_AsyncNetGet00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  ScutNetwork::CHttpClientResponse* resp = ((ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AsyncNetGet'", NULL);
#endif
  {
   int tolua_ret = (int)  self->AsyncNetGet(url,resp);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AsyncNetGet'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: FullReset of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_FullReset00
static int tolua_Scut_ScutNetwork_CTcpClient_FullReset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'FullReset'", NULL);
#endif
  {
   self->FullReset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'FullReset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Reset of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_Reset00
static int tolua_Scut_ScutNetwork_CTcpClient_Reset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Reset'", NULL);
#endif
  {
   self->Reset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Reset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetHost of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_GetHost00
static int tolua_Scut_ScutNetwork_CTcpClient_GetHost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetHost'", NULL);
#endif
  {
   CScutString tolua_ret = (CScutString)  self->GetHost();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((CScutString)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(CScutString));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetHost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetPort of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_GetPort00
static int tolua_Scut_ScutNetwork_CTcpClient_GetPort00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetPort'", NULL);
#endif
  {
   unsigned short tolua_ret = (unsigned short)  self->GetPort();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetPort'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetUrlHost of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_GetUrlHost00
static int tolua_Scut_ScutNetwork_CTcpClient_GetUrlHost00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* url = ((const char*)  tolua_tostring(tolua_S,2,0));
  char* host = ((char*)  tolua_tostring(tolua_S,3,0));
  {
   int tolua_ret = (int)  ScutNetwork::CTcpClient::GetUrlHost(url,host);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetUrlHost'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: wait_on_socket of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CTcpClient_wait_on_socket00
static int tolua_Scut_ScutNetwork_CTcpClient_wait_on_socket00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CTcpClient",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  int sockfd = ((int)  tolua_tonumber(tolua_S,2,0));
  int for_recv = ((int)  tolua_tonumber(tolua_S,3,0));
  long timeout_ms = ((long)  tolua_tonumber(tolua_S,4,0));
  {
   int tolua_ret = (int)  ScutNetwork::CTcpClient::wait_on_socket(sockfd,for_recv,timeout_ms);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'wait_on_socket'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_new00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CHttpClientResponse* tolua_ret = (ScutNetwork::CHttpClientResponse*)  Mtolua_new((ScutNetwork::CHttpClientResponse)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpClientResponse");
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

/* method: new_local of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_new00_local
static int tolua_Scut_ScutNetwork_CHttpClientResponse_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CHttpClientResponse* tolua_ret = (ScutNetwork::CHttpClientResponse*)  Mtolua_new((ScutNetwork::CHttpClientResponse)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpClientResponse");
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

/* method: delete of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_delete00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
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

/* method: GetBodyPtr of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyPtr00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyPtr00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetBodyPtr'", NULL);
#endif
  {
   char* tolua_ret = (char*)  self->GetBodyPtr();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetBodyPtr'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetBodyLength of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyLength00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyLength00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetBodyLength'", NULL);
#endif
  {
   unsigned int tolua_ret = (unsigned int)  self->GetBodyLength();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetBodyLength'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: DataContains of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_DataContains00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_DataContains00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  const char* searchStr = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'DataContains'", NULL);
#endif
  {
   BOOL tolua_ret = (BOOL)  self->DataContains(searchStr);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((BOOL)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(BOOL));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'DataContains'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ContentTypeContains of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_ContentTypeContains00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_ContentTypeContains00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  char* searchStr = ((char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ContentTypeContains'", NULL);
#endif
  {
   BOOL tolua_ret = (BOOL)  self->ContentTypeContains(searchStr);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((BOOL)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(BOOL));
     tolua_pushusertype(tolua_S,tolua_obj,"BOOL");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ContentTypeContains'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Reset of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_Reset00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_Reset00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Reset'", NULL);
#endif
  {
   self->Reset();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Reset'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetContentType of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetContentType00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetContentType00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  char* contentType = ((char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetContentType'", NULL);
#endif
  {
   self->SetContentType(contentType);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetContentType'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetTargetFile of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetFile00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetFile00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetTargetFile'", NULL);
#endif
  {
   char* tolua_ret = (char*)  self->GetTargetFile();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetTargetFile'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetTargetFile of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetTargetFile00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetTargetFile00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetTargetFile'", NULL);
#endif
  {
   self->SetTargetFile(pszFileName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetTargetFile'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetTarget of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetTarget00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetTarget00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetTarget'", NULL);
#endif
  {
   ScutSystem::CStream* tolua_ret = (ScutSystem::CStream*)  self->GetTarget();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CStream");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetTarget'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetTarget of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetTarget00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetTarget00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  ScutSystem::CStream* pTarget = ((ScutSystem::CStream*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetTarget'", NULL);
#endif
  {
   self->SetTarget(pTarget);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetTarget'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetUseDataResume of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetUseDataResume00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetUseDataResume00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetUseDataResume'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->GetUseDataResume();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetUseDataResume'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetUseDataResume of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetUseDataResume00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetUseDataResume00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  bool bUse = ((bool)  tolua_toboolean(tolua_S,2,false));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetUseDataResume'", NULL);
#endif
  {
   self->SetUseDataResume(bUse);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetUseDataResume'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetRequestUrl of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetRequestUrl00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetRequestUrl00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetRequestUrl'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->GetRequestUrl();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetRequestUrl'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetRequestUrl of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetRequestUrl00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetRequestUrl00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  const char* pszUrl = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetRequestUrl'", NULL);
#endif
  {
   self->SetRequestUrl(pszUrl);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetRequestUrl'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetLastResponseUrl of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetLastResponseUrl'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->GetLastResponseUrl();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetLastResponseUrl'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetLastResponseUrl of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  const char* pszUrl = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetLastResponseUrl'", NULL);
#endif
  {
   self->SetLastResponseUrl(pszUrl);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetLastResponseUrl'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetTargetRawSize of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetRawSize00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetRawSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetTargetRawSize'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetTargetRawSize();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetTargetRawSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetStatusCode of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetStatusCode00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetStatusCode00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetStatusCode'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetStatusCode();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetStatusCode'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetStatusCode of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetStatusCode00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetStatusCode00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  int nCode = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetStatusCode'", NULL);
#endif
  {
   self->SetStatusCode(nCode);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetStatusCode'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetSendData of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_SetSendData00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_SetSendData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
  const char* data = ((const char*)  tolua_tostring(tolua_S,2,0));
  unsigned int dataLen = ((unsigned int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSendData'", NULL);
#endif
  {
   self->SetSendData(data,dataLen);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSendData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetSendData of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendData00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetSendData'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->GetSendData();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetSendData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetSendDataLength of class  ScutNetwork::CHttpClientResponse */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendDataLength00
static int tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendDataLength00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClientResponse",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetSendDataLength'", NULL);
#endif
  {
   unsigned int tolua_ret = (unsigned int)  self->GetSendDataLength();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetSendDataLength'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: DeleteCookies of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_DeleteCookies00
static int tolua_Scut_ScutNetwork_CHttpSession_DeleteCookies00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'DeleteCookies'", NULL);
#endif
  {
   self->DeleteCookies();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'DeleteCookies'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* get function: cookieJar of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_get_ScutNetwork__CHttpSession_cookieJar
static int tolua_get_ScutNetwork__CHttpSession_cookieJar(lua_State* tolua_S)
{
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'cookieJar'",NULL);
#endif
   tolua_pushusertype(tolua_S,(void*)&self->cookieJar,"CScutString");
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: cookieJar of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_set_ScutNetwork__CHttpSession_cookieJar
static int tolua_set_ScutNetwork__CHttpSession_cookieJar(lua_State* tolua_S)
{
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'cookieJar'",NULL);
  if ((tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"CScutString",0,&tolua_err)))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->cookieJar = *((CScutString*)  tolua_tousertype(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* method: Initialize of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_Initialize00
static int tolua_Scut_ScutNetwork_CHttpSession_Initialize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
  const char* username = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Initialize'", NULL);
#endif
  {
   self->Initialize(username);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Initialize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetCookies of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_GetCookies00
static int tolua_Scut_ScutNetwork_CHttpSession_GetCookies00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
  ScutNetwork::CHttpClient* req = ((ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetCookies'", NULL);
#endif
  {
   CScutString tolua_ret = (CScutString)  self->GetCookies(req);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((CScutString)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(CScutString));
     tolua_pushusertype(tolua_S,tolua_obj,"CScutString");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetCookies'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: AddCookie of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_AddCookie00
static int tolua_Scut_ScutNetwork_CHttpSession_AddCookie00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
  const char* value = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'AddCookie'", NULL);
#endif
  {
   self->AddCookie(value);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'AddCookie'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_new00
static int tolua_Scut_ScutNetwork_CHttpSession_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CHttpSession* tolua_ret = (ScutNetwork::CHttpSession*)  Mtolua_new((ScutNetwork::CHttpSession)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpSession");
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

/* method: new_local of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_new00_local
static int tolua_Scut_ScutNetwork_CHttpSession_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutNetwork::CHttpSession* tolua_ret = (ScutNetwork::CHttpSession*)  Mtolua_new((ScutNetwork::CHttpSession)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutNetwork::CHttpSession");
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

/* method: delete of class  ScutNetwork::CHttpSession */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutNetwork_CHttpSession_delete00
static int tolua_Scut_ScutNetwork_CHttpSession_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpSession",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*)  tolua_tousertype(tolua_S,1,0);
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

/* function: PrintMD5 */
#ifndef TOLUA_DISABLE_tolua_Scut_PrintMD500
static int tolua_Scut_PrintMD500(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_istable(tolua_S,1,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  unsigned char md5Digest[16];
  {
#ifndef TOLUA_RELEASE
   if (!tolua_isnumberarray(tolua_S,1,16,0,&tolua_err))
    goto tolua_lerror;
   else
#endif
   {
    int i;
    for(i=0; i<16;i++)
    md5Digest[i] = ((char)  tolua_tofieldnumber(tolua_S,1,i+1,0));
   }
  }
  {
   char* tolua_ret = (char*)  PrintMD5(md5Digest);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
  {
   int i;
   for(i=0; i<16;i++)
    tolua_pushfieldnumber(tolua_S,1,i+1,(lua_Number) md5Digest[i]);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'PrintMD5'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* function: MD5String */
#ifndef TOLUA_DISABLE_tolua_Scut_MD5String00
static int tolua_Scut_MD5String00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isstring(tolua_S,1,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  char* szString = ((char*)  tolua_tostring(tolua_S,1,0));
  {
   char* tolua_ret = (char*)  MD5String(szString);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'MD5String'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* function: MD5File */
#ifndef TOLUA_DISABLE_tolua_Scut_MD5File00
static int tolua_Scut_MD5File00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isstring(tolua_S,1,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  char* szFilename = ((char*)  tolua_tostring(tolua_S,1,0));
  {
   char* tolua_ret = (char*)  MD5File(szFilename);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'MD5File'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_new00
static int tolua_Scut_md5_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   md5* tolua_ret = (md5*)  Mtolua_new((md5)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"md5");
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

/* method: new_local of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_new00_local
static int tolua_Scut_md5_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   md5* tolua_ret = (md5*)  Mtolua_new((md5)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"md5");
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

/* method: Init of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_Init00
static int tolua_Scut_md5_Init00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  md5* self = (md5*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Init'", NULL);
#endif
  {
   self->Init();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Init'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Update of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_Update00
static int tolua_Scut_md5_Update00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  md5* self = (md5*)  tolua_tousertype(tolua_S,1,0);
  unsigned char chInput = (( unsigned char)  tolua_tonumber(tolua_S,2,0));
  unsigned int nInputLen = (( unsigned int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Update'", NULL);
#endif
  {
   self->Update(&chInput,nInputLen);
   tolua_pushnumber(tolua_S,(lua_Number)chInput);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Update'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Finalize of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_Finalize00
static int tolua_Scut_md5_Finalize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  md5* self = (md5*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Finalize'", NULL);
#endif
  {
   self->Finalize();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Finalize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Digest of class  md5 */
#ifndef TOLUA_DISABLE_tolua_Scut_md5_Digest00
static int tolua_Scut_md5_Digest00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"md5",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  md5* self = (md5*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Digest'", NULL);
#endif
  {
   void* tolua_ret = (void*)  self->Digest();
   tolua_pushuserdata(tolua_S,(void*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Digest'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: DesEncrypt of class  ScutSystem::CScutUtility */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CScutUtility_DesEncrypt00
static int tolua_Scut_ScutSystem_CScutUtility_DesEncrypt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CScutUtility",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* lpszKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* lpDataIn = ((const char*)  tolua_tostring(tolua_S,3,0));
  std::string strDataOut = ((std::string)  tolua_tocppstring(tolua_S,4,0));
  {
   int tolua_ret = (int)  ScutSystem::CScutUtility::DesEncrypt(lpszKey,lpDataIn,strDataOut);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
   tolua_pushcppstring(tolua_S,(const char*)strDataOut);
  }
 }
 return 2;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'DesEncrypt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: StdDesEncrypt of class  ScutSystem::CScutUtility */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CScutUtility_StdDesEncrypt00
static int tolua_Scut_ScutSystem_CScutUtility_StdDesEncrypt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CScutUtility",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* lpszKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* lpDataIn = ((const char*)  tolua_tostring(tolua_S,3,0));
  std::string strDataOut = ((std::string)  tolua_tocppstring(tolua_S,4,0));
  {
   ScutSystem::CScutUtility::StdDesEncrypt(lpszKey,lpDataIn,strDataOut);
   tolua_pushcppstring(tolua_S,(const char*)strDataOut);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'StdDesEncrypt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: StdDesDecrypt of class  ScutSystem::CScutUtility */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CScutUtility_StdDesDecrypt00
static int tolua_Scut_ScutSystem_CScutUtility_StdDesDecrypt00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CScutUtility",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* lpszKey = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* lpDataIn = ((const char*)  tolua_tostring(tolua_S,3,0));
  std::string strDataOut = ((std::string)  tolua_tocppstring(tolua_S,4,0));
  {
   ScutSystem::CScutUtility::StdDesDecrypt(lpszKey,lpDataIn,strDataOut);
   tolua_pushcppstring(tolua_S,(const char*)strDataOut);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'StdDesDecrypt'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetTickCount of class  ScutSystem::CScutUtility */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CScutUtility_GetTickCount00
static int tolua_Scut_ScutSystem_CScutUtility_GetTickCount00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CScutUtility",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   unsigned long tolua_ret = (unsigned long)  ScutSystem::CScutUtility::GetTickCount();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetTickCount'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: CScutUtility::GetNowTime of class  ScutSystem::CScutUtility */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CScutUtility_CScutUtility__GetNowTime00
static int tolua_Scut_ScutSystem_CScutUtility_CScutUtility__GetNowTime00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CScutUtility",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CScutUtility* self = (ScutSystem::CScutUtility*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'CScutUtility::GetNowTime'", NULL);
#endif
  {
   std::string tolua_ret = (std::string)  self->CScutUtility::GetNowTime();
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'CScutUtility__GetNowTime'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_delete00
static int tolua_Scut_ScutSystem_CStream_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
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

/* method: GetPosition of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_GetPosition00
static int tolua_Scut_ScutSystem_CStream_GetPosition00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetPosition'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetPosition();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetPosition'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetPosition of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_SetPosition00
static int tolua_Scut_ScutSystem_CStream_SetPosition00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  const int nPos = ((const int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetPosition'", NULL);
#endif
  {
   self->SetPosition(nPos);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetPosition'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetSize of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_GetSize00
static int tolua_Scut_ScutSystem_CStream_GetSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetSize'", NULL);
#endif
  {
   int tolua_ret = (int)  self->GetSize();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetSize of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_SetSize00
static int tolua_Scut_ScutSystem_CStream_SetSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  const int nSize = ((const int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSize'", NULL);
#endif
  {
   self->SetSize(nSize);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Read of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_Read00
static int tolua_Scut_ScutSystem_CStream_Read00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  char* pszBuffer = ((char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Read'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Read(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Read'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Write of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_Write00
static int tolua_Scut_ScutSystem_CStream_Write00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  const char* pszBuffer = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Write'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Write(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Write'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Seek of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_Seek00
static int tolua_Scut_ScutSystem_CStream_Seek00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  int nOffset = ((int)  tolua_tonumber(tolua_S,2,0));
  ScutSystem::EStreamOrigin origin = ((ScutSystem::EStreamOrigin) (int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Seek'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Seek(nOffset,origin);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Seek'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ReadBuffer of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_ReadBuffer00
static int tolua_Scut_ScutSystem_CStream_ReadBuffer00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  char* pszBuffer = ((char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ReadBuffer'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->ReadBuffer(pszBuffer,nSize);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ReadBuffer'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: WriteBuffer of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_WriteBuffer00
static int tolua_Scut_ScutSystem_CStream_WriteBuffer00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  const char* pszBuffer = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'WriteBuffer'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->WriteBuffer(pszBuffer,nSize);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'WriteBuffer'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: CopyFrom of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_CopyFrom00
static int tolua_Scut_ScutSystem_CStream_CopyFrom00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  ScutSystem::CStream* pSource = ((ScutSystem::CStream*)  tolua_tousertype(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'CopyFrom'", NULL);
#endif
  {
   int tolua_ret = (int)  self->CopyFrom(pSource,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'CopyFrom'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetBuffer of class  ScutSystem::CStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CStream_GetBuffer00
static int tolua_Scut_ScutSystem_CStream_GetBuffer00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CStream* self = (ScutSystem::CStream*)  tolua_tousertype(tolua_S,1,0);
  int nSize = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetBuffer'", NULL);
#endif
  {
   char* tolua_ret = (char*)  self->GetBuffer(nSize);
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetBuffer'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_new00
static int tolua_Scut_ScutSystem_CHandleStream_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CHandleStream* tolua_ret = (ScutSystem::CHandleStream*)  Mtolua_new((ScutSystem::CHandleStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CHandleStream");
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

/* method: new_local of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_new00_local
static int tolua_Scut_ScutSystem_CHandleStream_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CHandleStream* tolua_ret = (ScutSystem::CHandleStream*)  Mtolua_new((ScutSystem::CHandleStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CHandleStream");
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

/* method: SetSize of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_SetSize00
static int tolua_Scut_ScutSystem_CHandleStream_SetSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
  const int nSize = ((const int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSize'", NULL);
#endif
  {
   self->SetSize(nSize);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Read of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_Read00
static int tolua_Scut_ScutSystem_CHandleStream_Read00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
  char* pszBuffer = ((char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Read'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Read(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Read'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Write of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_Write00
static int tolua_Scut_ScutSystem_CHandleStream_Write00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
  const char* pszBuffer = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Write'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Write(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Write'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Seek of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_Seek00
static int tolua_Scut_ScutSystem_CHandleStream_Seek00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
  int nOffset = ((int)  tolua_tonumber(tolua_S,2,0));
  ScutSystem::EStreamOrigin origin = ((ScutSystem::EStreamOrigin) (int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Seek'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Seek(nOffset,origin);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Seek'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetHandle of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_GetHandle00
static int tolua_Scut_ScutSystem_CHandleStream_GetHandle00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetHandle'", NULL);
#endif
  {
   intptr_t tolua_ret = (intptr_t)  self->GetHandle();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((intptr_t)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"intptr_t");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(intptr_t));
     tolua_pushusertype(tolua_S,tolua_obj,"intptr_t");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetHandle'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetHandle of class  ScutSystem::CHandleStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CHandleStream_SetHandle00
static int tolua_Scut_ScutSystem_CHandleStream_SetHandle00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CHandleStream",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"intptr_t",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*)  tolua_tousertype(tolua_S,1,0);
  intptr_t hHandle = *((intptr_t*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetHandle'", NULL);
#endif
  {
   self->SetHandle(hHandle);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetHandle'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutSystem::CFileStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CFileStream_new00
static int tolua_Scut_ScutSystem_CFileStream_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CFileStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CFileStream* tolua_ret = (ScutSystem::CFileStream*)  Mtolua_new((ScutSystem::CFileStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CFileStream");
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

/* method: new_local of class  ScutSystem::CFileStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CFileStream_new00_local
static int tolua_Scut_ScutSystem_CFileStream_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CFileStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CFileStream* tolua_ret = (ScutSystem::CFileStream*)  Mtolua_new((ScutSystem::CFileStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CFileStream");
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

/* method: Open of class  ScutSystem::CFileStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CFileStream_Open00
static int tolua_Scut_ScutSystem_CFileStream_Open00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CFileStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"DWORD",0,&tolua_err)) ||
     (tolua_isvaluenil(tolua_S,4,&tolua_err) || !tolua_isusertype(tolua_S,4,"DWORD",0,&tolua_err)) ||
     !tolua_isstring(tolua_S,5,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,6,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CFileStream* self = (ScutSystem::CFileStream*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
  DWORD dwFlag = *((DWORD*)  tolua_tousertype(tolua_S,3,0));
  DWORD dwMode = *((DWORD*)  tolua_tousertype(tolua_S,4,0));
  char* chMode = ((char*)  tolua_tostring(tolua_S,5,NULL));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Open'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Open(lpszFileName,dwFlag,dwMode,chMode);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Open'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutSystem::CFileStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CFileStream_delete00
static int tolua_Scut_ScutSystem_CFileStream_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CFileStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CFileStream* self = (ScutSystem::CFileStream*)  tolua_tousertype(tolua_S,1,0);
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

/* method: SetSize of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_SetSize00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_SetSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const int nSize = ((const int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSize'", NULL);
#endif
  {
   self->SetSize(nSize);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Write of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_Write00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_Write00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const char* pszBuffer = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Write'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Write(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Write'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Read of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_Read00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_Read00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  char* pszBuffer = ((char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Read'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Read(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Read'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Seek of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_Seek00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_Seek00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  int nOffset = ((int)  tolua_tonumber(tolua_S,2,0));
  ScutSystem::EStreamOrigin origin = ((ScutSystem::EStreamOrigin) (int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Seek'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Seek(nOffset,origin);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Seek'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SaveTo of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  ScutSystem::CStream* pDest = ((ScutSystem::CStream*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SaveTo'", NULL);
#endif
  {
   self->SaveTo(pDest);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SaveTo'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SaveTo of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo01
static int tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SaveTo'", NULL);
#endif
  {
   self->SaveTo(lpszFileName);
  }
 }
 return 0;
tolua_lerror:
 return tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetMemory of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CBaseMemoryStream_GetMemory00
static int tolua_Scut_ScutSystem_CBaseMemoryStream_GetMemory00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CBaseMemoryStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetMemory'", NULL);
#endif
  {
   void* tolua_ret = (void*)  self->GetMemory();
   tolua_pushuserdata(tolua_S,(void*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetMemory'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: new of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_new00
static int tolua_Scut_ScutSystem_CMemoryStream_new00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CMemoryStream* tolua_ret = (ScutSystem::CMemoryStream*)  Mtolua_new((ScutSystem::CMemoryStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CMemoryStream");
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

/* method: new_local of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_new00_local
static int tolua_Scut_ScutSystem_CMemoryStream_new00_local(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutSystem::CMemoryStream* tolua_ret = (ScutSystem::CMemoryStream*)  Mtolua_new((ScutSystem::CMemoryStream)());
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutSystem::CMemoryStream");
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

/* method: delete of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_delete00
static int tolua_Scut_ScutSystem_CMemoryStream_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
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

/* method: Clear of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_Clear00
static int tolua_Scut_ScutSystem_CMemoryStream_Clear00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Clear'", NULL);
#endif
  {
   self->Clear();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Clear'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: LoadFrom of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_LoadFrom00
static int tolua_Scut_ScutSystem_CMemoryStream_LoadFrom00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutSystem::CStream",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  ScutSystem::CStream* pSource = ((ScutSystem::CStream*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'LoadFrom'", NULL);
#endif
  {
   self->LoadFrom(pSource);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'LoadFrom'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: LoadFrom of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_LoadFrom01
static int tolua_Scut_ScutSystem_CMemoryStream_LoadFrom01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const char* lpszFileName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'LoadFrom'", NULL);
#endif
  {
   self->LoadFrom(lpszFileName);
  }
 }
 return 0;
tolua_lerror:
 return tolua_Scut_ScutSystem_CMemoryStream_LoadFrom00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetSize of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_SetSize00
static int tolua_Scut_ScutSystem_CMemoryStream_SetSize00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const int nSize = ((const int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSize'", NULL);
#endif
  {
   self->SetSize(nSize);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSize'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Write of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutSystem_CMemoryStream_Write00
static int tolua_Scut_ScutSystem_CMemoryStream_Write00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutSystem::CMemoryStream",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*)  tolua_tousertype(tolua_S,1,0);
  const char* pszBuffer = ((const char*)  tolua_tostring(tolua_S,2,0));
  int nSize = ((int)  tolua_tonumber(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Write'", NULL);
#endif
  {
   int tolua_ret = (int)  self->Write(pszBuffer,nSize);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Write'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetPlatformType of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_GetPlatformType00
static int tolua_Scut_ScutUtility_ScutUtils_GetPlatformType00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::EPlatformType tolua_ret = (ScutUtility::EPlatformType)  ScutUtility::ScutUtils::GetPlatformType();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetPlatformType'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getImsi of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getImsi00
static int tolua_Scut_ScutUtility_ScutUtils_getImsi00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getImsi();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getImsi'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getImei of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getImei00
static int tolua_Scut_ScutUtility_ScutUtils_getImei00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getImei();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getImei'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: scheduleLocalNotification of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_scheduleLocalNotification00
static int tolua_Scut_ScutUtility_ScutUtils_scheduleLocalNotification00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isstring(tolua_S,4,0,&tolua_err) ||
     !tolua_isstring(tolua_S,5,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,6,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,7,0,&tolua_err) ||
     !tolua_isboolean(tolua_S,8,0,&tolua_err) ||
     !tolua_isstring(tolua_S,9,1,&tolua_err) ||
     !tolua_isboolean(tolua_S,10,1,&tolua_err) ||
     !tolua_isnumber(tolua_S,11,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,12,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszSoundName = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* pszAlertBody = ((const char*)  tolua_tostring(tolua_S,3,0));
  const char* pszAlertAction = ((const char*)  tolua_tostring(tolua_S,4,0));
  const char* pszLaunchImage = ((const char*)  tolua_tostring(tolua_S,5,0));
  double timeIntervalSince1970 = ((double)  tolua_tonumber(tolua_S,6,0));
  int repeatInterval = ((int)  tolua_tonumber(tolua_S,7,0));
  bool hasAction = ((bool)  tolua_toboolean(tolua_S,8,0));
  const char* pszAlertTitle = ((const char*)  tolua_tostring(tolua_S,9,NULL));
  bool hasVibration = ((bool)  tolua_toboolean(tolua_S,10,true));
  int iconResId = ((int)  tolua_tonumber(tolua_S,11,1));
  {
   int tolua_ret = (int)  ScutUtility::ScutUtils::scheduleLocalNotification(pszSoundName,pszAlertBody,pszAlertAction,pszLaunchImage,timeIntervalSince1970,repeatInterval,hasAction,pszAlertTitle,hasVibration,iconResId);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'scheduleLocalNotification'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: cancelLocalNotification of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotification00
static int tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotification00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  int nNotificationID = ((int)  tolua_tonumber(tolua_S,2,0));
  {
   ScutUtility::ScutUtils::cancelLocalNotification(nNotificationID);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'cancelLocalNotification'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: cancelLocalNotifications of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotifications00
static int tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotifications00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::ScutUtils::cancelLocalNotifications();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'cancelLocalNotifications'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getMacAddress of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getMacAddress00
static int tolua_Scut_ScutUtility_ScutUtils_getMacAddress00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getMacAddress();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getMacAddress'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setTextToClipBoard of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_setTextToClipBoard00
static int tolua_Scut_ScutUtility_ScutUtils_setTextToClipBoard00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string content = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutUtility::ScutUtils::setTextToClipBoard(content);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setTextToClipBoard'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getTextFromClipBoard of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getTextFromClipBoard00
static int tolua_Scut_ScutUtility_ScutUtils_getTextFromClipBoard00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getTextFromClipBoard();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getTextFromClipBoard'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: launchApp of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_launchApp00
static int tolua_Scut_ScutUtility_ScutUtils_launchApp00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,3,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string packageName = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  std::string data = ((std::string)  tolua_tocppstring(tolua_S,3,""));
  {
   ScutUtility::ScutUtils::launchApp(packageName,data);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'launchApp'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: installPackage of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_installPackage00
static int tolua_Scut_ScutUtility_ScutUtils_installPackage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string packageFilePath = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutUtility::ScutUtils::installPackage(packageFilePath);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'installPackage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: checkAppInstalled of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_checkAppInstalled00
static int tolua_Scut_ScutUtility_ScutUtils_checkAppInstalled00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isboolean(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string packageName = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  bool bForceUpdate = ((bool)  tolua_toboolean(tolua_S,3,0));
  {
   bool tolua_ret = (bool)  ScutUtility::ScutUtils::checkAppInstalled(packageName,bForceUpdate);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'checkAppInstalled'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerWebviewCallback of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_registerWebviewCallback00
static int tolua_Scut_ScutUtility_ScutUtils_registerWebviewCallback00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  std::string strFun = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutUtility::ScutUtils::registerWebviewCallback(strFun);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerWebviewCallback'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInstalledApps of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getInstalledApps00
static int tolua_Scut_ScutUtility_ScutUtils_getInstalledApps00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getInstalledApps();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getInstalledApps'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getCurrentAppId of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getCurrentAppId00
static int tolua_Scut_ScutUtility_ScutUtils_getCurrentAppId00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getCurrentAppId();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getCurrentAppId'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GoBack of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_GoBack00
static int tolua_Scut_ScutUtility_ScutUtils_GoBack00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::ScutUtils::GoBack();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GoBack'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getOpenUrlData of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getOpenUrlData00
static int tolua_Scut_ScutUtility_ScutUtils_getOpenUrlData00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::ScutUtils::getOpenUrlData();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getOpenUrlData'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: isJailBroken of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_isJailBroken00
static int tolua_Scut_ScutUtility_ScutUtils_isJailBroken00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   bool tolua_ret = (bool)  ScutUtility::ScutUtils::isJailBroken();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'isJailBroken'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getActiveNetworkInfo of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_ScutUtils_getActiveNetworkInfo00
static int tolua_Scut_ScutUtility_ScutUtils_getActiveNetworkInfo00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::ScutUtils",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::EActiveNetworkType tolua_ret = (ScutUtility::EActiveNetworkType)  ScutUtility::ScutUtils::getActiveNetworkInfo();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getActiveNetworkInfo'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setLanguage of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_setLanguage00
static int tolua_Scut_ScutUtility_CLocale_setLanguage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszLanguage = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutUtility::CLocale::setLanguage(pszLanguage);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setLanguage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getLanguage of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_getLanguage00
static int tolua_Scut_ScutUtility_CLocale_getLanguage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::CLocale::getLanguage();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getLanguage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getImsi of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_getImsi00
static int tolua_Scut_ScutUtility_CLocale_getImsi00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::CLocale::getImsi();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getImsi'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getImei of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_getImei00
static int tolua_Scut_ScutUtility_CLocale_getImei00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   const char* tolua_ret = (const char*)  ScutUtility::CLocale::getImei();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getImei'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setImsi of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_setImsi00
static int tolua_Scut_ScutUtility_CLocale_setImsi00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const char* pszImsi = ((const char*)  tolua_tostring(tolua_S,2,0));
  {
   ScutUtility::CLocale::setImsi(pszImsi);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setImsi'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: isSDCardExist of class  ScutUtility::CLocale */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CLocale_isSDCardExist00
static int tolua_Scut_ScutUtility_CLocale_isSDCardExist00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CLocale",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   bool tolua_ret = (bool)  ScutUtility::CLocale::isSDCardExist();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'isSDCardExist'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: instance of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_instance00
static int tolua_Scut_ScutUtility_CScutLuaLan_instance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::CScutLuaLan* tolua_ret = (ScutUtility::CScutLuaLan*)  ScutUtility::CScutLuaLan::instance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutUtility::CScutLuaLan");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'instance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Add of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_Add00
static int tolua_Scut_ScutUtility_CScutLuaLan_Add00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutUtility::CScutLuaLan* self = (ScutUtility::CScutLuaLan*)  tolua_tousertype(tolua_S,1,0);
  const char* pszLan = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* pszPath = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Add'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->Add(pszLan,pszPath);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Add'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Switch of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_Switch00
static int tolua_Scut_ScutUtility_CScutLuaLan_Switch00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutUtility::CScutLuaLan* self = (ScutUtility::CScutLuaLan*)  tolua_tousertype(tolua_S,1,0);
  const char* pszLan = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Switch'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->Switch(pszLan);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Switch'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Remove of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_Remove00
static int tolua_Scut_ScutUtility_CScutLuaLan_Remove00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutUtility::CScutLuaLan* self = (ScutUtility::CScutLuaLan*)  tolua_tousertype(tolua_S,1,0);
  const char* pszLan = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Remove'", NULL);
#endif
  {
   self->Remove(pszLan);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Remove'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: Get of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_Get00
static int tolua_Scut_ScutUtility_CScutLuaLan_Get00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutUtility::CScutLuaLan* self = (ScutUtility::CScutLuaLan*)  tolua_tousertype(tolua_S,1,0);
  const char* group = ((const char*)  tolua_tostring(tolua_S,2,0));
  const char* key = ((const char*)  tolua_tostring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'Get'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->Get(group,key);
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

/* method: RemoveAll of class  ScutUtility::CScutLuaLan */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLuaLan_RemoveAll00
static int tolua_Scut_ScutUtility_CScutLuaLan_RemoveAll00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutUtility::CScutLuaLan",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutUtility::CScutLuaLan* self = (ScutUtility::CScutLuaLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RemoveAll'", NULL);
#endif
  {
   self->RemoveAll();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RemoveAll'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetLanInstance of class  ScutUtility::CScutLanFactory */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutUtility_CScutLanFactory_GetLanInstance00
static int tolua_Scut_ScutUtility_CScutLanFactory_GetLanInstance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutUtility::CScutLanFactory",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutUtility::CScutLan* tolua_ret = (ScutUtility::CScutLan*)  ScutUtility::CScutLanFactory::GetLanInstance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutUtility::CScutLan");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetLanInstance'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szOK of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szOK
static int tolua_get_ScutUtility__CScutLan_m_szOK(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szOK'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szOK);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szOK of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szOK
static int tolua_set_ScutUtility__CScutLan_m_szOK(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szOK'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szOK = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szCancel of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szCancel
static int tolua_get_ScutUtility__CScutLan_m_szCancel(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szCancel'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szCancel);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szCancel of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szCancel
static int tolua_set_ScutUtility__CScutLan_m_szCancel(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szCancel'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szCancel = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szKnown of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szKnown
static int tolua_get_ScutUtility__CScutLan_m_szKnown(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szKnown'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szKnown);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szKnown of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szKnown
static int tolua_set_ScutUtility__CScutLan_m_szKnown(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szKnown'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szKnown = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szTimeOut of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szTimeOut
static int tolua_get_ScutUtility__CScutLan_m_szTimeOut(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szTimeOut'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szTimeOut);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szTimeOut of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szTimeOut
static int tolua_set_ScutUtility__CScutLan_m_szTimeOut(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szTimeOut'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szTimeOut = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szFalseConnect of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szFalseConnect
static int tolua_get_ScutUtility__CScutLan_m_szFalseConnect(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szFalseConnect'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szFalseConnect);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szFalseConnect of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szFalseConnect
static int tolua_set_ScutUtility__CScutLan_m_szFalseConnect(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szFalseConnect'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szFalseConnect = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szUpdateError of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szUpdateError
static int tolua_get_ScutUtility__CScutLan_m_szUpdateError(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szUpdateError'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szUpdateError);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szUpdateError of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szUpdateError
static int tolua_set_ScutUtility__CScutLan_m_szUpdateError(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szUpdateError'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szUpdateError = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szDownload of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szDownload
static int tolua_get_ScutUtility__CScutLan_m_szDownload(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szDownload'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szDownload);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szDownload of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szDownload
static int tolua_set_ScutUtility__CScutLan_m_szDownload(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szDownload'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szDownload = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szExit of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szExit
static int tolua_get_ScutUtility__CScutLan_m_szExit(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szExit'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szExit);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szExit of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szExit
static int tolua_set_ScutUtility__CScutLan_m_szExit(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szExit'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szExit = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szSDNotExist of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szSDNotExist
static int tolua_get_ScutUtility__CScutLan_m_szSDNotExist(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szSDNotExist'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szSDNotExist);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szSDNotExist of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szSDNotExist
static int tolua_set_ScutUtility__CScutLan_m_szSDNotExist(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szSDNotExist'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szSDNotExist = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szSDReadError of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szSDReadError
static int tolua_get_ScutUtility__CScutLan_m_szSDReadError(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szSDReadError'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szSDReadError);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szSDReadError of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szSDReadError
static int tolua_set_ScutUtility__CScutLan_m_szSDReadError(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szSDReadError'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szSDReadError = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szResPackageLoadingTip of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szResPackageLoadingTip
static int tolua_get_ScutUtility__CScutLan_m_szResPackageLoadingTip(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szResPackageLoadingTip'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szResPackageLoadingTip);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szResPackageLoadingTip of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szResPackageLoadingTip
static int tolua_set_ScutUtility__CScutLan_m_szResPackageLoadingTip(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szResPackageLoadingTip'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szResPackageLoadingTip = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* get function: m_szResPackageFinishTip of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_get_ScutUtility__CScutLan_m_szResPackageFinishTip
static int tolua_get_ScutUtility__CScutLan_m_szResPackageFinishTip(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szResPackageFinishTip'",NULL);
#endif
  tolua_pushcppstring(tolua_S,(const char*)self->m_szResPackageFinishTip);
 return 1;
}
#endif //#ifndef TOLUA_DISABLE

/* set function: m_szResPackageFinishTip of class  ScutUtility::CScutLan */
#ifndef TOLUA_DISABLE_tolua_set_ScutUtility__CScutLan_m_szResPackageFinishTip
static int tolua_set_ScutUtility__CScutLan_m_szResPackageFinishTip(lua_State* tolua_S)
{
  ScutUtility::CScutLan* self = (ScutUtility::CScutLan*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  tolua_Error tolua_err;
  if (!self) tolua_error(tolua_S,"invalid 'self' in accessing variable 'm_szResPackageFinishTip'",NULL);
  if (!tolua_iscppstring(tolua_S,2,0,&tolua_err))
   tolua_error(tolua_S,"#vinvalid type in variable assignment.",&tolua_err);
#endif
  self->m_szResPackageFinishTip = ((std::string)  tolua_tocppstring(tolua_S,2,0))
;
 return 0;
}
#endif //#ifndef TOLUA_DISABLE

/* method: Init of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_Init00
static int tolua_Scut_ScutExt_Init00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const std::string resRootDir = ((const std::string)  tolua_tocppstring(tolua_S,2,0));
  {
   ScutExt::Init(resRootDir);
   tolua_pushcppstring(tolua_S,(const char*)resRootDir);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'Init'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getInstance of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_getInstance00
static int tolua_Scut_ScutExt_getInstance00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  {
   ScutExt* tolua_ret = (ScutExt*)  ScutExt::getInstance();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutExt");
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

/* method: RegisterPauseHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterPauseHandler00
static int tolua_Scut_ScutExt_RegisterPauseHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterPauseHandler'", NULL);
#endif
  {
   self->RegisterPauseHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterPauseHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterResumeHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterResumeHandler00
static int tolua_Scut_ScutExt_RegisterResumeHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterResumeHandler'", NULL);
#endif
  {
   self->RegisterResumeHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterResumeHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterBackHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterBackHandler00
static int tolua_Scut_ScutExt_RegisterBackHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterBackHandler'", NULL);
#endif
  {
   self->RegisterBackHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterBackHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterErrorHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterErrorHandler00
static int tolua_Scut_ScutExt_RegisterErrorHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterErrorHandler'", NULL);
#endif
  {
   self->RegisterErrorHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterErrorHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterSocketPushHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterSocketPushHandler00
static int tolua_Scut_ScutExt_RegisterSocketPushHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterSocketPushHandler'", NULL);
#endif
  {
   self->RegisterSocketPushHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterSocketPushHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetSocketPushHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_GetSocketPushHandler00
static int tolua_Scut_ScutExt_GetSocketPushHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetSocketPushHandler'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->GetSocketPushHandler();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetSocketPushHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: RegisterSocketErrorHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_RegisterSocketErrorHandler00
static int tolua_Scut_ScutExt_RegisterSocketErrorHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const char* pszFuncName = ((const char*)  tolua_tostring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'RegisterSocketErrorHandler'", NULL);
#endif
  {
   self->RegisterSocketErrorHandler(pszFuncName);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'RegisterSocketErrorHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetSocketErrorHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_GetSocketErrorHandler00
static int tolua_Scut_ScutExt_GetSocketErrorHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetSocketErrorHandler'", NULL);
#endif
  {
   const char* tolua_ret = (const char*)  self->GetSocketErrorHandler();
   tolua_pushstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetSocketErrorHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: UnregisterErrorHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_UnregisterErrorHandler00
static int tolua_Scut_ScutExt_UnregisterErrorHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'UnregisterErrorHandler'", NULL);
#endif
  {
   self->UnregisterErrorHandler();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'UnregisterErrorHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetErrorHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_GetErrorHandler00
static int tolua_Scut_ScutExt_GetErrorHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'GetErrorHandler'", NULL);
#endif
  {
   std::string tolua_ret = (std::string)  self->GetErrorHandler();
   tolua_pushcppstring(tolua_S,(const char*)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'GetErrorHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ExcuteBackHandler of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_ExcuteBackHandler00
static int tolua_Scut_ScutExt_ExcuteBackHandler00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ExcuteBackHandler'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->ExcuteBackHandler();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ExcuteBackHandler'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: EndDirector of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_EndDirector00
static int tolua_Scut_ScutExt_EndDirector00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'EndDirector'", NULL);
#endif
  {
   self->EndDirector();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'EndDirector'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: PauseDirector of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_PauseDirector00
static int tolua_Scut_ScutExt_PauseDirector00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'PauseDirector'", NULL);
#endif
  {
   self->PauseDirector();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'PauseDirector'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ResumeDirector of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_ResumeDirector00
static int tolua_Scut_ScutExt_ResumeDirector00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ResumeDirector'", NULL);
#endif
  {
   self->ResumeDirector();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ResumeDirector'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: pushfunc of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_pushfunc00
static int tolua_Scut_ScutExt_pushfunc00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  const std::string strFunc = ((const std::string)  tolua_tocppstring(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'pushfunc'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->pushfunc(strFunc);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
   tolua_pushcppstring(tolua_S,(const char*)strFunc);
  }
 }
 return 2;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'pushfunc'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: executeLogEvent of class  ScutExt */
#ifndef TOLUA_DISABLE_tolua_Scut_ScutExt_executeLogEvent00
static int tolua_Scut_ScutExt_executeLogEvent00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutExt",0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,2,0,&tolua_err) ||
     !tolua_iscppstring(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutExt* self = (ScutExt*)  tolua_tousertype(tolua_S,1,0);
  std::string func = ((std::string)  tolua_tocppstring(tolua_S,2,0));
  std::string errlog = ((std::string)  tolua_tocppstring(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'executeLogEvent'", NULL);
#endif
  {
   self->executeLogEvent(func,errlog);
   tolua_pushcppstring(tolua_S,(const char*)func);
   tolua_pushcppstring(tolua_S,(const char*)errlog);
  }
 }
 return 2;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'executeLogEvent'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_Scut_open (lua_State* tolua_S)
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
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CLuaString_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CLuaString_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CLuaString_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutDataLogic_CLuaString_delete00);
    tolua_function(tolua_S,"setString",tolua_Scut_ScutDataLogic_CLuaString_setString00);
    tolua_function(tolua_S,"getCString",tolua_Scut_ScutDataLogic_CLuaString_getCString00);
    tolua_function(tolua_S,"getSize",tolua_Scut_ScutDataLogic_CLuaString_getSize00);
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CLuaString_new01);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CLuaString_new01_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CLuaString_new01_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutDataLogic_CLuaString_delete01);
    tolua_function(tolua_S,"setString",tolua_Scut_ScutDataLogic_CLuaString_setString01);
    tolua_function(tolua_S,"getCString",tolua_Scut_ScutDataLogic_CLuaString_getCString01);
    tolua_function(tolua_S,"getSize",tolua_Scut_ScutDataLogic_CLuaString_getSize01);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CNetWriter","ScutDataLogic::CNetWriter","",NULL);
   tolua_beginmodule(tolua_S,"CNetWriter");
    tolua_function(tolua_S,"writeInt32",tolua_Scut_ScutDataLogic_CNetWriter_writeInt3200);
    tolua_function(tolua_S,"writeFloat",tolua_Scut_ScutDataLogic_CNetWriter_writeFloat00);
    tolua_function(tolua_S,"writeString",tolua_Scut_ScutDataLogic_CNetWriter_writeString00);
    tolua_function(tolua_S,"writeInt64",tolua_Scut_ScutDataLogic_CNetWriter_writeInt6400);
    tolua_function(tolua_S,"writeInt64",tolua_Scut_ScutDataLogic_CNetWriter_writeInt6401);
    tolua_function(tolua_S,"writeWord",tolua_Scut_ScutDataLogic_CNetWriter_writeWord00);
    tolua_function(tolua_S,"writeBuf",tolua_Scut_ScutDataLogic_CNetWriter_writeBuf00);
    tolua_function(tolua_S,"setUrl",tolua_Scut_ScutDataLogic_CNetWriter_setUrl00);
    tolua_function(tolua_S,"url_encode",tolua_Scut_ScutDataLogic_CNetWriter_url_encode00);
    tolua_function(tolua_S,"url_encode",tolua_Scut_ScutDataLogic_CNetWriter_url_encode01);
    tolua_function(tolua_S,"getInstance",tolua_Scut_ScutDataLogic_CNetWriter_getInstance00);
    tolua_function(tolua_S,"generatePostData",tolua_Scut_ScutDataLogic_CNetWriter_generatePostData00);
    tolua_function(tolua_S,"resetData",tolua_Scut_ScutDataLogic_CNetWriter_resetData00);
    tolua_function(tolua_S,"setSessionID",tolua_Scut_ScutDataLogic_CNetWriter_setSessionID00);
    tolua_function(tolua_S,"setUserID",tolua_Scut_ScutDataLogic_CNetWriter_setUserID00);
    tolua_function(tolua_S,"setStime",tolua_Scut_ScutDataLogic_CNetWriter_setStime00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CNetReader","ScutDataLogic::CNetReader","ScutDataLogic::CNetStreamExport",tolua_collect_ScutDataLogic__CNetReader);
   #else
   tolua_cclass(tolua_S,"CNetReader","ScutDataLogic::CNetReader","ScutDataLogic::CNetStreamExport",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CNetReader");
    tolua_function(tolua_S,"delete",tolua_Scut_ScutDataLogic_CNetReader_delete00);
    tolua_function(tolua_S,"getCInt64",tolua_Scut_ScutDataLogic_CNetReader_getCInt6400);
    tolua_function(tolua_S,"getString",tolua_Scut_ScutDataLogic_CNetReader_getString00);
    tolua_function(tolua_S,"getByte",tolua_Scut_ScutDataLogic_CNetReader_getByte00);
    tolua_function(tolua_S,"getWord",tolua_Scut_ScutDataLogic_CNetReader_getWord00);
    tolua_function(tolua_S,"getInstance",tolua_Scut_ScutDataLogic_CNetReader_getInstance00);
    tolua_function(tolua_S,"getResult",tolua_Scut_ScutDataLogic_CNetReader_getResult00);
    tolua_function(tolua_S,"getRmId",tolua_Scut_ScutDataLogic_CNetReader_getRmId00);
    tolua_function(tolua_S,"getActionID",tolua_Scut_ScutDataLogic_CNetReader_getActionID00);
    tolua_function(tolua_S,"getErrMsg",tolua_Scut_ScutDataLogic_CNetReader_getErrMsg00);
    tolua_function(tolua_S,"getStrStime",tolua_Scut_ScutDataLogic_CNetReader_getStrStime00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
  tolua_endmodule(tolua_S);
  tolua_function(tolua_S,"getPath",tolua_Scut_getPath00);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CFileHelper","ScutDataLogic::CFileHelper","",NULL);
   tolua_beginmodule(tolua_S,"CFileHelper");
    tolua_function(tolua_S,"getPath",tolua_Scut_ScutDataLogic_CFileHelper_getPath00);
    tolua_function(tolua_S,"setAndroidSDCardDirPath",tolua_Scut_ScutDataLogic_CFileHelper_setAndroidSDCardDirPath00);
    tolua_function(tolua_S,"getAndroidSDCardDirPath",tolua_Scut_ScutDataLogic_CFileHelper_getAndroidSDCardDirPath00);
    tolua_function(tolua_S,"setAndroidResourcePath",tolua_Scut_ScutDataLogic_CFileHelper_setAndroidResourcePath00);
    tolua_function(tolua_S,"getFileData",tolua_Scut_ScutDataLogic_CFileHelper_getFileData00);
    tolua_function(tolua_S,"freeFileData",tolua_Scut_ScutDataLogic_CFileHelper_freeFileData00);
    tolua_function(tolua_S,"encryptPwd",tolua_Scut_ScutDataLogic_CFileHelper_encryptPwd00);
    tolua_function(tolua_S,"getFileState",tolua_Scut_ScutDataLogic_CFileHelper_getFileState00);
    tolua_function(tolua_S,"createDirs",tolua_Scut_ScutDataLogic_CFileHelper_createDirs00);
    tolua_function(tolua_S,"isDirExists",tolua_Scut_ScutDataLogic_CFileHelper_isDirExists00);
    tolua_function(tolua_S,"createDir",tolua_Scut_ScutDataLogic_CFileHelper_createDir00);
    tolua_function(tolua_S,"isFileExists",tolua_Scut_ScutDataLogic_CFileHelper_isFileExists00);
    tolua_function(tolua_S,"getWritablePath",tolua_Scut_ScutDataLogic_CFileHelper_getWritablePath00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CLuaIni","ScutDataLogic::CLuaIni","",tolua_collect_ScutDataLogic__CLuaIni);
   #else
   tolua_cclass(tolua_S,"CLuaIni","ScutDataLogic::CLuaIni","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CLuaIni");
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CLuaIni_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CLuaIni_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CLuaIni_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutDataLogic_CLuaIni_delete00);
    tolua_function(tolua_S,"Load",tolua_Scut_ScutDataLogic_CLuaIni_Load00);
    tolua_function(tolua_S,"APLoad",tolua_Scut_ScutDataLogic_CLuaIni_APLoad00);
    tolua_function(tolua_S,"Save",tolua_Scut_ScutDataLogic_CLuaIni_Save00);
    tolua_function(tolua_S,"Get",tolua_Scut_ScutDataLogic_CLuaIni_Get00);
    tolua_function(tolua_S,"GetInt",tolua_Scut_ScutDataLogic_CLuaIni_GetInt00);
    tolua_function(tolua_S,"Set",tolua_Scut_ScutDataLogic_CLuaIni_Set00);
    tolua_function(tolua_S,"SetInt",tolua_Scut_ScutDataLogic_CLuaIni_SetInt00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_constant(tolua_S,"reNetFailed",ScutDataLogic::reNetFailed);
   tolua_constant(tolua_S,"reNetTimeOut",ScutDataLogic::reNetTimeOut);
   tolua_cclass(tolua_S,"CDataRequest","ScutDataLogic::CDataRequest","INetStatusNotify",NULL);
   tolua_beginmodule(tolua_S,"CDataRequest");
    tolua_function(tolua_S,"Instance",tolua_Scut_ScutDataLogic_CDataRequest_Instance00);
    tolua_function(tolua_S,"ExecRequest",tolua_Scut_ScutDataLogic_CDataRequest_ExecRequest00);
    tolua_function(tolua_S,"AsyncExecRequest",tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecRequest00);
    tolua_function(tolua_S,"AsyncExecTcpRequest",tolua_Scut_ScutDataLogic_CDataRequest_AsyncExecTcpRequest00);
    tolua_function(tolua_S,"PeekLUAData",tolua_Scut_ScutDataLogic_CDataRequest_PeekLUAData00);
    tolua_function(tolua_S,"LuaHandlePushDataWithInt",tolua_Scut_ScutDataLogic_CDataRequest_LuaHandlePushDataWithInt00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutDataLogic",0);
  tolua_beginmodule(tolua_S,"ScutDataLogic");
   tolua_cclass(tolua_S,"CNetStreamExport","ScutDataLogic::CNetStreamExport","",NULL);
   tolua_beginmodule(tolua_S,"CNetStreamExport");
    tolua_function(tolua_S,"recordBegin",tolua_Scut_ScutDataLogic_CNetStreamExport_recordBegin00);
    tolua_function(tolua_S,"recordEnd",tolua_Scut_ScutDataLogic_CNetStreamExport_recordEnd00);
    tolua_function(tolua_S,"getBYTE",tolua_Scut_ScutDataLogic_CNetStreamExport_getBYTE00);
    tolua_function(tolua_S,"getWORD",tolua_Scut_ScutDataLogic_CNetStreamExport_getWORD00);
    tolua_function(tolua_S,"getDWORD",tolua_Scut_ScutDataLogic_CNetStreamExport_getDWORD00);
    tolua_function(tolua_S,"getInt",tolua_Scut_ScutDataLogic_CNetStreamExport_getInt00);
    tolua_function(tolua_S,"getFloat",tolua_Scut_ScutDataLogic_CNetStreamExport_getFloat00);
    tolua_function(tolua_S,"getDouble",tolua_Scut_ScutDataLogic_CNetStreamExport_getDouble00);
    tolua_function(tolua_S,"getInt64",tolua_Scut_ScutDataLogic_CNetStreamExport_getInt6400);
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
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CInt64_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CInt64_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CInt64_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutDataLogic_CInt64_delete00);
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CInt64_new01);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CInt64_new01_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CInt64_new01_local);
    tolua_function(tolua_S,"new",tolua_Scut_ScutDataLogic_CInt64_new02);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutDataLogic_CInt64_new02_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutDataLogic_CInt64_new02_local);
    tolua_function(tolua_S,".add",tolua_Scut_ScutDataLogic_CInt64__add00);
    tolua_function(tolua_S,".add",tolua_Scut_ScutDataLogic_CInt64__add01);
    tolua_function(tolua_S,".sub",tolua_Scut_ScutDataLogic_CInt64__sub00);
    tolua_function(tolua_S,".sub",tolua_Scut_ScutDataLogic_CInt64__sub01);
    tolua_function(tolua_S,".mul",tolua_Scut_ScutDataLogic_CInt64__mul00);
    tolua_function(tolua_S,".mul",tolua_Scut_ScutDataLogic_CInt64__mul01);
    tolua_function(tolua_S,".div",tolua_Scut_ScutDataLogic_CInt64__div00);
    tolua_function(tolua_S,".div",tolua_Scut_ScutDataLogic_CInt64__div01);
    tolua_function(tolua_S,".eq",tolua_Scut_ScutDataLogic_CInt64__eq00);
    tolua_function(tolua_S,".le",tolua_Scut_ScutDataLogic_CInt64__le00);
    tolua_function(tolua_S,".lt",tolua_Scut_ScutDataLogic_CInt64__lt00);
    tolua_function(tolua_S,"equal",tolua_Scut_ScutDataLogic_CInt64_equal00);
    tolua_function(tolua_S,"str",tolua_Scut_ScutDataLogic_CInt64_str00);
    tolua_function(tolua_S,"mod",tolua_Scut_ScutDataLogic_CInt64_mod00);
    tolua_function(tolua_S,"mod",tolua_Scut_ScutDataLogic_CInt64_mod01);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_constant(tolua_S,"ERROR_UNSUPPORTED_PROTOCOL",ERROR_UNSUPPORTED_PROTOCOL);
  tolua_constant(tolua_S,"ERROR_SOCK_CREATION_FAILED",ERROR_SOCK_CREATION_FAILED);
  tolua_constant(tolua_S,"ERROR_CONNECT_FAILED",ERROR_CONNECT_FAILED);
  tolua_constant(tolua_S,"HTTP_MAX_RETRIES",HTTP_MAX_RETRIES);
  tolua_module(tolua_S,"ScutNetwork",0);
  tolua_beginmodule(tolua_S,"ScutNetwork");
   tolua_constant(tolua_S,"aisNone",ScutNetwork::aisNone);
   tolua_constant(tolua_S,"aisProgress",ScutNetwork::aisProgress);
   tolua_constant(tolua_S,"aisSucceed",ScutNetwork::aisSucceed);
   tolua_constant(tolua_S,"aisTimeOut",ScutNetwork::aisTimeOut);
   tolua_constant(tolua_S,"aisFailed",ScutNetwork::aisFailed);
   tolua_constant(tolua_S,"amGet",ScutNetwork::amGet);
   tolua_constant(tolua_S,"amPost",ScutNetwork::amPost);
   tolua_constant(tolua_S,"ntNone",ScutNetwork::ntNone);
   tolua_constant(tolua_S,"ntWIFI",ScutNetwork::ntWIFI);
   tolua_constant(tolua_S,"ntCMWAP",ScutNetwork::ntCMWAP);
   tolua_constant(tolua_S,"ntCMNET",ScutNetwork::ntCMNET);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"AsyncInfo","ScutNetwork::AsyncInfo","",tolua_collect_ScutNetwork__AsyncInfo);
   #else
   tolua_cclass(tolua_S,"AsyncInfo","ScutNetwork::AsyncInfo","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"AsyncInfo");
    tolua_variable(tolua_S,"Sender",tolua_get_ScutNetwork__AsyncInfo_Sender_ptr,tolua_set_ScutNetwork__AsyncInfo_Sender_ptr);
    tolua_variable(tolua_S,"Response",tolua_get_ScutNetwork__AsyncInfo_Response_ptr,tolua_set_ScutNetwork__AsyncInfo_Response_ptr);
    tolua_variable(tolua_S,"Url",tolua_get_ScutNetwork__AsyncInfo_Url,tolua_set_ScutNetwork__AsyncInfo_Url);
    tolua_variable(tolua_S,"PostData",tolua_get_ScutNetwork__AsyncInfo_PostData,NULL);
    tolua_variable(tolua_S,"PostDataSize",tolua_get_ScutNetwork__AsyncInfo_PostDataSize,tolua_set_ScutNetwork__AsyncInfo_PostDataSize);
    tolua_variable(tolua_S,"FormFlag",tolua_get_ScutNetwork__AsyncInfo_FormFlag,tolua_set_ScutNetwork__AsyncInfo_FormFlag);
    tolua_variable(tolua_S,"Status",tolua_get_ScutNetwork__AsyncInfo_Status,tolua_set_ScutNetwork__AsyncInfo_Status);
    tolua_variable(tolua_S,"Mode",tolua_get_ScutNetwork__AsyncInfo_Mode,tolua_set_ScutNetwork__AsyncInfo_Mode);
    tolua_variable(tolua_S,"ProtocalType",tolua_get_ScutNetwork__AsyncInfo_ProtocalType,tolua_set_ScutNetwork__AsyncInfo_ProtocalType);
    tolua_variable(tolua_S,"pScene",tolua_get_ScutNetwork__AsyncInfo_pScene,tolua_set_ScutNetwork__AsyncInfo_pScene);
    tolua_variable(tolua_S,"Data1",tolua_get_ScutNetwork__AsyncInfo_Data1,tolua_set_ScutNetwork__AsyncInfo_Data1);
    tolua_variable(tolua_S,"Data2",tolua_get_ScutNetwork__AsyncInfo_Data2,tolua_set_ScutNetwork__AsyncInfo_Data2);
    tolua_function(tolua_S,"new",tolua_Scut_ScutNetwork_AsyncInfo_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutNetwork_AsyncInfo_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutNetwork_AsyncInfo_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutNetwork_AsyncInfo_delete00);
    tolua_function(tolua_S,"Reset",tolua_Scut_ScutNetwork_AsyncInfo_Reset00);
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CNetClientBase","ScutNetwork::CNetClientBase","",NULL);
   tolua_beginmodule(tolua_S,"CNetClientBase");
    tolua_function(tolua_S,"AsyncNetGet",tolua_Scut_ScutNetwork_CNetClientBase_AsyncNetGet00);
    tolua_function(tolua_S,"SetNetStautsNotify",tolua_Scut_ScutNetwork_CNetClientBase_SetNetStautsNotify00);
    tolua_function(tolua_S,"GetNetStautsNotify",tolua_Scut_ScutNetwork_CNetClientBase_GetNetStautsNotify00);
    tolua_function(tolua_S,"AddHeader",tolua_Scut_ScutNetwork_CNetClientBase_AddHeader00);
    tolua_function(tolua_S,"GetTimeOut",tolua_Scut_ScutNetwork_CNetClientBase_GetTimeOut00);
    tolua_function(tolua_S,"SetTimeOut",tolua_Scut_ScutNetwork_CNetClientBase_SetTimeOut00);
    tolua_function(tolua_S,"GetUseProgressReport",tolua_Scut_ScutNetwork_CNetClientBase_GetUseProgressReport00);
    tolua_function(tolua_S,"SetUseProgressReport",tolua_Scut_ScutNetwork_CNetClientBase_SetUseProgressReport00);
    tolua_function(tolua_S,"FullReset",tolua_Scut_ScutNetwork_CNetClientBase_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_Scut_ScutNetwork_CNetClientBase_Reset00);
    tolua_function(tolua_S,"GetHost",tolua_Scut_ScutNetwork_CNetClientBase_GetHost00);
    tolua_function(tolua_S,"IsBusy",tolua_Scut_ScutNetwork_CNetClientBase_IsBusy00);
    tolua_function(tolua_S,"GetAsyncInfo",tolua_Scut_ScutNetwork_CNetClientBase_GetAsyncInfo00);
    tolua_function(tolua_S,"GetCurlHandle",tolua_Scut_ScutNetwork_CNetClientBase_GetCurlHandle00);
    tolua_function(tolua_S,"RegisterCustomLuaHandle",tolua_Scut_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutNetwork",0);
  tolua_beginmodule(tolua_S,"ScutNetwork");
   tolua_cclass(tolua_S,"INetStatusNotify","ScutNetwork::INetStatusNotify","",NULL);
   tolua_beginmodule(tolua_S,"INetStatusNotify");
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CHttpClient","ScutNetwork::CHttpClient","ScutNetwork::CNetClientBase",tolua_collect_ScutNetwork__CHttpClient);
   #else
   tolua_cclass(tolua_S,"CHttpClient","ScutNetwork::CHttpClient","ScutNetwork::CNetClientBase",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CHttpClient");
    tolua_function(tolua_S,"new",tolua_Scut_ScutNetwork_CHttpClient_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutNetwork_CHttpClient_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutNetwork_CHttpClient_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutNetwork_CHttpClient_delete00);
    tolua_function(tolua_S,"HttpGet",tolua_Scut_ScutNetwork_CHttpClient_HttpGet00);
    tolua_function(tolua_S,"HttpPost",tolua_Scut_ScutNetwork_CHttpClient_HttpPost00);
    tolua_function(tolua_S,"AsyncHttpGet",tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpGet00);
    tolua_function(tolua_S,"AsyncHttpPost",tolua_Scut_ScutNetwork_CHttpClient_AsyncHttpPost00);
    tolua_function(tolua_S,"AsyncNetGet",tolua_Scut_ScutNetwork_CHttpClient_AsyncNetGet00);
    tolua_function(tolua_S,"GetNetType",tolua_Scut_ScutNetwork_CHttpClient_GetNetType00);
    tolua_function(tolua_S,"AddHeader",tolua_Scut_ScutNetwork_CHttpClient_AddHeader00);
    tolua_function(tolua_S,"UseHttpProxy",tolua_Scut_ScutNetwork_CHttpClient_UseHttpProxy00);
    tolua_function(tolua_S,"SetHttpProxy",tolua_Scut_ScutNetwork_CHttpClient_SetHttpProxy00);
    tolua_function(tolua_S,"UseHttpsProxy",tolua_Scut_ScutNetwork_CHttpClient_UseHttpsProxy00);
    tolua_function(tolua_S,"SetHttpsProxy",tolua_Scut_ScutNetwork_CHttpClient_SetHttpsProxy00);
    tolua_function(tolua_S,"FullReset",tolua_Scut_ScutNetwork_CHttpClient_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_Scut_ScutNetwork_CHttpClient_Reset00);
    tolua_function(tolua_S,"GetUrlHost",tolua_Scut_ScutNetwork_CHttpClient_GetUrlHost00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutNetwork",0);
  tolua_beginmodule(tolua_S,"ScutNetwork");
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CTcpClient","ScutNetwork::CTcpClient","ScutNetwork::CNetClientBase",tolua_collect_ScutNetwork__CTcpClient);
   #else
   tolua_cclass(tolua_S,"CTcpClient","ScutNetwork::CTcpClient","ScutNetwork::CNetClientBase",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CTcpClient");
    tolua_function(tolua_S,"new",tolua_Scut_ScutNetwork_CTcpClient_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutNetwork_CTcpClient_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutNetwork_CTcpClient_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutNetwork_CTcpClient_delete00);
    tolua_function(tolua_S,"TcpGet",tolua_Scut_ScutNetwork_CTcpClient_TcpGet00);
    tolua_function(tolua_S,"TcpGet",tolua_Scut_ScutNetwork_CTcpClient_TcpGet01);
    tolua_function(tolua_S,"AsyncTcpGet",tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet00);
    tolua_function(tolua_S,"AsyncTcpGet",tolua_Scut_ScutNetwork_CTcpClient_AsyncTcpGet01);
    tolua_function(tolua_S,"AsyncNetGet",tolua_Scut_ScutNetwork_CTcpClient_AsyncNetGet00);
    tolua_function(tolua_S,"FullReset",tolua_Scut_ScutNetwork_CTcpClient_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_Scut_ScutNetwork_CTcpClient_Reset00);
    tolua_function(tolua_S,"GetHost",tolua_Scut_ScutNetwork_CTcpClient_GetHost00);
    tolua_function(tolua_S,"GetPort",tolua_Scut_ScutNetwork_CTcpClient_GetPort00);
    tolua_function(tolua_S,"GetUrlHost",tolua_Scut_ScutNetwork_CTcpClient_GetUrlHost00);
    tolua_function(tolua_S,"wait_on_socket",tolua_Scut_ScutNetwork_CTcpClient_wait_on_socket00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutNetwork",0);
  tolua_beginmodule(tolua_S,"ScutNetwork");
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CHttpClientResponse","ScutNetwork::CHttpClientResponse","",tolua_collect_ScutNetwork__CHttpClientResponse);
   #else
   tolua_cclass(tolua_S,"CHttpClientResponse","ScutNetwork::CHttpClientResponse","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CHttpClientResponse");
    tolua_function(tolua_S,"new",tolua_Scut_ScutNetwork_CHttpClientResponse_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutNetwork_CHttpClientResponse_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutNetwork_CHttpClientResponse_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutNetwork_CHttpClientResponse_delete00);
    tolua_function(tolua_S,"GetBodyPtr",tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyPtr00);
    tolua_function(tolua_S,"GetBodyLength",tolua_Scut_ScutNetwork_CHttpClientResponse_GetBodyLength00);
    tolua_function(tolua_S,"DataContains",tolua_Scut_ScutNetwork_CHttpClientResponse_DataContains00);
    tolua_function(tolua_S,"ContentTypeContains",tolua_Scut_ScutNetwork_CHttpClientResponse_ContentTypeContains00);
    tolua_function(tolua_S,"Reset",tolua_Scut_ScutNetwork_CHttpClientResponse_Reset00);
    tolua_function(tolua_S,"SetContentType",tolua_Scut_ScutNetwork_CHttpClientResponse_SetContentType00);
    tolua_function(tolua_S,"GetTargetFile",tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetFile00);
    tolua_function(tolua_S,"SetTargetFile",tolua_Scut_ScutNetwork_CHttpClientResponse_SetTargetFile00);
    tolua_function(tolua_S,"GetTarget",tolua_Scut_ScutNetwork_CHttpClientResponse_GetTarget00);
    tolua_function(tolua_S,"SetTarget",tolua_Scut_ScutNetwork_CHttpClientResponse_SetTarget00);
    tolua_function(tolua_S,"GetUseDataResume",tolua_Scut_ScutNetwork_CHttpClientResponse_GetUseDataResume00);
    tolua_function(tolua_S,"SetUseDataResume",tolua_Scut_ScutNetwork_CHttpClientResponse_SetUseDataResume00);
    tolua_function(tolua_S,"GetRequestUrl",tolua_Scut_ScutNetwork_CHttpClientResponse_GetRequestUrl00);
    tolua_function(tolua_S,"SetRequestUrl",tolua_Scut_ScutNetwork_CHttpClientResponse_SetRequestUrl00);
    tolua_function(tolua_S,"GetLastResponseUrl",tolua_Scut_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00);
    tolua_function(tolua_S,"SetLastResponseUrl",tolua_Scut_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00);
    tolua_function(tolua_S,"GetTargetRawSize",tolua_Scut_ScutNetwork_CHttpClientResponse_GetTargetRawSize00);
    tolua_function(tolua_S,"GetStatusCode",tolua_Scut_ScutNetwork_CHttpClientResponse_GetStatusCode00);
    tolua_function(tolua_S,"SetStatusCode",tolua_Scut_ScutNetwork_CHttpClientResponse_SetStatusCode00);
    tolua_function(tolua_S,"SetSendData",tolua_Scut_ScutNetwork_CHttpClientResponse_SetSendData00);
    tolua_function(tolua_S,"GetSendData",tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendData00);
    tolua_function(tolua_S,"GetSendDataLength",tolua_Scut_ScutNetwork_CHttpClientResponse_GetSendDataLength00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutNetwork",0);
  tolua_beginmodule(tolua_S,"ScutNetwork");
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CHttpSession","ScutNetwork::CHttpSession","",tolua_collect_ScutNetwork__CHttpSession);
   #else
   tolua_cclass(tolua_S,"CHttpSession","ScutNetwork::CHttpSession","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CHttpSession");
    tolua_function(tolua_S,"DeleteCookies",tolua_Scut_ScutNetwork_CHttpSession_DeleteCookies00);
    tolua_variable(tolua_S,"cookieJar",tolua_get_ScutNetwork__CHttpSession_cookieJar,tolua_set_ScutNetwork__CHttpSession_cookieJar);
    tolua_function(tolua_S,"Initialize",tolua_Scut_ScutNetwork_CHttpSession_Initialize00);
    tolua_function(tolua_S,"GetCookies",tolua_Scut_ScutNetwork_CHttpSession_GetCookies00);
    tolua_function(tolua_S,"AddCookie",tolua_Scut_ScutNetwork_CHttpSession_AddCookie00);
    tolua_function(tolua_S,"new",tolua_Scut_ScutNetwork_CHttpSession_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutNetwork_CHttpSession_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutNetwork_CHttpSession_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutNetwork_CHttpSession_delete00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_function(tolua_S,"PrintMD5",tolua_Scut_PrintMD500);
  tolua_function(tolua_S,"MD5String",tolua_Scut_MD5String00);
  tolua_function(tolua_S,"MD5File",tolua_Scut_MD5File00);
  #ifdef __cplusplus
  tolua_cclass(tolua_S,"md5","md5","",tolua_collect_md5);
  #else
  tolua_cclass(tolua_S,"md5","md5","",NULL);
  #endif
  tolua_beginmodule(tolua_S,"md5");
   tolua_function(tolua_S,"new",tolua_Scut_md5_new00);
   tolua_function(tolua_S,"new_local",tolua_Scut_md5_new00_local);
   tolua_function(tolua_S,".call",tolua_Scut_md5_new00_local);
   tolua_function(tolua_S,"Init",tolua_Scut_md5_Init00);
   tolua_function(tolua_S,"Update",tolua_Scut_md5_Update00);
   tolua_function(tolua_S,"Finalize",tolua_Scut_md5_Finalize00);
   tolua_function(tolua_S,"Digest",tolua_Scut_md5_Digest00);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutSystem",0);
  tolua_beginmodule(tolua_S,"ScutSystem");
   tolua_cclass(tolua_S,"CScutUtility","ScutSystem::CScutUtility","",NULL);
   tolua_beginmodule(tolua_S,"CScutUtility");
    tolua_function(tolua_S,"DesEncrypt",tolua_Scut_ScutSystem_CScutUtility_DesEncrypt00);
    tolua_function(tolua_S,"StdDesEncrypt",tolua_Scut_ScutSystem_CScutUtility_StdDesEncrypt00);
    tolua_function(tolua_S,"StdDesDecrypt",tolua_Scut_ScutSystem_CScutUtility_StdDesDecrypt00);
    tolua_function(tolua_S,"GetTickCount",tolua_Scut_ScutSystem_CScutUtility_GetTickCount00);
    tolua_function(tolua_S,"CScutUtility__GetNowTime",tolua_Scut_ScutSystem_CScutUtility_CScutUtility__GetNowTime00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutSystem",0);
  tolua_beginmodule(tolua_S,"ScutSystem");
   tolua_constant(tolua_S,"soBegin",ScutSystem::soBegin);
   tolua_constant(tolua_S,"soCurrent",ScutSystem::soCurrent);
   tolua_constant(tolua_S,"soEnd",ScutSystem::soEnd);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CStream","ScutSystem::CStream","",tolua_collect_ScutSystem__CStream);
   #else
   tolua_cclass(tolua_S,"CStream","ScutSystem::CStream","",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CStream");
    tolua_function(tolua_S,"delete",tolua_Scut_ScutSystem_CStream_delete00);
    tolua_function(tolua_S,"GetPosition",tolua_Scut_ScutSystem_CStream_GetPosition00);
    tolua_function(tolua_S,"SetPosition",tolua_Scut_ScutSystem_CStream_SetPosition00);
    tolua_function(tolua_S,"GetSize",tolua_Scut_ScutSystem_CStream_GetSize00);
    tolua_function(tolua_S,"SetSize",tolua_Scut_ScutSystem_CStream_SetSize00);
    tolua_function(tolua_S,"Read",tolua_Scut_ScutSystem_CStream_Read00);
    tolua_function(tolua_S,"Write",tolua_Scut_ScutSystem_CStream_Write00);
    tolua_function(tolua_S,"Seek",tolua_Scut_ScutSystem_CStream_Seek00);
    tolua_function(tolua_S,"ReadBuffer",tolua_Scut_ScutSystem_CStream_ReadBuffer00);
    tolua_function(tolua_S,"WriteBuffer",tolua_Scut_ScutSystem_CStream_WriteBuffer00);
    tolua_function(tolua_S,"CopyFrom",tolua_Scut_ScutSystem_CStream_CopyFrom00);
    tolua_function(tolua_S,"GetBuffer",tolua_Scut_ScutSystem_CStream_GetBuffer00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CHandleStream","ScutSystem::CHandleStream","ScutSystem::CStream",tolua_collect_ScutSystem__CHandleStream);
   #else
   tolua_cclass(tolua_S,"CHandleStream","ScutSystem::CHandleStream","ScutSystem::CStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CHandleStream");
    tolua_function(tolua_S,"new",tolua_Scut_ScutSystem_CHandleStream_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutSystem_CHandleStream_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutSystem_CHandleStream_new00_local);
    tolua_function(tolua_S,"SetSize",tolua_Scut_ScutSystem_CHandleStream_SetSize00);
    tolua_function(tolua_S,"Read",tolua_Scut_ScutSystem_CHandleStream_Read00);
    tolua_function(tolua_S,"Write",tolua_Scut_ScutSystem_CHandleStream_Write00);
    tolua_function(tolua_S,"Seek",tolua_Scut_ScutSystem_CHandleStream_Seek00);
    tolua_function(tolua_S,"GetHandle",tolua_Scut_ScutSystem_CHandleStream_GetHandle00);
    tolua_function(tolua_S,"SetHandle",tolua_Scut_ScutSystem_CHandleStream_SetHandle00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CFileStream","ScutSystem::CFileStream","ScutSystem::CHandleStream",tolua_collect_ScutSystem__CFileStream);
   #else
   tolua_cclass(tolua_S,"CFileStream","ScutSystem::CFileStream","ScutSystem::CHandleStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CFileStream");
    tolua_function(tolua_S,"new",tolua_Scut_ScutSystem_CFileStream_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutSystem_CFileStream_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutSystem_CFileStream_new00_local);
    tolua_function(tolua_S,"Open",tolua_Scut_ScutSystem_CFileStream_Open00);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutSystem_CFileStream_delete00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CBaseMemoryStream","ScutSystem::CBaseMemoryStream","ScutSystem::CStream",tolua_collect_ScutSystem__CBaseMemoryStream);
   #else
   tolua_cclass(tolua_S,"CBaseMemoryStream","ScutSystem::CBaseMemoryStream","ScutSystem::CStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CBaseMemoryStream");
    tolua_function(tolua_S,"SetSize",tolua_Scut_ScutSystem_CBaseMemoryStream_SetSize00);
    tolua_function(tolua_S,"Write",tolua_Scut_ScutSystem_CBaseMemoryStream_Write00);
    tolua_function(tolua_S,"Read",tolua_Scut_ScutSystem_CBaseMemoryStream_Read00);
    tolua_function(tolua_S,"Seek",tolua_Scut_ScutSystem_CBaseMemoryStream_Seek00);
    tolua_function(tolua_S,"SaveTo",tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo00);
    tolua_function(tolua_S,"SaveTo",tolua_Scut_ScutSystem_CBaseMemoryStream_SaveTo01);
    tolua_function(tolua_S,"GetMemory",tolua_Scut_ScutSystem_CBaseMemoryStream_GetMemory00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CMemoryStream","ScutSystem::CMemoryStream","ScutSystem::CBaseMemoryStream",tolua_collect_ScutSystem__CMemoryStream);
   #else
   tolua_cclass(tolua_S,"CMemoryStream","ScutSystem::CMemoryStream","ScutSystem::CBaseMemoryStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CMemoryStream");
    tolua_function(tolua_S,"new",tolua_Scut_ScutSystem_CMemoryStream_new00);
    tolua_function(tolua_S,"new_local",tolua_Scut_ScutSystem_CMemoryStream_new00_local);
    tolua_function(tolua_S,".call",tolua_Scut_ScutSystem_CMemoryStream_new00_local);
    tolua_function(tolua_S,"delete",tolua_Scut_ScutSystem_CMemoryStream_delete00);
    tolua_function(tolua_S,"Clear",tolua_Scut_ScutSystem_CMemoryStream_Clear00);
    tolua_function(tolua_S,"LoadFrom",tolua_Scut_ScutSystem_CMemoryStream_LoadFrom00);
    tolua_function(tolua_S,"LoadFrom",tolua_Scut_ScutSystem_CMemoryStream_LoadFrom01);
    tolua_function(tolua_S,"SetSize",tolua_Scut_ScutSystem_CMemoryStream_SetSize00);
    tolua_function(tolua_S,"Write",tolua_Scut_ScutSystem_CMemoryStream_Write00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_constant(tolua_S,"ptWin32",ScutUtility::ptWin32);
   tolua_constant(tolua_S,"ptiPod",ScutUtility::ptiPod);
   tolua_constant(tolua_S,"ptiPad",ScutUtility::ptiPad);
   tolua_constant(tolua_S,"ptiPhone",ScutUtility::ptiPhone);
   tolua_constant(tolua_S,"ptiPhone_AppStore",ScutUtility::ptiPhone_AppStore);
   tolua_constant(tolua_S,"ptANDROID",ScutUtility::ptANDROID);
   tolua_constant(tolua_S,"ptMac",ScutUtility::ptMac);
   tolua_constant(tolua_S,"ptwindowsPhone7",ScutUtility::ptwindowsPhone7);
   tolua_constant(tolua_S,"ptUnknow",ScutUtility::ptUnknow);
   tolua_constant(tolua_S,"antNone",ScutUtility::antNone);
   tolua_constant(tolua_S,"antWIFI",ScutUtility::antWIFI);
   tolua_constant(tolua_S,"ant2G",ScutUtility::ant2G);
   tolua_constant(tolua_S,"ant3G",ScutUtility::ant3G);
   tolua_cclass(tolua_S,"ScutUtils","ScutUtility::ScutUtils","",NULL);
   tolua_beginmodule(tolua_S,"ScutUtils");
    tolua_function(tolua_S,"GetPlatformType",tolua_Scut_ScutUtility_ScutUtils_GetPlatformType00);
    tolua_function(tolua_S,"getImsi",tolua_Scut_ScutUtility_ScutUtils_getImsi00);
    tolua_function(tolua_S,"getImei",tolua_Scut_ScutUtility_ScutUtils_getImei00);
    tolua_function(tolua_S,"scheduleLocalNotification",tolua_Scut_ScutUtility_ScutUtils_scheduleLocalNotification00);
    tolua_function(tolua_S,"cancelLocalNotification",tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotification00);
    tolua_function(tolua_S,"cancelLocalNotifications",tolua_Scut_ScutUtility_ScutUtils_cancelLocalNotifications00);
    tolua_function(tolua_S,"getMacAddress",tolua_Scut_ScutUtility_ScutUtils_getMacAddress00);
    tolua_function(tolua_S,"setTextToClipBoard",tolua_Scut_ScutUtility_ScutUtils_setTextToClipBoard00);
    tolua_function(tolua_S,"getTextFromClipBoard",tolua_Scut_ScutUtility_ScutUtils_getTextFromClipBoard00);
    tolua_function(tolua_S,"launchApp",tolua_Scut_ScutUtility_ScutUtils_launchApp00);
    tolua_function(tolua_S,"installPackage",tolua_Scut_ScutUtility_ScutUtils_installPackage00);
    tolua_function(tolua_S,"checkAppInstalled",tolua_Scut_ScutUtility_ScutUtils_checkAppInstalled00);
    tolua_function(tolua_S,"registerWebviewCallback",tolua_Scut_ScutUtility_ScutUtils_registerWebviewCallback00);
    tolua_function(tolua_S,"getInstalledApps",tolua_Scut_ScutUtility_ScutUtils_getInstalledApps00);
    tolua_function(tolua_S,"getCurrentAppId",tolua_Scut_ScutUtility_ScutUtils_getCurrentAppId00);
    tolua_function(tolua_S,"GoBack",tolua_Scut_ScutUtility_ScutUtils_GoBack00);
    tolua_function(tolua_S,"getOpenUrlData",tolua_Scut_ScutUtility_ScutUtils_getOpenUrlData00);
    tolua_function(tolua_S,"isJailBroken",tolua_Scut_ScutUtility_ScutUtils_isJailBroken00);
    tolua_function(tolua_S,"getActiveNetworkInfo",tolua_Scut_ScutUtility_ScutUtils_getActiveNetworkInfo00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CLocale","ScutUtility::CLocale","",NULL);
   tolua_beginmodule(tolua_S,"CLocale");
    tolua_function(tolua_S,"setLanguage",tolua_Scut_ScutUtility_CLocale_setLanguage00);
    tolua_function(tolua_S,"getLanguage",tolua_Scut_ScutUtility_CLocale_getLanguage00);
    tolua_function(tolua_S,"getImsi",tolua_Scut_ScutUtility_CLocale_getImsi00);
    tolua_function(tolua_S,"getImei",tolua_Scut_ScutUtility_CLocale_getImei00);
    tolua_function(tolua_S,"setImsi",tolua_Scut_ScutUtility_CLocale_setImsi00);
    tolua_function(tolua_S,"isSDCardExist",tolua_Scut_ScutUtility_CLocale_isSDCardExist00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CScutLuaLan","ScutUtility::CScutLuaLan","",NULL);
   tolua_beginmodule(tolua_S,"CScutLuaLan");
    tolua_function(tolua_S,"instance",tolua_Scut_ScutUtility_CScutLuaLan_instance00);
    tolua_function(tolua_S,"Add",tolua_Scut_ScutUtility_CScutLuaLan_Add00);
    tolua_function(tolua_S,"Switch",tolua_Scut_ScutUtility_CScutLuaLan_Switch00);
    tolua_function(tolua_S,"Remove",tolua_Scut_ScutUtility_CScutLuaLan_Remove00);
    tolua_function(tolua_S,"Get",tolua_Scut_ScutUtility_CScutLuaLan_Get00);
    tolua_function(tolua_S,"RemoveAll",tolua_Scut_ScutUtility_CScutLuaLan_RemoveAll00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CScutLanFactory","ScutUtility::CScutLanFactory","",NULL);
   tolua_beginmodule(tolua_S,"CScutLanFactory");
    tolua_function(tolua_S,"GetLanInstance",tolua_Scut_ScutUtility_CScutLanFactory_GetLanInstance00);
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CScutLan","ScutUtility::CScutLan","",NULL);
   tolua_beginmodule(tolua_S,"CScutLan");
    tolua_variable(tolua_S,"m_szOK",tolua_get_ScutUtility__CScutLan_m_szOK,tolua_set_ScutUtility__CScutLan_m_szOK);
    tolua_variable(tolua_S,"m_szCancel",tolua_get_ScutUtility__CScutLan_m_szCancel,tolua_set_ScutUtility__CScutLan_m_szCancel);
    tolua_variable(tolua_S,"m_szKnown",tolua_get_ScutUtility__CScutLan_m_szKnown,tolua_set_ScutUtility__CScutLan_m_szKnown);
    tolua_variable(tolua_S,"m_szTimeOut",tolua_get_ScutUtility__CScutLan_m_szTimeOut,tolua_set_ScutUtility__CScutLan_m_szTimeOut);
    tolua_variable(tolua_S,"m_szFalseConnect",tolua_get_ScutUtility__CScutLan_m_szFalseConnect,tolua_set_ScutUtility__CScutLan_m_szFalseConnect);
    tolua_variable(tolua_S,"m_szUpdateError",tolua_get_ScutUtility__CScutLan_m_szUpdateError,tolua_set_ScutUtility__CScutLan_m_szUpdateError);
    tolua_variable(tolua_S,"m_szDownload",tolua_get_ScutUtility__CScutLan_m_szDownload,tolua_set_ScutUtility__CScutLan_m_szDownload);
    tolua_variable(tolua_S,"m_szExit",tolua_get_ScutUtility__CScutLan_m_szExit,tolua_set_ScutUtility__CScutLan_m_szExit);
    tolua_variable(tolua_S,"m_szSDNotExist",tolua_get_ScutUtility__CScutLan_m_szSDNotExist,tolua_set_ScutUtility__CScutLan_m_szSDNotExist);
    tolua_variable(tolua_S,"m_szSDReadError",tolua_get_ScutUtility__CScutLan_m_szSDReadError,tolua_set_ScutUtility__CScutLan_m_szSDReadError);
    tolua_variable(tolua_S,"m_szResPackageLoadingTip",tolua_get_ScutUtility__CScutLan_m_szResPackageLoadingTip,tolua_set_ScutUtility__CScutLan_m_szResPackageLoadingTip);
    tolua_variable(tolua_S,"m_szResPackageFinishTip",tolua_get_ScutUtility__CScutLan_m_szResPackageFinishTip,tolua_set_ScutUtility__CScutLan_m_szResPackageFinishTip);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_cclass(tolua_S,"ScutExt","ScutExt","",NULL);
  tolua_beginmodule(tolua_S,"ScutExt");
   tolua_function(tolua_S,"Init",tolua_Scut_ScutExt_Init00);
   tolua_function(tolua_S,"getInstance",tolua_Scut_ScutExt_getInstance00);
   tolua_function(tolua_S,"RegisterPauseHandler",tolua_Scut_ScutExt_RegisterPauseHandler00);
   tolua_function(tolua_S,"RegisterResumeHandler",tolua_Scut_ScutExt_RegisterResumeHandler00);
   tolua_function(tolua_S,"RegisterBackHandler",tolua_Scut_ScutExt_RegisterBackHandler00);
   tolua_function(tolua_S,"RegisterErrorHandler",tolua_Scut_ScutExt_RegisterErrorHandler00);
   tolua_function(tolua_S,"RegisterSocketPushHandler",tolua_Scut_ScutExt_RegisterSocketPushHandler00);
   tolua_function(tolua_S,"GetSocketPushHandler",tolua_Scut_ScutExt_GetSocketPushHandler00);
   tolua_function(tolua_S,"RegisterSocketErrorHandler",tolua_Scut_ScutExt_RegisterSocketErrorHandler00);
   tolua_function(tolua_S,"GetSocketErrorHandler",tolua_Scut_ScutExt_GetSocketErrorHandler00);
   tolua_function(tolua_S,"UnregisterErrorHandler",tolua_Scut_ScutExt_UnregisterErrorHandler00);
   tolua_function(tolua_S,"GetErrorHandler",tolua_Scut_ScutExt_GetErrorHandler00);
   tolua_function(tolua_S,"ExcuteBackHandler",tolua_Scut_ScutExt_ExcuteBackHandler00);
   tolua_function(tolua_S,"EndDirector",tolua_Scut_ScutExt_EndDirector00);
   tolua_function(tolua_S,"PauseDirector",tolua_Scut_ScutExt_PauseDirector00);
   tolua_function(tolua_S,"ResumeDirector",tolua_Scut_ScutExt_ResumeDirector00);
   tolua_function(tolua_S,"pushfunc",tolua_Scut_ScutExt_pushfunc00);
   tolua_function(tolua_S,"executeLogEvent",tolua_Scut_ScutExt_executeLogEvent00);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_Scut (lua_State* tolua_S) {
 return tolua_Scut_open(tolua_S);
};
#endif

