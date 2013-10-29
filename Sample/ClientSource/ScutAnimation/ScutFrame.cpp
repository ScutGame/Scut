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
#include "ScutFrame.h"
#include "ScutTile.h"
#include "ScutAniGroup.h"

namespace ScutAnimation
{
	CScutFrame::CScutFrame()
	{
		m_duration = 0;
		m_nTexCount = 0;
		m_nMaxIndex = 0;
		m_pIndices = (NULL);
		m_pTexture = (NULL);	
		m_pQuads = (NULL);
		m_nMaxTexture = (NULL);
	}
	
	CScutFrame::~CScutFrame()
	{
		for (vec_tile_it it = this->m_vTiles.begin(); it != this->m_vTiles.end(); it++)
		{
			CC_SAFE_DELETE((*it));
		}
		for (map_V3F_T2F_Quad_it itQuad = m_mapTexture.begin(); itQuad != m_mapTexture.end(); itQuad++)
		{
			CC_SAFE_FREE((*itQuad).second);
		}
		CC_SAFE_FREE(m_pIndices);
	}
	
	int CScutFrame::getTileCount()
	{
		return this->m_vTiles.size();
	}
	
	CScutTile* CScutFrame::getTileByIndex(int nIndex)
	{
		int nCount = this->m_vTiles.size();
		assert(nIndex > -1 && nIndex < nCount);
		if (nIndex > -1 && nIndex < nCount)
		{
			return this->m_vTiles[nIndex];
		}
		return NULL;
	}

	void CScutFrame::calQuads()
	{
		if (getTileCount() == 0)
		{
			return;
		}

		int nCount = getTileCount();

		calTextureTileCount(nCount);

		initQuads(nCount);

		initIndices(nCount);
	}

	void CScutFrame::initQuads(int nTotalCount)
	{
		int indexVBO = 0;

		for (map_TextureCount_it itCnt = m_mapTextureCount.begin(); itCnt != m_mapTextureCount.end(); itCnt++, indexVBO++)
		{
			int count = (*itCnt).second;
			m_pTexture = (*itCnt).first;
			m_pQuads = (ccV3F_T2F_Quad*)calloc( sizeof(ccV3F_T2F_Quad) * count, 1 );
			
			CScutTile* pTile = NULL;
			int indexQuad = 0;
			for (int i = 0; i < nTotalCount; i++, indexQuad++)
			{
				pTile = getTileByIndex(i);

				if (m_ScutAniGroup->getTextureByTile(pTile).pTex != m_pTexture)
				{
					indexQuad--;
					continue;
				}

				ccV3F_T2F_Quad pQuad;

				pQuad.bl.vertices.x = pTile->vertices[0];
				pQuad.bl.vertices.y = pTile->vertices[1];
				pQuad.bl.vertices.z = pTile->vertices[2];
				pQuad.bl.texCoords.u = pTile->m_tt.texCoord[0];
				pQuad.bl.texCoords.v = pTile->m_tt.texCoord[1];

				pQuad.br.vertices.x = pTile->vertices[3];
				pQuad.br.vertices.y = pTile->vertices[4];
				pQuad.br.vertices.z = pTile->vertices[5];
				pQuad.br.texCoords.u = pTile->m_tt.texCoord[2];
				pQuad.br.texCoords.v = pTile->m_tt.texCoord[3];

				pQuad.tl.vertices.x = pTile->vertices[6];
				pQuad.tl.vertices.y = pTile->vertices[7];
				pQuad.tl.vertices.z = pTile->vertices[8];
				pQuad.tl.texCoords.u = pTile->m_tt.texCoord[4];
				pQuad.tl.texCoords.v = pTile->m_tt.texCoord[5];

				pQuad.tr.vertices.x = pTile->vertices[9];
				pQuad.tr.vertices.y = pTile->vertices[10];
				pQuad.tr.vertices.z = pTile->vertices[11];
				pQuad.tr.texCoords.u = pTile->m_tt.texCoord[6];
				pQuad.tr.texCoords.v = pTile->m_tt.texCoord[7];

				m_pQuads[indexQuad] = pQuad;	
			}
			m_mapTexture[m_pTexture] = m_pQuads;
		}
	}

