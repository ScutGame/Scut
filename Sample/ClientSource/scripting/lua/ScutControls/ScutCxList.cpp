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
#include "ScutCxList.h"
namespace ScutCxControl 
{

	const int TURN_SLIDE_LEN = 30;
#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
#define FLOAT_EQUAL(x,y)    (fabs((x)-(y)) < 5.0f)
#else
#define FLOAT_EQUAL(x,y)    (fabs((x)-(y)) < 0.05f)
#endif


	ScutCxList* ScutCxList::node(float row_height, 
		ccColor4B bg_color, 
		CCSize size)
	{
		ScutCxList*pRet = new ScutCxList(row_height, bg_color, size);
		pRet->autorelease();
		return pRet;
	}

	ScutCxList::ScutCxList(float row_height, ccColor4B bg_color, CCSize size,bool horizontal_mode)
		: sel_item_(NULL)
		, row_height_(row_height)
		, row_width_(row_height)
		, old_y_(0.f)
		, min_y_(0.f)
		, max_y_(0.f)
		, old_x_(0.f)
		, min_x_(0.f)
		, max_x_(0.f)
		, touch_began_x_(0.f)
		, touch_ended_x_(0.f)
		, touch_began_y_(0.f)
		, touch_ended_y_(0.f)
		, snap_flag_(false)
		, list_state_(LS_WAITING)
		, item_click_listener_(NULL)
		,be_pageturn_effect(false)
		,recodeNumberPerPage(1)
		,m_pLoaderListener(NULL)
		,nParentX(0)
		,nParentY(0)
		,nParentW(0)
		,nParentH(0)
		,m_bSilence(false)
		,m_nSelectIndex(-1)
		,inner_panel_(NULL)
		,slide_dir(SLIDER_NO)
		,m_nScriptLoadFunc(0)
	    ,m_nScriptUnloadFunc(0)
	    ,m_nScriptUnselectFunc(0)
		,m_nScriptSeletor(0)

	{
		if (size.width <= 0 || size.height <= 0)
		{
			size = CCDirector::sharedDirector()->getWinSize();
		}


		size.width *= CC_CONTENT_SCALE_FACTOR();
		size.height *= CC_CONTENT_SCALE_FACTOR();

		initWithColor(bg_color, size.width, size.height);

		setTouchEnabled(true);


		inner_panel_ = CCLayer::create();
		inner_panel_->setPosition(CCPointZero);
		inner_panel_->setContentSize(size);
		CCLayerColor::addChild(inner_panel_);


		line_color_ = ccc3(0xBD, 0xBD, 0xBD);
		sel_item_end_color_ = ccc3(0, 0xFF, 0xFF);
		sel_item_start_color_ = ccc3(0xFF, 0xFF, 0);
		is_horizontal_mode = horizontal_mode;

		CCSize panel_size = inner_panel_->getContentSize();

		if(is_horizontal_mode)
		{
			recodeNumberPerPage =  panel_size.width/row_width_;
		}
		else
		{
			recodeNumberPerPage =  panel_size.height/row_height_;			
		}


	}


	CCSize ScutCxList::getContentSize()
	{

		CCSize sz	= CCNode::getContentSize();
		sz.height	/= CC_CONTENT_SCALE_FACTOR();
		sz.width	/= CC_CONTENT_SCALE_FACTOR();
		return sz;
	}

	ScutCxList::~ScutCxList(void) 
	{
		//inner_panel_->removeAllChildrenWithCleanup(true);
		unregisterItemClickListener();
		unregisterLoadEvent();
		unregisterUnloadEvent();
		unregisterUnselectEvent();
	}


	void ScutCxList::registerUnselectEvent(int fun)
	{
		unregisterUnselectEvent();
		m_nScriptUnselectFunc = fun;
		LUALOG("[LUA] Add ScutCxList script UnselectEvent handler: %d", m_nScriptUnselectFunc);
	}

	void ScutCxList::registerLoadEvent(int fun)
	{
		unregisterLoadEvent();
		m_nScriptLoadFunc = fun;
		LUALOG("[LUA] Add ScutCxList script LoadEvent handler: %d", m_nScriptLoadFunc);
	}


	void ScutCxList::registerUnloadEvent(int fun)
	{
		unregisterUnloadEvent();
		m_nScriptUnloadFunc = fun;
		LUALOG("[LUA] Add ScutCxList script UnloadEvent handler: %d", m_nScriptUnloadFunc);
	}


	void ScutCxList::unregisterUnselectEvent()
	{
		if (m_nScriptUnselectFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->removeScriptHandler(m_nScriptUnselectFunc);
			LUALOG("[LUA] Remove ScutCxList script UnselectEvent handler: %d", m_nScriptUnselectFunc);
			m_nScriptUnselectFunc = 0;
		}
	}

	void ScutCxList::unregisterLoadEvent()
	{
		if (m_nScriptLoadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->removeScriptHandler(m_nScriptLoadFunc);
			LUALOG("[LUA] Remove ScutCxList script LoadEvent handler: %d", m_nScriptLoadFunc);
			m_nScriptLoadFunc = 0;
		}
	}


