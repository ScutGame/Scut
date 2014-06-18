/*
** Lua binding: ScutCxList
** Generated automatically by tolua++-1.0.92 on 09/24/13 14:15:16.
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
TOLUA_API int  tolua_ScutCxList_open (lua_State* tolua_S);


/* function to release collected object via destructor */
#ifdef __cplusplus

static int tolua_collect_ScutCxList (lua_State* tolua_S)
{
 ScutCxList* self = (ScutCxList*) tolua_tousertype(tolua_S,1,0);
    Mtolua_delete(self);
    return 0;
}
#endif


/* function to register type */
static void tolua_reg_types (lua_State* tolua_S)
{
 tolua_usertype(tolua_S,"CCLayerColor");
 tolua_usertype(tolua_S,"CCEvent");
 tolua_usertype(tolua_S,"CCTouch");
 tolua_usertype(tolua_S,"ScutCxListItem");
 tolua_usertype(tolua_S,"ScutCxList");
 tolua_usertype(tolua_S,"CCSize");
 tolua_usertype(tolua_S,"ccColor3B");
 tolua_usertype(tolua_S,"ccColor4B");
 tolua_usertype(tolua_S,"ScutCxListItemClickListener");
 
}

/* method: onClick of class  ScutCxListItemClickListener */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxListItemClickListener_onClick00
static int tolua_ScutCxList_ScutCxListItemClickListener_onClick00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxListItemClickListener",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxListItemClickListener* self = (ScutCxListItemClickListener*)  tolua_tousertype(tolua_S,1,0);
  int index = ((int)  tolua_tonumber(tolua_S,2,0));
  ScutCxListItem* item = ((ScutCxListItem*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'onClick'", NULL);
#endif
  {
   self->onClick(index,item);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'onClick'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: delete of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_delete00
static int tolua_ScutCxList_ScutCxList_delete00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
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

/* method: node of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_node00
static int tolua_ScutCxList_ScutCxList_node00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertable(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"ccColor4B",0,&tolua_err)) ||
     (tolua_isvaluenil(tolua_S,4,&tolua_err) || !tolua_isusertype(tolua_S,4,"CCSize",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,5,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  float row_height = ((float)  tolua_tonumber(tolua_S,2,0));
  ccColor4B bg_color = *((ccColor4B*)  tolua_tousertype(tolua_S,3,0));
  CCSize size = *((CCSize*)  tolua_tousertype(tolua_S,4,0));
  {
   ScutCxList* tolua_ret = (ScutCxList*)  ScutCxList::node(row_height,bg_color,size);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutCxList");
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

/* method: addListItem of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_addListItem00
static int tolua_ScutCxList_ScutCxList_addListItem00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,3,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  ScutCxListItem* item = ((ScutCxListItem*)  tolua_tousertype(tolua_S,2,0));
  bool scroll_to_view = ((bool)  tolua_toboolean(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'addListItem'", NULL);
#endif
  {
   int tolua_ret = (int)  self->addListItem(item,scroll_to_view);
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'addListItem'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: DeleteChild of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_DeleteChild00
static int tolua_ScutCxList_ScutCxList_DeleteChild00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutCxListItem",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  ScutCxListItem* child = ((ScutCxListItem*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'DeleteChild'", NULL);
#endif
  {
   self->DeleteChild(child);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'DeleteChild'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: DeleteChild of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_DeleteChild01
static int tolua_ScutCxList_ScutCxList_DeleteChild01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int nIndex = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'DeleteChild'", NULL);
#endif
  {
   self->DeleteChild(nIndex);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutCxList_ScutCxList_DeleteChild00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* method: getChild of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getChild00
static int tolua_ScutCxList_ScutCxList_getChild00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int row_index = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getChild'", NULL);
#endif
  {
   ScutCxListItem* tolua_ret = (ScutCxListItem*)  self->getChild(row_index);
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutCxListItem");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getChild'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: clear of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_clear00
static int tolua_ScutCxList_ScutCxList_clear00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'clear'", NULL);
#endif
  {
   self->clear();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'clear'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: selectChild of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_selectChild00
static int tolua_ScutCxList_ScutCxList_selectChild00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int row_index = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'selectChild'", NULL);
#endif
  {
   self->selectChild(row_index);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'selectChild'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getSelectedChild of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getSelectedChild00
static int tolua_ScutCxList_ScutCxList_getSelectedChild00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getSelectedChild'", NULL);
#endif
  {
   ScutCxListItem* tolua_ret = (ScutCxListItem*)  self->getSelectedChild();
    tolua_pushusertype(tolua_S,(void*)tolua_ret,"ScutCxListItem");
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getSelectedChild'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setLineColor of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setLineColor00
static int tolua_ScutCxList_ScutCxList_setLineColor00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"ccColor3B",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  ccColor3B* color = ((ccColor3B*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setLineColor'", NULL);
#endif
  {
   self->setLineColor(*color);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setLineColor'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setSelectedItemColor of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setSelectedItemColor00
static int tolua_ScutCxList_ScutCxList_setSelectedItemColor00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !tolua_isusertype(tolua_S,2,"ccColor3B",0,&tolua_err)) ||
     (tolua_isvaluenil(tolua_S,3,&tolua_err) || !tolua_isusertype(tolua_S,3,"ccColor3B",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  ccColor3B* start_color = ((ccColor3B*)  tolua_tousertype(tolua_S,2,0));
  ccColor3B* end_color = ((ccColor3B*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setSelectedItemColor'", NULL);
#endif
  {
   self->setSelectedItemColor(*start_color,*end_color);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setSelectedItemColor'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getChildCount of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getChildCount00
static int tolua_ScutCxList_ScutCxList_getChildCount00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getChildCount'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getChildCount();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getChildCount'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setHorizontal of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setHorizontal00
static int tolua_ScutCxList_ScutCxList_setHorizontal00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,1,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  bool bHorizontal = ((bool)  tolua_toboolean(tolua_S,2,false));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setHorizontal'", NULL);
#endif
  {
   self->setHorizontal(bHorizontal);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setHorizontal'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: isHorizontal_mode of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_isHorizontal_mode00
static int tolua_ScutCxList_ScutCxList_isHorizontal_mode00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'isHorizontal_mode'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->isHorizontal_mode();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'isHorizontal_mode'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setRowHeight of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setRowHeight00
static int tolua_ScutCxList_ScutCxList_setRowHeight00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  float height = ((float)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setRowHeight'", NULL);
#endif
  {
   self->setRowHeight(height);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setRowHeight'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setRowWidth of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setRowWidth00
static int tolua_ScutCxList_ScutCxList_setRowWidth00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  float width = ((float)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setRowWidth'", NULL);
#endif
  {
   self->setRowWidth(width);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setRowWidth'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setPageTurnEffect of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setPageTurnEffect00
static int tolua_ScutCxList_ScutCxList_setPageTurnEffect00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  bool bPageTurn = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setPageTurnEffect'", NULL);
#endif
  {
   self->setPageTurnEffect(bPageTurn);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setPageTurnEffect'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getPageTurnEffect of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getPageTurnEffect00
static int tolua_ScutCxList_ScutCxList_getPageTurnEffect00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getPageTurnEffect'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->getPageTurnEffect();
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getPageTurnEffect'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: turnToPage of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_turnToPage00
static int tolua_ScutCxList_ScutCxList_turnToPage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int nPageIndex = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'turnToPage'", NULL);
#endif
  {
   self->turnToPage(nPageIndex);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'turnToPage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: setRecodeNumPerPage of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_setRecodeNumPerPage00
static int tolua_ScutCxList_ScutCxList_setRecodeNumPerPage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int nNumber = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'setRecodeNumPerPage'", NULL);
#endif
  {
   self->setRecodeNumPerPage(nNumber);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'setRecodeNumPerPage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getRecodeNumPerPage of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getRecodeNumPerPage00
static int tolua_ScutCxList_ScutCxList_getRecodeNumPerPage00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getRecodeNumPerPage'", NULL);
#endif
  {
   int tolua_ret = (int)  self->getRecodeNumPerPage();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getRecodeNumPerPage'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetSilence of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_SetSilence00
static int tolua_ScutCxList_ScutCxList_SetSilence00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isboolean(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  bool bSilence = ((bool)  tolua_toboolean(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetSilence'", NULL);
#endif
  {
   self->SetSilence(bSilence);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetSilence'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getRowHeight of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getRowHeight00
static int tolua_ScutCxList_ScutCxList_getRowHeight00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getRowHeight'", NULL);
#endif
  {
   float tolua_ret = (float)  self->getRowHeight();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getRowHeight'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: getRowWidth of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_getRowWidth00
static int tolua_ScutCxList_ScutCxList_getRowWidth00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'getRowWidth'", NULL);
#endif
  {
   float tolua_ret = (float)  self->getRowWidth();
   tolua_pushnumber(tolua_S,(lua_Number)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'getRowWidth'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerLoadEvent of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerLoadEvent00
static int tolua_ScutCxList_ScutCxList_registerLoadEvent00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !toluafix_isfunction(tolua_S,2,"LUA_FUNCTION",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  LUA_FUNCTION fun = (  toluafix_ref_function(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerLoadEvent'", NULL);
#endif
  {
   self->registerLoadEvent(fun);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerLoadEvent'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerUnloadEvent of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerUnloadEvent00
static int tolua_ScutCxList_ScutCxList_registerUnloadEvent00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !toluafix_isfunction(tolua_S,2,"LUA_FUNCTION",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  LUA_FUNCTION fun = (  toluafix_ref_function(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerUnloadEvent'", NULL);
#endif
  {
   self->registerUnloadEvent(fun);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerUnloadEvent'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerUnselectEvent of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerUnselectEvent00
static int tolua_ScutCxList_ScutCxList_registerUnselectEvent00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     (tolua_isvaluenil(tolua_S,2,&tolua_err) || !toluafix_isfunction(tolua_S,2,"LUA_FUNCTION",0,&tolua_err)) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  LUA_FUNCTION fun = (  toluafix_ref_function(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerUnselectEvent'", NULL);
#endif
  {
   self->registerUnselectEvent(fun);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerUnselectEvent'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: SetParentListRect of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_SetParentListRect00
static int tolua_ScutCxList_ScutCxList_SetParentListRect00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,3,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,4,0,&tolua_err) ||
     !tolua_isnumber(tolua_S,5,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,6,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int nX = ((int)  tolua_tonumber(tolua_S,2,0));
  int nY = ((int)  tolua_tonumber(tolua_S,3,0));
  int nW = ((int)  tolua_tonumber(tolua_S,4,0));
  int nH = ((int)  tolua_tonumber(tolua_S,5,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'SetParentListRect'", NULL);
#endif
  {
   self->SetParentListRect(nX,nY,nW,nH);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'SetParentListRect'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: disableAllCtrlEvent of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_disableAllCtrlEvent00
static int tolua_ScutCxList_ScutCxList_disableAllCtrlEvent00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'disableAllCtrlEvent'", NULL);
#endif
  {
   self->disableAllCtrlEvent();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'disableAllCtrlEvent'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: onEnter of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_onEnter00
static int tolua_ScutCxList_ScutCxList_onEnter00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'onEnter'", NULL);
#endif
  {
   self->onEnter();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'onEnter'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: onExit of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_onExit00
static int tolua_ScutCxList_ScutCxList_onExit00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'onExit'", NULL);
#endif
  {
   self->onExit();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'onExit'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerWithTouchDispatcher of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerWithTouchDispatcher00
static int tolua_ScutCxList_ScutCxList_registerWithTouchDispatcher00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerWithTouchDispatcher'", NULL);
#endif
  {
   self->registerWithTouchDispatcher();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerWithTouchDispatcher'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ccTouchBegan of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_ccTouchBegan00
static int tolua_ScutCxList_ScutCxList_ccTouchBegan00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCTouch",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"CCEvent",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  CCTouch* touch = ((CCTouch*)  tolua_tousertype(tolua_S,2,0));
  CCEvent* event = ((CCEvent*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ccTouchBegan'", NULL);
#endif
  {
   bool tolua_ret = (bool)  self->ccTouchBegan(touch,event);
   tolua_pushboolean(tolua_S,(bool)tolua_ret);
  }
 }
 return 1;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ccTouchBegan'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ccTouchEnded of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_ccTouchEnded00
static int tolua_ScutCxList_ScutCxList_ccTouchEnded00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCTouch",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"CCEvent",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  CCTouch* touch = ((CCTouch*)  tolua_tousertype(tolua_S,2,0));
  CCEvent* event = ((CCEvent*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ccTouchEnded'", NULL);
#endif
  {
   self->ccTouchEnded(touch,event);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ccTouchEnded'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ccTouchCancelled of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_ccTouchCancelled00
static int tolua_ScutCxList_ScutCxList_ccTouchCancelled00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCTouch",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"CCEvent",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  CCTouch* touch = ((CCTouch*)  tolua_tousertype(tolua_S,2,0));
  CCEvent* event = ((CCEvent*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ccTouchCancelled'", NULL);
#endif
  {
   self->ccTouchCancelled(touch,event);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ccTouchCancelled'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: ccTouchMoved of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_ccTouchMoved00
static int tolua_ScutCxList_ScutCxList_ccTouchMoved00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"CCTouch",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,3,"CCEvent",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,4,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  CCTouch* touch = ((CCTouch*)  tolua_tousertype(tolua_S,2,0));
  CCEvent* event = ((CCEvent*)  tolua_tousertype(tolua_S,3,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'ccTouchMoved'", NULL);
#endif
  {
   self->ccTouchMoved(touch,event);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'ccTouchMoved'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: destroy of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_destroy00
static int tolua_ScutCxList_ScutCxList_destroy00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'destroy'", NULL);
#endif
  {
   self->destroy();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'destroy'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: keep of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_keep00
static int tolua_ScutCxList_ScutCxList_keep00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'keep'", NULL);
#endif
  {
   self->keep();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'keep'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerItemClickListener of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerItemClickListener00
static int tolua_ScutCxList_ScutCxList_registerItemClickListener00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isusertype(tolua_S,2,"ScutCxListItemClickListener",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  ScutCxListItemClickListener* szfunc = ((ScutCxListItemClickListener*)  tolua_tousertype(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerItemClickListener'", NULL);
#endif
  {
   self->registerItemClickListener(szfunc);
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'registerItemClickListener'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: unregisterItemClickListener of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_unregisterItemClickListener00
static int tolua_ScutCxList_ScutCxList_unregisterItemClickListener00(lua_State* tolua_S)
{
#ifndef TOLUA_RELEASE
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,2,&tolua_err)
 )
  goto tolua_lerror;
 else
#endif
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'unregisterItemClickListener'", NULL);
#endif
  {
   self->unregisterItemClickListener();
  }
 }
 return 0;
#ifndef TOLUA_RELEASE
 tolua_lerror:
 tolua_error(tolua_S,"#ferror in function 'unregisterItemClickListener'.",&tolua_err);
 return 0;
#endif
}
#endif //#ifndef TOLUA_DISABLE

/* method: registerItemClickListener of class  ScutCxList */
#ifndef TOLUA_DISABLE_tolua_ScutCxList_ScutCxList_registerItemClickListener01
static int tolua_ScutCxList_ScutCxList_registerItemClickListener01(lua_State* tolua_S)
{
 tolua_Error tolua_err;
 if (
     !tolua_isusertype(tolua_S,1,"ScutCxList",0,&tolua_err) ||
     !tolua_isnumber(tolua_S,2,0,&tolua_err) ||
     !tolua_isnoobj(tolua_S,3,&tolua_err)
 )
  goto tolua_lerror;
 else
 {
  ScutCxList* self = (ScutCxList*)  tolua_tousertype(tolua_S,1,0);
  int szSeletor = ((int)  tolua_tonumber(tolua_S,2,0));
#ifndef TOLUA_RELEASE
  if (!self) tolua_error(tolua_S,"invalid 'self' in function 'registerItemClickListener'", NULL);
#endif
  {
   self->registerItemClickListener(szSeletor);
  }
 }
 return 0;
tolua_lerror:
 return tolua_ScutCxList_ScutCxList_registerItemClickListener00(tolua_S);
}
#endif //#ifndef TOLUA_DISABLE

/* Open function */
TOLUA_API int tolua_ScutCxList_open (lua_State* tolua_S)
{
 tolua_open(tolua_S);
 tolua_reg_types(tolua_S);
 tolua_module(tolua_S,NULL,0);
 tolua_beginmodule(tolua_S,NULL);
  tolua_constant(tolua_S,"LS_WAITING",LS_WAITING);
  tolua_constant(tolua_S,"LS_TRACKINGTOUCH",LS_TRACKINGTOUCH);
  tolua_cclass(tolua_S,"ScutCxListItemClickListener","ScutCxListItemClickListener","",NULL);
  tolua_beginmodule(tolua_S,"ScutCxListItemClickListener");
   tolua_function(tolua_S,"onClick",tolua_ScutCxList_ScutCxListItemClickListener_onClick00);
  tolua_endmodule(tolua_S);
  #ifdef __cplusplus
  tolua_cclass(tolua_S,"ScutCxList","ScutCxList","CCLayerColor",tolua_collect_ScutCxList);
  #else
  tolua_cclass(tolua_S,"ScutCxList","ScutCxList","CCLayerColor",NULL);
  #endif
  tolua_beginmodule(tolua_S,"ScutCxList");
   tolua_function(tolua_S,"delete",tolua_ScutCxList_ScutCxList_delete00);
   tolua_function(tolua_S,"node",tolua_ScutCxList_ScutCxList_node00);
   tolua_function(tolua_S,"addListItem",tolua_ScutCxList_ScutCxList_addListItem00);
   tolua_function(tolua_S,"DeleteChild",tolua_ScutCxList_ScutCxList_DeleteChild00);
   tolua_function(tolua_S,"DeleteChild",tolua_ScutCxList_ScutCxList_DeleteChild01);
   tolua_function(tolua_S,"getChild",tolua_ScutCxList_ScutCxList_getChild00);
   tolua_function(tolua_S,"clear",tolua_ScutCxList_ScutCxList_clear00);
   tolua_function(tolua_S,"selectChild",tolua_ScutCxList_ScutCxList_selectChild00);
   tolua_function(tolua_S,"getSelectedChild",tolua_ScutCxList_ScutCxList_getSelectedChild00);
   tolua_function(tolua_S,"setLineColor",tolua_ScutCxList_ScutCxList_setLineColor00);
   tolua_function(tolua_S,"setSelectedItemColor",tolua_ScutCxList_ScutCxList_setSelectedItemColor00);
   tolua_function(tolua_S,"getChildCount",tolua_ScutCxList_ScutCxList_getChildCount00);
   tolua_function(tolua_S,"setHorizontal",tolua_ScutCxList_ScutCxList_setHorizontal00);
   tolua_function(tolua_S,"isHorizontal_mode",tolua_ScutCxList_ScutCxList_isHorizontal_mode00);
   tolua_function(tolua_S,"setRowHeight",tolua_ScutCxList_ScutCxList_setRowHeight00);
   tolua_function(tolua_S,"setRowWidth",tolua_ScutCxList_ScutCxList_setRowWidth00);
   tolua_function(tolua_S,"setPageTurnEffect",tolua_ScutCxList_ScutCxList_setPageTurnEffect00);
   tolua_function(tolua_S,"getPageTurnEffect",tolua_ScutCxList_ScutCxList_getPageTurnEffect00);
   tolua_function(tolua_S,"turnToPage",tolua_ScutCxList_ScutCxList_turnToPage00);
   tolua_function(tolua_S,"setRecodeNumPerPage",tolua_ScutCxList_ScutCxList_setRecodeNumPerPage00);
   tolua_function(tolua_S,"getRecodeNumPerPage",tolua_ScutCxList_ScutCxList_getRecodeNumPerPage00);
   tolua_function(tolua_S,"SetSilence",tolua_ScutCxList_ScutCxList_SetSilence00);
   tolua_function(tolua_S,"getRowHeight",tolua_ScutCxList_ScutCxList_getRowHeight00);
   tolua_function(tolua_S,"getRowWidth",tolua_ScutCxList_ScutCxList_getRowWidth00);
   tolua_function(tolua_S,"registerLoadEvent",tolua_ScutCxList_ScutCxList_registerLoadEvent00);
   tolua_function(tolua_S,"registerUnloadEvent",tolua_ScutCxList_ScutCxList_registerUnloadEvent00);
   tolua_function(tolua_S,"registerUnselectEvent",tolua_ScutCxList_ScutCxList_registerUnselectEvent00);
   tolua_function(tolua_S,"SetParentListRect",tolua_ScutCxList_ScutCxList_SetParentListRect00);
   tolua_function(tolua_S,"disableAllCtrlEvent",tolua_ScutCxList_ScutCxList_disableAllCtrlEvent00);
   tolua_function(tolua_S,"onEnter",tolua_ScutCxList_ScutCxList_onEnter00);
   tolua_function(tolua_S,"onExit",tolua_ScutCxList_ScutCxList_onExit00);
   tolua_function(tolua_S,"registerWithTouchDispatcher",tolua_ScutCxList_ScutCxList_registerWithTouchDispatcher00);
   tolua_function(tolua_S,"ccTouchBegan",tolua_ScutCxList_ScutCxList_ccTouchBegan00);
   tolua_function(tolua_S,"ccTouchEnded",tolua_ScutCxList_ScutCxList_ccTouchEnded00);
   tolua_function(tolua_S,"ccTouchCancelled",tolua_ScutCxList_ScutCxList_ccTouchCancelled00);
   tolua_function(tolua_S,"ccTouchMoved",tolua_ScutCxList_ScutCxList_ccTouchMoved00);
   tolua_function(tolua_S,"destroy",tolua_ScutCxList_ScutCxList_destroy00);
   tolua_function(tolua_S,"keep",tolua_ScutCxList_ScutCxList_keep00);
   tolua_function(tolua_S,"registerItemClickListener",tolua_ScutCxList_ScutCxList_registerItemClickListener00);
   tolua_function(tolua_S,"unregisterItemClickListener",tolua_ScutCxList_ScutCxList_unregisterItemClickListener00);
   tolua_function(tolua_S,"registerItemClickListener",tolua_ScutCxList_ScutCxList_registerItemClickListener01);
  tolua_endmodule(tolua_S);
 tolua_endmodule(tolua_S);
 return 1;
}


#if defined(LUA_VERSION_NUM) && LUA_VERSION_NUM >= 501
 TOLUA_API int luaopen_ScutCxList (lua_State* tolua_S) {
 return tolua_ScutCxList_open(tolua_S);
};
#endif

