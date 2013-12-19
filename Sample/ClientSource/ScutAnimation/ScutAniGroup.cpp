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

#include "ScutAniGroup.h"
#include "FileHelper.h"
#include "SpriteFileStream.h"
#include "ScutTile.h"
#include "ScutFrame.h"
#include "ScutAniData.h"

using namespace ScutDataLogic;
using namespace ScutSystem;

#define	min(a,b)	((a) < (b) ? (a) : (b))
#define	max(a,b)	((a) > (b) ? (a) : (b))

namespace ScutAnimation
{
	CScutAniGroup::CScutAniGroup()
	{
		
	}
	
	CScutAniGroup::~CScutAniGroup()
	{
		for (vec_texture_it itTex = this->m_vTexture.begin(); itTex != this->m_vTexture.end(); itTex++)
		{
			CC_SAFE_RELEASE((*itTex).pTex);
		}
		
		for (vec_ani_data_it it = this->m_vAniData.begin(); it != this->m_vAniData.end(); it++)
		{
			CC_SAFE_DELETE((*it));
		}
	}
	
	CScutAniData* CScutAniGroup::getAniData(int nIndex)
	{
		int aniCount = this->m_vAniData.size();
		assert(nIndex > -1 && nIndex < aniCount);
		if (nIndex > -1 && nIndex < aniCount)
		{
			return this->m_vAniData[nIndex];
		}
		return NULL;
	}

	int CScutAniGroup::getAniCount()
	{
		return this->m_vAniData.size();
	}
	
	ReplaceTexture CScutAniGroup::getTextureByTile(CScutTile* pTile)
	{
		assert(pTile);
		if (pTile)
		{
			assert(pTile->m_tt.imgIndex > -1 && pTile->m_tt.imgIndex < this->m_vTexture.size());
			if (pTile->m_tt.imgIndex > -1 && pTile->m_tt.imgIndex < this->m_vTexture.size())
			{
				return this->m_vTexture[pTile->m_tt.imgIndex];
			}
		}
		return ReplaceTexture();
	}
	
	void CScutAniGroup::caculateTileTableTextureCoord()
	{
		CCTexture2D* pTexture = NULL;
		double factor = 1.0;
		for (vec_tile_table_it it = this->m_vTileTable.begin(); it != this->m_vTileTable.end(); it++)
		{
			ScutTileTable& tt = *it;
			assert(tt.imgIndex > -1 && tt.imgIndex < m_vTexture.size());
			if (!(tt.imgIndex > -1 && tt.imgIndex < m_vTexture.size()))
			{
				continue;
			}
			
			pTexture = this->m_vTexture[tt.imgIndex].pTex;
			assert(pTexture);
			if (!pTexture)
			{
				continue;
			}

			factor = this->m_vTexture[tt.imgIndex].dFactor;

			tt.clip.origin.x *= factor;
			tt.clip.origin.y *= factor;
			tt.clip.size.width *= factor;
			tt.clip.size.height *= factor;
			
			int uPixelsWidth = pTexture->getPixelsWide();
			int uPixelsHeight = pTexture->getPixelsHigh();
			
			assert((tt.clip.origin.x + tt.clip.size.width) <= uPixelsWidth + 5);
			assert((tt.clip.origin.y + tt.clip.size.height) <= uPixelsHeight + 5);
			
			GLfloat fMinS = max(tt.clip.origin.x / uPixelsWidth, 0);
			GLfloat fMinT = max(tt.clip.origin.y / uPixelsHeight, 0);
			GLfloat fMaxS = min((tt.clip.origin.x + tt.clip.size.width) / uPixelsWidth, 1);
			GLfloat fMaxT = min((tt.clip.origin.y + tt.clip.size.height) / uPixelsHeight, 1);
			
			tt.texCoord[0] = fMinS;
			tt.texCoord[1] = fMaxT;
			tt.texCoord[2] = fMaxS;
			tt.texCoord[3] = fMaxT;
			tt.texCoord[4] = fMinS;
			tt.texCoord[5] = fMinT;
			tt.texCoord[6] = fMaxS;
			tt.texCoord[7] = fMinT;
		}
	}
	
