/*
** Lua binding: ScutUtility
** Generated automatically by tolua++-1.0.92 on 10/07/13 15:28:47.
*/

#ifndef __cplusplus
#include "stdlib.h"
#endif
#include "string.h"

#include "tolua++.h"

/* Exported function */
TOLUA_API int  tolua_ScutUtility_open (lua_State* tolua_S);

#include"../ScutLocale.h"
#include"../ScutUtils.h"
#include"../ScutLuaLan.h"
#include"../ScutLanFactory.h"
#include <string>

/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
#ifndef Mtolua_typeid
#define Mtolua_typeid(L,TI,T)
#endif
 tolua_usertype(tolua_S,"ScutUtility::CScutLan");
 Mtolua_typeid(tolua_S,typeid(ScutUtility::CScutLan), "ScutUtility::CScutLan");
 tolua_usertype(tolua_S,"ScutUtility::CScutLuaLan");
 Mtolua_typeid(tolua_S,typeid(ScutUtility::CScutLuaLan), "ScutUtility::CScutLuaLan");
 tolua_usertype(tolua_S,"ScutUtility::CScutLanFactory");
 Mtolua_typeid(tolua_S,typeid(ScutUtility::CScutLanFactory), "ScutUtility::CScutLanFactory");
 tolua_usertype(tolua_S,"ScutUtility::ScutUtils");
 Mtolua_typeid(tolua_S,typeid(ScutUtility::ScutUtils), "ScutUtility::ScutUtils");
 tolua_usertype(tolua_S,"ScutUtility::CLocale");
 Mtolua_typeid(tolua_S,typeid(ScutUtility::CLocale), "ScutUtility::CLocale");
}

/* method: GetPlatformType of class  ScutUtility::ScutUtils */
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_GetPlatformType00
static int tolua_ScutUtility_ScutUtility_ScutUtils_GetPlatformType00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getImsi00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getImsi00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getImei00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getImei00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_scheduleLocalNotification00
static int tolua_ScutUtility_ScutUtility_ScutUtils_scheduleLocalNotification00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotification00
static int tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotification00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotifications00
static int tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotifications00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getMacAddress00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getMacAddress00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_setTextToClipBoard00
static int tolua_ScutUtility_ScutUtility_ScutUtils_setTextToClipBoard00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getTextFromClipBoard00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getTextFromClipBoard00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_launchApp00
static int tolua_ScutUtility_ScutUtility_ScutUtils_launchApp00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_installPackage00
static int tolua_ScutUtility_ScutUtility_ScutUtils_installPackage00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_checkAppInstalled00
static int tolua_ScutUtility_ScutUtility_ScutUtils_checkAppInstalled00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_registerWebviewCallback00
static int tolua_ScutUtility_ScutUtility_ScutUtils_registerWebviewCallback00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getInstalledApps00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getInstalledApps00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getCurrentAppId00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getCurrentAppId00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_GoBack00
static int tolua_ScutUtility_ScutUtility_ScutUtils_GoBack00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getOpenUrlData00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getOpenUrlData00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_isJailBroken00
static int tolua_ScutUtility_ScutUtility_ScutUtils_isJailBroken00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_ScutUtils_getActiveNetworkInfo00
static int tolua_ScutUtility_ScutUtility_ScutUtils_getActiveNetworkInfo00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_setLanguage00
static int tolua_ScutUtility_ScutUtility_CLocale_setLanguage00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_getLanguage00
static int tolua_ScutUtility_ScutUtility_CLocale_getLanguage00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_getImsi00
static int tolua_ScutUtility_ScutUtility_CLocale_getImsi00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_getImei00
static int tolua_ScutUtility_ScutUtility_CLocale_getImei00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_setImsi00
static int tolua_ScutUtility_ScutUtility_CLocale_setImsi00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CLocale_isSDCardExist00
static int tolua_ScutUtility_ScutUtility_CLocale_isSDCardExist00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_instance00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_instance00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_Add00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_Add00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_Switch00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_Switch00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_Remove00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_Remove00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_Get00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_Get00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLuaLan_RemoveAll00
static int tolua_ScutUtility_ScutUtility_CScutLuaLan_RemoveAll00(lua_State* tolua_S)
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
#ifndef TOLUA_DISABLE_tolua_ScutUtility_ScutUtility_CScutLanFactory_GetLanInstance00
static int tolua_ScutUtility_ScutUtility_CScutLanFactory_GetLanInstance00(lua_State* tolua_S)
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

