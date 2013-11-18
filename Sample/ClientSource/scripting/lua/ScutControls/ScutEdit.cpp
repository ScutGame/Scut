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
#include "ScutEdit.h"
#include "CCPlatformMacros.h"
#if (CC_TARGET_PLATFORM == CC_PLATFORM_IOS)
#endif

#if (CC_TARGET_PLATFORM == CC_PLATFORM_ANDROID)
#include "./ANDROID/ScutEdit_ANDROID.cpp"
#endif


#if (CC_TARGET_PLATFORM == CC_PLATFORM_WIN32)
#include "./Win32/ScutEdit_win32.cpp"
#endif


/*
JAVA中这样写:setBackgroundDrawable(new BitmapDrawable(("bg_focus.png")));
background属性为一张图片即可

设置背景图片
1、将背景图片放置在 drawable-mdpi 目录下，假设图片名为 bgimg.jpg 。

2、main.xml 文件

以下为引用内容： 
<EditText   ANDROID:layout_width="fill_parent"
   ANDROID:layout_height="wrap_content"    ANDROID:background="@drawable/bgimg"   />
   */
