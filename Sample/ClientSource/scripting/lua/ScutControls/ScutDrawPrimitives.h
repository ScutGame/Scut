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

#ifndef _SCUTDRAWPRIMITIVES_H
#define _SCUTDRAWPRIMITIVES_H
#include "../cocos2dx/draw_nodes/CCDrawingPrimitives.h"
#include "../cocos2dx_support/CCLuaEngine.h"
#include "ccTypes.h"
#include "../cocos2dx/base_nodes/CCNode.h"

using namespace cocos2d;
namespace ScutCxControl
{

	/**
	* @brief  »­Ïß
	* @remarks   
	* @see		
	*/
	class LUA_DLL ScutLineNode: public CCNode
	{
	public:
		ScutLineNode(){ m_fLineWidth = 1.f;}
		~ScutLineNode(){}
		void draw();
		static ScutLineNode* lineWithPoint(cocos2d::CCPoint origin, cocos2d::CCPoint destination ,
			float fLineWidth, cocos2d::ccColor4B color);

		void  DrawLine( cocos2d::CCPoint origin, cocos2d::CCPoint destination, 
			float fLineWidth, cocos2d::ccColor4B color);
	private:
		float				m_fLineWidth;
		cocos2d::CCPoint	m_originPt;
		cocos2d::CCPoint	m_desPt;
		cocos2d::ccColor4B	m_color4b;

	};
	


}
#endif//_CNDDRAWPRIMITIVES_H