	void ScutCxList::unregisterUnloadEvent()
	{
		if (m_nScriptUnloadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->removeScriptHandler(m_nScriptUnloadFunc);
			LUALOG("[LUA] Remove ScutCxList script UnloadEvent handler: %d", m_nScriptUnloadFunc);
			m_nScriptUnloadFunc = 0;
		}
	}



	// yay 2011-8-1
	int insertChild(ScutCxListItem *item, int nIndex)
	{
		if(!item)
			return 0;

		return 1;
	}


	//动态设置是否响应事件
	void ScutCxList::SetSilence(bool bSilence)
	{
		m_bSilence = bSilence;

		if(!inner_panel_)
			return;
		CCArray *children = inner_panel_->getChildren();

		if(!children)
			return;

		int item_count = children->count();

		for (int i = 0; i < item_count; ++i)
		{			 
			((ScutCxListItem *)children->objectAtIndex(i))->SetSilence(bSilence);			 
		}

	}



	int ScutCxList::addListItem(ScutCxListItem *item, bool scroll_to_view)
	{
		if(!item)
			return 0;
		inner_panel_->addChild(item);
		//item->release();

		CCArray *children = inner_panel_->getChildren();
		int item_count = children->count();

		/*
		if (1 == item_count)
		{
		item->setDrawTopLine(true);
		}
		*/

		CCSize panel_size = inner_panel_->getContentSize();

		item->setLineColor(line_color_);
		item->setSelectedColor(sel_item_start_color_, sel_item_end_color_);

		// yay
		if(is_horizontal_mode)
		{
			item->initWithWidthHeight(row_width_,panel_size.height);
			item->setTouchEnabled(true);
		}
		else
		{
			item->initWithWidthHeight(panel_size.width / CC_CONTENT_SCALE_FACTOR(), row_height_);
			item->setTouchEnabled(true);
		}

		item->requestLayout();

		// 计算位置，item的高度可能在调用requestLayout后有所变化

		CCSize item_size = item->getContentSize();


		//add by yay
		if(is_horizontal_mode)
		{
			float item_x_pos = 0.f;
			if (item_count > 1)
			{
				CCNode *last_child = (CCNode *)children->objectAtIndex(item_count - 2);
				item_x_pos = last_child->getPosition().x + item_size.width;
			}
			else
			{
				item_x_pos = 0;
			}
			item->setPosition(CCPointMake(item_x_pos, 0));
		}
		else
		{
			float item_y_pos = 0.f;
			if (item_count > 1)
			{
				CCNode *last_child = (CCNode *)children->objectAtIndex(item_count - 2);
				item_y_pos = last_child->getPosition().y - item_size.height;
			}
			else
			{
				item_y_pos = panel_size.height/CC_CONTENT_SCALE_FACTOR() - item_size.height;
			}
			item->setPosition(CCPointMake(0, item_y_pos));
		}




		float total_height = 0.f;
		for (int i = 0; i < (int)children->count(); ++i)
		{
			if(is_horizontal_mode)
			{
				total_height += ((CCNode *)children->objectAtIndex(i))->getContentSize().width;
			}
			else
			{
				total_height += ((CCNode *)children->objectAtIndex(i))->getContentSize().height;
			}
		}



		int rst = children->count() - 1;
		if(is_horizontal_mode)		
		{
			if (total_height >= panel_size.width/CC_CONTENT_SCALE_FACTOR())
			{
				max_x_ = total_height - panel_size.width/CC_CONTENT_SCALE_FACTOR();
			}
			else
			{
				max_x_ = 0;
			}

			if (scroll_to_view && (rst + 1) * row_width_ > panel_size.width)
			{
				doFitPos(max_x_);
			} 

		}
		else
		{

			if (total_height >= panel_size.height/CC_CONTENT_SCALE_FACTOR())
			{
				max_y_ = total_height - panel_size.height/CC_CONTENT_SCALE_FACTOR();
			}
			else
			{
				max_y_ = 0;
			}

			if (scroll_to_view && (rst + 1) * row_height_ > panel_size.height/CC_CONTENT_SCALE_FACTOR())
			{
				doFitPos(max_y_);
			}

		}	 

		return rst;
	}

	ScutCxListItem *ScutCxList::getChild(int row_index)
	{
		return (ScutCxListItem *)inner_panel_->getChildren()->objectAtIndex(row_index);
	}

	void ScutCxList::clear(void)
	{
		if(!inner_panel_)
			return;			

		CCArray *children = inner_panel_->getChildren();
		if(!children)
			return;


		//清空嵌套列表子列表
		if(nParentW || nParentH)
		{
			int nCount  = children->count();
			for( int i = 0; i < nCount ; i++ )
			{			
				ScutCxListItem * pItem =  getChild(i);
				if (pItem)
				{					
					pItem->clear();
				}
			}
		}


		inner_panel_->removeAllChildrenWithCleanup(true);
		sel_item_ = NULL;
		old_y_ = 0.f;
		min_y_ = 0.f;
		max_y_ = 0.f;
		touch_began_y_ = 0.f;
		touch_ended_y_ = 0.f;
		old_x_ = 0.f;
		min_x_ = 0.f;
		max_x_ = 0.f;
		touch_began_x_ = 0.f;
		touch_ended_x_ = 0.f;	 

		snap_flag_ = false;
		list_state_ = LS_WAITING;
		inner_panel_->setPosition(CCPointZero);
	}

