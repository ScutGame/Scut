#include "AndroidWindow.h"
#include "CCCommon.h"
#include <android/log.h>

using namespace std;

static CAndroidWindow m_wInstance;

static long  CANDROID_WINDOW_UID = 1;

extern JavaVM *gJavaVM;



CAndroidWindow::CAndroidWindow()
{
	m_gJavaVM = NULL;
	m_jWnd = NULL;
	m_pEditMap = new map<long,string>();
	m_pTextChangedCallback = NULL;
}

long CAndroidWindow::CreateWindow(int type, int bkColor, int foreColor)
{
	CAndroidWindow* pWnd =  CAndroidWindow::SharedDefault();
	long id = CANDROID_WINDOW_UID ++;
	pWnd->createChild(id,type,bkColor,foreColor);
	return id; 
	
}

void CAndroidWindow::MoveWindow( long id, int ox, int oy, int width, int height )
{
	CAndroidWindow::SharedDefault()->setChildRect(id,ox,oy,width,height);
}



void CAndroidWindow::setJavaInfo( JavaVM* javaVM, jobject wnd )
{
	cocos2d::CCLog("m_gJavaVM = javaVM.");
	m_gJavaVM = javaVM;//gJavaVM;//javaVM;
	m_jWnd = wnd;
}

void CAndroidWindow::DestroyWindow( long id )
{
	CAndroidWindow::SharedDefault()->removeChild(id);
}

void CAndroidWindow::SetTextCount( long id, int type, int nTextCount )
{
	CAndroidWindow::SharedDefault()->setChildTextCount(id,type,nTextCount);
}

bool CAndroidWindow::setChildRect( long child , int ox , int oy , int width , int height )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}
		 

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	/*static */jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildPos","(IIIII)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid,child, ox, oy, width, height);
	}

	return true;
}

bool CAndroidWindow::removeChild( long child )
{
	m_pEditMap->erase(child);

	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"removeChild","(I)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid,child);
	}

	return true;
}

bool CAndroidWindow::createChild(long id,  int type, int bkColor, int foreColor)
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}
		mid = pEnv->GetMethodID(ndrWndClass,"createChild","(IIII)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}

	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid, id, type, bkColor, foreColor);
	}
	return true;

}

CAndroidWindow::~CAndroidWindow()
{
	delete m_pEditMap;
}

CAndroidWindow* CAndroidWindow::SharedDefault()
{
	return &m_wInstance;
}

void CAndroidWindow::editStringChanged( long uid,const char* str)
{
	map<long,std::string>::iterator it = m_pEditMap->find(uid);
	if (it != m_pEditMap->end())
	{
		it->second = string(str);
	} else
	{
		m_pEditMap->insert(pair<int,std::string>(uid,string(str)));
	}
	if (m_pTextChangedCallback)
	{
		m_pTextChangedCallback(uid);
	}
}

std::string CAndroidWindow::getEditString( long uid )
{
	map<long,std::string>::iterator it = m_pEditMap->find(uid);
	if (it != m_pEditMap->end())
	{
		return it->second;
	}
	
	return string();

}

std::string CAndroidWindow::GetEditText(long id)
{
	return CAndroidWindow::SharedDefault()->getEditString(id);
}

void CAndroidWindow::ShowWindow( long id,int type )
{
	CAndroidWindow::SharedDefault()->showChild(id, type);
}

bool CAndroidWindow::setChildTextFocus(long id)
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass, "setChildTextFocus", "(I)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}

	if (mid == NULL)
	{
		return false;
	} 
	else 
	{
		pEnv->CallVoidMethod(m_jWnd, mid, id);
	}
	return true;
}

bool CAndroidWindow::showChild( long id,int type )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"showChild","(II)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid,id, type);
	}
	return true;
}

void CAndroidWindow::SetWindowText( long id, int type, const char* cStr )
{
	CAndroidWindow::SharedDefault()->setChildText(id, type,cStr);
}


///////////////////////////////////////////////////////////////// 
//add by yay: 2011-8-15
void CAndroidWindow::SetWindowTextSize( long id, int type,int nTextSize )
{
	CAndroidWindow::SharedDefault()->setChildTextSize(id, type,nTextSize);
}