	bool CScutAniGroup::Load(const char* sprName)
	{
		if (NULL == sprName)
		{
			return false;
		}
		
		string spr = sprName;
		
		if (spr.size() == 0)
		{
			return false;
		}
		
		if (this->m_sprName.size() > 0)
		{
			return this->m_sprName == spr;
		}
		
		this->m_sprName = sprName;

		spr += ".spr";
		spr = SPRITE_FILE_DIR + spr;
		spr = CFileHelper::getPath(spr.c_str());
		
		char cMode[3] = {'r', 'b', 0};
		unsigned long uSize = 0;
		unsigned char* pBuf = (unsigned char*)CFileHelper::getFileData(spr.c_str(), cMode, &uSize);
		CSpriteFileStream fileSpr;
		if (fileSpr.Write((const char*)pBuf, uSize) == 0)
		{
			delete[] pBuf;
			return false;
		}

		delete[] pBuf;
		fileSpr.SetPosition(0);
		int tableSize = fileSpr.ReadByte();
		this->m_vTileTable.assign(tableSize, ScutTileTable());

		for (int i = 0; i < tableSize; i++)
		{
			ScutTileTable& tt = this->m_vTileTable[i];
			tt.imgIndex = fileSpr.ReadByte();
			tt.clip.origin.x = fileSpr.ReadShort();
			tt.clip.origin.y = fileSpr.ReadShort();
			tt.clip.size.width = fileSpr.ReadShort();
			tt.clip.size.height = fileSpr.ReadShort();
		}

		double pixcelFactor = 1.0;
		
		int imgSize = fileSpr.ReadByte();
		CCTexture2D* pTex = NULL;
		this->m_vTexture.assign(imgSize, ReplaceTexture());
		string texFile;
		for (int j = 0; j < imgSize; j++)
		{
			ReplaceTexture& texture = this->m_vTexture[j];
			fileSpr.ReadUTF(texFile);
			texFile = SPRITE_IMAGE_DIR + texFile;
			texFile = CFileHelper::getPath(texFile.c_str());
			pTex = CCTextureCache::sharedTextureCache()->addImage(texFile.c_str());
			assert(pTex != NULL);
			CC_SAFE_RETAIN(pTex);
			texture.pTex = pTex;
			texture.replaceIndex = fileSpr.ReadByte();
			texture.dFactor = (double)pTex->getContentSizeInPixels().width / (double)fileSpr.ReadShort();

			// 取第一张图片的缩放比例作为动画整体的估计值
			if (0 == j)
			{
				pixcelFactor = texture.dFactor;
			}
			
		}

		this->caculateTileTableTextureCoord();
		
		CScutAniData* pAniData = NULL;
		CScutFrame* pFrame = NULL;
		CScutTile* pTile = NULL;
		
		int aniSize = fileSpr.ReadByte();
		this->m_vAniData.assign(aniSize, pAniData);
		
		for (int m = 0; m < aniSize; m++)
		{
			pAniData = new CScutAniData;
			this->m_vAniData[m] = pAniData;

			pAniData->m_tContentSize.width = fileSpr.ReadShort() * pixcelFactor;
			pAniData->m_tContentSize.height = fileSpr.ReadShort() * pixcelFactor;
			pAniData->m_anchorX = fileSpr.ReadShort();
			pAniData->m_anchorY = fileSpr.ReadShort();
			pAniData->m_type = fileSpr.ReadByte();
			
			int frameSize = fileSpr.ReadByte();
			pAniData->m_vFrame.assign(frameSize, pFrame);
			
			for (int n = 0; n < frameSize; n++)
			{
				pFrame = new CScutFrame;
				pAniData->m_vFrame[n] = pFrame;
				pFrame->m_duration = fileSpr.ReadShort();
				
				int tileSize = fileSpr.ReadByte();
				pFrame->m_vTiles.assign(tileSize, pTile);
				
				for (int tl = 0; tl < tileSize; tl++)
				{
					float fx = fileSpr.ReadShort();
					float fy = fileSpr.ReadShort();
					int flipXY = fileSpr.ReadByte();
					float rotation = fileSpr.ReadShort();
					float scale = fileSpr.ReadFloat();
					int tableIndex = fileSpr.ReadShort();
					
					assert(tableIndex > -1 && tableIndex < this->m_vTileTable.size());
					if (tableIndex > -1 && tableIndex < this->m_vTileTable.size())
					{
						pTile = new CScutTile(this->m_vTileTable[tableIndex]);
						assert(pTile->m_tt.imgIndex > -1 && pTile->m_tt.imgIndex < m_vTexture.size());

						// 根据每个tile所属的图片精确计算缩放因子
						double factor = this->m_vTexture[pTile->m_tt.imgIndex].dFactor;

						pFrame->m_vTiles[tl] = pTile;
						pFrame->setScutAniGroup(this);
						pTile->m_frameX = fx * factor;
						pTile->m_frameY = fy * factor;
						pTile->m_flipX = (flipXY & CScutTile::FLIP_X) > 0;
						pTile->m_flipY = (flipXY & CScutTile::FLIP_Y) > 0;
						pTile->m_rotation = rotation;
						pTile->m_scale = scale;
						pTile->caculateTileVertices(pAniData->m_anchorX * factor, pAniData->m_anchorY * factor);
					}
				}
			}
		}

		return true;
	}
}