	void ScutCxList::onEnter(void)
	{
		//setIsTouchEnabled(true);
		CCLayerColor::onEnter();
	}

	void ScutCxList::onExit(void)
	{
		//setIsTouchEnabled(false);
		CCLayerColor::onExit();
	}

	void ScutCxList::registerWithTouchDispatcher()
	{
		//	CCTouchDispatcher::sharedDispatcher()->addTargetedDelegate(this, INT_MIN+9999, false);s

		CCDirector::sharedDirector()->getTouchDispatcher()->addTargetedDelegate(this, kCCMenuHandlerPriority, false);
	}

	bool ScutCxList::ccTouchBegan(CCTouch* touch, CCEvent* event)
	{
		if(m_bSilence)
			return false;

		if (!containsTouchLocation(touch) || !isVisible())
		{
			return false;
		}

		CCArray *children = inner_panel_->getChildren();
		if (LS_WAITING != list_state_ || !m_bVisible || !children)
		{
			return false;
		}

		list_state_ = LS_TRACKINGTOUCH;
		CCPoint touchPoint = touch->getLocationInView();

		old_y_ = inner_panel_->getPosition().y;
		old_x_ = inner_panel_->getPosition().x;
		touch_began_y_ = touch_ended_y_ = CCDirector::sharedDirector()->convertToGL(touchPoint).y;
		touch_began_x_ = touch_ended_x_ = CCDirector::sharedDirector()->convertToGL(touchPoint).x;


		snap_flag_ = true;
		ScutCxListItem *sel_item = itemForTouch(touch);

		if (sel_item && sel_item != sel_item_)
		{
			sel_item->selected();
			if (sel_item_)
			{
				sel_item_->unselected();
			}		
		}
		sel_item_ = sel_item;
		//CCTime::gettimeofdayCocos2d(&touch_began_time_, NULL);
		touch_began_time_ = clock();

		return true;
	}

	void ScutCxList::doFitPos(float y_pos)
	{
		inner_panel_->stopAllActions();

		CCMoveTo *move_to = new CCMoveTo();

		if(is_horizontal_mode)		
		{
			move_to->initWithDuration(0.66f, CCPointMake(y_pos,0 ));
		}
		else
		{
			move_to->initWithDuration(0.66f, CCPointMake(0, y_pos));
		}

		CCEaseExponentialOut *ease_action = new CCEaseExponentialOut();
		ease_action->initWithAction(move_to);
		move_to->release();
		inner_panel_->runAction(ease_action);
		ease_action->release();
	}



	void  ScutCxList::triggerLoaderEvent(int nBegin,bool bForward)
	{

		int nLoadPage = nBegin;

		if(bForward)		 
			nLoadPage ++;
		else
			nLoadPage --;

		if(nLoadPage < 0)
			nLoadPage = 0;

		if (kScriptTypeNone != m_eScriptType && m_nScriptLoadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeListLoader(this, m_nScriptLoadFunc,nLoadPage);
		}

		if(kScriptTypeNone != m_eScriptType &&m_nScriptUnloadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeListLoader(this, m_nScriptUnloadFunc,nLoadPage);
		}


		if(!m_pLoaderListener)
			return;

		m_pLoaderListener->OnLoadItem(nLoadPage);
		m_pLoaderListener->OnUnLoadItem(nBegin);		


	}