bool CAndroidWindow::setChildTextSize(long id, int type, int nSize)
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildTextSize","(III)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} 
	else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid,id, type,nSize);
	}

	return true;
}
//end add
//////////////////////////////////////////


bool CAndroidWindow::setChildText( long id, int type, const char* cStr )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildText","(IILjava/lang/String;)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} 
	else 
	{
		jstring jStr = pEnv->NewStringUTF(cStr);
		pEnv->CallVoidMethod(m_jWnd,mid,id, type,jStr);
		pEnv->DeleteLocalRef(jStr);
	}

	return true;
}

bool CAndroidWindow::setChildTextCount( long id, int type, int nTextCount )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildTextCount","(III)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd,mid,id, type, nTextCount);
	}

	return true;
}

bool CAndroidWindow::setChildTextMult( long id )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildTextMult","(I)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd, mid, id);
	}

	return true;
}

bool CAndroidWindow::setChildEditPWD( long id )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildTextPWD","(I)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd, mid, id);
	}

	return true;
}

void CAndroidWindow::setTextChangedCallback(LPTextChangedCallback pCallBack)
{
	m_pTextChangedCallback = pCallBack;
}

bool CAndroidWindow::setChildInputNumber( long id )
{
	if (m_jWnd == NULL || m_gJavaVM == NULL) 
	{
		return false;
	}

	JNIEnv* pEnv = NULL;
	m_gJavaVM->AttachCurrentThread(&pEnv,NULL);
	if (pEnv == NULL)
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = pEnv->FindClass("org/cocos2dx/lib/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = pEnv->GetMethodID(ndrWndClass,"setChildTextInput","(I)V");
		pEnv->DeleteLocalRef(ndrWndClass);
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		pEnv->CallVoidMethod(m_jWnd, mid, id);
	}

	return true;
}

void CAndroidWindow::SetTextMult( long id )
{
	CAndroidWindow::SharedDefault()->setChildTextMult(id);
}

void CAndroidWindow::SetEditPWD( long id )
{
	CAndroidWindow::SharedDefault()->setChildEditPWD(id);
}

void CAndroidWindow::SetTextInputNumber( long id )
{
	CAndroidWindow::SharedDefault()->setChildInputNumber(id);
}

void CAndroidWindow::SetTextFocus( long id )
{
	CAndroidWindow::SharedDefault()->setChildTextFocus(id);
}


/*
bool CAndroidWindow::setChild( long id,int type,jobject[] value )
{
	if (m_pEnv == NULL || m_jWnd == NULL) 
	{
		return false;
	}

	static jmethodID mid = NULL;

	if (mid == NULL)
	{
		jclass ndrWndClass = m_pEnv->FindClass("cn/com/nd/view/AndroidWindow");

		if (ndrWndClass == NULL)
		{
			return false;
		}

		mid = m_pEnv->GetMethodID(ndrWndClass,"createChild","(JI)V");
	}


	if (mid == NULL)
	{
		return false;
	} else 
	{
		m_pEnv->CallVoidMethod(m_jWnd,mid,type,id);
	}

	return true;
}
*/

JNIEXPORT void JNICALL Java_org_cocos2dx_lib_AndroidWindow_nativeInit(JNIEnv * env, jclass thisCls, jobject wnd)
{
	jobject ref = env->NewGlobalRef(wnd);
	JavaVM* vm = NULL;
	env->GetJavaVM(&vm);
	CAndroidWindow::SharedDefault()->setJavaInfo(vm,ref);
}


JNIEXPORT void JNICALL Java_org_cocos2dx_lib_AndroidWindow_childTextChange(JNIEnv *env, jobject thisZ, jint uid, jstring curStr)
{
	const char* cStr;
	jboolean isCopy;
	cStr = env->GetStringUTFChars(curStr,&isCopy);
	if (cStr == NULL)
	{
		return;
	}
	
	if (isCopy)
	{
		CAndroidWindow::SharedDefault()->editStringChanged(uid,cStr);
		env->ReleaseStringUTFChars(curStr,cStr);
	}
}
