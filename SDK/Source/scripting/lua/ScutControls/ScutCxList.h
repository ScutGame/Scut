/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

http://www.scutgame.com

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
#ifndef __SCUTCX_LIST_H__
#define __SCUTCX_LIST_H__

#include <time.h>
/****************************************************************************
Copyright (c) 2013-2015 Scutgame.com

http://www.scutgame.com

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
#include "platform.h"
#include "ScutCxListItem.h"
#include <vector>
#include "../cocos2dx_support/CCLuaEngine.h"
#include "ScutListLoaderListener.h"


/*
*  LIST 控件增加了横屏模式和翻页效果， 接口与旧版本兼容。
*ScutCxList(float row_height, 
*			ccColor4B bg_color, 
*			CCSize size,
*			bool horizontal_mode=false
*			);
*最后一个参数是设模竖模式的，默认为竖模式。
*
* .设是否翻页
*		void setPageTurnEffect(bool bPageTurn);
*		bool getPageTurnEffect();
*		 
*		//设页的记录数 ,默认为一
*		void setRecodeNumPerPage(int nNumber);
*		int  getRecodeNumPerPage();
*/



using namespace cocos2d;
namespace  ScutCxControl 
{
	class ScutCxListItem;
	typedef enum  
	{
		LS_WAITING,
		LS_TRACKINGTOUCH,
	} LIST_STATE;


	typedef enum  
	{
		SLIDER_NO,
		SLIDER_H,
		SLIDER_W,		
	} LIST_SLIDE_DIR;


	/**
	* @brief  列表项点击事件侦听器
	* @remarks   
	* @see		
	*/
	struct LUA_DLL ScutCxListItemClickListener
	{
		virtual void onClick(int index, ScutCxListItem *item) = 0;
	};

	/**
	* @brief  列表控件
	* @remarks   
	* @see		
	*/
	class LUA_DLL ScutCxList : public CCLayerColor
	{
	public:

		virtual ~ScutCxList(void);

		static ScutCxList* node(float row_height, 
			ccColor4B bg_color, 
			CCSize size);

	public:
		/**
		* @brief  初始化列表控件
		* @n<b>函数名称</b>					: addListItem
		* @n@param   float   row_height 每行或列的宽/高度
		* @param  ccColor4B bg_color 背景色
		* @param  CCSize size  例表大小
		* @param  bool horizontal_mode 是否模屏模式
		* @return   
		* @remarks  
		* @see		
		*/
		ScutCxList(float row_height, 
			ccColor4B bg_color, 
			CCSize size,
			bool horizontal_mode=false
			);

		/**
		* @brief 为例表增加一个子项
		* @n<b>函数名称</b>					: addListItem
		* @n@param   ScutCxListItem   item
		* @param   ScutCxListItem*   item
		* @return  bool  是否滑动例表到些ITEM可见
		* @remarks 旧版本接口名为addChild,因在脚本中使用容易与CCNODE的混淆而更名
		* @see		
		*/
		int addListItem(ScutCxListItem *item, bool scroll_to_view = true);


		/**
		* @brief 删除指定子项
		* @n<b>函数名称</b>					: DeleteChild
		* @n@param   ScutCxListItem*   item
		* @remarks 
		* @see		
		*/
		void DeleteChild(ScutCxListItem* child);
		/**
		* @brief 删除指定子项
		* @n<b>函数名称</b>					: DeleteChild
		* @n@param   int   第几个ITEM
		* @remarks 
		* @see		
		*/
		void DeleteChild(int nIndex);


		/**
		* @brief 插入子项
		* @n<b>函数名称</b>					: insertChild
		* @n@param   ScutCxListItem *item
		* @param   int   位置索引号
		* @remarks 
		* @see		
		*/
		int insertChild(ScutCxListItem *item, int nIndex);

		/**
		* @brief 获取指定索引的子项
		* @n<b>函数名称</b>					: getChild
		* @n@param   ScutCxListItem *item
		* @param   int   位置索引号
		* @remarks 
		* @see		
		*/
		ScutCxListItem *getChild(int row_index);


		/**
		* @brief 清除列表内容
		* @remarks 
		* @see		
		*/
		void clear(void);

		/**
		* @brief 选中列表指定行/列
		* @remarks 
		* @see		
		*/
		void selectChild(int row_index);

		/**
		* @brief 获取当前选中的子项
		* @remarks 
		* @see		
		*/
		ScutCxListItem *getSelectedChild(void);


		/**
		* @brief 设画线颜色
		* @remarks 
		* @see		
		*/
		void setLineColor(ccColor3B &color);