/* Open function */
TOLUA_API int tolua_ScutUtility_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
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
    tolua_function(tolua_S,"GetPlatformType",tolua_ScutUtility_ScutUtility_ScutUtils_GetPlatformType00);
    tolua_function(tolua_S,"getImsi",tolua_ScutUtility_ScutUtility_ScutUtils_getImsi00);
    tolua_function(tolua_S,"getImei",tolua_ScutUtility_ScutUtility_ScutUtils_getImei00);
    tolua_function(tolua_S,"scheduleLocalNotification",tolua_ScutUtility_ScutUtility_ScutUtils_scheduleLocalNotification00);
    tolua_function(tolua_S,"cancelLocalNotification",tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotification00);
    tolua_function(tolua_S,"cancelLocalNotifications",tolua_ScutUtility_ScutUtility_ScutUtils_cancelLocalNotifications00);
    tolua_function(tolua_S,"getMacAddress",tolua_ScutUtility_ScutUtility_ScutUtils_getMacAddress00);
    tolua_function(tolua_S,"setTextToClipBoard",tolua_ScutUtility_ScutUtility_ScutUtils_setTextToClipBoard00);
    tolua_function(tolua_S,"getTextFromClipBoard",tolua_ScutUtility_ScutUtility_ScutUtils_getTextFromClipBoard00);
    tolua_function(tolua_S,"launchApp",tolua_ScutUtility_ScutUtility_ScutUtils_launchApp00);
    tolua_function(tolua_S,"installPackage",tolua_ScutUtility_ScutUtility_ScutUtils_installPackage00);
    tolua_function(tolua_S,"checkAppInstalled",tolua_ScutUtility_ScutUtility_ScutUtils_checkAppInstalled00);
    tolua_function(tolua_S,"registerWebviewCallback",tolua_ScutUtility_ScutUtility_ScutUtils_registerWebviewCallback00);
    tolua_function(tolua_S,"getInstalledApps",tolua_ScutUtility_ScutUtility_ScutUtils_getInstalledApps00);
    tolua_function(tolua_S,"getCurrentAppId",tolua_ScutUtility_ScutUtility_ScutUtils_getCurrentAppId00);
    tolua_function(tolua_S,"GoBack",tolua_ScutUtility_ScutUtility_ScutUtils_GoBack00);
    tolua_function(tolua_S,"getOpenUrlData",tolua_ScutUtility_ScutUtility_ScutUtils_getOpenUrlData00);
    tolua_function(tolua_S,"isJailBroken",tolua_ScutUtility_ScutUtility_ScutUtils_isJailBroken00);
    tolua_function(tolua_S,"getActiveNetworkInfo",tolua_ScutUtility_ScutUtility_ScutUtils_getActiveNetworkInfo00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CLocale","ScutUtility::CLocale","",NULL);
   tolua_beginmodule(tolua_S,"CLocale");
    tolua_function(tolua_S,"setLanguage",tolua_ScutUtility_ScutUtility_CLocale_setLanguage00);
    tolua_function(tolua_S,"getLanguage",tolua_ScutUtility_ScutUtility_CLocale_getLanguage00);
    tolua_function(tolua_S,"getImsi",tolua_ScutUtility_ScutUtility_CLocale_getImsi00);
    tolua_function(tolua_S,"getImei",tolua_ScutUtility_ScutUtility_CLocale_getImei00);
    tolua_function(tolua_S,"setImsi",tolua_ScutUtility_ScutUtility_CLocale_setImsi00);
    tolua_function(tolua_S,"isSDCardExist",tolua_ScutUtility_ScutUtility_CLocale_isSDCardExist00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CScutLuaLan","ScutUtility::CScutLuaLan","",NULL);
   tolua_beginmodule(tolua_S,"CScutLuaLan");
    tolua_function(tolua_S,"instance",tolua_ScutUtility_ScutUtility_CScutLuaLan_instance00);
    tolua_function(tolua_S,"Add",tolua_ScutUtility_ScutUtility_CScutLuaLan_Add00);
    tolua_function(tolua_S,"Switch",tolua_ScutUtility_ScutUtility_CScutLuaLan_Switch00);
    tolua_function(tolua_S,"Remove",tolua_ScutUtility_ScutUtility_CScutLuaLan_Remove00);
    tolua_function(tolua_S,"Get",tolua_ScutUtility_ScutUtility_CScutLuaLan_Get00);
    tolua_function(tolua_S,"RemoveAll",tolua_ScutUtility_ScutUtility_CScutLuaLan_RemoveAll00);
   tolua_endmodule(tolua_S);
  tolua_endmodule(tolua_S);
  tolua_module(tolua_S,"ScutUtility",0);
  tolua_beginmodule(tolua_S,"ScutUtility");
   tolua_cclass(tolua_S,"CScutLanFactory","ScutUtility::CScutLanFactory","",NULL);
   tolua_beginmodule(tolua_S,"CScutLanFactory");
    tolua_function(tolua_S,"GetLanInstance",tolua_ScutUtility_ScutUtility_CScutLanFactory_GetLanInstance00);
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
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutUtility (lua_State* tolua_S) {
 return tolua_ScutUtility_open(tolua_S);
};
#endif

