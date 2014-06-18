/*
** Lua binding: ScutCxListItem
** Generated automatically by tolua++-1.0.92 on 09/24/13 14:22:21.
*/

/****************************************************************************
 Copyright (c) 2011 cocos2d-x.org

 http://www.cocos2d-x.org

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

extern "C" {
#include "tolua_fix.h"
}

#include <map>
#include <string>
#include "cocos2d.h"
#include "CCLuaEngine.h"
#include "SimpleAudioEngine.h"
#include "cocos-ext.h"

using namespace cocos2d;
using namespace cocos2d::extension;
using namespace CocosDenshion;

/* Exported function */
TOLUA_API int  tolua_ScutCxListItem_open (lua_State* tolua_S);


/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_CCSize (lua_State* tolua_S)
{
 CCSize* self = (CCSize*) tolua_tousertype(tolua_S,1,0);
    Mtolua_delete(self);
    return 0;
}

static int tolua_collect_CCRect (lua_State* tolua_S)
{
 CCRect* self = (CCRect*) tolua_tousertype(tolua_S,1,0);
    Mtolua_delete(self);
    return 0;
}
#endif


/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
 tolua_usertype(tolua_S,"CCLayerColor");
 tolua_usertype(tolua_S,"CCSize");
 tolua_usertype(tolua_S,"ccColor3B");
 tolua_usertype(tolua_S,"LayoutParam");
 tolua_usertype(tolua_S,"ScutCxListItem");
 tolua_usertype(tolua_S,"CCNode");
 tolua_usertype(tolua_S,"CCRect");
}

/* method: itemWithColor of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_itemWithColor00
static int tolua_ScutCxListItem_ScutCxListItem_itemWithColor00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ccColor3B",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  const ccColor3B* color = ((const ccColor3B*)  tolua_tousertype(tolua_S,2,0));
  {
   ScutCxListItem* tolua_ret = (ScutCxListItem*)  ScutCxListItem::itemWithColor(*color);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutCxListItem");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'itemWithColor'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: rect of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_rect00
static int tolua_ScutCxListItem_ScutCxListItem_rect00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'rect'", NULL);
#endif
  {
   CCRect tolua_ret = (CCRect)  self->rect();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((CCRect)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"CCRect");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(CCRect));
     tolua_pushusertype(tolua_S,tolua_obj,"CCRect");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'rect'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: selected of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_selected00
static int tolua_ScutCxListItem_ScutCxListItem_selected00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'selected'", NULL);
#endif
  {
   self->selected();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'selected'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: unselected of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_unselected00
static int tolua_ScutCxListItem_ScutCxListItem_unselected00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'unselected'", NULL);
#endif
  {
   self->unselected();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'unselected'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setItemColor of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_setItemColor00
static int tolua_ScutCxListItem_ScutCxListItem_setItemColor00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"const ccColor3B",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  const ccColor3B* color = ((const ccColor3B*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setItemColor'", NULL);
#endif
  {
   self->setItemColor(*color);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setItemColor'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setMargin of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_setMargin00
static int tolua_ScutCxListItem_ScutCxListItem_setMargin00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"CCSize",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  CCSize margin = *((CCSize*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setMargin'", NULL);
#endif
  {
   self->setMargin(margin);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setMargin'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getMargin of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_getMargin00
static int tolua_ScutCxListItem_ScutCxListItem_getMargin00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getMargin'", NULL);
#endif
  {
   CCSize tolua_ret = (CCSize)  self->getMargin();
   {
#ifdef __cplusplus
    void* tolua_obj = Mtolua_new((CCSize)(tolua_ret));
     tolua_pushusertype(tolua_S,tolua_obj,"CCSize");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#else
    void* tolua_obj = tolua_copy(tolua_S,(void*)&tolua_ret,sizeof(CCSize));
     tolua_pushusertype(tolua_S,tolua_obj,"CCSize");
    tolua_register_gc(tolua_S,lua_gettop(tolua_S));
#endif
   }
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getMargin'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: addChildItem of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_addChildItem00
static int tolua_ScutCxListItem_ScutCxListItem_addChildItem00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCNode",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  CCNode* child = ((CCNode*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'addChildItem'", NULL);
#endif
  {
   self->addChildItem(child);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'addChildItem'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: addChildItem of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_addChildItem01
static int tolua_ScutCxListItem_ScutCxListItem_addChildItem01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCNode",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"const LayoutParam",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  CCNode* child = ((CCNode*)  tolua_tousertype(tolua_S,2,0));
  const LayoutParam* layout = ((const LayoutParam*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'addChildItem'", NULL);
#endif
  {
   self->addChildItem(child,*layout);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutCxListItem_ScutCxListItem_addChildItem00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: addChildItem of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_addChildItem02
static int tolua_ScutCxListItem_ScutCxListItem_addChildItem02(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCNode",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"const LayoutParam",0,&tolua_err)) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  CCNode* child = ((CCNode*)  tolua_tousertype(tolua_S,2,0));
  const LayoutParam* layout = ((const LayoutParam*)  tolua_tousertype(tolua_S,3,0));
  int tag = ((int)  tolua_tonumber(tolua_S,4,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'addChildItem'", NULL);
#endif
  {
   self->addChildItem(child,*layout,tag);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutCxListItem_ScutCxListItem_addChildItem01(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getChildByTag of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_getChildByTag00
static int tolua_ScutCxListItem_ScutCxListItem_getChildByTag00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  int tag = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getChildByTag'", NULL);
#endif
  {
   CCNode* tolua_ret = (CCNode*)  self->getChildByTag(tag);
    int nID = (tolua_ret) ? (int)tolua_ret->m_uID : -1;
    int* pLuaID = (tolua_ret) ? &tolua_ret->m_nLuaID : NULL;
    toluafix_pushusertype_ccobject(tolua_S, nID, pLuaID, (void*)tolua_ret,"CCNode");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getChildByTag'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setDrawTopLine of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_setDrawTopLine00
static int tolua_ScutCxListItem_ScutCxListItem_setDrawTopLine00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  bool value = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setDrawTopLine'", NULL);
#endif
  {
   self->setDrawTopLine(value);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setDrawTopLine'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setDrawBottomLine of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_setDrawBottomLine00
static int tolua_ScutCxListItem_ScutCxListItem_setDrawBottomLine00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  bool value = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setDrawBottomLine'", NULL);
#endif
  {
   self->setDrawBottomLine(value);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setDrawBottomLine'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setDrawSelected of class  ScutCxListItem */
