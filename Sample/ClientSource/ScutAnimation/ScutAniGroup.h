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

#ifndef __SCUT_ANI_GROUP_H__
#define __SCUT_ANI_GROUP_H__

#include "cocos2d.h"
#include "Defines.h"
#include "ScutTileTable.h"
#include <vector>

using namespace std;
#ifdef WIN32
#define SPRITE_FILE_DIR "animation\\sprite\\"
#define SPRITE_IMAGE_DIR "animation\\image\\"
#else
#define SPRITE_FILE_DIR "animation/sprite/"
#define SPRITE_IMAGE_DIR "animation/image/"
#endif

USING_NS_CC;

namespace ScutAnimation
{
	struct ReplaceTexture 
	{
		ReplaceTexture()
		{
			pTex = NULL;
			replaceIndex = -1;
			dFactor = 1.0;
		}

		CCTexture2D* pTex;
		int replaceIndex;
		double dFactor; // 用于不同分辨率资源自适应
	};
	
	class CScutTile;
	class CScutAniData;
	class CScutAniGroup : public CCObject
	{
	public:
		CScutAniGroup();
		~CScutAniGroup();
		// 加载解析资源
		bool Load(const char* sprName);
		// 返回指定索引的动画数据
		CScutAniData* getAniData(int nIndex);
		// 返回指定图块对应的纹理资源
		ReplaceTexture getTextureByTile(CScutTile* pTile);

		int getAniCount();
		
	private:
		string m_sprName;
		
		typedef vector<ScutTileTable> vec_tile_table;
		typedef vec_tile_table::iterator vec_tile_table_it;
		vec_tile_table m_vTileTable;
		
		typedef vector<ReplaceTexture> vec_texture;
		typedef vec_texture::iterator vec_texture_it;
		vec_texture m_vTexture;
		
		typedef vector<CScutAniData*> vec_ani_data;
		typedef vec_ani_data::iterator vec_ani_data_it;
		vec_ani_data m_vAniData;
		
	private:
		void caculateTileTableTextureCoord();
	};
}

#endif