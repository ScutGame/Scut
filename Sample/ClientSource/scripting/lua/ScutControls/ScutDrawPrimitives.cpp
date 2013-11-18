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
#include "ScutDrawPrimitives.h"
namespace ScutCxControl
{
	void  ScutLineNode::DrawLine( cocos2d::CCPoint origin, cocos2d::CCPoint destination, float fLineWidth, cocos2d::ccColor4B color)
	{
		if (fLineWidth > 1.0f)
		{
			//glDisable(GL_LINE_SMOOTH);
		}
		else
		{
			//glEnable(GL_LINE_SMOOTH);
		}
		glLineWidth(fLineWidth);
		//glColor4f(color.r/ 255.f, color.g / 255.f, color.b/ 255.f, color.a / 255.f);
		ccDrawLine(origin, destination);
		
		//glDisable(GL_LINE_SMOOTH);
		glLineWidth(1.0);
		//glColor4f(1.0, 1.0, 1.0, 1.0);
	}

	ScutLineNode* ScutLineNode::lineWithPoint( cocos2d::CCPoint origin, cocos2d::CCPoint destination , float fLineWidth, cocos2d::ccColor4B color )
	{
		ScutLineNode* pNode = new ScutLineNode();
		pNode->m_fLineWidth	= fLineWidth;
		pNode->m_originPt	= origin;
		pNode->m_desPt		= destination;
		pNode->m_color4b	= color;
		pNode->autorelease();
		return pNode;
	}

	void ScutLineNode::draw()
	{
		CCNode::draw();
		DrawLine(m_originPt, m_desPt, m_fLineWidth, m_color4b);
	}
}