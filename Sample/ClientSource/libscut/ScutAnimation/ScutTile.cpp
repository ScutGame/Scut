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

#include "ScutTile.h"
#include "cocoa/CCAffineTransform.h"

using namespace cocos2d;

namespace ScutAnimation
{
	CScutTile::CScutTile(const ScutTileTable& tt) : m_tt(tt)
	{
		m_frameX = 0;
		m_frameY = 0;
		m_rotation = 0;
		m_flipX = false;
		m_flipY = false;
		m_scale = 1.0f;
		m_centerX = 0.0f;
		m_centerY = 0.0f;
		::memset(vertices, 0L, sizeof(vertices));
	}
	
	CScutTile::~CScutTile()
	{
		
	}
	
	void CScutTile::caculateTileVertices(float anchorX, float anchorY)
	{
		GLfloat width = this->m_tt.clip.size.width * m_scale;
		GLfloat height = this->m_tt.clip.size.height * m_scale;

		float theta = (float)m_rotation / 180 * M_PI;
		
		m_centerX = this->m_frameX - anchorX + (height * fabs(sinf(theta))  + width * fabs(cosf(theta))) / 2.0f;
		m_centerY = anchorY - this->m_frameY - (height * fabs(cosf(theta)) + width * fabs(sinf(theta))) / 2.0f;

		CCPoint dest = ccp(- width / 2, - height / 2);

		CCPoint point = caculateTileObsoluteVertices(dest.x, dest.y);

		this->vertices[0] = point.x;
		this->vertices[1] = point.y;
		this->vertices[2] = 0.0f;

		point = caculateTileObsoluteVertices(width + dest.x, dest.y);

		this->vertices[3] = point.x;
		this->vertices[4] = point.y;
		this->vertices[5] = 0.0f;

		point = caculateTileObsoluteVertices(dest.x, height  + dest.y);

		this->vertices[6] = point.x;
		this->vertices[7] = point.y;
		this->vertices[8] = 0.0f;

		point = caculateTileObsoluteVertices(width + dest.x, height  + dest.y);

		this->vertices[9] = point.x;
		this->vertices[10] = point.y;
		this->vertices[11] = 0.0f;
	}

	CCPoint CScutTile::caculateTileObsoluteVertices(float x, float y)
	{
		CCAffineTransform t = CCAffineTransformIdentity;
		t = CCAffineTransformTranslate(t, m_centerX, m_centerY);

		if( m_rotation != 0 )
			t = CCAffineTransformRotate(t, -CC_DEGREES_TO_RADIANS(m_rotation));

		float scaleX = m_flipX ? -1.0f : 1.0f;
		float scaleY = m_flipY ? -1.0f : 1.0f;

		t = CCAffineTransformScale(t, scaleX, scaleY);

		CCPoint p(x, y);
		p = CCPointApplyAffineTransform(p, t);

		return p;
	}
}