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
#include "ScutCxListItem.h"
#include "ScutCxList.h"


const int TOUCHBEGIN	= 1;
const int TOUCHEND		= 2;
const int TOUCHMOVING	= 3;
const int TOUCHCANCELLED= 4;


bool g_bCatchLayer = false;

namespace ScutCxControl 
{

	ScutCxListItem::ScutCxListItem(void) 
		: selected_(false)
		, draw_top_line_(false)
		, draw_bottom_line_(false)
		, draw_selected(true)
		, horizontal_margin_(10.f)
		, vertical_margin_(10.f)
		, m_bSilence(false)
	{
		setTouchEnabled(true);

	}

	ScutCxListItem::~ScutCxListItem(void) 
	{

	}

	void ScutCxListItem::updateColor(void)
	{
		const unsigned int l = sizeof(m_pSquareColors) / sizeof(m_pSquareColors[0]);

		if (selected_)
		{
			ccColor3B color = sel_item_start_color_;
			const unsigned int half = l / 2;
			for (unsigned int i=0; i < l; ++i)
			{
				if (i >= half)
				{
					color = sel_item_end_color_;
				}

				if (i % 4 == 0)
					m_pSquareColors[i].r = color.r;
				else if (i % 4 == 1)
					m_pSquareColors[i].g = color.g;
				else if (i % 4 ==2)
					m_pSquareColors[i].b = color.b;
				else
					m_pSquareColors[i].a = _displayedOpacity;
			}
		}
		else
		{
			for( unsigned int i=0; i < 4; i++ )
			{
				m_pSquareColors[i].r     = _displayedColor.r;
				m_pSquareColors[i].g = _displayedColor.g;
				m_pSquareColors[i].b = _displayedColor.b;
				m_pSquareColors[i].a = _displayedOpacity;
			}
		}
	}

	void ScutCxListItem::draw(void)
	{
		//mark es2.0
		//判断是否超出范围，如果超出，那么不进行绘画
		if (this->getParent())
		{
			if (this->getPosition().y + this->getParent()->getPosition().y > this->getParent()->getContentSize().height || this->getPosition().y + this->getParent()->getPosition().y + this->getContentSize().height < 0)
			{
				return;
			}
		}

		updateColor();      

		CCSize size = getContentSize();
		if (selected_)
		{
			if(draw_selected)
			{
				CC_NODE_DRAW_SETUP();

				ccGLBlendFunc( m_tBlendFunc.src, m_tBlendFunc.dst );

				ccGLEnableVertexAttribs( kCCVertexAttribFlag_Position |  kCCVertexAttribFlag_Color);

				// vertex
				glVertexAttribPointer(kCCVertexAttrib_Position, 2, GL_FLOAT, GL_FALSE, 0, (void*)m_pSquareVertices);

				// color
				glVertexAttribPointer(kCCVertexAttrib_Color, 4, GL_FLOAT, GL_TRUE, 0, (void*)m_pSquareColors);

				glDrawArrays(GL_TRIANGLE_STRIP, 0, 4);

				CHECK_GL_ERROR_DEBUG();

				CC_INCREMENT_GL_DRAWS(1);
			}
		}
		else
		{
			CCLayerColor::draw();
		}

		//glDisable(GL_LINE_SMOOTH);


		if (draw_top_line_)
		{
			glLineWidth(1.0f);
			cocos2d::ccDrawColor4F(line_color_.r, line_color_.g, line_color_.b, 0xFF);
			ccDrawLine(CCPointMake(0, size.height-0.5f), CCPointMake(size.width, size.height-0.5f));
		}

		if (draw_bottom_line_)
		{
			glLineWidth(1.0f);
			cocos2d::ccDrawColor4F(line_color_.r, line_color_.g, line_color_.b, 0xFF);
			ccDrawLine(CCPointMake(0, 0.5f), CCPointMake(size.width, 0.5f));
		}

	}