		/**
		* @brief 设选中时画线颜色
		* @remarks 
		* @see		
		*/
		void setSelectedItemColor(ccColor3B &start_color, ccColor3B &end_color);


		/**
		* @brief 获取子项数量
		* @remarks 
		* @see		
		*/
		int getChildCount(void);

		///////////////////////////
		//以下必须在添加item之前调用


		/**
		* @brief 设置是否横屏模式
		* 
		* 
		* @n<b>函数名称</b>					: setHorizontal
		* @n@param   bool     bHorizontal : 是否横屏模式， 默认为[FALSE]竖屏
		* @return  无
		* @remarks 必须在添加item之前调用
		* @see		
		*/

		void setHorizontal(bool bHorizontal=false);


		/**
		* @brief  获取是否横屏模式
		* 
		* 
		* @n<b>函数名称</b>					: isHorizontal_mode
		* @return  TRUE 横屏模式， FALSE 竖屏模式
		* @remarks 必须在添加item之前调用
		* @see		
		*/
		bool isHorizontal_mode();



		/**
		* @brief  设置列表控件高度
		* 
		* 
		* @n<b>函数名称</b>					: setRowHeight
		* @n@param  float 高度
		* @return  void
		* @remarks 必须在添加item之前调用
		* @see		
		*/
		void setRowHeight(float height);


		/**
		* @brief  设置列表控件宽度
		* @n<b>函数名称</b>					: setRowWidth
		* @n@param  int 宽度
		* @return  void
		* @remarks 必须在添加item之前调用
		* @see		
		*/
		void setRowWidth(float width);
		/////////////////////////////////



		/**
		* @brief  设置是否翻页
		* 
		* 
		* @n<b>函数名称</b>					: setPageTurnEffect
		* @n@param  bool 是否翻页
		* @return  void
		* @remarks
		* @see		
		*/
		void setPageTurnEffect(bool bPageTurn);


		/**
		* @brief  获取是否翻页
		* 
		* 
		* @n<b>函数名称</b>					: getPageTurnEffect
		* @return  bool 是否翻页
		* @remarks
		* @see		
		*/
		bool getPageTurnEffect();


		/**
		* @brief  翻到指定页
		* @n<b>函数名称</b>					: setPageTurnEffect
		* @n@param  int 翻到指定页
		* @return  void
		* @remarks
		* @see		
		*/
		void turnToPage(int nPageIndex);

		/**
		* @brief  设页的记录数 ,默认为一
		* 
		* 
		* @n<b>函数名称</b>					: setRecodeNumPerPage
		* @n@param  int 一屏的记录数
		* @return  void
		* @remarks
		* @see		
		*/
		void setRecodeNumPerPage(int nNumber);


		/**
		* @brief  获取页的记录数  
		* 
		* 
		* @n<b>函数名称</b>					: getRecodeNumPerPage
		* @return  int 一屏的记录数
		* @remarks
		* @see		
		*/
		int  getRecodeNumPerPage();


		/**
		* @brief  动态设置是否响应事件 
		* @n<b>函数名称</b>					: SetSilence
		* @n@param  bool 是否响应事件
		* @remarks
		* @see		
		*/
		void SetSilence(bool bSilence);


		/**
		* @brief  获取竖屏列表控件子项的高度 
		* @n<b>函数名称</b>					: getRowHeight
		* @see		
		*/
		float getRowHeight(void);

		/**
		* @brief  获取横屏列表控件子项的宽度 
		* @n<b>函数名称</b>					: getRowWidth
		* @see		
		*/
		float getRowWidth(void);





		/**
		* @brief  注册C++ LOADER 事件 
		* @n<b>函数名称</b>					: registerLoaderListern
		* @n@param  CNdListLoaderListener 事件监听器
		* @remarks
		* @see		
		*/
		void	registerLoaderListern(const CScutListLoaderListener* pListener)
		{
			m_pLoaderListener = const_cast<CScutListLoaderListener*>(pListener);
		}



		/**
		* @brief  LUA unselect 事件回调注册, 回调function 带一个int 参数，为Item的index值
		* @n<b>函数名称</b>					: registerUnselectEvent
		* @n@param  char*  回调名
		* @remarks
		* @see		
		*/	 
		void registerUnselectEvent(int fun);

