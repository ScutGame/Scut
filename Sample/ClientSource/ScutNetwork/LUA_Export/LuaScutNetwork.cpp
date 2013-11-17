/*
** Lua binding: ScutNetwork
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:19:21.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutNetwork_open (lua_State* tolua_S);

#include"../NetClientBase.h"
#include"../HttpClient.h"
#include"../TcpClient.h"
#include"../HttpClientResponse.h"
#include"../HttpSession.h"

/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutNetwork__AsyncInfo (lua_State* tolua_S)
{
 ScutNetwork::AsyncInfo* self = (ScutNetwork::AsyncInfo*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpSession (lua_State* tolua_S)
{
 ScutNetwork::CHttpSession* self = (ScutNetwork::CHttpSession*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpClientResponse (lua_State* tolua_S)
{
 ScutNetwork::CHttpClientResponse* self = (ScutNetwork::CHttpClientResponse*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_bool (lua_State* tolua_S)
{
 bool* self = (bool*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CHttpClient (lua_State* tolua_S)
{
 ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutNetwork__CTcpClient (lua_State* tolua_S)
{
 ScutNetwork::CTcpClient* self = (ScutNetwork::CTcpClient*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_CScutString (lua_State* tolua_S)
{
 CScutString* self = (CScutString*) tolua_tousertype(tolua_S,1,0);
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
 tolua_usertype(tolua_S,"ScutNetwork::CTcpClient");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::CTcpClient), "ScutNetwork::CTcpClient");
 tolua_usertype(tolua_S,"ScutNetwork::CNetClientBase");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::CNetClientBase), "ScutNetwork::CNetClientBase");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpClientResponse");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::CHttpClientResponse), "ScutNetwork::CHttpClientResponse");
 tolua_usertype(tolua_S,"bool");
 Mtolua_typeid(tolua_S,typeid(bool), "bool");
 tolua_usertype(tolua_S,"ScutNetwork::INetStatusNotify");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::INetStatusNotify), "ScutNetwork::INetStatusNotify");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpSession");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::CHttpSession), "ScutNetwork::CHttpSession");
 tolua_usertype(tolua_S,"CScutString");
 Mtolua_typeid(tolua_S,typeid(CScutString), "CScutString");
 tolua_usertype(tolua_S,"ScutSystem::CStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CStream), "ScutSystem::CStream");
 tolua_usertype(tolua_S,"ScutNetwork::CHttpClient");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::CHttpClient), "ScutNetwork::CHttpClient");
 tolua_usertype(tolua_S,"ScutNetwork::AsyncInfo");
 Mtolua_typeid(tolua_S,typeid(ScutNetwork::AsyncInfo), "ScutNetwork::AsyncInfo");
 tolua_usertype(tolua_S,"CURL");
 Mtolua_typeid(tolua_S,typeid(CURL), "CURL");
}

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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00
static int tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00_local
static int tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_AsyncInfo_delete00
static int tolua_ScutNetwork_ScutNetwork_AsyncInfo_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_AsyncInfo_Reset00
static int tolua_ScutNetwork_ScutNetwork_AsyncInfo_Reset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_AsyncNetGet00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_AsyncNetGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetNetStautsNotify00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetNetStautsNotify00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetNetStautsNotify00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetNetStautsNotify00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_AddHeader00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_AddHeader00(lua_State* tolua_S)
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
   bool tolua_ret = (bool)  self->AddHeader(name,value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((bool)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(bool));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetTimeOut00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetTimeOut00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetTimeOut00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetTimeOut00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetUseProgressReport00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetUseProgressReport00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetUseProgressReport00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetUseProgressReport00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_FullReset00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_FullReset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_Reset00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_Reset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetHost00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetHost00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_IsBusy00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_IsBusy00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetAsyncInfo00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetAsyncInfo00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetCurlHandle00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetCurlHandle00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00
static int tolua_ScutNetwork_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_new00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_new00_local
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_delete00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpGet00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpPost00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpPost00(lua_State* tolua_S)
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
   bool tolua_ret = (bool)  self->HttpPost(url,postData,nPostDataSize,*resp,formflag);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((bool)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(bool));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpGet00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpPost00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpPost00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncNetGet00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncNetGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_GetNetType00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_GetNetType00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_AddHeader00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_AddHeader00(lua_State* tolua_S)
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
   bool tolua_ret = (bool)  self->AddHeader(name,value);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((bool)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(bool));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpProxy00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"bool",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  bool bUseProxy = *((bool*)  tolua_tousertype(tolua_S,2,0));
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpProxy00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpProxy00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpsProxy00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpsProxy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutNetwork::CHttpClient",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"bool",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutNetwork::CHttpClient* self = (ScutNetwork::CHttpClient*)  tolua_tousertype(tolua_S,1,0);
  bool bUseProxy = *((bool*)  tolua_tousertype(tolua_S,2,0));
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpsProxy00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpsProxy00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_FullReset00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_FullReset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_Reset00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_Reset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClient_GetUrlHost00
static int tolua_ScutNetwork_ScutNetwork_CHttpClient_GetUrlHost00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_new00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_new00_local
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_delete00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet01
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet01(lua_State* tolua_S)
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
 return tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncTcpGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet01
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet01(lua_State* tolua_S)
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
 return tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: AsyncNetGet of class  ScutNetwork::CTcpClient */
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncNetGet00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncNetGet00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_FullReset00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_FullReset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_Reset00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_Reset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_GetHost00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_GetHost00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_GetPort00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_GetPort00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_GetUrlHost00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_GetUrlHost00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CTcpClient_wait_on_socket00
static int tolua_ScutNetwork_ScutNetwork_CTcpClient_wait_on_socket00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00_local
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_delete00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyPtr00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyPtr00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyLength00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyLength00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_DataContains00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_DataContains00(lua_State* tolua_S)
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
   bool tolua_ret = (bool)  self->DataContains(searchStr);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((bool)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(bool));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_ContentTypeContains00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_ContentTypeContains00(lua_State* tolua_S)
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
   bool tolua_ret = (bool)  self->ContentTypeContains(searchStr);
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((bool)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(bool));
     tolua_pushusertype(tolua_S,tolua_obj,"bool");
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_Reset00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_Reset00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetContentType00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetContentType00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetFile00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetFile00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTargetFile00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTargetFile00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTarget00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTarget00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTarget00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTarget00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetUseDataResume00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetUseDataResume00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetUseDataResume00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetUseDataResume00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetRequestUrl00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetRequestUrl00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetRequestUrl00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetRequestUrl00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetRawSize00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetRawSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetStatusCode00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetStatusCode00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetStatusCode00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetStatusCode00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetSendData00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetSendData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendData00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendDataLength00
static int tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendDataLength00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_DeleteCookies00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_DeleteCookies00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_Initialize00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_Initialize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_GetCookies00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_GetCookies00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_AddCookie00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_AddCookie00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_new00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_new00_local
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutNetwork_ScutNetwork_CHttpSession_delete00
static int tolua_ScutNetwork_ScutNetwork_CHttpSession_delete00(lua_State* tolua_S)
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