	void ScutCxList::ccTouchEnded(CCTouch* touch, CCEvent* event)
	{
		//CCLog("XXXX ScutCxList ccTouchEnded click");
		if(m_bSilence)
		{
			slide_dir = SLIDER_NO;
			list_state_ = LS_WAITING;
			return;
		}

		if (LS_TRACKINGTOUCH != list_state_)
		{
			return;
		}

		if (sel_item_)
		{

			bool bTouched = false;
			if(is_horizontal_mode)		
			{
				if (FLOAT_EQUAL(touch_began_x_, touch_ended_x_))
					bTouched = true;
			}
			else
			{
				if (FLOAT_EQUAL(touch_began_y_, touch_ended_y_))
					bTouched = true;
			}

			//CCLog("XXXX bTouched");

			if (bTouched)// FLOAT_EQUAL(touch_began_y_, touch_ended_y_))
			{
				if (item_click_listener_)
				{
					item_click_listener_->onClick(
						inner_panel_->getChildren()->indexOfObject(sel_item_), sel_item_);
				}

				if(sel_item_)					
				{
					 //当没有点出到ITEM 上面的任意按扭时才触发ITEM 的事件
					//CCLog("XXXX Check selecteditem c hild catch event");
					if(!sel_item_->getIsItemCatch())
					{						
						if (m_nScriptSeletor)
						{
							//CCLog("XXXX Call script click event");
							CCLuaEngine::defaultEngine()->executeListItem(m_nScriptSeletor, 
								inner_panel_->getChildren()->indexOfObject(sel_item_), sel_item_);
						}
						onItemClick(inner_panel_->getChildren()->indexOfObject(sel_item_), sel_item_);
						
					}
					else
					{
						sel_item_->setIsItemCatch(false);						
						 
					}

				}

				
			}
		}

		// 如果超出范围，则反弹回去。
		CCPoint pos = inner_panel_->getPosition();


		if(is_horizontal_mode)		
		{

			if(slide_dir != SLIDER_W)
			{
				slide_dir = SLIDER_NO;
				list_state_ = LS_WAITING;
				return;
			}

			/*
			////模屏模式下，移动时x > y 才有效
			if(!  (abs(nOffsetX) > abs(nOffsetY)) )
			{
			list_state_ = LS_WAITING;
			return;

			}
			*/



			if (FLOAT_EQUAL(min_x_, max_x_))
			{

				if (!FLOAT_EQUAL(pos.x, 0.f))
					doFitPos(0.f);
			}
			else
			{
				if (pos.x > min_x_)
				{
					if (old_x_ < 0 && (pos.x - old_x_) > row_width_)
					{
						turnToPage(0);
					}
					else
						doFitPos(min_x_);
				}
				else if (pos.x < - max_x_ && ! be_pageturn_effect)
				{
					doFitPos(- max_x_);
				}
				else
				{
					if (!FLOAT_EQUAL(touch_began_x_, touch_ended_x_))
					{
						float acce_val = 0.f;
						float fit_pos = inner_panel_->getPosition().x;
						float abs_distance = fabs(touch_ended_x_ - touch_began_x_);

						/*cc_timeval end_time, sub_time;
						CCTime::gettimeofdayCocos2d(&end_time, NULL);
						CCTime::timersubCocos2d(&sub_time, &touch_began_time_, &end_time);
						int time_consume = sub_time.tv_sec * 1000 + sub_time.tv_usec/1000;*/
						int time_consume = clock() - touch_began_time_;
						if (time_consume < 400 && abs_distance > row_width_)
						{
							acce_val = (abs_distance / row_width_)  * row_width_ * 3.f;
						}
						else
						{
							acce_val = float((int)row_width_ / 3);
						}                    


						// 向左拖拽
						int width =  recodeNumberPerPage*row_width_;
						if (touch_began_x_ > touch_ended_x_)
						{							
							if(be_pageturn_effect)								
							{		
								if( (touch_began_x_ - touch_ended_x_) > TURN_SLIDE_LEN ) 
								{

									fit_pos = old_x_ - width;

									// trigger loader event
									float row_index = fit_pos/row_width_;
									row_index = row_index/recodeNumberPerPage;
									row_index = (int)(fabs(row_index)+0.5);// + recodeNumberPerPage;

									//triggerLoaderEvent(row_index,true);
									turnToPage(row_index);
									list_state_ = LS_WAITING;
									return;

								}
								else
									fit_pos = old_x_;

							}
							else
							{
								fit_pos -= acce_val;

								// trigger loader event
								float row_index = old_x_/row_width_ ;
								row_index = (int)(fabs(row_index)+0.5);// + recodeNumberPerPage;
								row_index = row_index/recodeNumberPerPage;
								triggerLoaderEvent(row_index,true);
							}

							if (fit_pos > min_x_) 
							{
								fit_pos = min_x_;
							}
							if ( fit_pos < -max_x_ ) 
							{
								if(!be_pageturn_effect)
									fit_pos = -max_x_; 
								else
								{
									if(fit_pos < -max_x_ - row_width_* (recodeNumberPerPage-1))
										fit_pos = -max_x_ - row_width_*(recodeNumberPerPage-1);
								}
							}
						}

						else
						{
							if(be_pageturn_effect)
							{	
								if( (touch_ended_x_ - touch_began_x_ ) > TURN_SLIDE_LEN)
								{

									fit_pos = old_x_ + width; 
									float row_index = fit_pos/width ;	
									row_index = (int)(fabs(row_index)+0.5);


									turnToPage(row_index);
									list_state_ = LS_WAITING;
									return;

								}
								else
									fit_pos = old_x_;

							}
							else
							{
								fit_pos += acce_val;

								// trigger loader event
								float row_index = old_x_/row_width_ ;
								row_index = (int)(fabs(row_index)+0.5);
								row_index = row_index/recodeNumberPerPage;
								triggerLoaderEvent(row_index,true);
							}

							if(fit_pos > min_x_)
								fit_pos = min_x_;

							if (fit_pos < -max_x_ )
							{
								if(!be_pageturn_effect)
									fit_pos = -max_x_; 
								else
								{
									if(fit_pos < (-max_x_ - row_width_* (recodeNumberPerPage-1)) )
										fit_pos = -max_x_ - row_width_* (recodeNumberPerPage-1);
								}
							}
						}

						doFitPos(fit_pos);
					}
				}
			}

		}
		else// be vertical mode
		{

			if(slide_dir != SLIDER_H)
			{
				slide_dir = SLIDER_NO;
				list_state_ = LS_WAITING;
				return;
			}
			/*
			//竖屏模式下，移动时y> x 才有效
			if( !( abs(nOffsetY) > abs(nOffsetX) ) )
			{
			list_state_ = LS_WAITING;
			return;
			}
			*/


			if (FLOAT_EQUAL(min_y_, max_y_))
			{
				if (!FLOAT_EQUAL(pos.y, 0.f))
				
					doFitPos(0.f);
					
			}
			else
			{
				if (pos.y < min_y_)
				{					
					doFitPos(min_y_);
				}
				else if (pos.y > max_y_ && ! be_pageturn_effect)
				{
					doFitPos(max_y_);
				}
				else
				{
					if (!FLOAT_EQUAL(touch_began_y_, touch_ended_y_))
					{
						float acce_val = 0.f;
						float fit_pos = inner_panel_->getPosition().y;
						float abs_distance = fabs(touch_ended_y_ - touch_began_y_);

						/*cc_timeval end_time, sub_time;
						CCTime::gettimeofdayCocos2d(&end_time, NULL);
						CCTime::timersubCocos2d(&sub_time, &touch_began_time_, &end_time);
						int time_consume = sub_time.tv_sec * 1000 + sub_time.tv_usec/1000;*/
						int time_consume = clock() - touch_began_time_;
						if (time_consume < 400 && abs_distance > row_height_)
						{
							acce_val = (abs_distance / row_height_) * 3.f * row_height_;
						}
						else
						{
							acce_val = float((int)row_height_ / 3);
						}                    

						// 向下拖拽
						int height =  recodeNumberPerPage*row_height_;
						if (touch_began_y_ > touch_ended_y_)
						{
							if(be_pageturn_effect)
							{
								if( (touch_began_y_ - touch_ended_y_) > TURN_SLIDE_LEN)	
								{ 

									fit_pos = old_y_ - height;
									float row_index = fit_pos/height;										 
									row_index = (int)(fabs(row_index)+0.5);

									turnToPage(row_index);
									list_state_ = LS_WAITING;
									return;

								}
								else
									fit_pos = old_y_;
							}
							else
							{
								fit_pos -= acce_val;
								// trigger loader event
								float row_index = old_y_/row_height_;								 
								row_index = (int)(fabs(row_index)+0.5);
								row_index = row_index/recodeNumberPerPage;
								triggerLoaderEvent(row_index,false);							

							}

							if (fit_pos < min_y_)
							{
								fit_pos = min_y_;
							}
						}
						else
						{
							if(be_pageturn_effect)
							{
								if( (touch_ended_y_ - touch_began_y_ ) > TURN_SLIDE_LEN)		
								{	

									fit_pos = old_y_ + height;
									float row_index = fit_pos/height;									
									row_index = (int)(fabs(row_index)+0.5);

									// trigger loader event
									//triggerLoaderEvent(row_index,true);
									turnToPage(row_index);
									list_state_ = LS_WAITING;
									return;
								}
								else
									fit_pos = old_y_;
							}
							else
							{
								fit_pos += acce_val;
								// trigger loader event
								float row_index = old_y_/row_height_ ;
								row_index = (int)(fabs(row_index)+0.5) + recodeNumberPerPage;
								row_index = row_index/recodeNumberPerPage;
								triggerLoaderEvent(row_index,true);
							}

							if (fit_pos > max_y_)
							{
								fit_pos = max_y_;
							}
						}

						doFitPos(fit_pos);
					}
				}
			}
		}

		slide_dir = SLIDER_NO;
		list_state_ = LS_WAITING;

	}


