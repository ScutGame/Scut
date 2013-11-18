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
#ifndef __SCUTCX_LIST_ITEM_H_
#define __SCUTCX_LIST_ITEM_H_

#include "ControlDefine.h"
#include "cocos2d.h"
#include <map>
#include <list>
using namespace cocos2d;

namespace ScutCxControl {

#define ScutCxListItemChild CCNode

	/**
	* @brief  例表控件子项
	* @remarks   
	* @see		
	*/
	class LUA_DLL ScutCxListItem : public CCLayerColor
	{
		friend class ScutCxList;
	protected:
		ScutCxListItem(void);
		virtual ~ScutCxListItem(void);

	public:
		static ScutCxListItem *itemWithColor(const ccColor3B &color);

		/**
		* @brief  获取控件子项的RECT
		* @n<b>函数名称</b>					: rect
		* @remarks
		* @see		
		*/
		CCRect rect(void);

		/**
		* @brief  设控件子项为选中状态
		* @n<b>函数名称</b>					: selected
		* @remarks
		* @see		
		*/
		void selected(void);
		/**
		* @brief  设控件子项为非选中状态
		* @n<b>函数名称</b>					: unselected
		* @remarks
		* @see		
		*/
		void unselected(void);
		/**
		* @brief  设控件子项为非选中状态
		* @n<b>函数名称</b>					: unselected
		* @remarks
		* @see		
		*/
		void setItemColor(const ccColor3B &color);
		/**
		* @brief  设控件子项的边框
		* @n<b>函数名称</b>					: setMargin
		* @remarks
		* @see		
		*/
		void setMargin(CCSize margin);
		/**
		* @brief  获取控件子项的边框
		* @n<b>函数名称</b>					: getMargin
		* @remarks
		* @see		
		*/
		CCSize getMargin(void);

	public:

		/**
		* @brief  ADD 控件到列表控件子项上  采用默认布局,以 0,0 点为基点
		* @n<b>函数名称</b>					: addChildItem
		* @n@param  child 控件指针
		* @remarks
		* @see		
		*/
		void addChildItem(CCNode *child);

		/**
		* @brief  ADD 控件到列表控件子项上
		* @n<b>函数名称</b>					: addChildItem
		* @n@param  child 控件指针
		* @param    LayoutParam 布局
		* @remarks
		* @see		
		*/
		void addChildItem(CCNode *child, const LayoutParam &layout);

		/**
		* @brief  ADD子控件到列表控件子项上，并且由列表控件来接管控件的事件 
		* @n<b>函数名称</b>					: addChildItem
		* @n@param  child 控件指针
		* @param    LayoutParam 布局
		* @param    int 父LIST 的SIZE.w
		* @param    int tag==1  是说明加进来的控件需要由LIST 来接管事件分发
		* @remarks  子控件具有TOUCH 事件时 才设置TAG 值为1 。
		* @see		
		*/
		void addChildItem(CCNode *child, const LayoutParam &layout, int tag );



		/**
		* @brief  通过TAG 获取列表控件子项
		* @n<b>函数名称</b>					: getChildByTag
		* @n@param  int 控件的tag值
		* @see		
		*/
		ScutCxListItemChild *getChildByTag(int tag);


		/**
		* @brief  在列表控件子项最上面画线 
		* @n<b>函数名称</b>					: setDrawTopLine
		* @n@param  bool 是否画线
		* @remarks  默认为FALSE 。
		* @see		
		*/
		void setDrawTopLine(bool value) { draw_top_line_ = value; }
		/**
		* @brief  在列表控件子项最下面画线 
		* @n<b>函数名称</b>					: setDrawBottomLine
		* @n@param  bool 是否画线
		* @remarks  默认为FALSE 。
		* @see		
		*/
		void setDrawBottomLine(bool value) { draw_bottom_line_ = value; }

		/**
		* @brief  是否画列表控件子项选中时的底色
		* @n<b>函数名称</b>					: setDrawBottomLine
		* @n@param  bool 是否画线
		* @remarks  默认为FALSE 。
		* @see		
		*/
		void setDrawSelected(bool value) { draw_selected = value; }

		/**
		* @brief  对控件进行布局坐标设置 
		* @n<b>函数名称</b>					: requestLayout
		* @n@param  void  
		* @remarks  对控件进行布局坐标设置  。
		* @see		
		*/
		void requestLayout(void);

		/**
		* @brief  设置列表控件的事件分发 
		* @remarks   
		* @see		
		*/
		void registerWithTouchDispatcher();
		virtual bool ccTouchBegan(CCTouch* touch, CCEvent* event);
		virtual void ccTouchEnded(CCTouch* touch, CCEvent* event);
		virtual void ccTouchCancelled(CCTouch *touch, CCEvent* event);
		virtual void ccTouchMoved(CCTouch* touch, CCEvent* event);

		virtual void MyTouchDispatcher(CCNode* pParent, int nToutchEvent, CCTouch* touch, CCEvent* event);


		/**
		* @brief  onEnter事件
		* @n<b>函数名称</b>					: onEnter
		* @see		
		*/
		virtual void onEnter(void);


		/**
		* @brief  onExit事件
		* @n<b>函数名称</b>					: onEnter
		* @see		
		*/
		virtual void onExit(void);

		/**
		* @brief  动态设是否响应事件 
		* @n<b>函数名称</b>					: SetSilence
		* @n@param  bool  是否响应事件
		* @remarks  使用例子： 弹出子界面时, 设置背后的Listview是否跟着滑动 。
		* @see		
		*/
		void SetSilence(bool bSilence)
		{
			m_bSilence = bSilence;
		}

		bool getIsItemCatch()
		{
			return m_bCatchEndNode;
		}
		void setIsItemCatch(bool bCatch)
		{
			m_bCatchEndNode = bCatch;
		}


	protected:
		bool initWithWidthHeight(GLfloat width, GLfloat height);

		void setLineColor(const ccColor3B &color);
		void setSelectedColor(const ccColor3B &start_color, const ccColor3B &end_color);
		void updateColor(void);


		bool containsTouchLocation(CCTouch* touch)
		{	 
			return rect().containsPoint(convertTouchToNodeSpaceAR(touch));
		}

		virtual void draw(void);

		void disableCtrlEvent();
		void disableCtrlChildEvent(CCNode* pParent);

		void clear();

		bool findInDeque(CCNode* node);
		bool findInTouchEventLayerDeque(CCNode* node);

	private:
		bool selected_;
		ccColor3B line_color_;
		ccColor3B sel_item_start_color_;
		ccColor3B sel_item_end_color_;
		bool draw_top_line_;
		bool draw_bottom_line_;
		bool draw_selected;
		std::map<CCNode *, LayoutParam> layout_info_;
		float horizontal_margin_;
		float vertical_margin_;

		//弹出子界面时, 设置背后的Listview是否跟着滑动
		bool m_bSilence;

		typedef std::vector<CCLayer*> TOUCHBEGAN_VEC;
		TOUCHBEGAN_VEC	m_pVectorTouchBegan;

		//记录加入LIST 时，有带事件的层
		TOUCHBEGAN_VEC	m_pVectorTouchEventLayer;

		bool m_bCatchEndNode;


	};

}

#endif