// stdafx.h : 标准系统包含文件的包含文件，
// 或是经常使用但不常更改的
// 特定于项目的包含文件
//

#pragma once

#ifdef WIN32

//#if !defined(AFX_STDAFX_H__E2B26DD5_886E_11D1_9BF5_00A0243D1382__INCLUDED_)
//#define AFX_STDAFX_H__E2B26DD5_886E_11D1_9BF5_00A0243D1382__INCLUDED_
//
//#if _MSC_VER >= 1000
//#pragma once
//#endif // _MSC_VER >= 1000
//
//#define VC_EXTRALEAN		// Exclude rarely-used stuff from windows headers
//
//#include <afxwin.h>         // MFC core and standard components
//#include <afxext.h>         // MFC extensions
//#ifndef _AFX_NO_AFXCMN_SUPPORT
//#include <afxcmn.h>			// MFC support for windows Common Controls
//#endif // _AFX_NO_AFXCMN_SUPPORT
//
//#include <Afxdisp.h>
//
//#include <winsock2.h>
//#include <afxmt.h>

// windows 头文件:


#endif

#ifdef SCUT_WIN32
	#include <windows.h>
#else
	#include <stdlib.h>
	#include <string.h>
	#include <stdio.h>
#endif

#include "Defines.h"
#include "Trace.h"