	//翻到指定页
	void ScutCxList::turnToPage(int nPageIndex)
	{
		// 向左拖拽
		if(nPageIndex < 0)
			nPageIndex = 0;

		if(recodeNumberPerPage <= 0)
			recodeNumberPerPage = 1;

		int nAmount = getChildCount();
		if(nPageIndex >= nAmount/recodeNumberPerPage )
		{
			if(nAmount%recodeNumberPerPage)
				nPageIndex = nAmount/recodeNumberPerPage;
			else
				nPageIndex = nAmount/recodeNumberPerPage - 1;

		}

		int nPos = 0;
		int nOldPos = 0;
		float fOldIndex = 0;
		if(is_horizontal_mode)		
		{		
			nPos =  -nPageIndex*recodeNumberPerPage*row_width_;
			nOldPos = inner_panel_->getPosition().x;

			fOldIndex = nOldPos/row_width_ ;
			fOldIndex = (int)(fabs(fOldIndex)+0.5);
			fOldIndex = fOldIndex/recodeNumberPerPage;

		}else
		{
			nPos =  nPageIndex*recodeNumberPerPage*row_height_;
			nOldPos = inner_panel_->getPosition().y;

			fOldIndex = nOldPos/row_height_ ;
			fOldIndex = (int)(fabs(fOldIndex)+0.5);
			fOldIndex = fOldIndex/recodeNumberPerPage;

		}

		doFitPos(nPos);

		if (kScriptTypeNone != m_eScriptType && m_nScriptLoadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeListLoader(this, m_nScriptLoadFunc,nPageIndex);
		}

		if(kScriptTypeNone != m_eScriptType && m_nScriptUnloadFunc)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->executeListLoader(this, m_nScriptUnloadFunc,fOldIndex);
		}