/* Open function */
TOLUA_API int tolua_ScutNetwork_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
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
    tolua_function(tolua_S,"new",tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutNetwork_ScutNetwork_AsyncInfo_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutNetwork_ScutNetwork_AsyncInfo_delete00);
    tolua_function(tolua_S,"Reset",tolua_ScutNetwork_ScutNetwork_AsyncInfo_Reset00);
   tolua_endmodule(tolua_S);
   tolua_cclass(tolua_S,"CNetClientBase","ScutNetwork::CNetClientBase","",NULL);
   tolua_beginmodule(tolua_S,"CNetClientBase");
    tolua_function(tolua_S,"AsyncNetGet",tolua_ScutNetwork_ScutNetwork_CNetClientBase_AsyncNetGet00);
    tolua_function(tolua_S,"SetNetStautsNotify",tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetNetStautsNotify00);
    tolua_function(tolua_S,"GetNetStautsNotify",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetNetStautsNotify00);
    tolua_function(tolua_S,"AddHeader",tolua_ScutNetwork_ScutNetwork_CNetClientBase_AddHeader00);
    tolua_function(tolua_S,"GetTimeOut",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetTimeOut00);
    tolua_function(tolua_S,"SetTimeOut",tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetTimeOut00);
    tolua_function(tolua_S,"GetUseProgressReport",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetUseProgressReport00);
    tolua_function(tolua_S,"SetUseProgressReport",tolua_ScutNetwork_ScutNetwork_CNetClientBase_SetUseProgressReport00);
    tolua_function(tolua_S,"FullReset",tolua_ScutNetwork_ScutNetwork_CNetClientBase_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_ScutNetwork_ScutNetwork_CNetClientBase_Reset00);
    tolua_function(tolua_S,"GetHost",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetHost00);
    tolua_function(tolua_S,"IsBusy",tolua_ScutNetwork_ScutNetwork_CNetClientBase_IsBusy00);
    tolua_function(tolua_S,"GetAsyncInfo",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetAsyncInfo00);
    tolua_function(tolua_S,"GetCurlHandle",tolua_ScutNetwork_ScutNetwork_CNetClientBase_GetCurlHandle00);
    tolua_function(tolua_S,"RegisterCustomLuaHandle",tolua_ScutNetwork_ScutNetwork_CNetClientBase_RegisterCustomLuaHandle00);
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
    tolua_function(tolua_S,"new",tolua_ScutNetwork_ScutNetwork_CHttpClient_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutNetwork_ScutNetwork_CHttpClient_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutNetwork_ScutNetwork_CHttpClient_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutNetwork_ScutNetwork_CHttpClient_delete00);
    tolua_function(tolua_S,"HttpGet",tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpGet00);
    tolua_function(tolua_S,"HttpPost",tolua_ScutNetwork_ScutNetwork_CHttpClient_HttpPost00);
    tolua_function(tolua_S,"AsyncHttpGet",tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpGet00);
    tolua_function(tolua_S,"AsyncHttpPost",tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncHttpPost00);
    tolua_function(tolua_S,"AsyncNetGet",tolua_ScutNetwork_ScutNetwork_CHttpClient_AsyncNetGet00);
    tolua_function(tolua_S,"GetNetType",tolua_ScutNetwork_ScutNetwork_CHttpClient_GetNetType00);
    tolua_function(tolua_S,"AddHeader",tolua_ScutNetwork_ScutNetwork_CHttpClient_AddHeader00);
    tolua_function(tolua_S,"UseHttpProxy",tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpProxy00);
    tolua_function(tolua_S,"SetHttpProxy",tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpProxy00);
    tolua_function(tolua_S,"UseHttpsProxy",tolua_ScutNetwork_ScutNetwork_CHttpClient_UseHttpsProxy00);
    tolua_function(tolua_S,"SetHttpsProxy",tolua_ScutNetwork_ScutNetwork_CHttpClient_SetHttpsProxy00);
    tolua_function(tolua_S,"FullReset",tolua_ScutNetwork_ScutNetwork_CHttpClient_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_ScutNetwork_ScutNetwork_CHttpClient_Reset00);
    tolua_function(tolua_S,"GetUrlHost",tolua_ScutNetwork_ScutNetwork_CHttpClient_GetUrlHost00);
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
    tolua_function(tolua_S,"new",tolua_ScutNetwork_ScutNetwork_CTcpClient_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutNetwork_ScutNetwork_CTcpClient_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutNetwork_ScutNetwork_CTcpClient_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutNetwork_ScutNetwork_CTcpClient_delete00);
    tolua_function(tolua_S,"TcpGet",tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet00);
    tolua_function(tolua_S,"TcpGet",tolua_ScutNetwork_ScutNetwork_CTcpClient_TcpGet01);
    tolua_function(tolua_S,"AsyncTcpGet",tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet00);
    tolua_function(tolua_S,"AsyncTcpGet",tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncTcpGet01);
    tolua_function(tolua_S,"AsyncNetGet",tolua_ScutNetwork_ScutNetwork_CTcpClient_AsyncNetGet00);
    tolua_function(tolua_S,"FullReset",tolua_ScutNetwork_ScutNetwork_CTcpClient_FullReset00);
    tolua_function(tolua_S,"Reset",tolua_ScutNetwork_ScutNetwork_CTcpClient_Reset00);
    tolua_function(tolua_S,"GetHost",tolua_ScutNetwork_ScutNetwork_CTcpClient_GetHost00);
    tolua_function(tolua_S,"GetPort",tolua_ScutNetwork_ScutNetwork_CTcpClient_GetPort00);
    tolua_function(tolua_S,"GetUrlHost",tolua_ScutNetwork_ScutNetwork_CTcpClient_GetUrlHost00);
    tolua_function(tolua_S,"wait_on_socket",tolua_ScutNetwork_ScutNetwork_CTcpClient_wait_on_socket00);
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
    tolua_function(tolua_S,"new",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_delete00);
    tolua_function(tolua_S,"GetBodyPtr",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyPtr00);
    tolua_function(tolua_S,"GetBodyLength",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetBodyLength00);
    tolua_function(tolua_S,"DataContains",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_DataContains00);
    tolua_function(tolua_S,"ContentTypeContains",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_ContentTypeContains00);
    tolua_function(tolua_S,"Reset",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_Reset00);
    tolua_function(tolua_S,"SetContentType",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetContentType00);
    tolua_function(tolua_S,"GetTargetFile",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetFile00);
    tolua_function(tolua_S,"SetTargetFile",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTargetFile00);
    tolua_function(tolua_S,"GetTarget",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTarget00);
    tolua_function(tolua_S,"SetTarget",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetTarget00);
    tolua_function(tolua_S,"GetUseDataResume",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetUseDataResume00);
    tolua_function(tolua_S,"SetUseDataResume",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetUseDataResume00);
    tolua_function(tolua_S,"GetRequestUrl",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetRequestUrl00);
    tolua_function(tolua_S,"SetRequestUrl",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetRequestUrl00);
    tolua_function(tolua_S,"GetLastResponseUrl",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetLastResponseUrl00);
    tolua_function(tolua_S,"SetLastResponseUrl",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetLastResponseUrl00);
    tolua_function(tolua_S,"GetTargetRawSize",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetTargetRawSize00);
    tolua_function(tolua_S,"GetStatusCode",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetStatusCode00);
    tolua_function(tolua_S,"SetStatusCode",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetStatusCode00);
    tolua_function(tolua_S,"SetSendData",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_SetSendData00);
    tolua_function(tolua_S,"GetSendData",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendData00);
    tolua_function(tolua_S,"GetSendDataLength",tolua_ScutNetwork_ScutNetwork_CHttpClientResponse_GetSendDataLength00);
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
    tolua_function(tolua_S,"DeleteCookies",tolua_ScutNetwork_ScutNetwork_CHttpSession_DeleteCookies00);
    tolua_variable(tolua_S,"cookieJar",tolua_get_ScutNetwork__CHttpSession_cookieJar,tolua_set_ScutNetwork__CHttpSession_cookieJar);
    tolua_function(tolua_S,"Initialize",tolua_ScutNetwork_ScutNetwork_CHttpSession_Initialize00);
    tolua_function(tolua_S,"GetCookies",tolua_ScutNetwork_ScutNetwork_CHttpSession_GetCookies00);
    tolua_function(tolua_S,"AddCookie",tolua_ScutNetwork_ScutNetwork_CHttpSession_AddCookie00);
    tolua_function(tolua_S,"new",tolua_ScutNetwork_ScutNetwork_CHttpSession_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutNetwork_ScutNetwork_CHttpSession_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutNetwork_ScutNetwork_CHttpSession_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutNetwork_ScutNetwork_CHttpSession_delete00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutNetwork (lua_State* tolua_S) {
 return tolua_ScutNetwork_open(tolua_S);
};
#endif

