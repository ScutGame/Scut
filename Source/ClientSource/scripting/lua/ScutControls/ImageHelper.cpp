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

#include <string>
#include "ImageHelper.h"

#include "cocos2d.h"

namespace ScutCxControl
{


	CImageHelper::CImageHelper()
	{

	}

	CImageHelper::~CImageHelper()
	{


	}

	cocos2d::CCTexture2D* CImageHelper::getImageTracingEx( const char* fileName, int nBorderWidth )
	{
		string skey = fileName;
		skey.append("_tracing_key");
		cocos2d::CCTexture2D* pTexture = cocos2d::CCTextureCache::sharedTextureCache()->textureForKey(skey.c_str());
		if (pTexture == NULL)
		{
			pTexture = createImageTracing(fileName, skey, nBorderWidth);
		} 

		return pTexture;
	}

	cocos2d::CCTexture2D* CImageHelper::getImageTracing( const char* file )
	{
		return getImageTracingEx(file, 1);
	}

	cocos2d::CCImage* CImageHelper::createTracingImage( const char* file, int nBorderWidth )
	{
		cocos2d::CCImage* myImage = getCCImage(file);
		if (myImage == NULL)
		{
			cocos2d::CCLog("createImageTracing error %s", file);
			return NULL;
		}
		if (!myImage->hasAlpha())
		{
			delete myImage;
			cocos2d::CCLog("createImageTracing only support has Alpha");
			return NULL;
		}

		unsigned char * pdata = myImage->getData();

		int w = myImage->getWidth();//
		int h = myImage->getHeight();//

		int nColorWord = 4;
		unsigned int length = (w * h * nColorWord);//myImage->getDataLen() * 4;
		unsigned char* pd = new unsigned char[length];
		memcpy(pd, pdata, length);

		if (nBorderWidth < 1 || nBorderWidth > 20)
		{
			nBorderWidth = 1;
		}
		for ( int r = 0; r < h; r++)
			for (int l = 0 ; l < w ;l++)
			{
				int i = (r * w * nColorWord) + l * nColorWord + 3;
				if(pd[i] < 10 * nBorderWidth 
					&& 
					((i+nColorWord < length && pd[i + nColorWord] != 0)
					|| (i - nColorWord > 0 && pd[i - nColorWord] != 0)
					|| (i - w* nColorWord > 0 && pd[i - w* nColorWord] != 0)
					|| (i + w* nColorWord < length  && pd[i + w* nColorWord] != 0)
					)
					)
				{
					pdata[i] = 255;
					pdata[i-1] = 255;
					pdata[i-2] = 255;
					pdata[i-3] = 255;
				} else
				{ 
					pdata[i] = 0;
					pdata[i-1] = 0;
					pdata[i-2] = 0;
					pdata[i-3] = 0;
				}
			}
		delete[] pd;
		return myImage;
	}

	cocos2d::CCImage* CImageHelper::createGrayImage( const char* file )
	{
		cocos2d::CCImage* myImage = getCCImage(file);
		if (myImage == NULL)
		{
			cocos2d::CCLog("createImageGray error %s", file);
			return NULL;
		}
		int PixelSize = 4;
		if (!myImage->hasAlpha())
		{
			PixelSize = 3;
		}

		unsigned char * pdata = myImage->getData();
		int width = myImage->getWidth();//
		int height = myImage->getHeight();//
		if (pdata)
		{
			for (int i = 0; i < height; i++)
			{
				for (int j = 0; j < width; j++)
				{

					int nValue = pdata[i* width * PixelSize + j * PixelSize] + 
						pdata[i* width* PixelSize + j * PixelSize + 1] + 
						pdata[i* width* PixelSize + j * PixelSize + 2];
					nValue /= 3; 
					pdata[i* width* PixelSize + j * PixelSize] = pdata[i* width * PixelSize+ j * PixelSize + 1] = 
						pdata[i* width* PixelSize + j * PixelSize + 2] = nValue;
				}
			}
		}

		return myImage;
	}

	cocos2d::CCTexture2D* CImageHelper::createImageTracing( const char* file ,const std::string& skey, int nBorderWidth )
	{
		cocos2d::CCImage* myImage = createTracingImage(file, nBorderWidth);

		cocos2d::CCTexture2D* pTexture = cocos2d::CCTextureCache::sharedTextureCache()->addUIImage(myImage, skey.c_str());
		delete myImage;
		return pTexture;
	}


	cocos2d::CCImage* CImageHelper::getCCImage( const char* file)
	{
		cocos2d::CCImage* pImage = new cocos2d::CCImage();
		bool bRet = pImage->initWithImageFile(file);
		if (!bRet)
		{
			delete	pImage;
			pImage	= NULL;
		}
		return pImage;
	}

	cocos2d::CCTexture2D* CImageHelper::getImageGray( const char* strFile )
	{
		string skey = strFile;
		skey.append("_gray_key");
		cocos2d::CCTexture2D* pTexture = cocos2d::CCTextureCache::sharedTextureCache()->textureForKey(skey.c_str());
		if (pTexture == NULL)
		{
			pTexture = createImageGray(strFile,skey);
		} 

		return pTexture;

	}

	cocos2d::CCTexture2D* CImageHelper::createImageGray( const char* strFile, const std::string& skey )
	{
		cocos2d::CCImage* myImage = createGrayImage(strFile);
		cocos2d::CCTexture2D* pTexture = cocos2d::CCTextureCache::sharedTextureCache()->addUIImage(myImage, skey.c_str());
		delete myImage;
		return pTexture;
	}
}