		/**
		* @brief  LUA LOADER 事件回调注册 ，回调function 带一个int 参数，为目标页码值
		* @n<b>函数名称</b>					: registerLoadEvent
		* @n@param  char*  回调名
		* @remarks
		* @see		
		*/	 
		virtual void registerLoadEvent(int fun);
		/**
		* @brief  LUA UNLOADER 事件回调注册 ，回调function 带一个int 参数，为目标页码值
		* @n<b>函数名称</b>					: registerUnloadEvent
		* @n@param  char*  回调名
		* @remarks
		* @see		
		*/
		virtual void registerUnloadEvent(int fun);

		void unregisterUnloadEvent();
		void unregisterLoadEvent();
		void unregisterUnselectEvent();

		/**
		* @brief  嵌套列表控件裁剪用， 参数坐标为父LIST 的POS 和 SIZE
		* @n<b>函数名称</b>					: registerUnloadEvent
		* @n@param  int 父LIST 的POS.x
		* @param    int 父LIST 的POS.y
		* @param    int 父LIST 的SIZE.w
		* @param    int 父LIST 的SIZE.h
		* @remarks
		* @see		
		*/
		void SetParentListRect(int nX,int nY,int nW,int nH)
		{
			nParentX = nX*CC_CONTENT_SCALE_FACTOR();
			nParentY = nY*CC_CONTENT_SCALE_FACTOR();
			nParentW = nW*CC_CONTENT_SCALE_FACTOR();
			nParentH = nH*CC_CONTENT_SCALE_FACTOR();
		}


		/**
		* @brief 清除列表控件所有控件的事件响应，由LIST 控件触发
		* @remarks
		* @see		
		*/
		void disableAllCtrlEvent();

		/**
		* @brief 在960*480 和480*320 屏幕分辨率下返回相同大小
		* @remarks
		* @see		
		*/
		virtual CCSize getContentSize();



	public:
		virtual void onEnter(void);
		virtual void onExit(void);
		virtual void registerWithTouchDispatcher(void);
		virtual bool ccTouchBegan(CCTouch* touch, CCEvent* event);
		virtual void ccTouchEnded(CCTouch* touch, CCEvent* event);
		virtual void ccTouchCancelled(CCTouch *touch, CCEvent* event);
		virtual void ccTouchMoved(CCTouch* touch, CCEvent* event);
		virtual void destroy(void);
		virtual void keep(void);
		virtual void registerItemClickListener(ScutCxListItemClickListener *listener);

		
		virtual void unregisterItemClickListener(void);


		void triggerLoaderEvent(int nBegin,bool bForward);

#if 1
		virtual void registerItemClickListener(LUA_FUNCTION nSeletor);
#endif


		int  getParentX()
		{
			return nParentX;
		}
		int  getParentY()
		{
			return nParentY;
		}
		int  getParentW()
		{
			return nParentW;
		}
		int  getParentH()
		{
			return nParentH;
		}

		CCLayer* getInner_panel()
		{
			return inner_panel_;
		}

	protected:
		virtual void onItemClick(int index, ScutCxListItem *item) {}
		virtual void visit(void);



	private:
		ScutCxListItem *itemForTouch(CCTouch * touch);
		void doLayout(void);
		void doFitPos(float y_pos);

		CCRect rect(void)
		{
			CCSize s = getContentSize();
			//return CCRectMake(-s.width / 2, -s.height / 2, s.width, s.height);
			return CCRectMake(0, 0, s.width, s.height);
		}

		bool containsTouchLocation(CCTouch* touch);
	private:


		CScutListLoaderListener*		m_pLoaderListener;

		//弹出子界面时, 设置背后的Listview是否跟着滑动
		bool m_bSilence;

		bool is_horizontal_mode;
		bool be_pageturn_effect;
		char recodeNumberPerPage;
		float touch_began_x_;
		float touch_ended_x_;
		float old_x_;
		float min_x_;
		float max_x_;
		float row_width_;

		int  nParentX;
		int  nParentY;
		int  nParentW;
		int  nParentH;

		ScutCxListItem *sel_item_;
		int		m_nSelectIndex;
		float row_height_;
		LIST_STATE list_state_;
		CCLayer *inner_panel_;
		float touch_began_y_;
		float touch_ended_y_;
		float old_y_;
		float min_y_;
		float max_y_;
		bool snap_flag_;
		LIST_SLIDE_DIR  slide_dir;
		//cc_timeval touch_began_time_;
		clock_t touch_began_time_;
		ccColor3B line_color_;
		ccColor3B sel_item_start_color_;
		ccColor3B sel_item_end_color_;

		int m_nScriptLoadFunc;
		int m_nScriptUnloadFunc;
		int m_nScriptUnselectFunc;


		ScutCxListItemClickListener *item_click_listener_;
#if 1
		int m_nScriptSeletor;
#endif

	};
}

#endif