#ifndef TOLUA_DISABLE_tolua_ScutCxListItem_ScutCxListItem_setDrawSelected00
static int tolua_ScutCxListItem_ScutCxListItem_setDrawSelected00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItem* self = (ScutCxListItem*)  tolua_tousertype(tolua_S,1,0);
  bool value = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setDrawSelected'", NULL);
#endif
  {
   self->setDrawSelected(value);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setDrawSelected'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_ScutCxListItem_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_cclass(tolua_S,"ScutCxListItem","ScutCxListItem","CCLayerColor",NULL);
  tolua_beginmodule(tolua_S,"ScutCxListItem");
   tolua_function(tolua_S,"itemWithColor",tolua_ScutCxListItem_ScutCxListItem_itemWithColor00);
   tolua_function(tolua_S,"rect",tolua_ScutCxListItem_ScutCxListItem_rect00);
   tolua_function(tolua_S,"selected",tolua_ScutCxListItem_ScutCxListItem_selected00);
   tolua_function(tolua_S,"unselected",tolua_ScutCxListItem_ScutCxListItem_unselected00);
   tolua_function(tolua_S,"setItemColor",tolua_ScutCxListItem_ScutCxListItem_setItemColor00);
   tolua_function(tolua_S,"setMargin",tolua_ScutCxListItem_ScutCxListItem_setMargin00);
   tolua_function(tolua_S,"getMargin",tolua_ScutCxListItem_ScutCxListItem_getMargin00);
   tolua_function(tolua_S,"addChildItem",tolua_ScutCxListItem_ScutCxListItem_addChildItem00);
   tolua_function(tolua_S,"addChildItem",tolua_ScutCxListItem_ScutCxListItem_addChildItem01);
   tolua_function(tolua_S,"addChildItem",tolua_ScutCxListItem_ScutCxListItem_addChildItem02);
   tolua_function(tolua_S,"getChildByTag",tolua_ScutCxListItem_ScutCxListItem_getChildByTag00);
   tolua_function(tolua_S,"setDrawTopLine",tolua_ScutCxListItem_ScutCxListItem_setDrawTopLine00);
   tolua_function(tolua_S,"setDrawBottomLine",tolua_ScutCxListItem_ScutCxListItem_setDrawBottomLine00);
   tolua_function(tolua_S,"setDrawSelected",tolua_ScutCxListItem_ScutCxListItem_setDrawSelected00);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutCxListItem (lua_State* tolua_S) {
 return tolua_ScutCxListItem_open(tolua_S);
};
#endif