		if(!m_pLoaderListener)
			return;

		m_pLoaderListener->OnLoadItem(nPageIndex);
		m_pLoaderListener->OnUnLoadItem(fOldIndex);

	}

	void ScutCxList::ccTouchCancelled(CCTouch *touch, CCEvent *event)
	{
		list_state_ = LS_WAITING;
		slide_dir = SLIDER_NO;
	}

	void ScutCxList::ccTouchMoved(CCTouch* touch, CCEvent* event)
	{
		if (LS_TRACKINGTOUCH != list_state_)
		{
			return;
		}

		CCPoint touchPoint = touch->getLocationInView();
		touchPoint = CCDirector::sharedDirector()->convertToGL(touchPoint);         
		touch_ended_y_ = touchPoint.y;
		touch_ended_x_ = touchPoint.x;	


		int nOffsetX = touch_ended_x_ - touch_began_x_;
		int nOffsetY = touch_ended_y_ - touch_began_y_;

		if(slide_dir == SLIDER_NO)
		{
			if(abs(nOffsetX) > abs(nOffsetY))
			{
				if( abs(nOffsetX) >=5 )
					slide_dir = SLIDER_W;
			}
			else
			{
				if( abs(nOffsetY) >=5 )
					slide_dir = SLIDER_H;
			}

		}

		////模屏模式下，移动时x > y 才有效
		if(is_horizontal_mode )
		{
			if( slide_dir == SLIDER_W)//abs(nOffsetX) > abs(nOffsetY))
			{
			//	if (snap_flag_ && fabs(touch_ended_x_ - touch_began_x_) < 2.f)		 
			//		return;
				snap_flag_ = false;
				inner_panel_->setPosition(CCPointMake(old_x_ + (touch_ended_x_ - touch_began_x_),inner_panel_->getPosition().y));
			}
		}
		else
		{
			//竖屏模式下，移动时y> x 才有效
			if( slide_dir == SLIDER_H)//abs(nOffsetY) > abs(nOffsetX))
			{
			//	if (snap_flag_ && fabs(touch_ended_y_ - touch_began_y_) < 5.f)		 
			//		return;	
				snap_flag_ = false;
				inner_panel_->setPosition(CCPointMake(inner_panel_->getPosition().x, old_y_ + (touch_ended_y_ - touch_began_y_)));
			}
		}       
	}


	void ScutCxList::destroy(void)
	{
		release();
	}

	void ScutCxList::keep(void)
	{
		retain();
	}

	ScutCxListItem* ScutCxList::itemForTouch(CCTouch * touch)
	{
		CCArray *children = inner_panel_->getChildren();
		if (!children || !children->count())
		{
			return NULL;
		}

		/*CCPoint touch_loc = touch->locationInView(touch->view());
		touch_loc = CCDirector::sharedDirector()->convertToGL(touch_loc);
		CCPoint local_loc = inner_panel_->convertToNodeSpace(touch_loc);*/

		CCPoint touch_loc = touch->getLocation();
		CCPoint local_loc = inner_panel_->convertToNodeSpace(touch_loc);

		for (int i = 0, l = (int)children->count(); i != l; ++i)
		{
			CCNode *node = (CCNode *)children->objectAtIndex(i);
			if (node->boundingBox().containsPoint(local_loc))
			{
				return (ScutCxListItem*)node;
			}
		}

		return NULL;
	}


	void ScutCxList::DeleteChild(ScutCxListItem *child)
	{
		if(!child)
			return;		
		CCArray *children = inner_panel_->getChildren();

		if(children)
			children->removeObject(child);
	}

	void ScutCxList::DeleteChild(int nIndex)
	{

		// delete item
		CCArray *children = inner_panel_->getChildren();

		if(!children)
			return;


		if(nIndex < 0 || nIndex >= children->count())
			return;
		//children->removeObjectAtIndex(nIndex);
		ScutCxListItem *item = (ScutCxListItem *)children->objectAtIndex(nIndex);
		inner_panel_->removeChild(item, true);



		//process all item pos
		float total_height = 0.f;
		float item_x_pos = 0.f;
		float item_y_pos = 0.f;

		CCSize item_size;
		CCSize panel_size = inner_panel_->getContentSize();

		for (int i = 0; i < (int)children->count(); ++i)
		{

			ScutCxListItem *item = (ScutCxListItem *)children->objectAtIndex(i);			

			if(is_horizontal_mode)		
			{
				if (i >= 1)
				{
					ScutCxListItem *last_child = (ScutCxListItem *)children->objectAtIndex(i - 1);
					item_size = last_child->getContentSize();
					item_x_pos = last_child->getPosition().x + item_size.width;					
				}
				else
				{
					item_x_pos = 0;
				}

				item->setPosition(CCPointMake(item_x_pos, 0));
				item_size = item->getContentSize();				
				total_height = item_x_pos + item_size.width;
			}
			else
			{
				item_size = item->getContentSize();	
				if (i >= 1)
				{
					ScutCxListItem *last_child = (ScutCxListItem *)children->objectAtIndex(i - 1);					
					item_y_pos = last_child->getPosition().y - item_size.height;
				}
				else
				{
					item_y_pos = panel_size.height/CC_CONTENT_SCALE_FACTOR() - item_size.height;
				}

				item->setPosition(CCPointMake(0, item_y_pos));				
				total_height = item_y_pos + item_size.height;

			}
		}


		if(is_horizontal_mode)		
		{
			if (total_height > panel_size.width)
			{
				max_x_ = total_height - panel_size.width;
			}
			else
			{
				max_x_ = 0;
			}
		}
		else
		{

			if (total_height > panel_size.height)
			{
				max_y_ = total_height - panel_size.height;
			}
			else
			{
				max_y_ = 0;
			}		

		}	 

	 
	}


	void ScutCxList::selectChild(int row_index)
	{
		CCArray *children = inner_panel_->getChildren();
		if (!children || !children->count() || row_index < 0 || row_index >= (int)children->count())
		{
			return;
		}

		ScutCxListItem *sel_item = (ScutCxListItem *)children->objectAtIndex(row_index);
		if (sel_item != sel_item_)
		{
			sel_item->selected();

			if (sel_item_)
			{
				sel_item_->unselected();
				if (kScriptTypeNone != m_eScriptType && m_nScriptLoadFunc && m_nSelectIndex >= 0)
				{				
					CCScriptEngineManager::sharedManager()->getScriptEngine()->executeListLoader(this, m_nScriptUnselectFunc,m_nSelectIndex);
				}
				m_nSelectIndex = row_index;
			}

			sel_item_ = sel_item;

			CCSize panel_size = inner_panel_->getContentSize();
			CCArray *children = inner_panel_->getChildren();


			if(is_horizontal_mode)
			{
				float inc_width = 0.f;
				for (int i = 0; i <= row_index; ++i)
				{
					inc_width += ((CCNode *)children->objectAtIndex(i))->getContentSize().width;
				}

				if (inc_width > panel_size.width)
				{
					doFitPos(inc_width - panel_size.width);
				}
			}
			else
			{
				float inc_height = 0.f;
				for (int i = 0; i <= row_index; ++i)
				{
					inc_height += ((CCNode *)children->objectAtIndex(i))->getContentSize().height;
				}

				if (inc_height > panel_size.height)
				{
					doFitPos(inc_height - panel_size.height);
				}
			}
		}
	}

	ScutCxListItem *ScutCxList::getSelectedChild(void)
	{
		return sel_item_;
	}

	void ScutCxList::setLineColor(ccColor3B &color)
	{
		line_color_ = color;
	}

	void ScutCxList::setSelectedItemColor(ccColor3B &start_color, ccColor3B &end_color)
	{
		sel_item_start_color_ = start_color;
		sel_item_end_color_ = end_color;
	}

	int ScutCxList::getChildCount(void)
	{
		CCArray *children = inner_panel_->getChildren();
		if (!children)
		{
			return 0;
		}
		else
		{
			return children->count();
		}
	}

	void ScutCxList::setRowHeight(float height)
	{
		row_height_ = height;
	}

	float ScutCxList::getRowHeight(void)
	{
		return row_height_;
	}


	void ScutCxList::setRowWidth(float width)
	{
		row_width_ = width;
	}

	float ScutCxList::getRowWidth(void)
	{
		return row_width_;
	}

	void ScutCxList::registerItemClickListener(ScutCxListItemClickListener *listener)
	{
		item_click_listener_ = listener;
	}

	void ScutCxList::unregisterItemClickListener(void)
	{
		item_click_listener_ = NULL;

#if 1
		if (m_nScriptSeletor)
		{
			CCScriptEngineManager::sharedManager()->getScriptEngine()->removeScriptHandler(m_nScriptSeletor);
			LUALOG("[LUA] Remove ScutCxList script item click handler: %d", m_nScriptSeletor);
			m_nScriptSeletor = 0;
		}
#endif
	}


