/*
** Lua binding: ScutSystem
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:26:42.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutSystem_open (lua_State* tolua_S);

#include"../md5.h"
#include"../ScutUtility.h"
#include"../Stream.h"
#include <string>
#include "../Defines.h"

/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutSystem__CMemoryStream (lua_State* tolua_S)
{
 ScutSystem::CMemoryStream* self = (ScutSystem::CMemoryStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_intptr_t (lua_State* tolua_S)
{
 intptr_t* self = (intptr_t*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_md5 (lua_State* tolua_S)
{
 md5* self = (md5*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CBaseMemoryStream (lua_State* tolua_S)
{
 ScutSystem::CBaseMemoryStream* self = (ScutSystem::CBaseMemoryStream*) tolua_tousertype(tolua_S,1,0);
	Mtolua_delete(self);
	return 0;
}

static int tolua_collect_ScutSystem__CHandleStream (lua_State* tolua_S)
{
 ScutSystem::CHandleStream* self = (ScutSystem::CHandleStream*) tolua_tousertype(tolua_S,1,0);
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
#endif


/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
#ifndef Mtolua_typeid
#define Mtolua_typeid(L,TI,T)
#endif
 tolua_usertype(tolua_S,"ScutSystem::CScutUtility");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CScutUtility), "ScutSystem::CScutUtility");
 tolua_usertype(tolua_S,"ScutSystem::CMemoryStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CMemoryStream), "ScutSystem::CMemoryStream");
 tolua_usertype(tolua_S,"ScutSystem::CBaseMemoryStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CBaseMemoryStream), "ScutSystem::CBaseMemoryStream");
 tolua_usertype(tolua_S,"DWORD");
 Mtolua_typeid(tolua_S,typeid(DWORD), "DWORD");
 tolua_usertype(tolua_S,"intptr_t");
 Mtolua_typeid(tolua_S,typeid(intptr_t), "intptr_t");
 tolua_usertype(tolua_S,"md5");
 Mtolua_typeid(tolua_S,typeid(md5), "md5");
 tolua_usertype(tolua_S,"ScutSystem::CHandleStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CHandleStream), "ScutSystem::CHandleStream");
 tolua_usertype(tolua_S,"ScutSystem::CFileStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CFileStream), "ScutSystem::CFileStream");
 tolua_usertype(tolua_S,"ScutSystem::CStream");
 Mtolua_typeid(tolua_S,typeid(ScutSystem::CStream), "ScutSystem::CStream");
}

/* function: PrintMD5 */
#ifndef TOLUA_DISABLE_tolua_ScutSystem_PrintMD500
static int tolua_ScutSystem_PrintMD500(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_MD5String00
static int tolua_ScutSystem_MD5String00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_MD5File00
static int tolua_ScutSystem_MD5File00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_new00
static int tolua_ScutSystem_md5_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_new00_local
static int tolua_ScutSystem_md5_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_Init00
static int tolua_ScutSystem_md5_Init00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_Update00
static int tolua_ScutSystem_md5_Update00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_Finalize00
static int tolua_ScutSystem_md5_Finalize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_md5_Digest00
static int tolua_ScutSystem_md5_Digest00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CScutUtility_DesEncrypt00
static int tolua_ScutSystem_ScutSystem_CScutUtility_DesEncrypt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CScutUtility_StdDesEncrypt00
static int tolua_ScutSystem_ScutSystem_CScutUtility_StdDesEncrypt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CScutUtility_StdDesDecrypt00
static int tolua_ScutSystem_ScutSystem_CScutUtility_StdDesDecrypt00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CScutUtility_GetTickCount00
static int tolua_ScutSystem_ScutSystem_CScutUtility_GetTickCount00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CScutUtility_CScutUtility__GetNowTime00
static int tolua_ScutSystem_ScutSystem_CScutUtility_CScutUtility__GetNowTime00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_delete00
static int tolua_ScutSystem_ScutSystem_CStream_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_GetPosition00
static int tolua_ScutSystem_ScutSystem_CStream_GetPosition00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_SetPosition00
static int tolua_ScutSystem_ScutSystem_CStream_SetPosition00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_GetSize00
static int tolua_ScutSystem_ScutSystem_CStream_GetSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_SetSize00
static int tolua_ScutSystem_ScutSystem_CStream_SetSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_Read00
static int tolua_ScutSystem_ScutSystem_CStream_Read00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_Write00
static int tolua_ScutSystem_ScutSystem_CStream_Write00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_Seek00
static int tolua_ScutSystem_ScutSystem_CStream_Seek00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_ReadBuffer00
static int tolua_ScutSystem_ScutSystem_CStream_ReadBuffer00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_WriteBuffer00
static int tolua_ScutSystem_ScutSystem_CStream_WriteBuffer00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_CopyFrom00
static int tolua_ScutSystem_ScutSystem_CStream_CopyFrom00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CStream_GetBuffer00
static int tolua_ScutSystem_ScutSystem_CStream_GetBuffer00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_new00
static int tolua_ScutSystem_ScutSystem_CHandleStream_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_new00_local
static int tolua_ScutSystem_ScutSystem_CHandleStream_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_SetSize00
static int tolua_ScutSystem_ScutSystem_CHandleStream_SetSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_Read00
static int tolua_ScutSystem_ScutSystem_CHandleStream_Read00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_Write00
static int tolua_ScutSystem_ScutSystem_CHandleStream_Write00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_Seek00
static int tolua_ScutSystem_ScutSystem_CHandleStream_Seek00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_GetHandle00
static int tolua_ScutSystem_ScutSystem_CHandleStream_GetHandle00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CHandleStream_SetHandle00
static int tolua_ScutSystem_ScutSystem_CHandleStream_SetHandle00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CFileStream_new00
static int tolua_ScutSystem_ScutSystem_CFileStream_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CFileStream_new00_local
static int tolua_ScutSystem_ScutSystem_CFileStream_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CFileStream_Open00
static int tolua_ScutSystem_ScutSystem_CFileStream_Open00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CFileStream_delete00
static int tolua_ScutSystem_ScutSystem_CFileStream_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SetSize00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SetSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Write00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Write00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Read00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Read00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Seek00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Seek00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo01
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo01(lua_State* tolua_S)
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
 return tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: GetMemory of class  ScutSystem::CBaseMemoryStream */
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CBaseMemoryStream_GetMemory00
static int tolua_ScutSystem_ScutSystem_CBaseMemoryStream_GetMemory00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_new00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_new00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_new00_local
static int tolua_ScutSystem_ScutSystem_CMemoryStream_new00_local(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_delete00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_delete00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_Clear00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_Clear00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom01
static int tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom01(lua_State* tolua_S)
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
 return tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetSize of class  ScutSystem::CMemoryStream */
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_SetSize00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_SetSize00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutSystem_ScutSystem_CMemoryStream_Write00
static int tolua_ScutSystem_ScutSystem_CMemoryStream_Write00(lua_State* tolua_S)
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

/* Open function */
TOLUA_API int tolua_ScutSystem_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_function(tolua_S,"PrintMD5",tolua_ScutSystem_PrintMD500);
  tolua_function(tolua_S,"MD5String",tolua_ScutSystem_MD5String00);
  tolua_function(tolua_S,"MD5File",tolua_ScutSystem_MD5File00);
  #ifdef __cplusplus
  tolua_cclass(tolua_S,"md5","md5","",tolua_collect_md5);
  #else
  tolua_cclass(tolua_S,"md5","md5","",NULL);
  #endif
  tolua_beginmodule(tolua_S,"md5");
   tolua_function(tolua_S,"new",tolua_ScutSystem_md5_new00);
   tolua_function(tolua_S,"new_local",tolua_ScutSystem_md5_new00_local);
   tolua_function(tolua_S,".call",tolua_ScutSystem_md5_new00_local);
   tolua_function(tolua_S,"Init",tolua_ScutSystem_md5_Init00);
   tolua_function(tolua_S,"Update",tolua_ScutSystem_md5_Update00);
   tolua_function(tolua_S,"Finalize",tolua_ScutSystem_md5_Finalize00);
   tolua_function(tolua_S,"Digest",tolua_ScutSystem_md5_Digest00);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutSystem",0);
  tolua_beginmodule(tolua_S,"ScutSystem");
   tolua_cclass(tolua_S,"CScutUtility","ScutSystem::CScutUtility","",NULL);
   tolua_beginmodule(tolua_S,"CScutUtility");
    tolua_function(tolua_S,"DesEncrypt",tolua_ScutSystem_ScutSystem_CScutUtility_DesEncrypt00);
    tolua_function(tolua_S,"StdDesEncrypt",tolua_ScutSystem_ScutSystem_CScutUtility_StdDesEncrypt00);
    tolua_function(tolua_S,"StdDesDecrypt",tolua_ScutSystem_ScutSystem_CScutUtility_StdDesDecrypt00);
    tolua_function(tolua_S,"GetTickCount",tolua_ScutSystem_ScutSystem_CScutUtility_GetTickCount00);
    tolua_function(tolua_S,"CScutUtility__GetNowTime",tolua_ScutSystem_ScutSystem_CScutUtility_CScutUtility__GetNowTime00);
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
    tolua_function(tolua_S,"delete",tolua_ScutSystem_ScutSystem_CStream_delete00);
    tolua_function(tolua_S,"GetPosition",tolua_ScutSystem_ScutSystem_CStream_GetPosition00);
    tolua_function(tolua_S,"SetPosition",tolua_ScutSystem_ScutSystem_CStream_SetPosition00);
    tolua_function(tolua_S,"GetSize",tolua_ScutSystem_ScutSystem_CStream_GetSize00);
    tolua_function(tolua_S,"SetSize",tolua_ScutSystem_ScutSystem_CStream_SetSize00);
    tolua_function(tolua_S,"Read",tolua_ScutSystem_ScutSystem_CStream_Read00);
    tolua_function(tolua_S,"Write",tolua_ScutSystem_ScutSystem_CStream_Write00);
    tolua_function(tolua_S,"Seek",tolua_ScutSystem_ScutSystem_CStream_Seek00);
    tolua_function(tolua_S,"ReadBuffer",tolua_ScutSystem_ScutSystem_CStream_ReadBuffer00);
    tolua_function(tolua_S,"WriteBuffer",tolua_ScutSystem_ScutSystem_CStream_WriteBuffer00);
    tolua_function(tolua_S,"CopyFrom",tolua_ScutSystem_ScutSystem_CStream_CopyFrom00);
    tolua_function(tolua_S,"GetBuffer",tolua_ScutSystem_ScutSystem_CStream_GetBuffer00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CHandleStream","ScutSystem::CHandleStream","ScutSystem::CStream",tolua_collect_ScutSystem__CHandleStream);
   #else
   tolua_cclass(tolua_S,"CHandleStream","ScutSystem::CHandleStream","ScutSystem::CStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CHandleStream");
    tolua_function(tolua_S,"new",tolua_ScutSystem_ScutSystem_CHandleStream_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutSystem_ScutSystem_CHandleStream_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutSystem_ScutSystem_CHandleStream_new00_local);
    tolua_function(tolua_S,"SetSize",tolua_ScutSystem_ScutSystem_CHandleStream_SetSize00);
    tolua_function(tolua_S,"Read",tolua_ScutSystem_ScutSystem_CHandleStream_Read00);
    tolua_function(tolua_S,"Write",tolua_ScutSystem_ScutSystem_CHandleStream_Write00);
    tolua_function(tolua_S,"Seek",tolua_ScutSystem_ScutSystem_CHandleStream_Seek00);
    tolua_function(tolua_S,"GetHandle",tolua_ScutSystem_ScutSystem_CHandleStream_GetHandle00);
    tolua_function(tolua_S,"SetHandle",tolua_ScutSystem_ScutSystem_CHandleStream_SetHandle00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CFileStream","ScutSystem::CFileStream","ScutSystem::CHandleStream",tolua_collect_ScutSystem__CFileStream);
   #else
   tolua_cclass(tolua_S,"CFileStream","ScutSystem::CFileStream","ScutSystem::CHandleStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CFileStream");
    tolua_function(tolua_S,"new",tolua_ScutSystem_ScutSystem_CFileStream_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutSystem_ScutSystem_CFileStream_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutSystem_ScutSystem_CFileStream_new00_local);
    tolua_function(tolua_S,"Open",tolua_ScutSystem_ScutSystem_CFileStream_Open00);
    tolua_function(tolua_S,"delete",tolua_ScutSystem_ScutSystem_CFileStream_delete00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CBaseMemoryStream","ScutSystem::CBaseMemoryStream","ScutSystem::CStream",tolua_collect_ScutSystem__CBaseMemoryStream);
   #else
   tolua_cclass(tolua_S,"CBaseMemoryStream","ScutSystem::CBaseMemoryStream","ScutSystem::CStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CBaseMemoryStream");
    tolua_function(tolua_S,"SetSize",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SetSize00);
    tolua_function(tolua_S,"Write",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Write00);
    tolua_function(tolua_S,"Read",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Read00);
    tolua_function(tolua_S,"Seek",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_Seek00);
    tolua_function(tolua_S,"SaveTo",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo00);
    tolua_function(tolua_S,"SaveTo",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_SaveTo01);
    tolua_function(tolua_S,"GetMemory",tolua_ScutSystem_ScutSystem_CBaseMemoryStream_GetMemory00);
   tolua_endmodule(tolua_S);
   #ifdef __cplusplus
   tolua_cclass(tolua_S,"CMemoryStream","ScutSystem::CMemoryStream","ScutSystem::CBaseMemoryStream",tolua_collect_ScutSystem__CMemoryStream);
   #else
   tolua_cclass(tolua_S,"CMemoryStream","ScutSystem::CMemoryStream","ScutSystem::CBaseMemoryStream",NULL);
   #endif
   tolua_beginmodule(tolua_S,"CMemoryStream");
    tolua_function(tolua_S,"new",tolua_ScutSystem_ScutSystem_CMemoryStream_new00);
    tolua_function(tolua_S,"new_local",tolua_ScutSystem_ScutSystem_CMemoryStream_new00_local);
    tolua_function(tolua_S,".call",tolua_ScutSystem_ScutSystem_CMemoryStream_new00_local);
    tolua_function(tolua_S,"delete",tolua_ScutSystem_ScutSystem_CMemoryStream_delete00);
    tolua_function(tolua_S,"Clear",tolua_ScutSystem_ScutSystem_CMemoryStream_Clear00);
    tolua_function(tolua_S,"LoadFrom",tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom00);
    tolua_function(tolua_S,"LoadFrom",tolua_ScutSystem_ScutSystem_CMemoryStream_LoadFrom01);
    tolua_function(tolua_S,"SetSize",tolua_ScutSystem_ScutSystem_CMemoryStream_SetSize00);
    tolua_function(tolua_S,"Write",tolua_ScutSystem_ScutSystem_CMemoryStream_Write00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutSystem (lua_State* tolua_S) {
 return tolua_ScutSystem_open(tolua_S);
};
#endif

