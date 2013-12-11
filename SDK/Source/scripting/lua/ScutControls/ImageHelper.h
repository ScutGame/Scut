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
#ifndef _IMAGEMANAGER_H_
#define _IMAGEMANAGER_H_
#include <string>
#include "CCTexture2D.h"
#include "CCLuaEngine.h"

using namespace std;

namespace ScutCxControl
{

	/**
	* @brief 图片描边 
	* @see		
	*/
	class LUA_DLL CImageHelper
	{
	public:
		CImageHelper();
		virtual ~CImageHelper();
	public:
		
		/**
		* @brief  通过file 的路径生成一张描边的CCTexture2D对象图片
		* @n<b>函数名称</b>					: getImageTracing
		* @n@param   char*   描边信息文件名
		* @return   
		* @remarks  
		* @see		
		*/
		
		static cocos2d::CCTexture2D* getImageTracingEx(const char* file, int nBorderWidth );
		static cocos2d::CCTexture2D* getImageTracing(const char* file);
		
		//添加灰度图特效
		static cocos2d::CCTexture2D* getImageGray(const char* strFile);

		static cocos2d::CCImage* createTracingImage(const char* file, int nBorderWidth);

		static cocos2d::CCImage* createGrayImage(const char* file);
	protected:
		static cocos2d::CCTexture2D* createImageTracing(const char* file, const std::string& skey, int nBorderWidth);
		static cocos2d::CCImage* getCCImage(const char* file);
	private:
		static cocos2d::CCTexture2D* createImageGray(const char* strFile, const std::string& skey);

	};
}


#endif