	CCRect ScutCxListItem::rect(void)
	{
		return CCRectMake( m_obPosition.x - m_obContentSize.width * m_obAnchorPoint.x, 
			m_obPosition.y - m_obContentSize.height * m_obAnchorPoint.y,
			m_obContentSize.width, m_obContentSize.height);
	}

	void ScutCxListItem::selected(void)
	{
		selected_ = true;
	}

	void ScutCxListItem::unselected(void)
	{
		selected_ = false;
	}

	void ScutCxListItem::setItemColor(const ccColor3B &color)
	{
		setColor(color);
	}

	void ScutCxListItem::setMargin(CCSize margin)
	{
		horizontal_margin_ = margin.width;
		vertical_margin_ = margin.height;
	}

	CCSize ScutCxListItem::getMargin(void)
	{
		return CCSizeMake(horizontal_margin_, vertical_margin_);
	}


	ScutCxListItemChild *ScutCxListItem::getChildByTag(int tag)
	{
		return (ScutCxListItemChild *)CCLayerColor::getChildByTag(tag);
	}


	bool ScutCxListItem::findInDeque(CCNode* node)
	{

		TOUCHBEGAN_VEC::iterator iter;
		iter=m_pVectorTouchBegan.begin();
		for(iter; iter!=m_pVectorTouchBegan.end(); iter++)
		{
			if(*iter && *iter == node)
				return true;
		}

		return false;
	}

	bool ScutCxListItem::findInTouchEventLayerDeque(CCNode* node)
	{
		TOUCHBEGAN_VEC::iterator iter;
		iter=m_pVectorTouchEventLayer.begin();
		for(iter; iter!=m_pVectorTouchEventLayer.end(); iter++)
		{
			if(*iter && *iter == node)
				return true;
		}

		return false;
	}

