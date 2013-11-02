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

#ifndef __SCUT_TILE_H__
#define __SCUT_TILE_H__

#include "cocos2d.h"
#include "Defines.h"
#include "ScutTileTable.h"

USING_NS_CC;

namespace ScutAnimation
{
	class CScutTile
	{
	public:
		enum TILE_FLIP
		{
			FLIP_X = 0x01,
			FLIP_Y = 0x02,
		};

	public:
		CScutTile(const ScutTileTable& tt);
		~CScutTile();
		// 优化措施,预计算图块顶点坐标
		void caculateTileVertices(float anchorX, float anchorY);
	
	private:
		CCPoint caculateTileObsoluteVertices(float x, float y);
	public:
		float m_frameX;
		float m_frameY;
		float m_rotation;
		bool m_flipX;
		bool m_flipY;
		float m_scale;
		const ScutTileTable& m_tt;
		GLfloat vertices[12];

		float m_centerX;
		float m_centerY;
	};
}

#endif