#if 1
	void ScutCxList::registerItemClickListener(int nSeletor)
	{
		unregisterItemClickListener();
		m_nScriptSeletor = nSeletor;
		LUALOG("[LUA] Add ScutCxList script item click handler: %d", m_nScriptSeletor);
	}
#endif

	void ScutCxList::visit(void)
	{
		// quick return if not visible
		if (!m_bVisible)
		{
			return;
		}
		kmGLPushMatrix();

		/*if (m_pGrid && m_pGrid->isActive())
		{
		m_pGrid->beforeDraw();
		this->transformAncestors();
		}*/

		this->transform();

		CCNode* pNode = NULL;
		unsigned int i = 0;

		if(m_pChildren && m_pChildren->count() > 0)
		{
			// draw children zOrder < 0
			ccArray *arrayData = m_pChildren->data;
			for( ; i < arrayData->num; i++ )
			{
				pNode = (CCNode*) arrayData->arr[i];

				if ( pNode && pNode->getZOrder() < 0 ) 
				{				
					pNode->visit();
				}
				else
				{
					break;
				}
			}
		}

		//判断是否超出范围，如果超出，那么不进行绘画
		bool bDraw = 1;
		if (this->getParent())
		{			 
			if (this->getPosition().y + nParentY > nParentH || this->getPosition().y + nParentY + this->getContentSize().height < 0)
			{
				bDraw = 0;				
			}
			if (this->getPosition().x + nParentX > nParentW || this->getPosition().x + nParentX + this->getContentSize().width < 0)
			{
				bDraw = 0;				
			}
		}

		// self draw
		if(bDraw)
			this->draw();

		// 左下角世界坐标
		//CCPoint world_pt = convertToWorldSpace(CCPoint(0,0));
		//CCPoint ui_pt = CCDirector::sharedDirector()->convertToUI(world_pt);
		//CCPoint gl_pt = CCDirector::sharedDirector()->convertToGL(ui_pt);


		CCPoint gl_pt = convertToWorldSpace(CCPoint(0,0));
		gl_pt.x *=CC_CONTENT_SCALE_FACTOR();
		gl_pt.y *=CC_CONTENT_SCALE_FACTOR();


		glEnable(GL_SCISSOR_TEST);


		//CCPoint Scis = gl_pt;
		int nScissorW = m_obContentSize.width;
		int nScissorH = m_obContentSize.height;



		if(nParentW)
		{

			nScissorW = nParentW;

			if(gl_pt.x < nParentX)
				gl_pt.x = nParentX;

			if(gl_pt.y > nParentY)
				gl_pt.y = nParentY;


			if(nScissorW > nParentW)
				nScissorW = nParentW;


			if(nScissorH > nParentH)
				nScissorH = nParentH;

		}



		if(nParentW)
			if(gl_pt.x + nScissorW > nParentW + nParentX)
			{
				nScissorW = nParentW + nParentX - gl_pt.x;
				if(nScissorW < 0)
					nScissorW = 0;
			}


			if(nParentH)
				if(gl_pt.y + nScissorH > nParentH + nParentY)
				{
					nScissorH = nParentH + nParentY - gl_pt.y;
					if(nScissorH < 0)
						nScissorH = 0;
				}

			glScissor((GLsizei)gl_pt.x, (GLsizei)gl_pt.y, (GLsizei)nScissorW , (GLsizei)nScissorH);

			// old	
			//glScissor((GLsizei)gl_pt.x, (GLsizei)gl_pt.y, (GLsizei)m_obContentSize.width , (GLsizei)m_obContentSize.height);

			// draw children zOrder >= 0
			if (m_pChildren && m_pChildren->count() > 0)
			{
				ccArray *arrayData = m_pChildren->data;
				for( ; i < arrayData->num; i++ )
				{
					pNode = (CCNode*) arrayData->arr[i];
					if (pNode)
					{					
						pNode->visit();
					}
				}		
			}

			glDisable(GL_SCISSOR_TEST);

			kmGLPopMatrix();
	}

	void ScutCxList::setPageTurnEffect(bool bPageTurn)
	{
		be_pageturn_effect = bPageTurn;
	}

	bool  ScutCxList::getPageTurnEffect()
	{
		return be_pageturn_effect;
	} 

	void ScutCxList::setRecodeNumPerPage(int nNumber)
	{
		recodeNumberPerPage = nNumber;
	}
	int ScutCxList::getRecodeNumPerPage()
	{
		return recodeNumberPerPage;
	}

	void ScutCxList::setHorizontal(bool bHorizontal)
	{
		is_horizontal_mode = bHorizontal;
	}

	bool ScutCxList::isHorizontal_mode()
	{
		return is_horizontal_mode;

	}

	void ScutCxList::disableAllCtrlEvent()
	{
		CCArray *children = inner_panel_->getChildren();
		if(!children)
			return;

		for (int i = 0, l = (int)children->count(); i != l; ++i)
		{
			CCNode *node = NULL;
			node = (CCNode *)children->objectAtIndex(i);
			if (node)
			{
				((ScutCxListItem*)node)->disableCtrlEvent();
			}
		}
	}


	bool ScutCxList::containsTouchLocation(CCTouch* touch)
	{

		CCPoint point = convertTouchToNodeSpace(touch);
		bool bIn =  rect().containsPoint(point);
		if(bIn)
			return true; 

		//对嵌套列表作特殊处理	
		if(nParentH || nParentW)
		{				
			CCPoint viewPoint = touch->getLocationInView();
			CCRect r = CCRectMake(nParentX,nParentY,nParentW,nParentH);			
			if (r.containsPoint(viewPoint))
			{
				return true;
			}	

		}		
		return false;
	}
}