	void ScutCxListItem::requestLayout(void)
	{

		if (m_pChildren && m_pChildren->count() > 0)
		{
			CCPoint child_pos;
			CCSize size = getContentSize();
			CCPoint curr_pos;
			CCPoint prev_pos;
			float vertical_height = size.height;
			bool height_changed = false;

			for (int i = 0, l = m_pChildren->count(); i < l; ++i)
			{
				CCNode *it = (CCNode *)m_pChildren->objectAtIndex(i);
				if (!(it))
				{
					break;
				}

				CCSize child_size = (*(it)).getContentSize();
				child_size.width *= (*(it)).getScaleX();
				child_size.height *= (*(it)).getScaleY();

				std::map<CCNode *, LayoutParam>::iterator pos = layout_info_.find(it);
				if(layout_info_.end() == pos)
					break;
				//   CCAssert(layout_info_.end() != pos, "!!");
				LayoutParam *lp = &pos->second;
				switch ((*lp).val_x.t)
				{
				case PARENT_CENTER:
					child_pos.x = (size.width - child_size.width) / 2;
					break;
				case HORIZONTAL_LEFT:
					child_pos.x = horizontal_margin_;
					break;
				case HORIZONTAL_RIGHT:
					child_pos.x = size.width - child_size.width - horizontal_margin_;
					break;
				case ABS_WITH_PIXEL:
					child_pos.x = (float)(*lp).val_x.val.pixel_val;
					break;
				case ABS_WITH_PERCENT:
					child_pos.x = horizontal_margin_ + (*lp).val_x.val.percent_val * (size.width - 2*horizontal_margin_);
					break;
				case REF_PREV_X_INC:
					{
						child_pos.x = prev_pos.x + (*lp).padding;
						break;
					}
				case REF_PREV_X_DEC:
					{
						child_pos.x = prev_pos.x - (*lp).padding;
						break;
					}
				case REL_FLOW:
					child_pos.x = curr_pos.x + (*lp).padding;
					break;
				}

				if (!(*lp).wrap)
				{
					curr_pos.x = child_pos.x + child_size.width;
				}

				switch ((*lp).val_y.t)
				{
				case PARENT_CENTER:
					{						
						child_pos.y = (vertical_height/CC_CONTENT_SCALE_FACTOR() - child_size.height)*0.5f;

						break;
					}
				case VERTICAL_TOP:
					{						
						child_pos.y = vertical_height - (child_size.height + vertical_margin_);

						break;
					}
				case VERTICAL_BOTTOM:
					{
						child_pos.y = vertical_margin_;

						if (child_size.height > vertical_height - 2*vertical_margin_)
						{
							vertical_height += child_size.height - (vertical_height - 2*vertical_margin_);
							height_changed = true;
						}


						break;
					}
				case ABS_WITH_PIXEL:
					{
						child_pos.y = (float)(*lp).val_y.val.pixel_val;
						if (child_pos.y + child_size.height > vertical_height - vertical_margin_)
						{
							vertical_height = child_pos.y + child_size.height + vertical_margin_;
							height_changed = true;
						}

						break;
					}
				case ABS_WITH_PERCENT:
					{
						child_pos.y = (*lp).val_y.val.percent_val * (vertical_height - 2*vertical_margin_);
						if (child_pos.y + child_size.height > vertical_height - vertical_margin_)
						{
							vertical_height = child_pos.y + child_size.height + vertical_margin_;
							height_changed = true;
						}

						break;
					}
				case REF_PREV_Y_INC:
					{
						child_pos.y = prev_pos.y + (*lp).padding;
						if (child_pos.y + child_size.height > + vertical_height - vertical_margin_)
						{
							vertical_height = child_pos.y + child_size.height + vertical_margin_;
							height_changed = true;
						}

						break;
					}
				case REF_PREV_Y_DEC:
					{
						child_pos.y = prev_pos.y - (child_size.height + (*lp).padding);
						if (child_pos.y < vertical_margin_)
						{
							vertical_height += vertical_margin_-child_pos.y + vertical_margin_;
							child_pos.y = vertical_margin_;
							height_changed = true;
						}

						break;
					}                    
				case REL_FLOW:
					{
						child_pos.y = curr_pos.y + (*lp).padding;
						if (child_pos.y + child_size.height > vertical_height - vertical_margin_)
						{
							vertical_height = curr_pos.y + child_size.height + vertical_margin_;
							height_changed = true;
						}

						break;
					}

					curr_pos.y = child_pos.y + child_size.height;
				}

				prev_pos = child_pos;

				(*it).setPosition(child_pos);
			}

			//忠实于用户的布局坐标
			/*
			setContentSize(CCSizeMake(size.width, vertical_height));	 
			if (height_changed)
			{
			requestLayout();
			}
			*/

		}
	}

	ScutCxListItem *ScutCxListItem::itemWithColor(const ccColor3B &color)
	{
		ScutCxListItem *pRet = new ScutCxListItem();
		pRet->setOpacity(255);
		pRet->setColor(color);
		pRet->autorelease();
		return pRet;
	}

	bool ScutCxListItem::initWithWidthHeight(GLfloat width, GLfloat height)
	{
		ccColor3B color = getColor();
		ccColor4B cl4b = { color.r, color.g, color.b, getOpacity() };
		return CCLayerColor::initWithColor(cl4b, width, height);
	}

	void ScutCxListItem::setLineColor(const ccColor3B &color)
	{
		line_color_ = color;
	}

	void ScutCxListItem::setSelectedColor(const ccColor3B &start_color, const ccColor3B &end_color)
	{
		sel_item_start_color_ = start_color;
		sel_item_end_color_ = end_color;
	}

	void ScutCxListItem::registerWithTouchDispatcher()
	{		                                                                  
		cocos2d::CCDirector::sharedDirector()->getTouchDispatcher()->addTargetedDelegate(this, kCCMenuHandlerPriority, false);
	}


