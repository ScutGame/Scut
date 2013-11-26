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

#ifndef __SCUT_FRAME_H__
#define __SCUT_FRAME_H__

#include "cocos2d.h"
#include <map>
using namespace cocos2d;
using namespace std;

namespace ScutAnimation
{
	class CScutTile;
	class CScutAniGroup;
	class CScutFrame
	{
	public:
		CScutFrame();
		~CScutFrame();
		
		// 返回该帧图块总数
		int getTileCount();
		// 返回指定索引的图块
		CScutTile* getTileByIndex(int nIndex);
		
		void calQuads();

		void drawQuads();

		void setScutAniGroup(CScutAniGroup* pScutAniGroup);
	public:
		int m_duration;
		
		typedef vector<CScutTile*> vec_tile;
		typedef vec_tile::iterator vec_tile_it;
		vec_tile m_vTiles;

	private:
		void initQuads(int nTotalCount);
		void initIndices(int count);

		void calTextureTileCount(int count);
		void onDraw();
	protected:
		int                 m_nTexCount;
		int                 m_nMaxIndex;
		CCTexture2D         *m_nMaxTexture;
		GLushort			*m_pIndices;
		ccV3F_T2F_Quad      *m_pQuads;
		CCTexture2D         *m_pTexture;
		CScutAniGroup         *m_ScutAniGroup;

		typedef map<CCTexture2D*, ccV3F_T2F_Quad*> map_V3F_T2F_Quad;
		typedef map_V3F_T2F_Quad::iterator map_V3F_T2F_Quad_it;
		map_V3F_T2F_Quad m_mapTexture;

		typedef map<CCTexture2D*, int> map_TextureCount;
		typedef map_TextureCount::iterator map_TextureCount_it;
		map_TextureCount m_mapTextureCount;

		typedef map<CCTexture2D*, GLuint> map_TextureIndex;
		typedef map_TextureIndex::iterator map_TextureIndex_it;
		map_TextureIndex map_mapTextureIndex;
	};
}

#endif