	void CScutFrame::initIndices(int count)
	{
		m_pIndices = (GLushort *)calloc( sizeof(GLushort) * count * 6, 1 );

		for( unsigned int i=0; i < count; i++)
		{
			m_pIndices[i*6+0] = (GLushort)(i*4+0);
			m_pIndices[i*6+1] = (GLushort)(i*4+1);
			m_pIndices[i*6+2] = (GLushort)(i*4+2);

			m_pIndices[i*6+3] = (GLushort)(i*4+3);
			m_pIndices[i*6+4] = (GLushort)(i*4+2);
			m_pIndices[i*6+5] = (GLushort)(i*4+1);		

		}
	}

	void CScutFrame::drawQuads()
	{
		//glDisableClientState(GL_COLOR_ARRAY);

		//for (map_V3F_T2F_Quad_it it = m_mapTexture.begin(); it != m_mapTexture.end(); it++)
		//{
		//	m_pTexture = (*it).first;
		//	if (m_pTexture == m_nMaxTexture)
		//	{
		//		continue;
		//	}

		//	m_pQuads = (*it).second;

		//	onDraw();
		//}

		//if (m_nTexCount > 1 && m_nMaxTexture != NULL)
		//{
		//	m_pTexture = m_nMaxTexture;;

		//	m_pQuads = m_mapTexture[m_pTexture];

		//	onDraw();
		//}
		//
		//glEnableClientState(GL_COLOR_ARRAY);
	}

	void CScutFrame::onDraw()
	{
		//mark es2.0
//		int count = m_mapTextureCount[m_pTexture];
//
//		glBiScutTexture(GL_TEXTURE_2D, m_pTexture->getName());
//
//#define kQuadSize sizeof(m_pQuads[0].bl)
//
//		unsigned int offset = (unsigned int)m_pQuads;
//
//		unsigned int diff = offsetof( ccV3F_T2F, vertices);
//		glVertexPointer(3, GL_FLOAT, kQuadSize, (GLvoid*) (offset + diff) );
//
//		diff = offsetof( ccV3F_T2F, texCoords);
//		glTexCoordPointer(2, GL_FLOAT, kQuadSize, (GLvoid*)(offset + diff));
//
//		glDrawElements(GL_TRIANGLES, count * 6, GL_UNSIGNED_SHORT, m_pIScutices);
	}

	void CScutFrame::calTextureTileCount(int count)
	{
		CCTexture2D* textures[32] = {NULL};
		int tileCount[256] = {0};

		CScutTile* pTile = NULL;
		int i = 0;
		for (; i < count; i++)
		{
			pTile = getTileByIndex(i);

			m_pTexture = m_ScutAniGroup->getTextureByTile(pTile).pTex;

			if (m_pTexture == NULL)
			{
				continue;
			}

			for (int j = 0; j < count; j++)
			{
				if (textures[j] == m_pTexture)
				{
					tileCount[j] += 1;
					m_mapTextureCount[m_pTexture] = tileCount[j];
					break;
				}
				else if (textures[j] == NULL)
				{
					m_nTexCount++;

					textures[j] = m_pTexture;
					tileCount[j] += 1;

					m_mapTextureCount[m_pTexture] = tileCount[j];
					break;
				}
			}		
		}

		i = 0;
		int nMaxCount = 0;
		for (; i < m_nTexCount; i++)
		{
			if (tileCount[i] > nMaxCount)
			{
				nMaxCount = tileCount[i];
				m_nMaxIndex = i;
			}
		}

		for (map_TextureCount_it itCnt = m_mapTextureCount.begin(); itCnt != m_mapTextureCount.end(); itCnt++)
		{
			if ((*itCnt).second == nMaxCount)
			{
				m_nMaxTexture = (*itCnt).first;
				break;
			}
		}
	}

	void CScutFrame::setScutAniGroup( CScutAniGroup* pScutAniGroup )
	{
		m_ScutAniGroup = pScutAniGroup;
	}
}