	void ScutCxListItem::addChildItem(CCNode *child)
	{
		LayoutParam layout;
		layout.val_x.t = ABS_WITH_PIXEL;
		layout.val_y.t = ABS_WITH_PIXEL;

		CCPoint point = child->getPosition();
		layout.val_x.val.pixel_val = point.x;
		layout.val_y.val.pixel_val = point.y;

		addChildItem(child,layout);
	}

	void ScutCxListItem::addChildItem(CCNode *child, const LayoutParam &layout)
	{
		addChildItem(child, layout, 0);		
	}

	void ScutCxListItem::addChildItem(CCNode *child, const LayoutParam &layout, int tag)
	{
		layout_info_.insert(std::make_pair(child, layout));
		child->ignoreAnchorPointForPosition(true);
		CCLayerColor::addChild(child, 0, tag);

		disableCtrlChildEvent(child);
	}


	void ScutCxListItem::clear()
	{	 
		if (m_pChildren && m_pChildren->count() > 0)
		{		
			CCNode* pItem = NULL; 
			for (int i = 0, l = m_pChildren->count(); i < l; ++i)
			{
				pItem = (CCNode *)m_pChildren->objectAtIndex(i);

				if(!pItem)
					continue;

				if(dynamic_cast<ScutCxList*>(pItem) != NULL)
				{
					ScutCxList* pList = (ScutCxList*)pItem;
					pList->clear();
					//pLayer->removeAllChildrenWithCleanup(true);
				}					 				

			} 	 
		}	

		m_pVectorTouchEventLayer.clear();
		m_pVectorTouchBegan.clear();

	}

	void ScutCxListItem::disableCtrlChildEvent(CCNode* pParent)
	{
		if(!pParent)
			return;

		if(dynamic_cast<CCLayer*>(pParent) != NULL)
		{	
			if( ((CCLayer*)pParent)->isTouchEnabled() )			
				m_pVectorTouchEventLayer.push_back((CCLayer*)pParent);
			
			((CCLayer*)pParent)->setTouchEnabled(false);
		}

		if (pParent->getChildren())
		{
			CCNode* pItem = NULL; 
			for (int i = 0, l = pParent->getChildren()->count(); i < l; ++i)
			{
				pItem = (CCNode *)pParent->getChildren()->objectAtIndex(i);
				if(pItem)
				{	
					if(pItem->isRunning() && dynamic_cast<CCLayer*>(pItem) != NULL)
					{
						//pItem->onExit();
						//assert(0);
					}
					disableCtrlChildEvent(pItem);		 
				} 
			}	 
		}
	}

