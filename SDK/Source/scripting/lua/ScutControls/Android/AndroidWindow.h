
#ifndef _ANDROID_WINDOW_H_
#define _ANDROID_WINDOW_H_

#include <jni.h>
#include<map>
#include <string>

	typedef void (*LPTextChangedCallback)(long);

	class CAndroidWindow
	{
	public:
		static long CreateWindow(int type, int bkColor, int foreColor);
		static void MoveWindow(long id, int ox, int oy, int	width, int height);
		static void DestroyWindow(long id);
		static void ShowWindow(long id,int type);
		static void SetWindowText(long id, int type, const char* cStr);
		static void SetWindowTextSize(long id, int type, int nSize);
		static void SetTextCount(long id, int type, int nTextCount);
		static void SetTextMult(long id);
		static void SetEditPWD(long id);
		static void SetTextInputNumber(long id);
		static std::string GetEditText(long id);
		static void SetTextFocus(long id);
		static CAndroidWindow* SharedDefault();
	public:
		CAndroidWindow();
		virtual ~CAndroidWindow();
		
		bool createChild(long id,int type, int bkColor, int foreColor);
		bool setChildRect(long child , int ox , int oy , int width , int height);
		bool removeChild(long child);
		bool showChild(long id,int type);
		bool setChildText(long id, int type, const char* cStr);
		bool setChildTextSize(long id, int type, int nSize);
		bool setChildTextCount(long id, int type, int nTextCount);
		bool setChildTextMult(long id);
		bool setChildEditPWD(long id);
		bool setChildInputNumber(long id);
		void setTextChangedCallback(LPTextChangedCallback pCallBack);

		//bool setChild(long id,int type,jobject values ...);
		void setJavaInfo(JavaVM* javaVM, jobject wnd);
		void editStringChanged(long uid,const char* str);
		std::string getEditString(long uid);

		bool setChildTextFocus(long id);
	public:
		static const long WND_UNDEFINE = 0;
	private:
		JavaVM* m_gJavaVM;
		jobject		m_jWnd;
		std::map<long,std::string>* m_pEditMap;
		LPTextChangedCallback m_pTextChangedCallback;
	};

	#ifdef __cplusplus
	extern "C" {
	#endif
	#undef org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT
	#define org_cocos2dx_lib_AndroidWindow_VIEW_TYPE_EDIT 1

	#undef org_cocos2dx_lib_AndroidWindow_OP_SW_VISIBLE
	#define org_cocos2dx_lib_AndroidWindow_OP_SW_VISIBLE 1

	#undef org_cocos2dx_lib_AndroidWindow_OP_SW_HIDE
	#define org_cocos2dx_lib_AndroidWindow_OP_SW_HIDE 2

	#undef org_cocos2dx_lib_AndroidWindow_OP_SW_INVISIBLE
	#define org_cocos2dx_lib_AndroidWindow_OP_SW_INVISIBLE 3

	#undef org_cocos2dx_lib_AndroidWindow_OP_CREATE_VIEW
	#define org_cocos2dx_lib_AndroidWindow_OP_CREATE_VIEW 4

	#undef org_cocos2dx_lib_AndroidWindow_OP_DELETE_VIEW
	#define org_cocos2dx_lib_AndroidWindow_OP_DELETE_VIEW 5

	#undef org_cocos2dx_lib_AndroidWindow_OP_SET_POSITION
	#define org_cocos2dx_lib_AndroidWindow_OP_SET_POSITION 6

	#undef org_cocos2dx_lib_AndroidWindow_OP_SET_TEXT
	#define org_cocos2dx_lib_AndroidWindow_OP_SET_TEXT 7
	
	#undef org_cocos2dx_lib_AndroidWindow_OP_ENABLE
	#define org_cocos2dx_lib_AndroidWindow_OP_ENABLE 100
	
	#undef org_cocos2dx_lib_AndroidWindow_OP_DISABLE
	#define org_cocos2dx_lib_AndroidWindow_OP_DISABLE 101

	/*
	* Class:     org_cocos2dx_lib_AndroidWindow
	* Method:    nativeInit
	* Signature: (Ljava/lang/Object;)V
	*/
	JNIEXPORT void JNICALL Java_org_cocos2dx_lib_AndroidWindow_nativeInit
	(JNIEnv *, jclass, jobject);


	/*
	* Class:     org_cocos2dx_lib_AndroidWindow
	* Method:    textChange
	* Signature: (Ljava/lang/String;Ljava/lang/String;)V
	*/
	JNIEXPORT void JNICALL Java_org_cocos2dx_lib_AndroidWindow_childTextChange
	(JNIEnv *, jobject, jint,	jstring);


	#ifdef __cplusplus
	}
	#endif
#endif