	void ScutCxListItem::disableCtrlEvent()
	{
		if (m_pChildren && m_pChildren->count() > 0)
		{
			CCNode* pItem = NULL; 
			for (int i = 0, l = m_pChildren->count(); i < l; ++i)
			{
				pItem = NULL;
				pItem= (CCNode *)m_pChildren->objectAtIndex(i);
				if(pItem)
				{
					if(dynamic_cast<ScutCxList*>(pItem) != NULL)
					{
						CCLayer* pLayer = ((ScutCxList*)pItem)->getInner_panel();
						disableCtrlChildEvent(pLayer);
					}
					else
					{
						disableCtrlChildEvent(pItem);	
					}
				}					
			}				 
		}
	}



	
	void ScutCxListItem::MyTouchDispatcher(CCNode* pParent, int nToutchEvent, CCTouch* touch, CCEvent* event)
	{

			CCNode* parent = pParent;
			while(parent)
			{	
				//对嵌套列表TOUCH END 做事件传递
				//if (parent->getClassType() == ScutLIST_classID && nToutchEvent == TOUCHEND)
				//	break;

				if(findInDeque(pParent))
				{
					 if(nToutchEvent == TOUCHEND)
						break;
				}

				CCPoint pos_parent =  parent->getPosition();
				CCPoint pos_in_parent  = parent->convertTouchToNodeSpace(touch);										
				CCSize  size_parent = parent->getContentSize();


				////对横屏例表作特殊处理
				if(pos_parent.x < 0)
					pos_in_parent.x = pos_in_parent.x + pos_parent.x;


				CCRect r;
				r.origin	= CCPointZero;
				r.size		= size_parent;

				if (!r.containsPoint(pos_in_parent))
				{

					if(pos_parent.y > 0)
					{
						//对竖屏例表作特殊处理
						pos_in_parent.y = pos_in_parent.y + pos_parent.y;
						if (!r.containsPoint(pos_in_parent))
						{
							return;
						}
					}
					else
					{
						return;
					}
				}				
				parent = parent->getParent();
			}



		switch(nToutchEvent)
		{			
			case TOUCHBEGIN:
				{
					if(dynamic_cast<CCLayer*>(pParent) != NULL)
				{
					bool bCatch = ((CCLayer*)pParent)->ccTouchBegan(touch,event);
					if(bCatch && findInTouchEventLayerDeque(pParent) )
					{
						m_pVectorTouchBegan.push_back((CCLayer*)pParent);						
						m_bCatchEndNode = true;	
						g_bCatchLayer = true;
					}
				}

				if (pParent && pParent->getChildren())
				{
					CCNode* pItem = NULL; 
					for (int i = 0, l = pParent->getChildren()->count(); i < l; ++i)
					{
						pItem = (CCNode *)pParent->getChildren()->objectAtIndex(i);		 
						if(pItem)
						{
							MyTouchDispatcher(pItem, nToutchEvent,touch,event);
						}						 
					}	 
				}
			}
			break;

		case TOUCHEND:
			{

				if(dynamic_cast<CCLayer*>(pParent) != NULL)
					((CCLayer*)pParent)->ccTouchEnded(touch,event);
				if (pParent && pParent->getChildren())
				{
					CCNode* pItem = NULL; 
					for (int i = 0, l = pParent->getChildren()->count(); i < l; ++i)
					{
						pItem = (CCNode *)pParent->getChildren()->objectAtIndex(i);
						if(pItem)
						{
							MyTouchDispatcher(pItem, nToutchEvent,touch,event);
						}
					}	 
				}

			}
			break;

		case TOUCHMOVING:
			{

				if(dynamic_cast<CCLayer*>(pParent) != NULL)			
					((CCLayer*)pParent)->ccTouchMoved(touch,event);
				if (pParent && pParent->getChildren())
				{
					CCNode* pItem = NULL; 
					for (int i = 0, l = pParent->getChildren()->count(); i < l; ++i)
					{
						pItem = (CCNode *)pParent->getChildren()->objectAtIndex(i);
						if(pItem)
						{
							MyTouchDispatcher(pItem, nToutchEvent,touch,event);
						}
					}	 
				}

			}
			break;
			
		case TOUCHCANCELLED:
			{				 
				if(dynamic_cast<CCLayer*>(pParent) != NULL)
					((CCLayer*)pParent)->ccTouchCancelled(touch,event);
				if (pParent && pParent->getChildren())
				{
					CCNode* pItem = NULL; 
					for (int i = 0, l = pParent->getChildren()->count(); i < l; ++i)
					{
						pItem = (CCNode *)pParent->getChildren()->objectAtIndex(i);
						if(pItem)
						{
							MyTouchDispatcher(pItem, nToutchEvent,touch,event);
						}
					}	 
				}
			}
			break;

		default:
			break;

		}
	}


	bool ScutCxListItem::ccTouchBegan(CCTouch* touch, CCEvent* event)
	{	
		m_pVectorTouchBegan.clear();
		m_bCatchEndNode = false;		

		if(m_bSilence)
			return false;

		CCRect r;
		r.origin = CCPointZero;
		r.size = m_obContentSize;

		CCPoint pos = convertTouchToNodeSpace(touch);					

		if (r.containsPoint(pos))
		{
			if (m_pChildren && m_pChildren->count() > 0)
			{
				CCNode* pItem = NULL;
				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem)
					{
						MyTouchDispatcher((CCLayer*)pItem, TOUCHBEGIN,touch ,event);
					}

				}	 
			}
		} 
		/*
		else
		{
			CCNode* pItem = NULL; 
			if(m_pChildren && selected_)
			{
				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem && pItem->getClassType() >= CCLayer_classID )
					{
						((CCLayer*)pItem)->ccTouchBegan(touch,event);
					}
				}
			}
		}
		*/
		return true;

	}

	void ScutCxListItem::ccTouchEnded(CCTouch* touch, CCEvent* event)
	{

		if(m_bSilence)
			return;

		CCRect r;
		r.origin = CCPointZero;
		r.size = m_obContentSize;

	//	CCPoint pos = convertTouchToNodeSpace(touch);	
		//if (CCRect::CCRectContainsPoint(r, pos))
		{			 
			if (m_pChildren && m_pChildren->count() > 0)
			{

				CCNode* pItem = NULL; 

				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem)
					{
						MyTouchDispatcher((CCLayer*)pItem, TOUCHEND,touch ,event);
					}

				}				 
			}
		}
		/*
		else
		{
			CCNode* pItem = NULL; 
			if(m_pChildren)
			{
				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem && pItem->getClassType() >= CCLayer_classID )
					{
						((CCLayer*)pItem)->ccTouchEnded(touch,event);
					}
				}
			}

		}
		*/

		m_pVectorTouchBegan.clear();

	}


	void ScutCxListItem::ccTouchCancelled(CCTouch *touch, CCEvent* event)
	{

		if(m_bSilence)
			return;

		CCRect r;
		r.origin = CCPointZero;
		r.size = m_obContentSize;

		//CCPoint pos = convertTouchToNodeSpace(touch);	
		//if (CCRect::CCRectContainsPoint(r, pos))
		{			 
			if (m_pChildren && m_pChildren->count() > 0)
			{

				CCNode* pItem = NULL; 

				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if(pItem)
					{
						MyTouchDispatcher((CCLayer*)pItem, TOUCHCANCELLED,touch ,event);						
					}						
				}				 
			}
		} 
		/*
		else
		{
			CCNode* pItem = NULL; 
			if(m_pChildren && selected_)
			{
				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem && pItem->getClassType() >= CCLayer_classID )
					{
						((CCLayer*)pItem)->ccTouchCancelled(touch,event);
					}

				}	
			}

		}*/


		m_pVectorTouchBegan.clear();

	}


	void ScutCxListItem::ccTouchMoved(CCTouch* touch, CCEvent* event)
	{
		if(m_bSilence)
			return;

		CCRect r;
		r.origin = CCPointZero;
		r.size = m_obContentSize;

		//CCPoint pos = convertTouchToNodeSpace(touch);
		//if (CCRect::CCRectContainsPoint(r, pos))
		{			 
			if (m_pChildren && m_pChildren->count() > 0)
			{

				CCNode* pItem = NULL; 

				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if(pItem)
					{
						MyTouchDispatcher((CCLayer*)pItem, TOUCHMOVING,touch ,event);						
					}						
				}				 
			}
		} 
		/*
		else
		{
			CCNode* pItem = NULL; 
			if(m_pChildren && selected_)
			{
				for (int i = 0, l = m_pChildren->count(); i < l; ++i)
				{
					pItem = (CCNode *)m_pChildren->objectAtIndex(i);
					if (pItem && pItem->getClassType() >= CCLayer_classID )
					{
						((CCLayer*)pItem)->ccTouchMoved(touch,event);
					}
				}	
			}
		}
		*/

	}

	void ScutCxListItem::onEnter(void)
	{

		//setIsTouchEnabled(true);
		CCLayerColor::onEnter();
	}

	void ScutCxListItem::onExit(void)
	{
		//setIsTouchEnabled(false);
		CCLayerColor::onExit();
	}

}